using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Element;

public class DragElement : IUndoCommand
{
    internal Entities.Element Element { get; }
    private readonly double _oldPositionX;
    private readonly double _oldPositionY;
    private readonly double _newPositionX;
    private readonly double _newPositionY;
    private readonly Action<Entities.Element> _mappingAction;
    private IMemento? _memento;

    public DragElement(Entities.Element element, double oldPositionX, double oldPositionY, 
        double newPositionX, double newPositionY, Action<Entities.Element> mappingAction)
    {
        Element = element;
        _oldPositionX = oldPositionX;
        _oldPositionY = oldPositionY;
        _newPositionX = newPositionX;
        _newPositionY = newPositionY;
        _mappingAction = mappingAction;
    }

    public void Execute()
    {
        Element.PositionX = _oldPositionX;
        Element.PositionY = _oldPositionY;
        _memento = Element.GetMemento();

        Element.PositionX = _newPositionX;
        Element.PositionY = _newPositionY;
        
        _mappingAction.Invoke(Element);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }
        
        Element.RestoreMemento(_memento);
        
        _mappingAction.Invoke(Element);
    }

    public void Redo() => Execute();
}