using System.Globalization;
using PersistEntities;
using PersistEntities.LearningContent;
using PersistEntities.LearningContent.Action;
using PersistEntities.LearningContent.Question;
using PersistEntities.LearningContent.Story;
using PersistEntities.LearningContent.Trigger;
using PersistEntities.LearningOutcome;
using Shared;
using Shared.Adaptivity;
using Shared.LearningOutcomes;
using Shared.Theme;

namespace TestHelpers;

public static class PersistEntityProvider
{
    public static LearningWorldPe GetLearningWorld(string append = "", List<LearningSpacePe>? learningSpaces = null)
    {
        return new LearningWorldPe("LWPn" + append, "LWPsn" + append, "LWPa" + append, "LWPl" + append, "LWPd" + append,
            "LWPg" + append, WorldTheme.CampusAschaffenburg, "LWPev" + append, "LWPek" + append, "LWPss" + append, "LWPse" + append, "LWPsp" + append, learningSpaces: learningSpaces);
    }

    public static LearningSpacePe GetLearningSpace(string append = "", FloorPlanEnum? floorPlan = null,
        LearningSpaceLayoutPe? learningSpaceLayout = null, double positionX = 0, double positionY = 0,
        List<IObjectInPathWayPe>? inBoundObjects = null,
        List<IObjectInPathWayPe>? outBoundObjects = null, TopicPe? assignedTopic = null, string name = "")
    {
        return new LearningSpacePe(name != "" ? name : "LSPn" + append, "LSPd" + append, 4,
            SpaceTheme.LearningArea, GetLearningOutcomeCollection(),
            learningSpaceLayout ?? (floorPlan == null ? null : GetLearningSpaceLayout((FloorPlanEnum)floorPlan)),
            positionX: positionX, positionY: positionY, inBoundObjects: inBoundObjects,
            outBoundObjects: outBoundObjects, assignedTopic: assignedTopic);
    }

    public static LearningOutcomeCollectionPe GetLearningOutcomeCollection(
        List<ILearningOutcomePe>? learningOutcomes = null)
    {
        return new LearningOutcomeCollectionPe
        {
            LearningOutcomes = learningOutcomes ?? new List<ILearningOutcomePe>()
        };
    }

    public static List<ILearningOutcomePe> GetLearningOutcomes()
    {
        return new List<ILearningOutcomePe>()
        {
            new ManualLearningOutcomePe("Outcome"),
            new StructuredLearningOutcomePe(TaxonomyLevel.Level1, "what", "whereby", "whatFor", "verbOfVisibility",
                CultureInfo.CurrentCulture)
        };
    }

    public static LearningSpaceLayoutPe GetLearningSpaceLayout(FloorPlanEnum floorPlan = FloorPlanEnum.R_20X20_6L,
        Dictionary<int, ILearningElementPe>? learningElements = null,
        Dictionary<int, ILearningElementPe>? storyElements = null)
    {
        return new LearningSpaceLayoutPe(learningElements ?? new Dictionary<int, ILearningElementPe>(),
            storyElements ?? new Dictionary<int, ILearningElementPe>(), floorPlan);
    }

    public static LearningElementPe GetLearningElement(string append = "", ILearningContentPe? content = null,
        string name = "")
    {
        return new LearningElementPe(name != "" ? name : "a" + append, content!, "d" + append, "e" + append,
            LearningElementDifficultyEnum.Easy, ElementModel.l_h5p_slotmachine_1);
    }

    public static PathWayConditionPe GetPathWayCondition(ConditionEnum condition = ConditionEnum.And,
        double positionX = 0, double positionY = 0,
        List<IObjectInPathWayPe>? inBoundObjects = null, List<IObjectInPathWayPe>? outBoundObjects = null)
    {
        return new PathWayConditionPe(condition, positionX, positionY, inBoundObjects, outBoundObjects);
    }

    public static LearningPathwayPe GetLearningPathway(IObjectInPathWayPe? source = null,
        IObjectInPathWayPe? target = null)
    {
        return new LearningPathwayPe(source ?? GetPathWayCondition(), target ?? GetPathWayCondition());
    }

    public static LinkContentPe GetLinkContent(string? name = null, string? link = null)
    {
        return new LinkContentPe(name ?? "a name", link ?? "a link");
    }

    public static FileContentPe GetFileContent(string? name = null, string? type = null, string? filepath = null, bool primitiveH5p = false)
    {
        return new FileContentPe(name ?? "a name", type ?? "a type", filepath ?? "a filepath", primitiveH5p);
    }

    public static StoryContentPe GetStoryContent(string? name = null, bool unsavedChanges = false,
        List<string>? story = null, string npcName = "a npc name", NpcMood npcMood = NpcMood.Welcome)
    {
        return new StoryContentPe(name ?? "a name", unsavedChanges,
            story ?? new List<string> { "this is a story", "of a", "duck", "debugging", "a", "bug", "with quacks" }, npcName, NpcMood.Welcome);
    }

