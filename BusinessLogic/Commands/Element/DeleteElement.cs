using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Element;

public class DeleteElement : IUndoCommand
{
    internal Entities.Element Element { get; }
    internal Entities.Space ParentSpace { get; }
    private readonly Action<Entities.Space> _mappingAction;
    private IMemento? _memento;
    private IMemento? _mementoSpaceLayout;

    public DeleteElement(Entities.Element element, Entities.Space parentSpace,
        Action<Entities.Space> mappingAction)
    {
        Element = element;
        ParentSpace = parentSpace;
        _mappingAction = mappingAction;
    }

    public void Execute()
    {
        _memento = ParentSpace.GetMemento();
        _mementoSpaceLayout = ParentSpace.SpaceLayout.GetMemento();

        var element = ParentSpace.SpaceLayout.Elements.First(x => x?.Id == Element.Id);
        var elementIndex = Array.IndexOf(ParentSpace.SpaceLayout.Elements, element);

        ParentSpace.SpaceLayout.Elements[elementIndex] = null;

        if (element == ParentSpace.SelectedElement || ParentSpace.SelectedElement == null)
        {
            ParentSpace.SelectedElement = ParentSpace.SpaceLayout.Elements.LastOrDefault();
        }

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