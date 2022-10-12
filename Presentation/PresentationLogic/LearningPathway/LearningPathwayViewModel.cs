using Presentation.PresentationLogic.LearningSpace;

namespace Presentation.PresentationLogic.LearningPathway;

public class LearningPathwayViewModel : ILearningPathWayViewModel
{
    protected LearningPathwayViewModel()
    {
        //We override nullability here because constructor is protected, only called by AutoMapper and field immediately
        //set by AutoMapper afterwards - m.ho
        SourceSpace = null!;
        TargetSpace = null!;
    }
    
    public LearningPathwayViewModel(ILearningSpaceViewModel sourceSpace, ILearningSpaceViewModel targetSpace)
    {
        SourceSpace = sourceSpace;
        TargetSpace = targetSpace;
    }

    public ILearningSpaceViewModel SourceSpace { get; set; }
    public ILearningSpaceViewModel TargetSpace { get; set; }
}