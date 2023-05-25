using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace PersistEntities;


[Serializable]
[DataContract]
[KnownType(typeof(PathWayConditionPe))]
[KnownType(typeof(LearningSpacePe))]
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
    
    [IgnoreDataMember]
    public Guid Id { get; set; }
    [DataMember]
    public double PositionX { get; set; }
    [DataMember]
    public double PositionY { get; set; }
    [IgnoreDataMember]
    public List<IObjectInPathWayPe> InBoundObjects { get; set; }
    [IgnoreDataMember]
    public List<IObjectInPathWayPe> OutBoundObjects { get; set; }
    [DataMember]
    public ConditionEnumPe Condition { get; set; }
    
    [OnDeserializing]
    private void OnDeserializing(StreamingContext context)
    {
        Id = Guid.NewGuid();
        InBoundObjects = new List<IObjectInPathWayPe>();
        OutBoundObjects = new List<IObjectInPathWayPe>();
    }
}