using BusinessLogic.API;
using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Element;

public class LoadElement : IUndoCommand
{
    private readonly IBusinessLogic _businessLogic;
    
    internal Entities.Space ParentSpace { get; }
    internal int SlotIndex { get; }
    internal Entities.Element? Element;
    private readonly string _filepath;
    private readonly Action<Entities.Space> _mappingAction;
    private IMemento? _memento;
    private IMemento? _mementoSpaceLayout;

    public LoadElement(Entities.Space parentSpace, int slotIndex, string filepath, IBusinessLogic businessLogic, 
        Action<Entities.Space> mappingAction)
    {
        ParentSpace = parentSpace;
        SlotIndex = slotIndex;
        _filepath = filepath;
        _businessLogic = businessLogic;
        _mappingAction = mappingAction;
    }
    
    public LoadElement(Entities.Space parentSpace, int slotIndex, Stream stream, IBusinessLogic businessLogic,
        Action<Entities.Space> mappingAction)
    {
        ParentSpace = parentSpace;
        SlotIndex = slotIndex;
        _filepath = "";
        _businessLogic = businessLogic;
        Element = _businessLogic.LoadElement(stream);
        _mappingAction = mappingAction;
    }
    public void Execute()
    {
        _memento = ParentSpace.GetMemento();
        _mementoSpaceLayout = ParentSpace.SpaceLayout.GetMemento();
        
        Element ??= _businessLogic.LoadElement(_filepath);
        Element.Parent = ParentSpace;
        ParentSpace.SpaceLayout.Elements[SlotIndex] = Element;
        ParentSpace.SelectedElement = Element;
        
        _mappingAction.Invoke(ParentSpace);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }
        if (_mementoSpaceLayout == null)
        {
            throw new InvalidOperationException("_mementoSpaceLayout is null");
        }
        
        ParentSpace.RestoreMemento(_memento);
        ParentSpace.SpaceLayout.RestoreMemento(_mementoSpaceLayout);
        
        _mappingAction.Invoke(ParentSpace);
    }

    public void Redo() => Execute();
}