    public static TopicPe GetTopic(string? name = null)
    {
        return new TopicPe(name ?? "a topic");
    }

    public static LearningWorldPe GetLearningWorldWithSpace()
    {
        var world = GetLearningWorld();
        world.LearningSpaces.Add(GetLearningSpace());
        return world;
    }

    public static LearningWorldPe GetLearningWorldWithElement()
    {
        var world = GetLearningWorld();
        var element = GetLearningElement();
        world.UnplacedLearningElements.Add(element);
        return world;
    }

    public static LearningSpacePe GetLearningSpaceWithElement()
    {
        var space = GetLearningSpace();
        var element = GetLearningElement();
        space.LearningSpaceLayout.LearningElements.Add(0, element);
        return space;
    }

    public static LearningWorldPe GetLearningWorldWithSpaceWithElement()
    {
        var world = GetLearningWorld();
        var space = GetLearningSpaceWithElement();
        world.LearningSpaces.Add(space);
        return world;
    }

    public static IAdaptivityContentPe GetAdaptivityContent()
    {
        var tasks = new List<IAdaptivityTaskPe> { GetAdaptivityTask() };
        return new AdaptivityContentPe("foo", tasks);
    }

    public static IAdaptivityContentPe GetAdaptivityContentFullStructure()
    {
        var tasks = new List<IAdaptivityTaskPe>
            { GetAdaptivityTask(), GetAdaptivityTask(GetMultipleChoiceMultipleResponseAdaptivityQuestion()) };
        tasks[1].Questions.First().Rules.Add(GetAdaptivityRule(GetTimeTrigger(), GetContentReferenceAction()));
        tasks[1].Questions.First().Rules.Add(GetAdaptivityRule(GetCompositeTrigger(), GetElementReferenceAction()));
        return new AdaptivityContentPe("foo", tasks);
    }

    private static IAdaptivityRulePe GetAdaptivityRule(IAdaptivityTriggerPe? trigger = null,
        IAdaptivityActionPe? action = null)
    {
        trigger ??= GetCorrectnessTrigger();
        action ??= GetCommentAction();
        return new AdaptivityRulePe(trigger, action);
    }

    private static IAdaptivityActionPe GetCommentAction()
    {
        return new CommentActionPe("comment");
    }

    public static IAdaptivityActionPe GetContentReferenceAction(ILearningContentPe? fileContentPe = null,
        string comment = "")
    {
        fileContentPe ??= GetFileContent();
        return new ContentReferenceActionPe(fileContentPe, comment);
    }

    public static IAdaptivityActionPe GetElementReferenceAction(Guid? elementId = null, string comment = "")
    {
        elementId ??= Guid.NewGuid();
        return new ElementReferenceActionPe(elementId.Value, comment);
    }

    private static IAdaptivityTriggerPe GetCorrectnessTrigger()
    {
        return new CorrectnessTriggerPe(AnswerResult.Correct);
    }

    private static IAdaptivityTriggerPe GetTimeTrigger()
    {
        return new TimeTriggerPe(123, TimeFrameType.Until);
    }

    private static IAdaptivityTriggerPe GetCompositeTrigger()
    {
        return new CompositeTriggerPe(ConditionEnum.And, GetCorrectnessTrigger(), GetTimeTrigger());
    }

    private static IAdaptivityTaskPe GetAdaptivityTask(IAdaptivityQuestionPe? question = null)
    {
        question ??= GetMultipleChoiceSingleResponseAdaptivityQuestion();
        var questions = new List<IAdaptivityQuestionPe> { question };
        return new AdaptivityTaskPe(questions, QuestionDifficulty.Hard, "taskname");
    }

    private static IAdaptivityQuestionPe GetMultipleChoiceSingleResponseAdaptivityQuestion()
    {
        var choices = new List<ChoicePe> { GetAdaptivityChoice() };
        var rules = new List<IAdaptivityRulePe> { GetAdaptivityRule() };
        return new MultipleChoiceSingleResponseQuestionPe(123, choices, "questionText", choices[0],
            QuestionDifficulty.Easy, rules);
    }

    private static IAdaptivityQuestionPe GetMultipleChoiceMultipleResponseAdaptivityQuestion()
    {
        var choices = new List<ChoicePe> { GetAdaptivityChoice() };
        var rules = new List<IAdaptivityRulePe> { GetAdaptivityRule() };
        return new MultipleChoiceMultipleResponseQuestionPe(123, choices, choices, rules,
            "questionText", QuestionDifficulty.Easy);
    }

    private static ChoicePe GetAdaptivityChoice()
    {
        return new ChoicePe("a choice");
    }
}