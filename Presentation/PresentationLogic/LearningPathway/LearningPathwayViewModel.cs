using JetBrains.Annotations;
using Presentation.PresentationLogic.LearningSpace;

namespace Presentation.PresentationLogic.LearningPathway;

public class LearningPathwayViewModel : ILearningPathWayViewModel
{
    private LearningPathwayViewModel()
    {
        SourceSpace = null;
        TargetSpace = null;
    }
    
    public LearningPathwayViewModel(ILearningSpaceViewModel sourceSpace, ILearningSpaceViewModel targetSpace)
    {
        SourceSpace = sourceSpace;
        TargetSpace = targetSpace;
    }

    public ILearningSpaceViewModel SourceSpace { get; set; }
    public ILearningSpaceViewModel TargetSpace { get; set; }
}