using JetBrains.Annotations;

namespace BusinessLogic.Entities;

public class LearningPathway: ILearningPathWay
{
    /// <summary>
    /// Constructor for Automapper. DO NOT USE.
    /// </summary>
    [UsedImplicitly]
    protected LearningPathway()
    {
        SourceSpace = null;
        TargetSpace = null;
    }
    
    public LearningPathway(LearningSpace sourceSpace, LearningSpace targetSpace)
    {
        SourceSpace = sourceSpace;
        TargetSpace = targetSpace;
    }
    
    public LearningSpace SourceSpace { get; set; }
    public LearningSpace TargetSpace { get; set; }
}