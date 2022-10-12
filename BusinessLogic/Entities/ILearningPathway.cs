namespace BusinessLogic.Entities;

internal interface ILearningPathway
{
    LearningSpace SourceSpace { get; set; }
    LearningSpace TargetSpace { get; set; }
}