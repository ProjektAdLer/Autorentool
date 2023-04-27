using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Pathway;

public class DragObjectInPathWay : IUndoCommand
{
    public string Name => nameof(DragObjectInPathWay);
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

        if(AnyChanges()) LearningObject.UnsavedChanges = true;
        LearningObject.PositionX = _newPositionX;
        LearningObject.PositionY = _newPositionY;
        
        _mappingAction.Invoke(LearningObject);
    }

    private bool AnyChanges() =>
        Math.Abs(LearningObject.PositionX - _newPositionX) > 0.01 ||
        Math.Abs(LearningObject.PositionY - _newPositionY) > 0.01;

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