using BusinessLogic.API;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.World;

public class LoadLearningWorld : ILoadLearningWorld
{
    private IMemento? _memento;

    public LoadLearningWorld(AuthoringToolWorkspace workspace, string filepath, IBusinessLogic businessLogic,
        Action<AuthoringToolWorkspace> mappingAction, ILogger<LoadLearningWorld> logger)
    {
        Workspace = workspace;
        Filepath = filepath;
        BusinessLogic = businessLogic;
        MappingAction = mappingAction;
        Logger = logger;
    }

    internal IBusinessLogic BusinessLogic { get; }

    internal AuthoringToolWorkspace Workspace { get; }
    internal string Filepath { get; }
    internal Action<AuthoringToolWorkspace> MappingAction { get; }
    private ILogger<LoadLearningWorld> Logger { get; }
    public string Name => nameof(LoadLearningWorld);
    public ILearningWorld? LearningWorld { get; private set; }

    public void Execute()
    {
        _memento = Workspace.GetMemento();

        LearningWorld = BusinessLogic.LoadLearningWorld(Filepath);

        Workspace.LearningWorlds.Add(LearningWorld);

        LearningWorld.SavePath = Filepath;

        Logger.LogTrace("Loaded LearningWorld {Name} ({Id}) from {Path}", LearningWorld.Name, LearningWorld.Id,
            Filepath);

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