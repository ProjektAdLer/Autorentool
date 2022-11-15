using JetBrains.Annotations;

namespace PersistEntities;

public class PathWayConditionPe : IObjectInPathWayPe
{
    /// <summary>
    /// Constructor for Automapper. DO NOT USE.
    /// </summary>
    [UsedImplicitly]
    private PathWayConditionPe()
    {
        Id = Guid.Empty;
        PositionX = 0;
        PositionY = 0;
        InBoundObjects = new List<IObjectInPathWayPe>();
        OutBoundObjects = new List<IObjectInPathWayPe>();
        Condition = ConditionEnumPe.None;
    }
    
    
    public PathWayConditionPe(ConditionEnumPe condition, double positionX = 0, double positionY = 0,
        List<IObjectInPathWayPe>? inBoundObjects = null, List<IObjectInPathWayPe>? outBoundObjects = null)
    {
        Id = Guid.NewGuid();
        Condition = condition;
        InBoundObjects = inBoundObjects ?? new List<IObjectInPathWayPe>();
        OutBoundObjects = outBoundObjects ?? new List<IObjectInPathWayPe>();
        PositionX = positionX;
        PositionY = positionY;
    }
    
    public Guid Id { get; set; }
    public double PositionX { get; set; }
    public double PositionY { get; set; }
    public List<IObjectInPathWayPe> InBoundObjects { get; set; }
    public List<IObjectInPathWayPe> OutBoundObjects { get; set; }
    public ConditionEnumPe Condition { get; set; }
}