using PersistEntities;
using PersistEntities.LearningContent;
using Shared;

namespace TestHelpers;

public static class PersistEntityProvider
{
    public static LearningWorldPe GetLearningWorld(string append = "", List<LearningSpacePe>? learningSpaces = null)
    {
        return new LearningWorldPe("LWPn" + append, "LWPsn" + append, "LWPa" + append, "LWPl" + append, "LWPd" + append,
            "LWPg" + append, "LWPev" + append, "LWPsp" + append, learningSpaces: learningSpaces);
    }

    public static LearningSpacePe GetLearningSpace(string append = "", FloorPlanEnum? floorPlan = null,
        LearningSpaceLayoutPe? learningSpaceLayout = null, double positionX = 0, double positionY = 0,
        List<IObjectInPathWayPe>? inBoundObjects = null,
        List<IObjectInPathWayPe>? outBoundObjects = null, TopicPe? assignedTopic = null, string name = "")
    {
        return new LearningSpacePe(name != "" ? name : "LSPn" + append, "LSPd" + append, "LSPg" + append, 4,
            Theme.Campus,
            learningSpaceLayout ?? (floorPlan == null ? null : GetLearningSpaceLayout((FloorPlanEnum)floorPlan)),
            positionX, positionY, inBoundObjects, outBoundObjects, assignedTopic);
    }

    public static LearningSpaceLayoutPe GetLearningSpaceLayout(FloorPlanEnum floorPlan = FloorPlanEnum.R_20X20_6L,
        Dictionary<int, ILearningElementPe>? learningElements = null)
    {
        return new LearningSpaceLayoutPe(learningElements ?? new Dictionary<int, ILearningElementPe>(), floorPlan);
    }

    public static LearningElementPe GetLearningElement(string append = "", ILearningContentPe? content = null,
        string name = "")
    {
        return new LearningElementPe(name != "" ? name : "a" + append, content, "d" + append, "e" + append,
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

    public static FileContentPe GetFileContent(string? name = null, string? type = null, string? filepath = null)
    {
        return new FileContentPe(name ?? "a name", type ?? "a type", filepath ?? "a filepath");
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
}