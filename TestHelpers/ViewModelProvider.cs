using Presentation.PresentationLogic;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningContent.FileContent;
using Presentation.PresentationLogic.LearningContent.LinkContent;
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
        return new LearningWorldViewModel("LWVMn", "LWVMsn", "LWVMa", "LWVMl", "LWVMd", "LWVMg", "LWVMev");
    }

    public static LearningSpaceViewModel GetLearningSpace(bool unsavedChanges = false, FloorPlanEnum? floorPlan = null,
        TopicViewModel? assignedTopic = null, double positionX = 0, double positionY = 0)
    {
        return new LearningSpaceViewModel("LSVMn", "LSVMd", "LSVMg", Theme.Campus, 4,
            floorPlan == null ? null : GetLearningSpaceLayout((FloorPlanEnum)floorPlan), positionX: positionX,
            positionY: positionY) { UnsavedChanges = unsavedChanges, AssignedTopic = assignedTopic };
    }

    public static LearningSpaceLayoutViewModel GetLearningSpaceLayout(
        FloorPlanEnum floorPlan = FloorPlanEnum.R_20X20_6L)
    {
        return new LearningSpaceLayoutViewModel(floorPlan);
    }

    public static LearningElementViewModel GetLearningElement(string append = "",
        ILearningContentViewModel? content = null, ILearningSpaceViewModel? parent = null, int workload = 1,
        int points = 1)
    {
        return new LearningElementViewModel("LEVMn" + append, content!, "LEVMd" + append, "LEVMg" + append,
            LearningElementDifficultyEnum.Easy, ElementModel.l_h5p_slotmachine_1, parent: parent, workload: workload,
            points: points);
    }

    public static PathWayConditionViewModel GetPathWayCondition()
    {
        return new PathWayConditionViewModel(ConditionEnum.And, false);
    }

    public static LearningPathwayViewModel GetLearningPathway(IObjectInPathWayViewModel? source = null,
        IObjectInPathWayViewModel? target = null)
    {
        return new LearningPathwayViewModel(source ?? GetPathWayCondition(), target ?? GetPathWayCondition());
    }

    public static LinkContentViewModel GetLinkContent()
    {
        return new LinkContentViewModel("LCVMn a name", "LCVMl a link");
    }

    public static FileContentViewModel GetFileContent(string? name = null, string? type = null, string? filepath = null)
    {
        return new FileContentViewModel(name ?? "FCVMn a name", type ?? "FCVMt a type", filepath ?? "FCVMf a filepath");
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