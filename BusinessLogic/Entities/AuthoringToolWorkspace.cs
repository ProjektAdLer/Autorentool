using JetBrains.Annotations;

namespace BusinessLogic.Entities;

public class AuthoringToolWorkspace : IOriginator
{
    /// <summary>
    /// Constructor for Automapper. DO NOT USE.
    /// </summary>
    [UsedImplicitly]
    private AuthoringToolWorkspace()
    {
        Worlds = new List<World>();
    }
    public AuthoringToolWorkspace(World? selectedWorld, List<World> worlds)
    {
        SelectedWorld = selectedWorld;
        Worlds = worlds;
    }
    public World? SelectedWorld { get; set; }
    public List<World> Worlds { get; set; }
    
    public IMemento GetMemento()
    {
        return new AuthoringToolWorkspaceMemento(this);
    }

    public void RestoreMemento(IMemento memento)
    {
        if (memento is not AuthoringToolWorkspaceMemento workspaceMemento)
            throw new ArgumentException("incorrect IMemento implementation", nameof(memento));
        SelectedWorld = workspaceMemento.SelectedWorld;
        Worlds = workspaceMemento.Worlds;
    }

    private record AuthoringToolWorkspaceMemento : IMemento
    {
        internal AuthoringToolWorkspaceMemento(AuthoringToolWorkspace workspace)
        {
            SelectedWorld = workspace.SelectedWorld;
            Worlds = workspace.Worlds.ToList();
        }

        internal World? SelectedWorld { get; }
        internal List<World> Worlds { get; }
    }
}