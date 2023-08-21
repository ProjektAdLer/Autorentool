using Presentation.PresentationLogic.AdvancedLearningSpaceEditor.AdvancedComponent;
using Presentation.PresentationLogic.AdvancedLearningSpaceEditor.AdvancedLayout;
using Presentation.PresentationLogic.LearningSpace;

namespace Presentation.PresentationLogic.AdvancedLearningSpaceEditor.AdvancedLearningSpace;

public interface IAdvancedLearningSpaceViewModel : IObjectInPathWayViewModel, ILearningSpaceViewModel
{
    IEnumerable<IAdvancedLearningElementSlotViewModel> ContainedAdvancedLearningElementSlots { get; }
    IAdvancedLearningSpaceLayoutViewModel AdvancedLearningSpaceLayout { get; set; }
}
    