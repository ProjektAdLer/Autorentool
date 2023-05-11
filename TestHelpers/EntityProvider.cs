using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using Shared;

namespace TestHelpers;

public static class EntityProvider
{
    public static LearningWorld GetLearningWorld()
    {
        return new LearningWorld("a", "b", "c", "d", "e", "f") { };
    }

    public static LearningSpace GetLearningSpace()
    {
        return new LearningSpace("a", "d", "e", 4);
    }

    public static LearningElement GetLearningElement()
    {
        return new LearningElement("a", null!, "d", "e", LearningElementDifficultyEnum.Easy);
    }
    
    public static PathWayCondition GetPathWayCondition()
    {
        return new PathWayCondition(ConditionEnum.And);
    }

    public static LinkContent GetLinkContent()
    {
        return new LinkContent("a name", "a link");
    }

    public static FileContent GetFileContent()
    {
        return new FileContent("a name", "a type", "a filepath");
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