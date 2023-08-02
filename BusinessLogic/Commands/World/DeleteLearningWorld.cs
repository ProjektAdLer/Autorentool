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
    private IMemento? Memento { get; set; }
    private ILogger<DeleteLearningWorld> Logger { get; }
    public string Name => nameof(DeleteLearningWorld);

    public void Execute()
    {
        Memento = AuthoringToolWorkspace.GetMemento();

        var realLearningWorld = AuthoringToolWorkspace.LearningWorlds.First(lw => lw.Id == LearningWorld.Id);

        AuthoringToolWorkspace.LearningWorlds.Remove(realLearningWorld);

        Logger.LogTrace("Deleted LearningWorld {Name} ({Id})", LearningWorld.Name, LearningWorld.Id);

        MappingAction.Invoke(AuthoringToolWorkspace);
    }

    public void Undo()
    {
        if (Memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }

        AuthoringToolWorkspace.RestoreMemento(Memento);

        Logger.LogTrace("Undone deletion of LearningWorld {Name} ({Id})", LearningWorld.Name, LearningWorld.Id);

        MappingAction.Invoke(AuthoringToolWorkspace);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing DeleteLearningWorld");
        Execute();
    }
}