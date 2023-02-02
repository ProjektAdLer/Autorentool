using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Layout;

/// <summary>
/// Places a  element that is already contained in the layout into a different slot in that layout.
/// </summary>
public class PlaceElementInLayoutFromLayout : IUndoCommand
{
    internal Entities.Space ParentSpace { get; }
    internal int NewSlotIndex { get; }
    internal IElement Element { get; }
    private readonly Action<Entities.Space> _mappingAction;
    private IMemento? _memento;

    public PlaceElementInLayoutFromLayout(Entities.Space parentSpace, IElement element, int newNewSlotIndex,
        Action<Entities.Space> mappingAction)
    {
        ParentSpace = parentSpace;
        Element = ParentSpace.ContainedElements.First(x => x.Id == element.Id);
        NewSlotIndex = newNewSlotIndex;
        _mappingAction = mappingAction;
    }

    public void Execute()
    {
        _memento = ParentSpace.SpaceLayout.GetMemento();

        var oldSlotIndex = Array.IndexOf(ParentSpace.SpaceLayout.Elements, Element);
        var replacedElement = ParentSpace.SpaceLayout.Elements[NewSlotIndex];
        ParentSpace.SpaceLayout.Elements[NewSlotIndex] = Element;
        ParentSpace.SpaceLayout.Elements[oldSlotIndex] = replacedElement;

        _mappingAction.Invoke(ParentSpace);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }

        ParentSpace.SpaceLayout.RestoreMemento(_memento);

        _mappingAction.Invoke(ParentSpace);
    }

    public void Redo() => Execute();
}