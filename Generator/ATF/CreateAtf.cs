using System.Diagnostics.CodeAnalysis;
using System.IO.Abstractions;
using System.Text;
using System.Text.Json;
using Generator.ATF.AdaptivityElement;
using Generator.WorldExport;
using Microsoft.Extensions.Logging;
using PersistEntities;
using PersistEntities.LearningContent;
using PersistEntities.LearningContent.Action;
using PersistEntities.LearningContent.Question;
using Shared;
using Shared.Adaptivity;
using Shared.Extensions;

namespace Generator.ATF;

public class CreateAtf : ICreateAtf
{
    private readonly IFileSystem _fileSystem;
    private string _atfPath;
    private string _author;
    private string _booleanAlgebraRequirements;
    private Dictionary<int, ContentReferenceActionPe> _dictionaryIdToContentReferenceAction;
    private Dictionary<int, ILearningElementPe> _dictionaryIdToLearningElement;
    private string _language;
    private List<ILearningElementPe> _listAllLearningElements;
    private List<int?> _listLearningSpaceElements;
    private int _questionId;
    private string _xmlFilesForExportPath;
    internal Dictionary<int, Guid> DictionarySpaceIdToGuid;
    internal LearningWorldJson LearningWorldJson;
    internal List<(IFileContentPe, string)> ListFileContent;
    internal List<LearningSpacePe> ListLearningSpaces;
    internal List<TopicPe> ListTopics;
    internal string Uuid;

    /// <summary>
    /// Read the PersistEntities and create a atf Document with a specified syntax.
    /// </summary>
    /// <param name="fileSystem"></param>
    /// <param name="logger"></param>
    public CreateAtf(IFileSystem fileSystem, ILogger<CreateAtf> logger)
    {
        Initialize();
        _fileSystem = fileSystem;
        Logger = logger;
        _author = "";
        _atfPath = "";
        _language = "";
        _xmlFilesForExportPath = "";
    }

    private ILogger<CreateAtf> Logger { get; }

    /// <inheritdoc cref="ICreateAtf.GenerateAndExportLearningWorldJson"/>
    public string GenerateAndExportLearningWorldJson(LearningWorldPe learningWorld)
    {
        Initialize();

        InitializeLearningWorldProperties(learningWorld);

        MapTopicsToLearningWorldJson(learningWorld.Topics);

        MapLearningSpacesToLearningWorldJson(learningWorld.ObjectsInPathWaysPe);

        var rootJson = CreateRootJson();
        var jsonFile = SerializeRootJson(rootJson);

        SetupDirectoryStructure();

        CopyLearningElementFiles();

        WriteAtfToFile(jsonFile);
        return _atfPath;
    }

    /// <summary>
    /// Searches for duplicate learning element names in a list of learning spaces. If duplicates are found, their names are incremented.
    /// </summary>
    /// <param name="listLearningSpace">The list of LearningSpacePe objects to search.</param>
    /// <returns>A list of LearningSpacePe objects with incremented names for duplicates.</returns>
    internal static List<LearningSpacePe> IncrementDuplicateLearningElementNames(
        List<LearningSpacePe> listLearningSpace)
    {
        // Extract all learning elements from the list of learning spaces
        var allLearningElements = listLearningSpace
            .SelectMany(space => space.LearningSpaceLayout.ContainedLearningElements)
            .ToList();

        // Identify names of learning elements that appear more than once
        var duplicateLearningElements = allLearningElements.GroupBy(element => element.Name)
            .Where(group => group.Count() > 1)
            .Select(group => group.Key)
            .ToList();

        var incrementedElementNames = new Dictionary<string, string>();

        foreach (var duplicateName in duplicateLearningElements)
        {
            // For each learning element with a duplicate name, rename it by incrementing its name
            foreach (var element in from space in listLearningSpace
                     from element in space.LearningSpaceLayout.ContainedLearningElements
                     where element.Name == duplicateName
                     select element)
            {
                if (!incrementedElementNames.ContainsKey(duplicateName))
                {
                    incrementedElementNames[duplicateName] = StringHelper.IncrementName(duplicateName);
                }
                else
                {
                    // If the name has been incremented before, increment it further based on the last incremented value
                    incrementedElementNames[duplicateName] =
                        StringHelper.IncrementName(incrementedElementNames[duplicateName]);
                }

                element.Name = incrementedElementNames[duplicateName];
            }
        }

        return listLearningSpace;
    }

