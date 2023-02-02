using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Space;

public class CreateSpace : IUndoCommand
{
    internal Entities.World World { get; }
    internal Entities.Space Space { get; }
    private readonly Action<Entities.World> _mappingAction;
    private IMemento? _memento;

    public CreateSpace(Entities.World world, string name, string shortname, string authors,
        string description, string goals, int requiredPoints, double positionX, double positionY, Action<Entities.World> mappingAction)
    {
        Space = new Entities.Space(name, shortname, authors, description, goals, requiredPoints, null, positionX, positionY);
        World = world;
        _mappingAction = mappingAction;
    }

    public CreateSpace(Entities.World world, Entities.Space space, Action<Entities.World> mappingAction)
    {
        Space = space;
        World = world;
        _mappingAction = mappingAction;
    }

    public void Execute()
    {
        _memento = World.GetMemento();

        World.Spaces.Add(Space);
        World.SelectedObject = Space;
        
        _mappingAction.Invoke(World);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }
        
        World.RestoreMemento(_memento);
        
        _mappingAction.Invoke(World);
    }

    public void Redo() => Execute();
}