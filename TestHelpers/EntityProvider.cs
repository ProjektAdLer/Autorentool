using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using BusinessLogic.Entities.LearningContent.Adaptivity;
using BusinessLogic.Entities.LearningContent.Adaptivity.Action;
using BusinessLogic.Entities.LearningContent.Adaptivity.Question;
using BusinessLogic.Entities.LearningContent.Adaptivity.Trigger;
using BusinessLogic.Entities.LearningContent.FileContent;
using BusinessLogic.Entities.LearningContent.LinkContent;
using Shared;
using Shared.Adaptivity;

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
                "g" + append)
            {UnsavedChanges = unsavedChanges};
    }

    public static LearningSpace GetLearningSpace(bool unsavedChanges = false, FloorPlanEnum? floorPlan = null,
        Topic? assignedTopic = null)
    {
        return new LearningSpace("a", "d", "e", 4, Theme.Campus,
                floorPlan == null ? null : GetLearningSpaceLayout((FloorPlanEnum) floorPlan))
            {UnsavedChanges = unsavedChanges, AssignedTopic = assignedTopic};
    }

    public static LearningSpaceLayout GetLearningSpaceLayout(FloorPlanEnum floorPlan = FloorPlanEnum.R_20X20_6L,
        IDictionary<int, ILearningElement>? learningElements = null)
    {
        return new LearningSpaceLayout(learningElements ?? new Dictionary<int, ILearningElement>(), floorPlan);
    }

    public static LearningSpaceLayout GetLearningSpaceLayoutWithElement()
    {
        return new LearningSpaceLayout(new Dictionary<int, ILearningElement> {{1, GetLearningElement()}},
            FloorPlanEnum.R_20X20_6L);
    }

    public static LearningElement GetLearningElement(bool unsavedChanges = false, ILearningContent? content = null,
        string append = "", ElementModel elementModel = ElementModel.l_h5p_slotmachine_1,
        ILearningSpace? parent = null, double positionX = 0, double positionY = 0)
    {
        return new LearningElement("a" + append, content!, "d" + append, "e" + append,
            LearningElementDifficultyEnum.Easy, elementModel, parent: parent, positionX: positionX,
            positionY: positionY) {UnsavedChanges = unsavedChanges};
    }

    public static PathWayCondition GetPathWayCondition()
    {
        return new PathWayCondition(ConditionEnum.And);
    }

    public static LearningPathway GetLearningPathway(IObjectInPathWay? source = null, IObjectInPathWay? target = null)
    {
        return new LearningPathway(source ?? GetPathWayCondition(), target ?? GetPathWayCondition());
    }

    public static LinkContent GetLinkContent()
    {
        return new LinkContent("a name", "a link");
    }

    public static FileContent GetFileContent(string append = "")
    {
        return new FileContent("a name" + append, "a type" + append, "a filepath" + append);
    }

    public static Topic GetTopic(string append = "")
    {
        return new Topic("a topic" + append);
    }

    public static SavedLearningWorldPath GetSavedLearningWorldPath()
    {
        return new SavedLearningWorldPath
            {Id = Guid.ParseExact("00000000-0000-0000-0000-000000000001", "D"), Name = "n1", Path = "p1"};
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
            _ => throw new ArgumentOutOfRangeException()
        })!;

    public static IAdaptivityAction GetContentReferenceAction()
    {
        return new ContentReferenceAction(GetLinkContent());
    }

    public static AdaptivityContent GetAdaptivityContent()
    {
        var tasks = new List<IAdaptivityTask> {GetAdaptivityTask()};
        return new AdaptivityContent(tasks);
    }

    public static AdaptivityTask GetAdaptivityTask()
    {
        var questions = new List<IAdaptivityQuestion> {GetAdaptivityQuestion()};
        return new AdaptivityTask(questions, QuestionDifficulty.Hard, "taskname");
    }

    private static IAdaptivityRule GetAdaptivityRule()
    {
        return new AdaptivityRule(GetAdaptivityTrigger(), GetAdaptivityAction());
    }

    private static IAdaptivityAction GetAdaptivityAction()
    {
        return new CommentAction("comment");
    }

    private static IAdaptivityTrigger GetAdaptivityTrigger()
    {
        return new CorrectnessTrigger(AnswerResult.Correct);
    }

    private static IAdaptivityQuestion GetAdaptivityQuestion()
    {
        var choices = new List<Choice> {GetAdaptivityChoice()};
        var rules = new List<IAdaptivityRule> {GetAdaptivityRule()};
        return new MultipleChoiceSingleResponseQuestion(123, choices, "questiontext", choices[0],
            QuestionDifficulty.Easy,
            rules);
    }

    public static MultipleChoiceMultipleResponseQuestion GetMultipleChoiceMultipleResponseQuestion()
    {
        var choices = new List<Choice> {GetAdaptivityChoice()};
        var rules = new List<IAdaptivityRule> {GetAdaptivityRule()};
        return new MultipleChoiceMultipleResponseQuestion(123, choices, choices, rules, "questiontext",
            QuestionDifficulty.Easy);
    }

    public static MultipleChoiceSingleResponseQuestion GetMultipleChoiceSingleResponseQuestion()
    {
        var choices = new List<Choice> {GetAdaptivityChoice()};
        var rules = new List<IAdaptivityRule> {GetAdaptivityRule()};
        return new MultipleChoiceSingleResponseQuestion(123, choices, "questiontext", choices[0],
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
}