    /// <summary>
    /// Takes a Condition and builds a boolean algebra string.
    /// Method searches recursively for all conditions and their inbound Spaces.
    /// </summary>
    /// <param name="pathWayCondition"></param>
    /// <returns>A string that describes a boolean algebra expression</returns>
    internal string DefineLogicalExpression(PathWayConditionPe pathWayCondition)
    {
        var conditionSymbol = GetConditionSymbol(pathWayCondition.Condition);
        var conditionExpression = new StringBuilder();

        foreach (var learningObject in pathWayCondition.InBoundObjects)
        {
            switch (learningObject)
            {
                case LearningSpacePe:
                    conditionExpression.Append(GetSpaceCondition(learningObject, conditionSymbol));
                    break;
                case PathWayConditionPe nestedCondition:
                    conditionExpression.Append(GetNestedCondition(nestedCondition, conditionSymbol));
                    break;
            }
        }

        return conditionExpression.ToString()[
            ..(conditionExpression.ToString().LastIndexOf(")", StringComparison.Ordinal) + 1)];
    }

    private static string GetConditionSymbol(ConditionEnum condition)
    {
        return condition switch
        {
            ConditionEnum.And => "^",
            ConditionEnum.Or => "v",
            _ => ""
        };
    }

    private string GetSpaceCondition(IObjectInPathWayPe learningObject, string conditionSymbol)
    {
        var spaceId = DictionarySpaceIdToGuid.FirstOrDefault(x => x.Value == learningObject.Id).Key.ToString();
        return $"({spaceId}){conditionSymbol}";
    }

    private string GetNestedCondition(PathWayConditionPe nestedCondition, string conditionSymbol)
    {
        var nestedExpression = DefineLogicalExpression(nestedCondition);
        return nestedCondition.InBoundObjects.Count == 1
            ? nestedExpression
            : $"({nestedExpression}){conditionSymbol}";
    }

    [MemberNotNull(nameof(ListFileContent), nameof(ListLearningSpaces), nameof(ListTopics),
        nameof(_listLearningSpaceElements), nameof(_booleanAlgebraRequirements), nameof(DictionarySpaceIdToGuid),
        nameof(_dictionaryIdToLearningElement), nameof(_dictionaryIdToContentReferenceAction), nameof(Uuid),
        nameof(_listAllLearningElements), nameof(LearningWorldJson))]
    private void Initialize()
    {
        ListFileContent = new List<(IFileContentPe, string)>();
        ListLearningSpaces = new List<LearningSpacePe>();
        ListTopics = new List<TopicPe>();
        _listLearningSpaceElements = new List<int?>();
        _booleanAlgebraRequirements = "";
        DictionarySpaceIdToGuid = new Dictionary<int, Guid>();
        _dictionaryIdToLearningElement = new Dictionary<int, ILearningElementPe>();
        _dictionaryIdToContentReferenceAction = new Dictionary<int, ContentReferenceActionPe>();
        var guid = Guid.NewGuid();
        Uuid = guid.ToString();
        _listAllLearningElements = new List<ILearningElementPe>();
        LearningWorldJson = new LearningWorldJson("", "", new List<ITopicJson>(), new List<ILearningSpaceJson>(),
            new List<IElementJson>(), "", Array.Empty<string>());
        _questionId = 1;
    }

    private void InitializeLearningWorldProperties(LearningWorldPe learningWorld)
    {
        //Setting Authors and Language for ATF Root
        _author = learningWorld.Authors;
        _language = "de";
        LearningWorldJson.WorldName = learningWorld.Name;
        LearningWorldJson.WorldUUID = learningWorld.Id.ToString();
        LearningWorldJson.WorldDescription = learningWorld.Description;
        LearningWorldJson.WorldGoals = learningWorld.Goals.Split("\n");
        LearningWorldJson.EvaluationLink = learningWorld.EvaluationLink;
        LearningWorldJson.EnrolmentKey = learningWorld.EnrolmentKey;
    }

