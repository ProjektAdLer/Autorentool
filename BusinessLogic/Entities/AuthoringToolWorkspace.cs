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
        LearningWorlds = new List<LearningWorld>();
    }
    public AuthoringToolWorkspace(List<LearningWorld> learningWorlds)
    {
        LearningWorlds = learningWorlds;
    }
    public List<LearningWorld> LearningWorlds { get; set; }
    
    public IMemento GetMemento()
    {
        return new AuthoringToolWorkspaceMemento(this);
    }

    public void RestoreMemento(IMemento memento)
    {
        if (memento is not AuthoringToolWorkspaceMemento workspaceMemento)
            throw new ArgumentException("incorrect IMemento implementation", nameof(memento));
        LearningWorlds = workspaceMemento.LearningWorlds;
    }

    private record AuthoringToolWorkspaceMemento : IMemento
    {
        internal AuthoringToolWorkspaceMemento(AuthoringToolWorkspace workspace)
        {
            LearningWorlds = workspace.LearningWorlds.ToList();
        }
        internal List<LearningWorld> LearningWorlds { get; }
    }
}