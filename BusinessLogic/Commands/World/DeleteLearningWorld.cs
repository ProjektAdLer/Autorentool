using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.World;

public class DeleteLearningWorld : IDeleteLearningWorld
{
    public DeleteLearningWorld(AuthoringToolWorkspace authoringToolWorkspace, LearningWorld learningWorld,
        Action<AuthoringToolWorkspace> mappingAction, ILogger<DeleteLearningWorld> logger)
    {
        AuthoringToolWorkspace = authoringToolWorkspace;
        LearningWorld = learningWorld;
        MappingAction = mappingAction;
        Logger = logger;
    }

    internal AuthoringToolWorkspace AuthoringToolWorkspace { get; }
    internal LearningWorld LearningWorld { get; }
    internal Action<AuthoringToolWorkspace> MappingAction { get; }
    private ILogger<DeleteLearningWorld> Logger { get; }
    public string Name => nameof(DeleteLearningWorld);

    public void Execute()
    {
        var realLearningWorld = AuthoringToolWorkspace.LearningWorlds.First(lw => lw.Id == LearningWorld.Id);

        AuthoringToolWorkspace.LearningWorlds.Remove(realLearningWorld);

        Logger.LogTrace("Deleted LearningWorld {Name} ({Id})", LearningWorld.Name, LearningWorld.Id);

        MappingAction.Invoke(AuthoringToolWorkspace);
    }
}