    /// <summary>
    /// Maps a list of topics to the Topics of LearningWorldJson
    /// </summary>
    /// <param name="topics">List of topics to be mapped.</param>
    private void MapTopicsToLearningWorldJson(List<TopicPe> topics)
    {
        ListTopics.AddRange(topics);

        var topicId = 1;
        foreach (var topic in ListTopics)
        {
            LearningWorldJson.Topics.Add(new TopicJson(topicId, topic.Name, new List<int>()));
            topicId++;
        }
    }

    /// <summary>
    /// Maps a list of learning spaces with its learning elements to the Spaces of LearningWorldJson.
    /// </summary>
    /// <param name="objectInPathWays">All objects in pathway of the learning world.</param>
    private void MapLearningSpacesToLearningWorldJson(List<IObjectInPathWayPe> objectInPathWays)
    {
        // Starting Value for Learning Space Ids in the ATF-Document
        var learningSpaceId = 1;

        ListLearningSpaces.AddRange(GetLearningSpacesInOrder(objectInPathWays));

        DictionarySpaceIdToGuid = ListLearningSpaces.Select((space, index) => new { index, space.Id })
            .ToDictionary(x => x.index + 1, x => x.Id);

        ListLearningSpaces = IncrementDuplicateLearningElementNames(ListLearningSpaces);

        WriteDictionaries();

        foreach (var space in ListLearningSpaces)
        {
            _listLearningSpaceElements =
                MapLearningSpaceElementsToIds(space, learningSpaceId);
            _booleanAlgebraRequirements = GetRequiredSpacesToEnter(space);

            AssignTopicToSpace(space, learningSpaceId);

            var spaceGoals = new string[space.LearningOutcomes.Count];
            var index = 0;

            foreach (var learningOutcome in space.LearningOutcomes)
            {
                spaceGoals[index] = learningOutcome.GetOutcome();
                index++;
            }

            LearningWorldJson.Spaces.Add(new LearningSpaceJson(learningSpaceId, space.Id.ToString(),
                space.Name, _listLearningSpaceElements,
                space.RequiredPoints, space.LearningSpaceLayout.FloorPlanName.ToString(), space.Theme.ToString(),
                space.Description, spaceGoals, _booleanAlgebraRequirements));

            learningSpaceId++;
        }

        LearningWorldJson.Elements = LearningWorldJson.Elements.OrderBy(x => x.ElementId).ToList();
    }

    /// <summary>
    /// Populates the dictionaries with learning elements and content reference actions.
    /// </summary>
    private void WriteDictionaries()
    {
        var idToElementId = 1;
        foreach (var space in ListLearningSpaces)
        {
            for (var i = 0; i <= GetMaxSlotNumber(space); i++)
            {
                if (!space.LearningSpaceLayout.LearningElements.TryGetValue(i, out var element)) continue;
                _dictionaryIdToLearningElement.Add(idToElementId, element);
                idToElementId++;
                idToElementId = AddContentReferenceActionsToDictionary(element, idToElementId);
            }
        }
    }

    /// <summary>
    /// Adds content reference actions to the dictionary based on the provided learning element.
    /// </summary>
    /// <param name="element">The learning element from which content reference actions are extracted.</param>
    /// <param name="idToElementId">The current ID to be used for dictionary entries.</param>
    /// <returns>The updated ID after adding entries to the dictionary.</returns>
    private int AddContentReferenceActionsToDictionary(ILearningElementPe element, int idToElementId)
    {
        if (element.LearningContent is not IAdaptivityContentPe adaptivityContentPe)
            return idToElementId;

        var contentReferenceActions = adaptivityContentPe.Tasks
            .SelectMany(task => task.Questions)
            .SelectMany(question => question.Rules)
            .Select(rule => rule.Action as ContentReferenceActionPe)
            .Where(action => action != null)
            .Cast<ContentReferenceActionPe>();

        foreach (var contentReferenceActionPe in contentReferenceActions)
        {
            _dictionaryIdToContentReferenceAction.Add(idToElementId, contentReferenceActionPe);
            idToElementId++;
        }

        return idToElementId;
    }

