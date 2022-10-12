using JetBrains.Annotations;

namespace BusinessLogic.Entities;

public class LearningPathway : ILearningPathway
{
    /// <summary>
    /// Constructor for Automapper. DO NOT USE.
    /// </summary>
    [UsedImplicitly]
    protected LearningPathway()
    {
        //We override nullability here because constructor is protected, only called by AutoMapper and field immediately
        //set by AutoMapper afterwards. - m.ho
        SourceSpace = null!;
        TargetSpace = null!;
    }
    
    public LearningPathway(LearningSpace sourceSpace, LearningSpace targetSpace)
    {
        SourceSpace = sourceSpace;
        TargetSpace = targetSpace;
    }
    
    public LearningSpace SourceSpace { get; set; }
    public LearningSpace TargetSpace { get; set; }
}