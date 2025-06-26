using BusinessLogic.API;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.World;

public class LoadLearningWorld : ILoadLearningWorld
{
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
        LearningWorld ??= BusinessLogic.LoadLearningWorld(Filepath);

        Workspace.LearningWorlds.Add(LearningWorld);

        LearningWorld.SavePath = Filepath;

        Logger.LogTrace("Loaded LearningWorld {Name} ({Id}) from {Path}", LearningWorld.Name, LearningWorld.Id,
            Filepath);

        MappingAction.Invoke(Workspace);
    }
}