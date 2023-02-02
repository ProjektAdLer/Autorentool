using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Layout;

/// <summary>
/// Places an unplaced  element in a slot in a layout.
/// </summary>
public class PlaceElementInLayoutFromUnplaced : IUndoCommand
{
    internal Entities.World World { get; }
    internal Entities.Space Space { get; }
    internal int NewSlotIndex { get; }
    internal IElement Element { get; }
    private readonly Action<Entities.World> _mappingAction;
    private IMemento? _mementoWorld;
    private IMemento? _mementoSpaceLayout;

    public PlaceElementInLayoutFromUnplaced(Entities.World world, Entities.Space space,
        IElement element, int newSlotIndex, Action<Entities.World> mappingAction)
    {
        World = world;
        Space = World.Spaces.First(x => x.Id == space.Id);
        Element = World.UnplacedElements.First(x => x.Id == element.Id);
        NewSlotIndex = newSlotIndex;
        _mappingAction = mappingAction;
    }

    public void Execute()
    {
        _mementoWorld = World.GetMemento();
        _mementoSpaceLayout = Space.SpaceLayout.GetMemento();

        if (World.UnplacedElements.Contains(Element))
        {
            World.UnplacedElements.Remove(Element);
        }

        var oldElement = Space.SpaceLayout.Elements[NewSlotIndex];
        if (oldElement != null)
        {
            if (World.UnplacedElements.Contains(oldElement) == false)
            {
                World.UnplacedElements.Add(oldElement);
            }
        }

        Space.SpaceLayout.Elements[NewSlotIndex] = Element;

        _mappingAction.Invoke(World);
    }

    public void Undo()
    {
        if (_mementoWorld == null || _mementoSpaceLayout == null)
        {
            throw new InvalidOperationException("_mementoWorld or _mementoSpaceLayout is null");
        }

        World.RestoreMemento(_mementoWorld);
        Space.SpaceLayout.RestoreMemento(_mementoSpaceLayout);

        _mappingAction.Invoke(World);
    }

    public void Redo() => Execute();
}