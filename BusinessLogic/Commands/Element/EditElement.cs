using BusinessLogic.Entities;
using Shared;

namespace BusinessLogic.Commands.Element;

public class EditElement : IUndoCommand
{
    internal Entities.Element Element { get; }
    internal Entities.Space ParentSpace { get; }
    private readonly string _name;
    private readonly string _shortName;
    private readonly string _url;
    private readonly string _authors;
    private readonly string _description;
    private readonly string _goals;
    private readonly ElementDifficultyEnum _difficulty;
    private readonly int _workload;
    private readonly int _points;
    private readonly Action<Entities.Element> _mappingAction;
    private IMemento? _memento;
    
    public EditElement(Entities.Element element, Entities.Space parentSpace, string name,
        string shortName, string url, string authors, string description, string goals, ElementDifficultyEnum difficulty,
        int workload, int points, Action<Entities.Element> mappingAction)
    {
        Element = element;
        ParentSpace = parentSpace;
        _name = name;
        _shortName = shortName;
        _url = url;
        _authors = authors;
        _description = description;
        _goals = goals;
        _difficulty = difficulty;
        _workload = workload;
        _points = points;
        _mappingAction = mappingAction;
    }

    public void Execute()
    {
        _memento = Element.GetMemento();

        Element.Name = _name;
        Element.Shortname = _shortName;
        Element.Parent = ParentSpace;
        Element.Url = _url;
        Element.Authors = _authors;
        Element.Description = _description;
        Element.Goals = _goals;
        Element.Difficulty = _difficulty;
        Element.Workload = _workload;
        Element.Points = _points;
        
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