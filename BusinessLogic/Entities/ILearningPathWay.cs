namespace BusinessLogic.Entities;

internal interface ILearningPathWay
{
    LearningSpace SourceSpace { get; set; }
    LearningSpace TargetSpace { get; set; }
}