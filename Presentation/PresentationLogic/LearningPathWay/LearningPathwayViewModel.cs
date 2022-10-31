namespace Presentation.PresentationLogic.LearningPathway;

public class LearningPathwayViewModel : ILearningPathWayViewModel
{
    protected LearningPathwayViewModel()
    {
        Id = Guid.Empty;
        //We override nullability here because constructor is protected, only called by AutoMapper and field immediately
        //set by AutoMapper afterwards - m.ho
        SourceObject = null!;
        TargetObject = null!;
    }
    
    public LearningPathwayViewModel(IObjectInPathWayViewModel sourceSpace, IObjectInPathWayViewModel targetSpace)
    {
        SourceObject = sourceSpace;
        TargetObject = targetSpace;
        Id = new Guid();
    }

    public IObjectInPathWayViewModel SourceObject { get; set; }
    public IObjectInPathWayViewModel TargetObject { get; set; }
    public Guid Id { get; private set; }
}