using BusinessLogic.API;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.World;

public class LoadLearningWorld : ILoadLearningWorld
{
    public string Name => nameof(LoadLearningWorld);
    internal IBusinessLogic BusinessLogic { get; }

    internal AuthoringToolWorkspace Workspace { get; }
    public ILearningWorld? LearningWorld { get; private set; }
    internal string Filepath { get; }
    internal Action<AuthoringToolWorkspace> MappingAction { get; }
    private IMemento? _memento;
    private ILogger<WorldCommandFactory> Logger { get; }

    public LoadLearningWorld(AuthoringToolWorkspace workspace, string filepath, IBusinessLogic businessLogic,
        Action<AuthoringToolWorkspace> mappingAction, ILogger<WorldCommandFactory> logger)
    {
        Workspace = workspace;
        Filepath = filepath;
        BusinessLogic = businessLogic;
        MappingAction = mappingAction;
        Logger = logger;
    }
    
    public LoadLearningWorld(AuthoringToolWorkspace workspace, Stream stream, IBusinessLogic businessLogic,
        Action<AuthoringToolWorkspace> mappingAction, ILogger<WorldCommandFactory> logger)
    {
        Filepath = "";
        Workspace = workspace;
        BusinessLogic = businessLogic;
        LearningWorld = BusinessLogic.LoadLearningWorld(stream);
        MappingAction = mappingAction;
        Logger = logger;
    }
    
    public void Execute()
    {
        _memento = Workspace.GetMemento();
        
        LearningWorld ??= BusinessLogic.LoadLearningWorld(Filepath);
        
        Workspace.LearningWorlds.Add(LearningWorld);

        LearningWorld.SavePath = Filepath;
        
        Logger.LogTrace("Loaded LearningWorld {name} ({id}) from {path}.", LearningWorld.Name, LearningWorld.Id, Filepath);
        
        MappingAction.Invoke(Workspace);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }
        
        Workspace.RestoreMemento(_memento);
        
        Logger.LogTrace("Undone loading of LearningWorld");
        
        MappingAction.Invoke(Workspace);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing LoadLearningWorld");
        Execute();
    }
}