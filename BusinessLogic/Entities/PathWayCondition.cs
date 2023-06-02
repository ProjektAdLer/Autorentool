using JetBrains.Annotations;
using Shared;

namespace BusinessLogic.Entities;

public class PathWayCondition : IObjectInPathWay
{
    /// <summary>
    /// Constructor for Automapper. DO NOT USE.
    /// </summary>
    [UsedImplicitly]
    private PathWayCondition()
    {
        Id = Guid.NewGuid();
        PositionX = 0;
        PositionY = 0;
        InBoundObjects = new List<IObjectInPathWay>();
        OutBoundObjects = new List<IObjectInPathWay>();
        Condition = ConditionEnum.Or;
    }
    
    
    public PathWayCondition(ConditionEnum condition, double positionX = 0, double positionY = 0,
        List<IObjectInPathWay>? inBoundObjects = null, List<IObjectInPathWay>? outBoundObjects = null)
    {
        Id = Guid.NewGuid();
        Condition = condition;
        InBoundObjects = inBoundObjects ?? new List<IObjectInPathWay>();
        OutBoundObjects = outBoundObjects ?? new List<IObjectInPathWay>();
        PositionX = positionX;
        PositionY = positionY;
        UnsavedChanges = true;
    }
    
    public Guid Id { get; set; }
    public double PositionX { get; set; }
    public double PositionY { get; set; }
    public bool UnsavedChanges { get; set; }
    public List<IObjectInPathWay> InBoundObjects { get; set; }
    public List<IObjectInPathWay> OutBoundObjects { get; set; }
    public ConditionEnum Condition { get; set; }
    public IMemento GetMemento()
    {
        return new PathWayConditionMemento(Condition, InBoundObjects, OutBoundObjects, unsavedChanges: UnsavedChanges, positionX: PositionX, positionY: PositionY);
    }

    public void RestoreMemento(IMemento memento)
    {
        if (memento is not PathWayConditionMemento pathWayConditionMemento)
        {
            throw new ArgumentException("Incorrect IMemento implementation", nameof(memento));
        }
        
        Condition = pathWayConditionMemento.Condition;
        InBoundObjects = pathWayConditionMemento.InBoundObjects;
        OutBoundObjects = pathWayConditionMemento.OutBoundObjects;
        PositionX = pathWayConditionMemento.PositionX;
        PositionY = pathWayConditionMemento.PositionY;
        UnsavedChanges = pathWayConditionMemento.UnsavedChanges;
    }
    
    private record PathWayConditionMemento : IMemento
    {
        internal PathWayConditionMemento(ConditionEnum condition, List<IObjectInPathWay> inBoundObjects,
            List<IObjectInPathWay> outBoundObjects, bool unsavedChanges, double positionX = 0, double positionY = 0)
        {
            Condition = condition;
            InBoundObjects = inBoundObjects;
            OutBoundObjects = outBoundObjects;
            PositionX = positionX;
            PositionY = positionY;
            UnsavedChanges = unsavedChanges;
        }
        public double PositionX { get; }
        public double PositionY { get; }
        public bool UnsavedChanges { get; }
        public List<IObjectInPathWay> InBoundObjects { get; }
        public List<IObjectInPathWay> OutBoundObjects { get; }
        public ConditionEnum Condition { get; }
    }
}