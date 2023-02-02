using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Space;

public class DeleteSpace : IUndoCommand
{
    internal Entities.World World { get; }
    internal Entities.Space Space { get; }
    private readonly Action<Entities.World> _mappingAction;
    private IMemento? _memento;

    public DeleteSpace(Entities.World world, Entities.Space space,
        Action<Entities.World> mappingAction)
    {
        World = world;
        Space = space;
        _mappingAction = mappingAction;
    }

    public void Execute()
    {
        _memento = World.GetMemento();

        var space = World.Spaces.First(x => x.Id == Space.Id);

        foreach (var inBoundSpace in space.InBoundObjects)
        {
            World.Pathways
                .Where(x => x.SourceObject.Id == inBoundSpace.Id && x.TargetObject.Id == space.Id)
                .ToList().ForEach(x => World.Pathways.Remove(x));
        }
        foreach (var outBoundSpace in space.OutBoundObjects)
        {
            World.Pathways
                .Where(x => x.SourceObject.Id == space.Id && x.TargetObject.Id == outBoundSpace.Id)
                .ToList().ForEach(x => World.Pathways.Remove(x));
        }
        World.Spaces.Remove(space);

        if (space == World.SelectedObject || World.SelectedObject == null)
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