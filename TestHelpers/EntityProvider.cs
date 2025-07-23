using System.Globalization;
using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using BusinessLogic.Entities.LearningContent.Adaptivity;
using BusinessLogic.Entities.LearningContent.Adaptivity.Action;
using BusinessLogic.Entities.LearningContent.Adaptivity.Question;
using BusinessLogic.Entities.LearningContent.Adaptivity.Trigger;
using BusinessLogic.Entities.LearningContent.FileContent;
using BusinessLogic.Entities.LearningContent.LinkContent;
using BusinessLogic.Entities.LearningContent.Story;
using BusinessLogic.Entities.LearningOutcome;
using Shared;
using Shared.Adaptivity;
using Shared.LearningOutcomes;
using Shared.Theme;

namespace TestHelpers;

public static class EntityProvider
{
    public static AuthoringToolWorkspace GetAuthoringToolWorkspace(List<ILearningWorld>? worlds = null)
    {
        return new AuthoringToolWorkspace(worlds ?? new List<ILearningWorld>());
    }

    public static LearningWorld GetLearningWorld(bool unsavedChanges = false, string append = "")
    {
        return new LearningWorld("a" + append, "b" + append, "c" + append, "d" + append, "e" + append, "f" + append,
                WorldTheme.CampusAschaffenburg, "g" + append, "h" + append, "i" + append, "j" + append, "k" + append,
                "l" + append)
            { UnsavedChanges = unsavedChanges };
    }

    public static LearningSpace GetLearningSpace(bool unsavedChanges = false, FloorPlanEnum? floorPlan = null,
        Topic? assignedTopic = null)
    {
        return new LearningSpace("a", "d", 4, SpaceTheme.LearningArea, GetLearningOutcomeCollection(),
                floorPlan == null ? null : GetLearningSpaceLayout((FloorPlanEnum)floorPlan))
            { UnsavedChanges = unsavedChanges, AssignedTopic = assignedTopic };
    }

    public static LearningOutcomeCollection GetLearningOutcomeCollection(ILearningOutcome? outcome = null,
        bool unsavedChanges = false)
    {
        return new LearningOutcomeCollection()
        {
            LearningOutcomes = GetLearningOutcomes(outcome),
            UnsavedChanges = unsavedChanges
        };
    }

    public static List<ILearningOutcome> GetLearningOutcomes(ILearningOutcome? outcome = null)
    {
        return new List<ILearningOutcome>()
        {
            new ManualLearningOutcome("ManualOutcome"),
            new StructuredLearningOutcome(TaxonomyLevel.Level1, "What", "Whereby", "WhatFor", "VerbOfVisibility",
                CultureInfo.CurrentCulture),
            outcome ?? new ManualLearningOutcome("ManualOutcome2")
        };
    }

    public static LearningSpaceLayout GetLearningSpaceLayout(FloorPlanEnum floorPlan = FloorPlanEnum.R_20X20_6L,
        IDictionary<int, ILearningElement>? learningElements = null,
        IDictionary<int, ILearningElement>? storyElements = null)
    {
        learningElements ??= new Dictionary<int, ILearningElement>();
        storyElements ??= new Dictionary<int, ILearningElement>();
        return new LearningSpaceLayout(learningElements, storyElements, floorPlan);
    }

    public static LearningSpaceLayout GetLearningSpaceLayoutWithElement()
    {
        return new LearningSpaceLayout(new Dictionary<int, ILearningElement> { { 1, GetLearningElement() } },
            new Dictionary<int, ILearningElement>(),
            FloorPlanEnum.R_20X20_6L);
    }

    public static LearningElement GetLearningElement(bool unsavedChanges = false, ILearningContent? content = null,
        string append = "", ElementModel elementModel = ElementModel.l_h5p_slotmachine_1,
        ILearningSpace? parent = null, double positionX = 0, double positionY = 0)
    {
        content ??= GetLinkContent(unsavedChanges);
        return new LearningElement("a" + append, content, "d" + append, "e" + append,
            LearningElementDifficultyEnum.Easy, elementModel, parent: parent, positionX: positionX,
            positionY: positionY) { UnsavedChanges = unsavedChanges };
    }

    public static LearningElement GetStoryElement(bool unsavedChanges = false, ILearningContent? content = null,
        string append = "", ElementModel elementModel = ElementModel.l_h5p_slotmachine_1,
        ILearningSpace? parent = null, double positionX = 0, double positionY = 0)
    {
        content ??= GetStoryContent("a" + append);
        return new LearningElement("a" + append, content, "d" + append, "e" + append,
            LearningElementDifficultyEnum.Easy, elementModel, parent: parent, positionX: positionX,
            positionY: positionY) { UnsavedChanges = unsavedChanges };
    }

    public static PathWayCondition GetPathWayCondition()
    {
        return new PathWayCondition(ConditionEnum.And);
    }

    public static LearningPathway GetLearningPathway(IObjectInPathWay? source = null, IObjectInPathWay? target = null)
    {
        return new LearningPathway(source ?? GetPathWayCondition(), target ?? GetPathWayCondition());
    }

    public static LinkContent GetLinkContent(bool unsavedChanges = false)
    {
        return new LinkContent("a name", "a link")
        {
            UnsavedChanges = unsavedChanges
        };
    }

    public static FileContent GetFileContent(string append = "", bool unsavedChanges = false)
    {
        return new FileContent("a name" + append, "a type" + append, "a filepath" + append)
            { UnsavedChanges = unsavedChanges };
    }

    public static StoryContent GetStoryContent(string name = "a name", List<string>? storyText = null,
        bool unsavedChanges = false, string npcName = "a npc name", NpcMood npcMood = NpcMood.Welcome)
    {
        storyText ??= new List<string> { "a story" };
        return new StoryContent(name, unsavedChanges, storyText, npcName, npcMood);
    }

