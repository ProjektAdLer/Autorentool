using BusinessLogic.Entities;

namespace BusinessLogic.Commands;

public class DragLearningSpace : IUndoCommand
{
    internal LearningSpace LearningSpace { get; }
    private readonly double _oldPositionX;
    private readonly double _oldPositionY;
    private readonly double _newPositionX;
    private readonly double _newPositionY;
    private readonly Action<LearningSpace> _mappingAction;
    private IMemento? _memento;

    public DragLearningSpace(LearningSpace learningSpace, double oldPositionX, double oldPositionY, 
        double newPositionX, double newPositionY, Action<LearningSpace> mappingAction)
    {
        LearningSpace = learningSpace;
        _oldPositionX = oldPositionX;
        _oldPositionY = oldPositionY;
        _newPositionX = newPositionX;
        _newPositionY = newPositionY;
        _mappingAction = mappingAction;
    }

    public void Execute()
    {
        LearningSpace.PositionX = _oldPositionX;
        LearningSpace.PositionY = _oldPositionY;
        _memento = LearningSpace.GetMemento();

        LearningSpace.PositionX = _newPositionX;
        LearningSpace.PositionY = _newPositionY;
        
        _mappingAction.Invoke(LearningSpace);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }
        
        LearningSpace.RestoreMemento(_memento);
        
        _mappingAction.Invoke(LearningSpace);
    }

    public void Redo() => Execute();
}