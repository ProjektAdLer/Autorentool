using BusinessLogic.Entities;
using Shared;

namespace BusinessLogic.Commands.Condition;

public class CreatePathWayCondition : IUndoCommand
{
    internal Entities.World World { get; }
    internal PathWayCondition PathWayCondition { get; }
    internal IObjectInPathWay? SourceObject { get; }
    internal Entities.Space? TargetObject { get; }
    private readonly Action<Entities.World> _mappingAction;
    private IMemento? _memento;

    public CreatePathWayCondition(Entities.World world, ConditionEnum condition, double positionX,
        double positionY, Action<Entities.World> mappingAction)
    {
        PathWayCondition = new PathWayCondition(condition, positionX, positionY);
        World = world;
        _mappingAction = mappingAction;
    }

    public CreatePathWayCondition(Entities.World world, ConditionEnum condition, IObjectInPathWay sourceObject,
        Entities.Space targetObject, Action<Entities.World> mappingAction)
    {
        World = world;
        SourceObject = World.ObjectsInPathWays.First(x => x.Id == sourceObject.Id);
        TargetObject = World.Spaces.First(x => x.Id == targetObject.Id);
        PathWayCondition = new PathWayCondition(condition, TargetObject.PositionX +42, TargetObject.PositionY - 60);
        _mappingAction = mappingAction;
    }

    public void Execute()
    {
        _memento = World.GetMemento();

        World.PathWayConditions.Add(PathWayCondition);
        World.SelectedObject = PathWayCondition;
        
        if (SourceObject != null && TargetObject != null)
        {
            var previousInBoundObject = TargetObject.InBoundObjects.FirstOrDefault();
            var previousPathWay = World.Pathways.FirstOrDefault(pw =>
                pw.TargetObject.Id == TargetObject.Id);
            if(previousPathWay == null)
                throw new ApplicationException("Previous pathway is null");
            if(previousInBoundObject == null)
                throw new ApplicationException("Previous in bound object is null");
                
            World.Pathways.Remove(previousPathWay);
            World.Pathways.Add(new Entities.Pathway(SourceObject, PathWayCondition));
            World.Pathways.Add(new Entities.Pathway(previousInBoundObject, PathWayCondition));
            World.Pathways.Add(new Entities.Pathway(PathWayCondition, TargetObject));
        }

        _mappingAction.Invoke(World);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }
        
        World.RestoreMemento(_memento);
        
        _mappingAction.Invoke(World);
    }

    public void Redo() => Execute();
}