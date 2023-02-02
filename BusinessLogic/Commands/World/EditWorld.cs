using BusinessLogic.Entities;

namespace BusinessLogic.Commands.World;

public class EditWorld : IUndoCommand
{
    internal Entities.World World { get; }
    private readonly string _name;
    private readonly string _shortname;
    private readonly string _authors;
    private readonly string _language;
    private readonly string _description;
    private readonly string _goals;
    private readonly Action<Entities.World> _mappingAction;
    private IMemento? _memento;

    public EditWorld(Entities.World world, string name, string shortname,
        string authors, string language, string description, string goals, Action<Entities.World> mappingAction)
    {
        World = world;
        _name = name;
        _shortname = shortname;
        _authors = authors;
        _language = language;
        _description = description;
        _goals = goals;
        _mappingAction = mappingAction;
    }

    public void Execute()
    {
        _memento ??= World.GetMemento();

        World.Name = _name;
        World.Shortname = _shortname;
        World.Authors = _authors;
        World.Language = _language;
        World.Description = _description;
        World.Goals = _goals;

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