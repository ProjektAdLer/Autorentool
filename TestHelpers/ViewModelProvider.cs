using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningPathway;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.PresentationLogic.Topic;
using Shared;

namespace TestHelpers;

public static class ViewModelProvider
{
    public static AuthoringToolWorkspaceViewModel GetAuthoringToolWorkspace()
    {
        return new AuthoringToolWorkspaceViewModel();
    }
    public static LearningWorldViewModel GetLearningWorld()
    {
        return new LearningWorldViewModel("a", "b", "c", "d", "e", "f");
    }

    public static LearningSpaceViewModel GetLearningSpace()
    {
        return new LearningSpaceViewModel("a", "d", "e", Theme.Campus);
    }

    public static LearningElementViewModel GetLearningElement()
    {
        return new LearningElementViewModel("a", null!, "d", "e", LearningElementDifficultyEnum.Easy);
    }
    
    public static PathWayConditionViewModel GetPathWayCondition()
    {
        return new PathWayConditionViewModel(ConditionEnum.And, false);
    }

    public static LearningPathwayViewModel GetLearningPathway()
    {
        return new LearningPathwayViewModel(GetPathWayCondition(), GetPathWayCondition());
    }

    public static LinkContentViewModel GetLinkContent()
    {
        return new LinkContentViewModel("a name", "a link");
    }

    public static FileContentViewModel GetFileContent()
    {
        return new FileContentViewModel("a name", "a type", "a filepath");
    }

    public static TopicViewModel GetTopic()
    {
        return new TopicViewModel("a topic");
    }

    public static LearningWorldViewModel GetLearningWorldWithSpace()
    {
        var world = GetLearningWorld();
        world.LearningSpaces.Add(GetLearningSpace());
        return world;
    }

    public static LearningWorldViewModel GetLearningWorldWithElement()
    {
        var world = GetLearningWorld();
        var element = GetLearningElement();
        world.UnplacedLearningElements.Add(element);
        return world;
    }

    public static LearningSpaceViewModel GetLearningSpaceWithElement()
    {
        var space = GetLearningSpace();
        var element = GetLearningElement();
        element.Parent = space;
        space.LearningSpaceLayout.LearningElements.Add(0, element);
        return space;
    }

    public static LearningWorldViewModel GetLearningWorldWithSpaceWithElement()
    {
        var world = GetLearningWorld();
        var space = GetLearningSpaceWithElement();
        world.LearningSpaces.Add(space);
        return world;
    }
}