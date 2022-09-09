using BusinessLogic.Entities;

namespace BusinessLogic.Commands;

public class DeleteLearningWorld : IUndoCommand
{
    private readonly AuthoringToolWorkspace _authoringToolWorkspace;
    private readonly LearningWorld _learningWorld;
    private readonly Action<AuthoringToolWorkspace> _mappingAction;
    private IMemento? _memento;

    public DeleteLearningWorld(AuthoringToolWorkspace authoringToolWorkspace, LearningWorld learningWorld,
        Action<AuthoringToolWorkspace> mappingAction)
    {
        _authoringToolWorkspace = authoringToolWorkspace;
        _learningWorld = learningWorld;
        _mappingAction = mappingAction;
    }

    public void Execute()
    {
        _memento = _authoringToolWorkspace.GetMemento();

        _authoringToolWorkspace.LearningWorlds.Remove(_learningWorld);

        _mappingAction.Invoke(_authoringToolWorkspace);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }

        _authoringToolWorkspace.RestoreMemento(_memento);

        _mappingAction.Invoke(_authoringToolWorkspace);
    }

    public void Redo()
    {
        _authoringToolWorkspace.LearningWorlds.Remove(_learningWorld);

        _mappingAction.Invoke(_authoringToolWorkspace);
    }
}