namespace BusinessLogic.Entities;

internal interface ILearningPathway : ISelectableObjectInWorld
{
    IObjectInPathWay SourceObject { get; set; }
    IObjectInPathWay TargetObject { get; set; }
}