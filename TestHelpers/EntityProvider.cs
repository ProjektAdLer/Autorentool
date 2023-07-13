using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using Shared;

namespace TestHelpers;

public static class EntityProvider
{
    public static AuthoringToolWorkspace GetAuthoringToolWorkspace(List<ILearningWorld>? worlds = null)
    {
        return new AuthoringToolWorkspace(worlds ?? new List<ILearningWorld>());
    }

    public static LearningWorld GetLearningWorld(bool unsavedChanges = false, string append = "")
    {
        return new LearningWorld("a" + append, "b" + append, "c" + append, "d" + append, "e" + append, "f" + append)
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
        return new SavedLearningWorldPath()
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
}