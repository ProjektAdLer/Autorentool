namespace BusinessLogic.Entities;

internal interface IPathway : ISelectableObjectInWorld
{
    IObjectInPathWay SourceObject { get; set; }
    IObjectInPathWay TargetObject { get; set; }
}