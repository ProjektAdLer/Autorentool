using Presentation.PresentationLogic.AdvancedLearningSpaceEditor.AdvancedComponent;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.Topic;
using Shared;

namespace Presentation.PresentationLogic.AdvancedLearningSpaceEditor.AdvancedLearningSpace;

public interface IAdvancedLearningSpaceViewModel : IObjectInPathWayViewModel
{
    IEnumerable<ILearningElementViewModel> ContainedLearningElements { get; }
    IEnumerable<IAdvancedLearningElementSlotViewModel> ContainedAdvancedLearningElementSlots { get; }
    TopicViewModel? AssignedTopic { get; set; }
    int Workload { get; }
    int Points { get; }
    int RequiredPoints { get; }
    Theme Theme { get; set; }
    bool AdvancedMode { get; }
    string Name { get; set; }
    string Description { get; set; }
    string Goals { get; set; }
    bool InternalUnsavedChanges { get; }
}
    