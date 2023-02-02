using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Pathway;

public class DragObjectInPathWay : IUndoCommand
{
    internal IObjectInPathWay DraggableObject { get; }
    private readonly double _oldPositionX;
    private readonly double _oldPositionY;
    private readonly double _newPositionX;
    private readonly double _newPositionY;
    private readonly Action<IObjectInPathWay> _mappingAction;
    private IMemento? _memento;

    public DragObjectInPathWay(IObjectInPathWay draggableObject, double oldPositionX, double oldPositionY, 
        double newPositionX, double newPositionY, Action<IObjectInPathWay> mappingAction)
    {
        DraggableObject = draggableObject;
        _oldPositionX = oldPositionX;
        _oldPositionY = oldPositionY;
        _newPositionX = newPositionX;
        _newPositionY = newPositionY;
        _mappingAction = mappingAction;
    }

    public void Execute()
    {
        DraggableObject.PositionX = _oldPositionX;
        DraggableObject.PositionY = _oldPositionY;
        _memento = DraggableObject.GetMemento();

        DraggableObject.PositionX = _newPositionX;
        DraggableObject.PositionY = _newPositionY;
        
        _mappingAction.Invoke(DraggableObject);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }
        
        DraggableObject.RestoreMemento(_memento);
        
        _mappingAction.Invoke(DraggableObject);
    }

    public void Redo() => Execute();
}