    public static Topic GetTopic(string append = "")
    {
        return new Topic("a topic" + append);
    }

    public static LearningWorld GetLearningWorldWithSpace()
    {
        var world = GetLearningWorld();
        world.LearningSpaces.Add(GetLearningSpace());
        return world;
    }

    public static LearningWorld GetLearningWorldWithElement()
    {
        var world = GetLearningWorld();
        var element = GetLearningElement();
        world.UnplacedLearningElements.Add(element);
        return world;
    }

    public static LearningSpace GetLearningSpaceWithElement()
    {
        var space = GetLearningSpace();
        var element = GetLearningElement();
        element.Parent = space;
        space.LearningSpaceLayout.LearningElements.Add(0, element);
        return space;
    }

    public static LearningWorld GetLearningWorldWithSpaceWithElement()
    {
        var world = GetLearningWorld();
        var space = GetLearningSpaceWithElement();
        world.LearningSpaces.Add(space);
        return world;
    }

    public static TEntity Get<TEntity>() where TEntity : class =>
        (typeof(TEntity).Name switch
        {
            nameof(LearningWorld) => GetLearningWorld() as TEntity,
            nameof(LearningSpace) => GetLearningSpace() as TEntity,
            nameof(LearningElement) => GetLearningElement() as TEntity,
            nameof(LinkContent) => GetLinkContent() as TEntity,
            nameof(IMultipleChoiceQuestion) => GetMultipleChoiceMultipleResponseQuestion() as TEntity,
            _ => throw new ArgumentOutOfRangeException()
        })!;

    public static ContentReferenceAction GetContentReferenceAction(ILearningContent? content = null,
        string comment = "")
    {
        content ??= GetLinkContent();
        return new ContentReferenceAction(content, comment);
    }

    public static AdaptivityContent GetAdaptivityContent()
    {
        var tasks = new List<IAdaptivityTask> { GetAdaptivityTask() };
        return new AdaptivityContent(tasks);
    }

    public static AdaptivityContent GetAdaptivityContentFullStructure()
    {
        var task = GetAdaptivityTask();
        var question = task.Questions.ElementAt(0);
        question.Rules.Clear();
        question.Rules.Add(GetAdaptivityRule(GetCompositeTrigger(), GetCommentAction()));
        question.Rules.Add(GetAdaptivityRule(GetTimeTrigger(), GetContentReferenceAction()));
        question.Rules.Add(GetAdaptivityRule(GetCorrectnessTrigger(), GetElementReferenceAction()));
        var tasks = new List<IAdaptivityTask> { task };
        return new AdaptivityContent(tasks);
    }

    public static AdaptivityTask GetAdaptivityTask()
    {
        var questions = new List<IAdaptivityQuestion> { GetAdaptivityQuestion() };
        return new AdaptivityTask(questions, QuestionDifficulty.Hard, "taskname");
    }

    public static IAdaptivityRule GetAdaptivityRule(IAdaptivityTrigger? trigger = null,
        IAdaptivityAction? action = null)
    {
        trigger ??= GetAdaptivityTrigger();
        action ??= GetAdaptivityAction();
        return new AdaptivityRule(trigger, action);
    }

    private static IAdaptivityAction GetAdaptivityAction()
    {
        return new CommentAction("comment");
    }

    private static IAdaptivityTrigger GetAdaptivityTrigger()
    {
        return GetCorrectnessTrigger();
    }

    private static CorrectnessTrigger GetCorrectnessTrigger()
    {
        return new CorrectnessTrigger(AnswerResult.Correct);
    }

    private static TimeTrigger GetTimeTrigger()
    {
        return new TimeTrigger(123, TimeFrameType.Until);
    }

    private static CompositeTrigger GetCompositeTrigger()
    {
        return new CompositeTrigger(ConditionEnum.And, GetTimeTrigger(), GetCorrectnessTrigger());
    }

    public static IAdaptivityQuestion GetAdaptivityQuestion()
    {
        var choices = new List<Choice> { GetAdaptivityChoice() };
        var rules = new List<IAdaptivityRule> { GetAdaptivityRule() };
        return new MultipleChoiceSingleResponseQuestion(123, choices, "questionText", choices[0],
            QuestionDifficulty.Easy, rules);
    }

    public static MultipleChoiceMultipleResponseQuestion GetMultipleChoiceMultipleResponseQuestion()
    {
        var choices = new List<Choice> { GetAdaptivityChoice() };
        var rules = new List<IAdaptivityRule> { GetAdaptivityRule() };
        return new MultipleChoiceMultipleResponseQuestion(123, choices, choices, rules, "questionText",
            QuestionDifficulty.Easy);
    }

    public static MultipleChoiceSingleResponseQuestion GetMultipleChoiceSingleResponseQuestion()
    {
        var choices = new List<Choice> { GetAdaptivityChoice() };
        var rules = new List<IAdaptivityRule> { GetAdaptivityRule() };
        return new MultipleChoiceSingleResponseQuestion(123, choices, "questionText", choices[0],
            QuestionDifficulty.Easy, rules);
    }

    private static Choice GetAdaptivityChoice()
    {
        return new Choice("a choice");
    }

    public static ILearningElement GetLearningElement(IAdaptivityContent content)
    {
        return new LearningElement("name", content, "description", "goals", LearningElementDifficultyEnum.Easy,
            ElementModel.l_h5p_deskpc_1);
    }

    public static CommentAction GetCommentAction()
    {
        return new CommentAction("default comment");
    }

    public static ElementReferenceAction GetElementReferenceAction(Guid? guid = null, string message = "")
    {
        guid ??= Guid.NewGuid();
        return new ElementReferenceAction(guid.Value, message);
    }
}