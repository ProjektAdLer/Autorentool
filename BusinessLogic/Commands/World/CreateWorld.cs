using BusinessLogic.Entities;
using Shared.Extensions;

namespace BusinessLogic.Commands.World;

public class CreateWorld : IUndoCommand
{
    internal AuthoringToolWorkspace AuthoringToolWorkspace { get; }
    private readonly Action<AuthoringToolWorkspace> _mappingAction;

    private IMemento? _memento;
    internal Entities.World World { get; }

    public CreateWorld(AuthoringToolWorkspace authoringToolWorkspace, string name, string shortname,
        string authors,
        string language, string description, string goals, Action<AuthoringToolWorkspace> mappingAction)
    {
        World = new Entities.World(name, shortname, authors, language, description, goals);
        AuthoringToolWorkspace = authoringToolWorkspace;
        _mappingAction = mappingAction;
    }
    
    public CreateWorld(AuthoringToolWorkspace authoringToolWorkspace, Entities.World world,
        Action<AuthoringToolWorkspace> mappingAction)
    {
        World = world;
        AuthoringToolWorkspace = authoringToolWorkspace;
        _mappingAction = mappingAction;
    }

    public void Execute()
    {
        _memento = AuthoringToolWorkspace.GetMemento();

        if (AuthoringToolWorkspace.Worlds.Any(lw => lw.Name == World.Name))
        {
            World.Name = StringHelper.GetUniqueName(AuthoringToolWorkspace.Worlds.Select(lw => lw.Name),
                World.Name);
        }
        AuthoringToolWorkspace.Worlds.Add(World);

        AuthoringToolWorkspace.SelectedWorld = World;

        _mappingAction.Invoke(AuthoringToolWorkspace);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }

        AuthoringToolWorkspace.RestoreMemento(_memento);

        _mappingAction.Invoke(AuthoringToolWorkspace);
    }

    public void Redo() => Execute();
}