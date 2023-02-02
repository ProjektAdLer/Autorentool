using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Condition;

public class DeletePathWayCondition : IUndoCommand
{
    internal Entities.World World { get; }
    internal PathWayCondition PathWayCondition { get; }
    private readonly Action<Entities.World> _mappingAction;
    private IMemento? _memento;

    public DeletePathWayCondition(Entities.World world, PathWayCondition pathWayCondition,
        Action<Entities.World> mappingAction)
    {
        World = world;
        PathWayCondition = pathWayCondition;
        _mappingAction = mappingAction;
    }

    public void Execute()
    {
        _memento = World.GetMemento();

        var pathWayCondition = World.PathWayConditions.First(x => x.Id == PathWayCondition.Id);

        foreach (var inBoundSpace in pathWayCondition.InBoundObjects)
        {
            World.Pathways
                .Where(x => x.SourceObject.Id == inBoundSpace.Id && x.TargetObject.Id == pathWayCondition.Id)
                .ToList().ForEach(x => World.Pathways.Remove(x));
        }
        foreach (var outBoundSpace in pathWayCondition.OutBoundObjects)
        {
            World.Pathways
                .Where(x => x.SourceObject.Id == pathWayCondition.Id && x.TargetObject.Id == outBoundSpace.Id)
                .ToList().ForEach(x => World.Pathways.Remove(x));
        }
        World.PathWayConditions.Remove(pathWayCondition);

        if (pathWayCondition == World.SelectedObject || World.SelectedObject == null)
        {
            World.SelectedObject = World.SelectableWorldObjects.LastOrDefault();
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