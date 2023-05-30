using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningPathway;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningSpace.SpaceLayout;
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
        return new LearningWorldViewModel("LWVMn", "LWVMsn", "LWVMa", "LWVMl", "LWVMd", "LWVMg");
    }

    public static LearningSpaceViewModel GetLearningSpace(bool unsavedChanges = false, FloorPlanEnum? floorPlan = null)
    {
        return new LearningSpaceViewModel("LSVMn", "LSVMd", "LSVMg", Theme.Campus,4,
                floorPlan == null ? null : GetLearningSpaceLayout((FloorPlanEnum) floorPlan))
            {UnsavedChanges = unsavedChanges};
    }

    public static LearningSpaceLayoutViewModel GetLearningSpaceLayout(FloorPlanEnum floorPlan = FloorPlanEnum.R_20X20_6L)
    {
        return new LearningSpaceLayoutViewModel(floorPlan);
    }
    
    public static LearningElementViewModel GetLearningElement()
    {
        return new LearningElementViewModel("LEVMn", null!, "LEVMd", "LEVMg", LearningElementDifficultyEnum.Easy);
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
        return new LinkContentViewModel("LCVMn a name", "LCVMl a link");
    }

    public static FileContentViewModel GetFileContent()
    {
        return new FileContentViewModel("FCVMn a name", "FCVMt a type", "FCVMf a filepath");
    }

    public static TopicViewModel GetTopic()
    {
        return new TopicViewModel("TVMn a topic");
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