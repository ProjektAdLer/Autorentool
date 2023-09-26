using System.Diagnostics.CodeAnalysis;
using System.IO.Abstractions;
using System.Text;
using System.Text.Json;
using Generator.DSL.AdaptivityElement;
using Generator.WorldExport;
using Microsoft.Extensions.Logging;
using PersistEntities;
using PersistEntities.LearningContent;
using PersistEntities.LearningContent.Action;
using PersistEntities.LearningContent.Question;
using Shared;
using Shared.Adaptivity;
using Shared.Extensions;

namespace Generator.DSL;

public class CreateDsl : ICreateDsl
{
    private const string AtfVersion = "0.4";
    private readonly IFileSystem _fileSystem;
    private string _author;
    private string _booleanAlgebraRequirements;
    private string _dslPath;
    private string _language;
    private List<ILearningElementPe> _listAllLearningElements;
    private List<int?> _listLearningSpaceElements;
    private string _xmlFilesForExportPath;
    public Dictionary<int, Guid> DictionarySpaceIdToGuid;
    public List<ILearningElementPe> ElementsWithFileContent;
    public LearningWorldJson LearningWorldJson;
    public List<LearningSpacePe> ListLearningSpaces;
    public List<TopicPe> ListTopics;
    public string Uuid;

    /// <summary>
    /// Read the PersistEntities and create a Dsl Document with a specified syntax.
    /// </summary>
    /// <param name="fileSystem"></param>
    /// <param name="logger"></param>
    public CreateDsl(IFileSystem fileSystem, ILogger<CreateDsl> logger)
    {
        Initialize();
        _fileSystem = fileSystem;
        Logger = logger;
        _author = "";
        _dslPath = "";
        _language = "";
        _xmlFilesForExportPath = "";
    }

    private ILogger<CreateDsl> Logger { get; }

    /// <inheritdoc cref="ICreateDsl.GenerateAndExportLearningWorldJson"/>
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

