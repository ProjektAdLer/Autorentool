using PersistEntities;
using PersistEntities.LearningContent;
using Shared;

namespace TestHelpers;

public static class PersistEntityProvider
{
    public static LearningWorldPe GetLearningWorld(string append = "")
    {
        return new LearningWorldPe("LWPn" + append, "LWPsn" + append, "LWPa" + append, "LWPl" + append, "LWPd" + append,
            "LWPg" + append, "LWPsp" + append);
    }

    public static LearningSpacePe GetLearningSpace(string append = "", FloorPlanEnum? floorPlan = null,
        double positionX = 0, double positionY = 0, List<IObjectInPathWayPe>? inBoundObjects = null,
        List<IObjectInPathWayPe>? outBoundObjects = null, TopicPe? assignedTopic = null)
    {
        return new LearningSpacePe("LSPn" + append, "LSPd" + append, "LSPg" + append, 4, Theme.Campus,
            floorPlan == null ? null : GetLearningSpaceLayout((FloorPlanEnum) floorPlan), positionX, positionY,
            inBoundObjects, outBoundObjects, assignedTopic);
    }

    public static LearningSpaceLayoutPe GetLearningSpaceLayout(FloorPlanEnum floorPlan = FloorPlanEnum.R_20X20_6L)
    {
        return new LearningSpaceLayoutPe(new Dictionary<int, ILearningElementPe>(), floorPlan);
    }

    public static LearningElementPe GetLearningElement(string append = "")
    {
        return new LearningElementPe("a" + append, null!, "d" + append, "e" + append,
            LearningElementDifficultyEnum.Easy);
    }

    public static PathWayConditionPe GetPathWayCondition(ConditionEnum condition = ConditionEnum.And, double positionX = 0, double positionY = 0,
        List<IObjectInPathWayPe>? inBoundObjects = null, List<IObjectInPathWayPe>? outBoundObjects = null)
    {
        return new PathWayConditionPe(condition, positionX, positionY, inBoundObjects, outBoundObjects);
    }

    public static LearningPathwayPe GetLearningPathway()
    {
        return new LearningPathwayPe(GetPathWayCondition(), GetPathWayCondition());
    }

    public static LinkContentPe GetLinkContent()
    {
        return new LinkContentPe("a name", "a link");
    }

    public static FileContentPe GetFileContent()
    {
        return new FileContentPe("a name", "a type", "a filepath");
    }

    public static TopicPe GetTopic()
    {
        return new TopicPe("a topic");
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
}