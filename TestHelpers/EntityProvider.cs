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

    public static LearningSpaceLayout GetLearningSpaceLayout(FloorPlanEnum floorPlan = FloorPlanEnum.R_20X20_6L)
    {
        return new LearningSpaceLayout(new Dictionary<int, ILearningElement>(), floorPlan);
    }

    public static LearningSpaceLayout GetLearningSpaceLayoutWithElement()
    {
        return new LearningSpaceLayout(new Dictionary<int, ILearningElement> {{1, GetLearningElement()}},
            FloorPlanEnum.R_20X20_6L);
    }

    public static LearningElement GetLearningElement(bool unsavedChanges = false, string append = "",
        ILearningSpace? parent = null, double positionX = 0, double positionY = 0)
    {
        return new LearningElement("a" + append, null!, "d" + append, "e" + append,
            LearningElementDifficultyEnum.Easy, ElementModel.L_H5P_SPIELAUTOMAT_1, parent: parent, positionX: positionX,
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

    public static FileContent GetFileContent()
    {
        return new FileContent("a name", "a type", "a filepath");
    }

    public static Topic GetTopic()
    {
        return new Topic("a topic");
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
}