        WriteDslToFile(jsonFile);
        return _dslPath;
    }

    /// <summary>
    /// Searches for duplicate learning element names in a list of learning spaces. If duplicates are found, their names are incremented.
    /// </summary>
    /// <param name="listLearningSpace">The list of LearningSpacePe objects to search.</param>
    /// <returns>A list of LearningSpacePe objects with incremented names for duplicates.</returns>
    public static List<LearningSpacePe> IncrementDuplicateLearningElementNames(List<LearningSpacePe> listLearningSpace)
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
    public string DefineLogicalExpression(PathWayConditionPe pathWayCondition)
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

    [MemberNotNull(nameof(ElementsWithFileContent), nameof(ListLearningSpaces), nameof(ListTopics),
        nameof(_listLearningSpaceElements), nameof(_booleanAlgebraRequirements), nameof(DictionarySpaceIdToGuid),
        nameof(Uuid), nameof(_listAllLearningElements), nameof(LearningWorldJson))]
    private void Initialize()
    {
        ElementsWithFileContent = new List<ILearningElementPe>();
        ListLearningSpaces = new List<LearningSpacePe>();
        ListTopics = new List<TopicPe>();
        _listLearningSpaceElements = new List<int?>();
        _booleanAlgebraRequirements = "";
        DictionarySpaceIdToGuid = new Dictionary<int, Guid>();
        var guid = Guid.NewGuid();
        Uuid = guid.ToString();
        _listAllLearningElements = new List<ILearningElementPe>();
        LearningWorldJson = new LearningWorldJson("", "", new List<TopicJson>(), new List<LearningSpaceJson>(),
            new List<IElementJson>(), "", Array.Empty<string>());
    }

    private void InitializeLearningWorldProperties(LearningWorldPe learningWorld)
    {
        //Setting Authors and Language for DSL Root
        _author = learningWorld.Authors;
        _language = "de";
        LearningWorldJson.WorldName = learningWorld.Name;
        LearningWorldJson.WorldUUID = learningWorld.Id.ToString();
        LearningWorldJson.WorldDescription = learningWorld.Description;
        LearningWorldJson.WorldGoals = learningWorld.Goals.Split("\n");
        LearningWorldJson.EvaluationLink = learningWorld.EvaluationLink;
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
        // Starting Value for Learning Space Ids and Learning Element Ids in the DSL-Document
        var learningSpaceId = 1;
        var learningElementId = 0;

        ListLearningSpaces.AddRange(GetLearningSpacesInOrder(objectInPathWays));

        DictionarySpaceIdToGuid = ListLearningSpaces.Select((space, index) => new { index, space.Id })
            .ToDictionary(x => x.index + 1, x => x.Id);

        ListLearningSpaces = IncrementDuplicateLearningElementNames(ListLearningSpaces);

        foreach (var space in ListLearningSpaces)
        {
            _listLearningSpaceElements =
                GenerateElementIdsForLearningSpace(space, ref learningElementId, learningSpaceId);
            _booleanAlgebraRequirements = GetRequiredSpacesToEnter(space);

            AssignTopicToSpace(space, learningSpaceId);

            LearningWorldJson.Spaces.Add(new LearningSpaceJson(learningSpaceId, space.Id.ToString(),
                space.Name, _listLearningSpaceElements,
                space.RequiredPoints, space.LearningSpaceLayout.FloorPlanName.ToString(), space.Theme.ToString(),
                space.Description, space.Goals.Split("\n"), _booleanAlgebraRequirements));

            learningSpaceId++;
        }
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
    /// Generates a list of element IDs for the provided learning space, incrementing the learning element ID as needed.
    /// </summary>
    /// <param name="space">The learning space for which element IDs should be generated.</param>
    /// <param name="learningElementId">Reference to the current learning element ID. This will be incremented during the method's execution.</param>
    /// <param name="learningSpaceId">The ID of the learning space being processed.</param>
    /// <returns>A list of element IDs for the learning space.</returns>
    private List<int?> GenerateElementIdsForLearningSpace(LearningSpacePe space, ref int learningElementId,
        int learningSpaceId)
    {
        var maxSlotNumber = GetMaxSlotNumber(space);
        var listLearningSpaceElements = new List<int?>();

        for (var i = 0; i <= maxSlotNumber; i++)
        {
            if (space.LearningSpaceLayout.LearningElements.TryGetValue(i, out var element))
            {
                CreateAndStoreLearningElementData(ref learningElementId, learningSpaceId, element,
                    listLearningSpaceElements);
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
    /// Creates the necessary data for a learning element and stores it in the provided list.
    /// </summary>
    /// <param name="learningElementId">Reference to the current learning element ID. This will be incremented during the method's execution.</param>
    /// <param name="learningSpaceId">The ID of the learning space associated with the learning element.</param>
    /// <param name="element">The learning element for which data should be created and stored.</param>
    /// <param name="listLearningSpaceElements">The list in which to store the generated element ID.</param>
    private void CreateAndStoreLearningElementData(ref int learningElementId, int learningSpaceId,
        ILearningElementPe element, List<int?> listLearningSpaceElements)
    {
        learningElementId++;
        IElementJson elementJson;

        switch (element.LearningContent)
        {
            case FileContentPe fileContentPe:
                var elementCategory = MapFileContentToElementCategory(fileContentPe);
                elementJson = new LearningElementJson(learningElementId, element.Id.ToString(), element.Name,
                    elementCategory, fileContentPe.Type, learningSpaceId, element.Points,
                    element.ElementModel.ToString(),
                    element.Description, element.Goals.Split("\n"));
                ElementsWithFileContent.Add(element);
                break;
            case LinkContentPe linkContentPe:
                elementJson = new LearningElementJson(learningElementId, element.Id.ToString(), element.Name,
                    linkContentPe.Link,
                    "video", "url", learningSpaceId, element.Points, element.ElementModel.ToString(),
                    element.Description, element.Goals.Split("\n"));
                break;
            case AdaptivityContentPe adaptivityContentPe:
                var adaptivityContent = MapAdaptivityContentPeToJson(adaptivityContentPe);
                elementJson = new AdaptivityElementJson(learningElementId, element.Id.ToString(), element.Name,
                    "adaptivity", "adaptivity", learningSpaceId, element.Points, element.ElementModel.ToString(),
                    adaptivityContent, element.Description, element.Goals.Split("\n"));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(element.LearningContent),
                    $"The given LearningContent of element {element.Name} is neither FileContent, LinkContent or AdaptivityContent");
        }

        listLearningSpaceElements.Add(learningElementId);
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
            { } type when FileContentIsTextType(type) => "image",
            { } type when FileContentIsImageType(type) => "text",
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
        return new AdaptivityContentJson(adaptivityContent.Name, false, adaptivityTasks);
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
                MapRequiredTaskDifficultyToInt(task.MinimumRequiredDifficulty), adaptivityQuestions));
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
        var questionId = 1;
        foreach (var question in taskQuestions)
        {
            var questionType = question switch
            {
                MultipleChoiceSingleResponseQuestionPe => "singleResponse",
                MultipleChoiceMultipleResponseQuestionPe => "multipleResponse",
                _ => ""
            };
            var adaptivityRules = MapAdaptivityRulesPeToJson(question.Rules);
            var choices = MapChoicesPeToJson((IMultipleChoiceQuestionPe)question);
            adaptivityQuestions.Add(new AdaptivityQuestionJson(questionType, questionId, question.Id.ToString(),
                MapRequiredQuestionDifficultyToInt(question.Difficulty), ((IMultipleChoiceQuestionPe)question).Text,
                adaptivityRules, choices));
            questionId++;
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
            adaptivityRules.Add(new AdaptivityRuleJson(triggerId, "incorrect",
                new CommentActionJson(((CommentActionPe)rule.Action).Comment)));
            triggerId++;
        }

        return adaptivityRules;
    }

    // <summary>
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
    private static int MapRequiredQuestionDifficultyToInt(QuestionDifficulty questionDifficulty)
    {
        return questionDifficulty switch
        {
            QuestionDifficulty.Easy => 0,
            QuestionDifficulty.Medium => 1,
            QuestionDifficulty.Hard => 2,
            _ => throw new ArgumentOutOfRangeException(nameof(questionDifficulty),
                questionDifficulty, null)
        };
    }

    /// <summary>
    /// Converts a task's minimum required difficulty to its corresponding integer representation in the ATF.
    /// </summary>
    private static int MapRequiredTaskDifficultyToInt(QuestionDifficulty? taskMinimumRequiredDifficulty)
    {
        return taskMinimumRequiredDifficulty switch
        {
            QuestionDifficulty.Easy => 000,
            QuestionDifficulty.Medium => 100,
            QuestionDifficulty.Hard => 200,
            null => 000,
            _ => throw new ArgumentOutOfRangeException(nameof(taskMinimumRequiredDifficulty),
                taskMinimumRequiredDifficulty, null)
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
    private DocumentRootJson CreateRootJson()
    {
        return new DocumentRootJson(AtfVersion, Constants.ApplicationVersion, _author, _language,
            LearningWorldJson);
    }

    /// <summary>
    /// Serializes a given <see cref="DocumentRootJson"/> object to a JSON string with specific formatting options.
    /// </summary>
    /// <param name="rootJson">The <see cref="DocumentRootJson"/> object to serialize.</param>
    /// <returns>A formatted JSON string representation of the provided <see cref="DocumentRootJson"/>.</returns>
    private static string SerializeRootJson(DocumentRootJson rootJson)
    {
        var options = new JsonSerializerOptions
            { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        return JsonSerializer.Serialize(rootJson, options);
    }

    /// <summary>
    /// Sets up the directory structure required for XML file exports and DSL document generation.
    /// </summary>
    private void SetupDirectoryStructure()
    {
        var currentDirectory = _fileSystem.Directory.GetCurrentDirectory();
        _xmlFilesForExportPath = _fileSystem.Path.Join(currentDirectory, "XMLFilesForExport");
        _dslPath = _fileSystem.Path.Join(currentDirectory, "XMLFilesForExport", "DSL_Document.json");

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
    /// <exception cref="FileNotFoundException">Thrown when the content of a learning element could not be found at the specified path.</exception>
    private void CopyLearningElementFiles()
    {
        foreach (var learningElement in ElementsWithFileContent)
        {
            try
            {
                //we know that all elements in this list have a FileContent, so we can safely cast it. - n.stich
                var castedFileContent = (FileContentPe)learningElement.LearningContent;
                _fileSystem.File.Copy(castedFileContent.Filepath,
                    _fileSystem.Path.Join("XMLFilesForExport", $"{learningElement.Name}.{castedFileContent.Type}"));
                Logger.LogTrace("Copied file from {Filepath} to XMLFilesForExport", castedFileContent.Filepath);
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException(
                    $"The Content {learningElement.LearningContent.Name} of the LearningElement {learningElement.Name} could not be found at Path {((FileContentPe)learningElement.LearningContent).Filepath}.");
            }
        }
    }

    /// <summary>
    /// Writes the provided JSON file content to the designated DSL path and logs the operation.
    /// </summary>
    /// <param name="jsonFile">The JSON content to be written to the file.</param>
    private void WriteDslToFile(string jsonFile)
    {
        _fileSystem.File.WriteAllText(_dslPath, jsonFile);
        Logger.LogTrace("Generated DSL Document: {JsonFile} at {Path}", jsonFile, _dslPath);
    }
}