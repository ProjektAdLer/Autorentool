using Presentation.PresentationLogic.LearningSpace;

namespace Presentation.PresentationLogic.LearningPathway;

public interface ILearningPathWayViewModel
{
    ILearningSpaceViewModel SourceSpace { get; set; }
    ILearningSpaceViewModel TargetSpace { get; set; }
}