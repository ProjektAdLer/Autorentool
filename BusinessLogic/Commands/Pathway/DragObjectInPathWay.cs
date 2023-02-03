using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Pathway;

public class DragObjectInPathWay : IUndoCommand
{
    internal IObjectInPathWay LearningObject { get; }
    private readonly double _oldPositionX;
    private readonly double _oldPositionY;
    private readonly double _newPositionX;
    private readonly double _newPositionY;
    private readonly Action<IObjectInPathWay> _mappingAction;
    private IMemento? _memento;

    public DragObjectInPathWay(IObjectInPathWay learningObject, double oldPositionX, double oldPositionY, 
        double newPositionX, double newPositionY, Action<IObjectInPathWay> mappingAction)
    {
        LearningObject = learningObject;
        _oldPositionX = oldPositionX;
        _oldPositionY = oldPositionY;
        _newPositionX = newPositionX;
        _newPositionY = newPositionY;
        _mappingAction = mappingAction;
    }

    public void Execute()
    {
        LearningObject.PositionX = _oldPositionX;
        LearningObject.PositionY = _oldPositionY;
        _memento = LearningObject.GetMemento();

        LearningObject.PositionX = _newPositionX;
        LearningObject.PositionY = _newPositionY;
        
        _mappingAction.Invoke(LearningObject);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }
        
        LearningObject.RestoreMemento(_memento);
        
        _mappingAction.Invoke(LearningObject);
    }

    public void Redo() => Execute();
}