    /// <summary>
    /// Determines the order of learning spaces based on the provided paths and returns them as a list.
    ///
    /// This algorithm uses a Breadth-First Search (BFS) approach to ensure that spaces residing on the same "level"
    /// are consecutive in the resulting list. Spaces without outgoing or incoming paths are appended at the end of the list.
    ///
    /// Spaces from the same path are ordered alphabetically.
    ///
    /// Note: This algorithm is also utilized in the "LearningWorldTreeView.razor". Changes made here should be reflected there as well.
    /// </summary>
    /// <param name="objectInPathWayViewModels">The collection of path objects used to determine the order.</param>
    /// <returns>A sorted list of learning spaces based on the provided paths.</returns>
    private static List<LearningSpacePe> GetLearningSpacesInOrder(
        IEnumerable<IObjectInPathWayPe> objectInPathWayViewModels)
    {
        var objectInPathWayList = objectInPathWayViewModels.ToList();
        var startObjects = objectInPathWayList
            .Where(x => x.InBoundObjects.Count == 0 && x is LearningSpacePe && x.OutBoundObjects.Count > 0).ToList();

        var visited = new HashSet<IObjectInPathWayPe>();
        var pathOrder = new List<LearningSpacePe>();
        var queue = new Queue<IObjectInPathWayPe>();

        foreach (var startObject in startObjects)
        {
            queue.Enqueue(startObject);
        }

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            if (visited.Contains(current))
                continue;

            visited.Add(current);

            if (current is LearningSpacePe learningSpace)
            {
                pathOrder.Add(learningSpace);
            }

            foreach (var nextObject in current.OutBoundObjects.OrderBy(o =>
                         o is LearningSpacePe lsvm ? lsvm.Name : string.Empty))
            {
                queue.Enqueue(nextObject);
            }
        }

        var spacesWithOutPaths = objectInPathWayList
            .Where(x => x.InBoundObjects.Count == 0 && x is LearningSpacePe && x.OutBoundObjects.Count == 0)
            .Cast<LearningSpacePe>().ToList();

