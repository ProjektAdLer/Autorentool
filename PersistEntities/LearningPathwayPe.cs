namespace PersistEntities;

public class LearningPathwayPe: ILearningPathWayPe
{
    protected LearningPathwayPe()
    {
        //We override nullability here because constructor is protected, only called by AutoMapper and field immediately
        //set by AutoMapper afterwards. - m.ho
        SourceSpace = null!;
        TargetSpace = null!;
    }
    
    public LearningPathwayPe(LearningSpacePe sourceSpace, LearningSpacePe targetSpace)
    {
        SourceSpace = sourceSpace;
        TargetSpace = targetSpace;
    }
    
    public LearningSpacePe SourceSpace { get; set; }
    public LearningSpacePe TargetSpace { get; set; }
}