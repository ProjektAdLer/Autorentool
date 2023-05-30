using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using Shared;

namespace TestHelpers;

public static class EntityProvider
{
    public static AuthoringToolWorkspace GetAuthoringToolWorkspace()
    {
        return new AuthoringToolWorkspace(new List<ILearningWorld>());
    }

    public static LearningWorld GetLearningWorld(bool unsavedChanges = false, string append = "")
    {
        return new LearningWorld("a" + append, "b" + append, "c" + append, "d" + append, "e" + append, "f" + append)
            {UnsavedChanges = unsavedChanges};
    }

    public static LearningSpace GetLearningSpace(bool unsavedChanges = false, FloorPlanEnum? floorPlan = null)
    {
        return new LearningSpace("a", "d", "e", 4, Theme.Campus,
                floorPlan == null ? null : GetLearningSpaceLayout((FloorPlanEnum) floorPlan))
            {UnsavedChanges = unsavedChanges};
    }

    public static LearningSpaceLayout GetLearningSpaceLayout(FloorPlanEnum floorPlan = FloorPlanEnum.R_20X20_6L)
    {
        return new LearningSpaceLayout(new Dictionary<int, ILearningElement>(), floorPlan);
    }

    public static LearningElement GetLearningElement(bool unsavedChanges = false, string append = "")
    {
        return new LearningElement("a" + append, null!, "d" + append, "e" + append,
            LearningElementDifficultyEnum.Easy) {UnsavedChanges = unsavedChanges};
    }

    public static PathWayCondition GetPathWayCondition()
    {
        return new PathWayCondition(ConditionEnum.And);
    }

    public static LearningPathway GetLearningPathway()
    {
        return new LearningPathway(GetPathWayCondition(), GetPathWayCondition());
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