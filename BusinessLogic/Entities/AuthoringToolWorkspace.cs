using BusinessLogic.Entities.LearningContent;
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
        LearningWorlds = new List<ILearningWorld>();
        LearningContents = new List<ILearningContent>();
    }
    public AuthoringToolWorkspace(List<ILearningWorld> learningWorlds, List<ILearningContent> learningContents)
    {
        LearningWorlds = learningWorlds;
        LearningContents = learningContents;
    }
    public List<ILearningWorld> LearningWorlds { get; set; }
    public List<ILearningContent> LearningContents { get; set; }
    
    public IMemento GetMemento()
    {
        return new AuthoringToolWorkspaceMemento(this);
    }

    public void RestoreMemento(IMemento memento)
    {
        if (memento is not AuthoringToolWorkspaceMemento workspaceMemento)
            throw new ArgumentException("incorrect IMemento implementation", nameof(memento));
        LearningWorlds = workspaceMemento.LearningWorlds;
        LearningContents = workspaceMemento.LearningContents;
    }

    private record AuthoringToolWorkspaceMemento : IMemento
    {
        internal AuthoringToolWorkspaceMemento(AuthoringToolWorkspace workspace)
        {
            LearningWorlds = workspace.LearningWorlds.ToList();
            LearningContents = workspace.LearningContents.ToList();
        }
        internal List<ILearningWorld> LearningWorlds { get; }
        internal List<ILearningContent> LearningContents { get; }
    }
}