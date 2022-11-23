namespace PersistEntities;

public interface IObjectInPathWayPe
{
    new Guid Id { get; }
    double PositionX { get; set; }
    double PositionY { get; set; }
    List<IObjectInPathWayPe> InBoundObjects { get; set; }
    List<IObjectInPathWayPe> OutBoundObjects { get; set; }
}