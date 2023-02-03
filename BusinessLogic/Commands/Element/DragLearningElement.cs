using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Element;

public class DragLearningElement : IUndoCommand
{
    internal LearningElement LearningElement { get; }
    private readonly double _oldPositionX;
    private readonly double _oldPositionY;
    private readonly double _newPositionX;
    private readonly double _newPositionY;
    private readonly Action<LearningElement> _mappingAction;
    private IMemento? _memento;

    public DragLearningElement(LearningElement learningElement, double oldPositionX, double oldPositionY, 
        double newPositionX, double newPositionY, Action<LearningElement> mappingAction)
    {
        LearningElement = learningElement;
        _oldPositionX = oldPositionX;
        _oldPositionY = oldPositionY;
        _newPositionX = newPositionX;
        _newPositionY = newPositionY;
        _mappingAction = mappingAction;
    }

    public void Execute()
    {
        LearningElement.PositionX = _oldPositionX;
        LearningElement.PositionY = _oldPositionY;
        _memento = LearningElement.GetMemento();

        LearningElement.PositionX = _newPositionX;
        LearningElement.PositionY = _newPositionY;
        
        _mappingAction.Invoke(LearningElement);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }
        
        LearningElement.RestoreMemento(_memento);
        
        _mappingAction.Invoke(LearningElement);
    }

    public void Redo() => Execute();
}