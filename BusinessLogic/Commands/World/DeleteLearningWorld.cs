using BusinessLogic.Entities;

namespace BusinessLogic.Commands.World;

public class DeleteLearningWorld : IUndoCommand
{
    public string Name => nameof(DeleteLearningWorld);
    internal AuthoringToolWorkspace AuthoringToolWorkspace { get; }
    internal LearningWorld LearningWorld { get; }
    private Action<AuthoringToolWorkspace> MappingAction { get; }
    private IMemento? Memento { get; set; }

    public DeleteLearningWorld(AuthoringToolWorkspace authoringToolWorkspace, LearningWorld learningWorld,
        Action<AuthoringToolWorkspace> mappingAction)
    {
        AuthoringToolWorkspace = authoringToolWorkspace;
        LearningWorld = learningWorld;
        MappingAction = mappingAction;
    }

    public void Execute()
    {
        Memento = AuthoringToolWorkspace.GetMemento();

        var realLearningWorld = AuthoringToolWorkspace.LearningWorlds.First(lw => lw.Id == LearningWorld.Id);

        AuthoringToolWorkspace.LearningWorlds.Remove(realLearningWorld);
        AuthoringToolWorkspace.SelectedLearningWorld = AuthoringToolWorkspace.LearningWorlds.LastOrDefault();

        MappingAction.Invoke(AuthoringToolWorkspace);
    }

    public void Undo()
    {
        if (Memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }

        AuthoringToolWorkspace.RestoreMemento(Memento);

        MappingAction.Invoke(AuthoringToolWorkspace);
    }

    public void Redo() => Execute();
}