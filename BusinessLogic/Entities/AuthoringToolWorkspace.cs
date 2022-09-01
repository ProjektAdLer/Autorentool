namespace BusinessLogic.Entities;

public class AuthoringToolWorkspace : IOriginator
{
    /// <summary>
    /// Private Constructor for AutoMapper
    /// </summary>
    private AuthoringToolWorkspace()
    {
    }
    
    public AuthoringToolWorkspace(LearningWorld? selectedLearningWorld, List<LearningWorld> learningWorlds)
    {
        SelectedLearningWorld = selectedLearningWorld;
        LearningWorlds = learningWorlds;
    }
    public LearningWorld? SelectedLearningWorld { get; set; }
    public List<LearningWorld> LearningWorlds { get; set; }
    
    public IMemento GetMemento()
    {
        return new AuthoringToolWorkspaceMemento(this);
    }

    public void RestoreMemento(IMemento memento)
    {
        if (memento is not AuthoringToolWorkspaceMemento workspaceMemento)
            throw new ArgumentException("incorrect IMemento implementation", nameof(memento));
        SelectedLearningWorld = workspaceMemento.SelectedLearningWorld;
        LearningWorlds = workspaceMemento.LearningWorlds;
    }

    private record AuthoringToolWorkspaceMemento : IMemento
    {
        internal AuthoringToolWorkspaceMemento(AuthoringToolWorkspace workspace)
        {
            SelectedLearningWorld = workspace.SelectedLearningWorld;
            LearningWorlds = workspace.LearningWorlds.ToList();
        }

        internal LearningWorld? SelectedLearningWorld { get; }
        internal List<LearningWorld> LearningWorlds { get; }
    }
}