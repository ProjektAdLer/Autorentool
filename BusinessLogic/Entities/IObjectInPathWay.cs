namespace BusinessLogic.Entities;

public interface IObjectInPathWay : IOriginator, ISelectableObjectInWorld
{
    new Guid Id { get; }
    double PositionX { get; set; }
    double PositionY { get; set; }
    bool UnsavedChanges { get; set; }
    List<IObjectInPathWay> InBoundObjects { get; set; }
    List<IObjectInPathWay> OutBoundObjects { get; set; }
}