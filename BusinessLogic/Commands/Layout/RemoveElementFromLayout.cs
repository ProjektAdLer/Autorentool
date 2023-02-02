using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Layout;

public class RemoveElementFromLayout : IUndoCommand
{
    internal Entities.World World { get; }
    internal Entities.Space Space { get; }
    internal IElement Element { get; }
    private readonly Action<Entities.World> _mappingAction;
    private IMemento? _mementoWorld;
    private IMemento? _mementoSpaceLayout;

    public RemoveElementFromLayout(Entities.World world, Entities.Space space,
        IElement element, Action<Entities.World> mappingAction)
    {
        World = world;
        Space = World.Spaces.First(x => x.Id == space.Id);
        Element = Space.ContainedElements.First(x => x.Id == element.Id);
        _mappingAction = mappingAction;
    }

    public void Execute()
    {
        _mementoWorld = World.GetMemento();
        _mementoSpaceLayout = Space.SpaceLayout.GetMemento();

        var oldSlot = Array.IndexOf(Space.SpaceLayout.Elements, Element);

        if (oldSlot >= 0)
        {
            Space.SpaceLayout.Elements[oldSlot] = null;
        }

        if (World.UnplacedElements.Contains(Element) == false)
        {
            World.UnplacedElements.Add(Element);
        }

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