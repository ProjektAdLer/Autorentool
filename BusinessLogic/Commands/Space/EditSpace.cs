using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Space;

public class EditSpace : IUndoCommand
{
    internal Entities.Space Space { get; }
    private readonly string _name;
    private readonly string _shortname;
    private readonly string _authors;
    private readonly string _description;
    private readonly string _goals;
    private readonly int _requiredPoints;
    private readonly Action<Entities.Space> _mappingAction;
    private IMemento? _memento;

    public EditSpace(Entities.Space space, string name, string shortname,
        string authors, string description, string goals, int requiredPoints, Action<Entities.Space> mappingAction)
    {
        Space = space;
        _name = name;
        _shortname = shortname;
        _authors = authors;
        _description = description;
        _goals = goals;
        _requiredPoints = requiredPoints;
        _mappingAction = mappingAction;
    }

    public void Execute()
    {
        _memento = Space.GetMemento();

        Space.Name = _name;
        Space.Shortname = _shortname;
        Space.Authors = _authors;
        Space.Description = _description;
        Space.Goals = _goals;
        Space.RequiredPoints = _requiredPoints;
        
        _mappingAction.Invoke(Space);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }
        
        Space.RestoreMemento(_memento);
        
        _mappingAction.Invoke(Space);
    }

    public void Redo() => Execute();
}