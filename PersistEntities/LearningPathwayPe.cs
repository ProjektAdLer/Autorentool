namespace PersistEntities;

public class LearningPathwayPe: ILearningPathWayPe
{
    private LearningPathwayPe()
    {
        SourceSpace = null;
        TargetSpace = null;
    }
    
    public LearningPathwayPe(LearningSpacePe sourceSpace, LearningSpacePe targetSpace)
    {
        SourceSpace = sourceSpace;
        TargetSpace = targetSpace;
    }
    
    public LearningSpacePe SourceSpace { get; set; }
    public LearningSpacePe TargetSpace { get; set; }
}