        pathOrder.AddRange(spacesWithOutPaths);
        return pathOrder;
    }

    /// <summary>
    /// Maps the elements of the given learning space to their respective IDs, generating a sequential list. 
    /// If an element doesn't exist for a particular slot, a null is added to the list.
    /// </summary>
    /// <param name="space">The learning space whose elements are to be mapped to their IDs.</param>
    /// <param name="learningSpaceId">The ID of the learning space being processed.</param>
    /// <returns>A sequential list of element IDs for the learning space, with nulls for missing elements.</returns>
    private List<int?> MapLearningSpaceElementsToIds(LearningSpacePe space, int learningSpaceId)
    {
        var maxSlotNumber = GetMaxSlotNumber(space);
        var listLearningSpaceElements = new List<int?>();

        for (var i = 0; i <= maxSlotNumber; i++)
        {
            if (space.LearningSpaceLayout.LearningElements.TryGetValue(i, out var element))
            {
                var elementId = _dictionaryIdToLearningElement.First(x => x.Value == element).Key;
                MapInternalLearningElementToLearningWorldJson(learningSpaceId, elementId, element);
                listLearningSpaceElements.Add(elementId);
            }
            else
            {
                listLearningSpaceElements.Add(null);
            }
        }

        return listLearningSpaceElements;
    }

    /// <summary>
    /// Determines the maximum slot number for a given learning space based on its floor plan name.
    /// </summary>
    /// <param name="space">The learning space for which the maximum slot number is to be determined.</param>
    /// <returns>The maximum slot number.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the floor plan name is not supported.</exception>
    private static int GetMaxSlotNumber(LearningSpacePe space)
    {
        var maxSlotNumber = space.LearningSpaceLayout.FloorPlanName switch
        {
            FloorPlanEnum.R_20X20_6L => 5,
            FloorPlanEnum.R_20X30_8L => 7,
            FloorPlanEnum.L_32X31_10L => 9,
            _ => throw new ArgumentOutOfRangeException(nameof(space.LearningSpaceLayout.FloorPlanName),
                $"The FloorPlanName {space.LearningSpaceLayout.FloorPlanName} of space {space.Name} is not supported")
        };
        return maxSlotNumber;
    }

    /// <summary>
    /// Maps a learning element to the learning elements of LearningWorldJson.
    /// </summary>
    /// <param name="learningSpaceId">The ID of the learning space associated with the learning element.</param>
    /// <param name="learningElementId">The ID of the learning element.</param>
    /// <param name="learningElement">The learning element to be mapped.</param>
    private void MapInternalLearningElementToLearningWorldJson(int learningSpaceId, int learningElementId,
        ILearningElementPe learningElement)
    {
        IElementJson elementJson;

        switch (learningElement.LearningContent)
        {
            case FileContentPe fileContentPe:
                var elementCategory = MapFileContentToElementCategory(fileContentPe);
                elementJson = new LearningElementJson(learningElementId, learningElement.Id.ToString(),
                    learningElement.Name,
                    elementCategory, fileContentPe.Type, learningSpaceId, learningElement.Points,
                    learningElement.ElementModel.ToString(),
                    learningElement.Description, learningElement.Goals.Split("\n"));
                ListFileContent.Add((fileContentPe, learningElement.Name));
                break;
            case LinkContentPe linkContentPe:
                elementJson = new LearningElementJson(learningElementId, learningElement.Id.ToString(),
                    learningElement.Name,
                    linkContentPe.Link,
                    "video", "url", learningSpaceId, learningElement.Points, learningElement.ElementModel.ToString(),
                    learningElement.Description, learningElement.Goals.Split("\n"));
                break;
            case AdaptivityContentPe adaptivityContentPe:
                var adaptivityContent = MapAdaptivityContentPeToJson(adaptivityContentPe);
                elementJson = new AdaptivityElementJson(learningElementId, learningElement.Id.ToString(),
                    learningElement.Name,
                    "adaptivity", "adaptivity", learningSpaceId, learningElement.Points,
                    learningElement.ElementModel.ToString(),
                    adaptivityContent, learningElement.Description, learningElement.Goals.Split("\n"));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(learningElement.LearningContent),
                    $"The given LearningContent of element {learningElement.Name} is neither FileContent, LinkContent or AdaptivityContent");
        }

        LearningWorldJson.Elements.Add(elementJson);
    }

    /// <summary>
    /// Maps the content of a file to its corresponding category.
    /// </summary>
    /// <param name="fileContent">The file content to be examined.</param>
    /// <returns>The category of the element based on the file type. </returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private static string MapFileContentToElementCategory(IFileContentPe fileContent)
    {
        return fileContent.Type switch
        {
            { } type when FileContentIsTextType(type) => "text",
            { } type when FileContentIsImageType(type) => "image",
            "h5p" => "h5p",
            "pdf" => "pdf",
            _ => throw new ArgumentOutOfRangeException(nameof(fileContent.Type),
                $"The given LearningContent Type of file {fileContent.Name} is not supported")
        };
    }

    /// <summary>
    /// Determines whether a given file content type is an image.
    /// </summary>
    private static bool FileContentIsImageType(string type) =>
        new[] { "png", "jpg", "bmp", "webp", "jpeg" }.Contains(type);

    /// <summary>
    /// Determines whether a given file content type is text.
    /// </summary>
    private static bool FileContentIsTextType(string type) =>
        new[] { "txt", "c", "h", "cpp", "cc", "c++", "py", "js", "php", "html", "css" }.Contains(type);

    /// <summary>
    /// Maps an adaptivity content object to its corresponding JSON representation in the ATF.
    /// </summary>
    /// <param name="adaptivityContent">The adaptivity content to be converted.</param>
    /// <returns>Returns the JSON representation of the adaptivity content.</returns>
    private IAdaptivityContentJson MapAdaptivityContentPeToJson(IAdaptivityContentPe adaptivityContent)
    {
        var adaptivityTasks = MapAdaptivityTasksPeToJson(adaptivityContent.Tasks);
        return new AdaptivityContentJson(adaptivityContent.Name, adaptivityTasks);
    }

    /// <summary>
    /// Maps a collection of adaptivity content tasks to their corresponding JSON representations in the ATF.
    /// </summary>
    /// <param name="adaptivityContentTasks">The adaptivity content tasks to be converted.</param>
    /// <returns>
    /// Returns a list of JSON representations of the adaptivity content tasks.
    /// </returns>
    private List<IAdaptivityTaskJson> MapAdaptivityTasksPeToJson(ICollection<IAdaptivityTaskPe> adaptivityContentTasks)
    {
        var adaptivityTasks = new List<IAdaptivityTaskJson>();
        var taskId = 1;
        foreach (var task in adaptivityContentTasks)
        {
            var optional = task.MinimumRequiredDifficulty == null;
            var adaptivityQuestions = MapAdaptivityQuestionsPeToJson(task.Questions);
            adaptivityTasks.Add(new AdaptivityTaskJson(taskId, task.Id.ToString(), task.Name, optional,
                MapRequiredQuestionDifficultyToInt(task.MinimumRequiredDifficulty), adaptivityQuestions));
            taskId++;
        }

        return adaptivityTasks;
    }

    /// <summary>
    /// Maps a collection of task questions to their corresponding JSON representations in the ATF.
    /// </summary>
    /// <param name="taskQuestions">The task questions to be converted.</param>
    /// <returns>
    /// Returns a list of JSON representations of the task questions.
    /// </returns>
    private List<IAdaptivityQuestionJson> MapAdaptivityQuestionsPeToJson(
        ICollection<IAdaptivityQuestionPe> taskQuestions)
    {
        var adaptivityQuestions = new List<IAdaptivityQuestionJson>();
        foreach (var question in taskQuestions)
        {
            var questionType = question switch
            {
                MultipleChoiceSingleResponseQuestionPe => ResponseType.singleResponse,
                MultipleChoiceMultipleResponseQuestionPe => ResponseType.multipleResponse,
                _ => throw new ArgumentOutOfRangeException($"ResponseType of Question {question.Id} is not supported")
            };
            var adaptivityRules = MapAdaptivityRulesPeToJson(question.Rules);
            var choices = MapChoicesPeToJson((IMultipleChoiceQuestionPe)question);
            var questionDifficulty = MapRequiredQuestionDifficultyToInt(question.Difficulty);
            if (questionDifficulty == null)
            {
                throw new ArgumentOutOfRangeException($"Question {question.Id} has no difficulty");
            }

            adaptivityQuestions.Add(new AdaptivityQuestionJson(questionType, _questionId, question.Id.ToString(),
                (int)questionDifficulty, ((IMultipleChoiceQuestionPe)question).Text,
                adaptivityRules, choices));
            _questionId++;
        }

        return adaptivityQuestions;
    }

    /// <summary>
    /// Maps a collection of question rules to their corresponding JSON representations in the ATF.
    /// </summary>
    /// <param name="questionRules">The question rules to be converted.</param>
    /// <returns>
    /// Returns a list of JSON representations of the question rules.
    /// </returns>
    private List<IAdaptivityRuleJson> MapAdaptivityRulesPeToJson(ICollection<IAdaptivityRulePe> questionRules)
    {
        var adaptivityRules = new List<IAdaptivityRuleJson>();
        var triggerId = 1;
        foreach (var rule in questionRules)
        {
            var adaptivityAction = MapAdaptivityActionPeToJson(rule.Action);
            adaptivityRules.Add(new AdaptivityRuleJson(triggerId, "incorrect", adaptivityAction));
            triggerId++;
        }

        return adaptivityRules;
    }

    /// <summary>
    /// Maps an <see cref="IAdaptivityActionPe"/> object to its corresponding JSON representation.
    /// </summary>
    /// <param name="adaptivityAction">The adaptivity action to map.</param>
    /// <returns>An object that implements the <see cref="IAdaptivityActionJson"/> interface.</returns>
    /// <exception cref="NotImplementedException">Thrown when attempting to map an unsupported <see cref="ContentReferenceActionPe"/> type.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the adaptivity action type is not supported.</exception>
    private IAdaptivityActionJson MapAdaptivityActionPeToJson(IAdaptivityActionPe adaptivityAction)
    {
        switch (adaptivityAction)
        {
            case CommentActionPe commentActionPe:
                return new CommentActionJson(commentActionPe.Comment);
            case ContentReferenceActionPe contentReferenceActionPe:
                MapContentToBaseLearningElement(contentReferenceActionPe);
                return new ContentReferenceActionJson(_dictionaryIdToContentReferenceAction
                        .First(x => x.Value.Id == contentReferenceActionPe.Id).Key,
                    contentReferenceActionPe.Comment == "" ? null : contentReferenceActionPe.Comment);
            case ElementReferenceActionPe elementReferenceActionPe:
                return new ElementReferenceActionJson(_dictionaryIdToLearningElement
                        .First(x => x.Value.Id == elementReferenceActionPe.ElementId).Key,
                    elementReferenceActionPe.Comment == "" ? null : elementReferenceActionPe.Comment);
            default:
                throw new ArgumentOutOfRangeException(nameof(adaptivityAction),
                    $"The adaptivityAction {adaptivityAction} is not supported");
        }
    }

    /// <summary>
    /// Maps the provided <see cref="ContentReferenceActionPe"/> content to its corresponding <see cref="BaseLearningElementJson"/> representation in the ATF
    /// and adds it to the LearningWorldJson.Elements collection.
    /// </summary>
    /// <param name="contentReferenceAction">The content reference action containing the content to map.</param>
    /// <exception cref="ArgumentException">Thrown when trying to reference from <see cref="AdaptivityContentPe"/> to <see cref="AdaptivityContentPe"/>, as this is not supported.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the content type within <paramref name="contentReferenceAction"/> is not supported.</exception>
    private void MapContentToBaseLearningElement(ContentReferenceActionPe contentReferenceAction)
    {
        BaseLearningElementJson baseLearningElement;
        var elementId = _dictionaryIdToContentReferenceAction.First(x => x.Value.Id == contentReferenceAction.Id).Key;
        switch (contentReferenceAction.Content)
        {
            case FileContentPe fileContentPe:
                baseLearningElement = new BaseLearningElementJson(elementId,
                    contentReferenceAction.Id.ToString(), fileContentPe.Name, "",
                    MapFileContentToElementCategory(fileContentPe), fileContentPe.Type);
                ListFileContent.Add((fileContentPe, fileContentPe.Name));
                break;
            case LinkContentPe linkContentPe:
                baseLearningElement = new BaseLearningElementJson(elementId, contentReferenceAction.Id.ToString(),
                    linkContentPe.Name, linkContentPe.Link, "video", "url");
                break;
            case AdaptivityContentPe:
                throw new ArgumentException("Reference from AdaptivityContent to AdaptivityContent is not supported");
            default:
                throw new ArgumentOutOfRangeException(nameof(contentReferenceAction));
        }

        LearningWorldJson.Elements.Add(baseLearningElement);
    }

    /// <summary>
    /// Maps a multiple choice question to its corresponding JSON representations of choices in the ATF.
    /// </summary>
    /// <param name="question">The multiple choice question.</param>
    /// <returns>
    /// Returns a list of JSON representations of the choices for the question.
    /// </returns>
    private List<IChoiceJson> MapChoicesPeToJson(IMultipleChoiceQuestionPe question)
    {
        return (from choice in question.Choices
            let isCorrect = question.CorrectChoices.Contains(choice)
            select new ChoiceJson(choice.Text, isCorrect)).Cast<IChoiceJson>().ToList();
    }

    /// <summary>
    /// Converts a question difficulty to its corresponding integer representation in the ATF.
    /// </summary>
    private static int? MapRequiredQuestionDifficultyToInt(QuestionDifficulty? questionDifficulty)
    {
        return questionDifficulty switch
        {
            QuestionDifficulty.Easy => 000,
            QuestionDifficulty.Medium => 100,
            QuestionDifficulty.Hard => 200,
            null => null,
            _ => throw new ArgumentOutOfRangeException(nameof(questionDifficulty),
                questionDifficulty, null)
        };
    }

    /// <summary>
    /// Creates learning space requirements
    /// If the inbound-type is not a PathWayCondition there can only be 1 LearningSpacePe,
    /// so we do not have to construct a boolean algebra expression.
    /// </summary>
    /// <param name="space">The learning space to get the requirements for.</param>
    /// <returns>A string that contains the booleanAlgebraRequirements for the learning space.</returns>
    private string GetRequiredSpacesToEnter(LearningSpacePe space)
    {
        var booleanAlgebraRequirements = "";
        if (space.InBoundObjects.Count <= 0) return booleanAlgebraRequirements;
        foreach (var inbound in space.InBoundObjects)
        {
            if (inbound is PathWayConditionPe curCondition)
            {
                booleanAlgebraRequirements = DefineLogicalExpression(curCondition);
            }
            else
            {
                booleanAlgebraRequirements = (DictionarySpaceIdToGuid.Where(x => x.Value == inbound.Id)
                    .Select(x => x.Key)
                    .FirstOrDefault().ToString());
            }
        }

        return booleanAlgebraRequirements;
    }

    /// <summary>
    /// Assigns a topic to a learning space based on the space's assigned topic name.
    /// </summary>
    /// <param name="space">The learning space object which might have an assigned topic.</param>
    /// <param name="learningSpaceId">The ID of the learning space, which will be added to the topic's content list if an associated topic is found.</param>
    private void AssignTopicToSpace(LearningSpacePe space, int learningSpaceId)
    {
        if (space.AssignedTopic == null) return;
        var assignedTopic = LearningWorldJson.Topics.Find(topic => topic.TopicName == space.AssignedTopic.Name);
        assignedTopic?.TopicContents.Add(learningSpaceId);
    }

    /// <summary>
    /// Creates a new <see cref="DocumentRootJson"/> object using the class properties.
    /// </summary>
    /// <returns></returns>
    private IDocumentRootJson CreateRootJson()
    {
        return new DocumentRootJson(Constants.AtfVersion, Constants.ApplicationVersion, _author, _language,
            LearningWorldJson);
    }

    /// <summary>
    /// Serializes a given <see cref="DocumentRootJson"/> object to a JSON string with specific formatting options.
    /// </summary>
    /// <param name="rootJson">The <see cref="DocumentRootJson"/> object to serialize.</param>
    /// <returns>A formatted JSON string representation of the provided <see cref="DocumentRootJson"/>.</returns>
    private static string SerializeRootJson(IDocumentRootJson rootJson)
    {
        var options = new JsonSerializerOptions
            { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        return JsonSerializer.Serialize(rootJson, options);
    }

    /// <summary>
    /// Sets up the directory structure required for XML file exports and ATF document generation.
    /// </summary>
    private void SetupDirectoryStructure()
    {
        var currentDirectory = _fileSystem.Directory.GetCurrentDirectory();
        _xmlFilesForExportPath = _fileSystem.Path.Join(currentDirectory, "XMLFilesForExport");
        _atfPath = _fileSystem.Path.Join(currentDirectory, "XMLFilesForExport", "ATF_Document.json");

        if (_fileSystem.Directory.Exists(_xmlFilesForExportPath))
        {
            _fileSystem.Directory.Delete(_xmlFilesForExportPath, true);
        }

        var createFolders = new BackupFileGenerator(_fileSystem);
        createFolders.CreateBackupFolders();
    }

    /// <summary>
    /// All LearningElements are created at the specified location = Easier access to files in further Export-Operations.
    /// After the files are added to the Backup-Structure, these Files will be deleted.
    /// </summary>
    /// <exception cref="FileNotFoundException">Thrown when the content could not be found at the specified path.</exception>
    private void CopyLearningElementFiles()
    {
        // The "name" attribute in ListFileContent refers to the learning element's name, if present, or the file name if no corresponding learning element exists.
        foreach (var (fileContent, name) in ListFileContent)
        {
            try
            {
                _fileSystem.File.Copy(fileContent.Filepath,
                    _fileSystem.Path.Join("XMLFilesForExport", $"{name}.{fileContent.Type}"));
                Logger.LogTrace("Copied file from {Filepath} to XMLFilesForExport", fileContent.Filepath);
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException(
                    $"The Content {fileContent.Name} could not be found at Path {fileContent.Filepath}.");
            }
        }
    }

    /// <summary>
    /// Writes the provided JSON file content to the designated ATF path and logs the operation.
    /// </summary>
    /// <param name="jsonFile">The JSON content to be written to the file.</param>
    private void WriteAtfToFile(string jsonFile)
    {
        _fileSystem.File.WriteAllText(_atfPath, jsonFile);
        Logger.LogTrace("Generated ATF Document at {Path} with LearningWorld {World} ({WorldId)}", _atfPath,
            LearningWorldJson.WorldName, LearningWorldJson.WorldUUID);
    }
}