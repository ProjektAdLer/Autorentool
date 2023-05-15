using BusinessLogic.API;
using BusinessLogic.Entities;

namespace BusinessLogic.Commands.World;

public class LoadLearningWorld : ILoadLearningWorld
{
    public string Name => nameof(LoadLearningWorld);
    internal IBusinessLogic BusinessLogic { get; }

    internal AuthoringToolWorkspace Workspace { get; }
    public LearningWorld? LearningWorld { get; private set; }
    internal string Filepath { get; }
    internal Action<AuthoringToolWorkspace> MappingAction { get; }
    private IMemento? _memento;

    public LoadLearningWorld(AuthoringToolWorkspace workspace, string filepath, IBusinessLogic businessLogic,
        Action<AuthoringToolWorkspace> mappingAction)
    {
        Workspace = workspace;
        Filepath = filepath;
        BusinessLogic = businessLogic;
        MappingAction = mappingAction;
    }
    
    public LoadLearningWorld(AuthoringToolWorkspace workspace, Stream stream, IBusinessLogic businessLogic,
        Action<AuthoringToolWorkspace> mappingAction)
    {
        Filepath = "";
        Workspace = workspace;
        BusinessLogic = businessLogic;
        LearningWorld = BusinessLogic.LoadLearningWorld(stream);
        MappingAction = mappingAction;
    }
    
    public void Execute()
    {
        _memento = Workspace.GetMemento();
        
        LearningWorld ??= BusinessLogic.LoadLearningWorld(Filepath);
        
        Workspace.LearningWorlds.Add(LearningWorld);

        LearningWorld.SavePath = Filepath;
        
        MappingAction.Invoke(Workspace);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }
        
        Workspace.RestoreMemento(_memento);
        
        MappingAction.Invoke(Workspace);
    }

    public void Redo() => Execute();
}