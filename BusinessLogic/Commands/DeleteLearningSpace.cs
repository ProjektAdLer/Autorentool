using BusinessLogic.Entities;

namespace BusinessLogic.Commands;

public class DeleteLearningSpace : IUndoCommand
{
    private readonly LearningWorld _learningWorld;
    private readonly LearningSpace _learningSpace;
    private readonly Action<LearningWorld> _mappingAction;
    private IMemento? _memento;

    public DeleteLearningSpace(LearningWorld learningWorld, LearningSpace learningSpace,
        Action<LearningWorld> mappingAction)
    {
        _learningWorld = learningWorld;
        _learningSpace = learningSpace;
        _mappingAction = mappingAction;
    }
    
    public void Execute()
    {
        _memento = _learningWorld.GetMemento();

        var space = _learningWorld.LearningSpaces.First(x => x.Id == _learningSpace.Id);
        
        _learningWorld.LearningSpaces.Remove(space);
        
        _mappingAction.Invoke(_learningWorld);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }
        
        _learningWorld.RestoreMemento(_memento);
        
        _mappingAction.Invoke(_learningWorld);
    }

    public void Redo() => Execute();
}