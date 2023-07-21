using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.World;

public class DeleteLearningWorld : IDeleteLearningWorld
{
    public string Name => nameof(DeleteLearningWorld);
    internal AuthoringToolWorkspace AuthoringToolWorkspace { get; }
    internal LearningWorld LearningWorld { get; }
    internal Action<AuthoringToolWorkspace> MappingAction { get; }
    private IMemento? Memento { get; set; }
    private ILogger<WorldCommandFactory> Logger { get; }

    public DeleteLearningWorld(AuthoringToolWorkspace authoringToolWorkspace, LearningWorld learningWorld,
        Action<AuthoringToolWorkspace> mappingAction, ILogger<WorldCommandFactory> logger)
    {
        AuthoringToolWorkspace = authoringToolWorkspace;
        LearningWorld = learningWorld;
        MappingAction = mappingAction;
        Logger = logger;
    }

    public void Execute()
    {
        Memento = AuthoringToolWorkspace.GetMemento();

        var realLearningWorld = AuthoringToolWorkspace.LearningWorlds.First(lw => lw.Id == LearningWorld.Id);

        AuthoringToolWorkspace.LearningWorlds.Remove(realLearningWorld);

        Logger.LogTrace("Deleted LearningWorld {name} ({id}).", LearningWorld.Name, LearningWorld.Id);

        MappingAction.Invoke(AuthoringToolWorkspace);
    }

    public void Undo()
    {
        if (Memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }

        AuthoringToolWorkspace.RestoreMemento(Memento);

        Logger.LogTrace("Undone deletion of LearningWorld {name} ({id}).", LearningWorld.Name, LearningWorld.Id);

        MappingAction.Invoke(AuthoringToolWorkspace);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing DeleteLearningWorld");
        Execute();
    }
}