using BusinessLogic.Entities;

namespace BusinessLogic.Commands.World;

public class DeleteWorld : IUndoCommand
{
    internal AuthoringToolWorkspace AuthoringToolWorkspace { get; }
    internal Entities.World World { get; }
    private Action<AuthoringToolWorkspace> MappingAction { get; }
    private IMemento? Memento { get; set; }

    public DeleteWorld(AuthoringToolWorkspace authoringToolWorkspace, Entities.World world,
        Action<AuthoringToolWorkspace> mappingAction)
    {
        AuthoringToolWorkspace = authoringToolWorkspace;
        World = world;
        MappingAction = mappingAction;
    }

    public void Execute()
    {
        Memento = AuthoringToolWorkspace.GetMemento();

        var realWorld = AuthoringToolWorkspace.Worlds.First(lw => lw.Id == World.Id);

        AuthoringToolWorkspace.Worlds.Remove(realWorld);
        AuthoringToolWorkspace.SelectedWorld = AuthoringToolWorkspace.Worlds.LastOrDefault();

        MappingAction.Invoke(AuthoringToolWorkspace);
    }

    public void Undo()
    {
        if (Memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }

        AuthoringToolWorkspace.RestoreMemento(Memento);

        MappingAction.Invoke(AuthoringToolWorkspace);
    }

    public void Redo() => Execute();
}