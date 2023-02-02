using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Pathway;

public class DeletePathWay : IUndoCommand
{
    internal Entities.World World { get; }
    internal Entities.Pathway Pathway { get; }
    private readonly Action<Entities.World> _mappingAction;
    private IMemento? _memento;
    public DeletePathWay(Entities.World world, Entities.Pathway pathway,
        Action<Entities.World> mappingAction)
    {
        World = world;
        Pathway = world.Pathways.Single(x => x.Id == pathway.Id);
        _mappingAction = mappingAction;
    }
    public void Execute()
    {
        _memento = World.GetMemento();
        
        World.Pathways.Remove(Pathway);
        
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