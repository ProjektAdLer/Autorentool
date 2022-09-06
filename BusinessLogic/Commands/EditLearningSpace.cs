using BusinessLogic.Entities;

namespace BusinessLogic.Commands;

public class EditLearningSpace : IUndoCommand
{
    private readonly LearningSpace _learningSpace;
    private readonly string _name;
    private readonly string _shortname;
    private readonly string _authors;
    private readonly string _description;
    private readonly string _goals;
    private readonly Action<LearningSpace> _mappingAction;
    private IMemento? _memento;

    public EditLearningSpace(LearningSpace learningSpace, string name, string shortname,
        string authors, string description, string goals, Action<LearningSpace> mappingAction)
    {
        _learningSpace = learningSpace;
        _name = name;
        _shortname = shortname;
        _authors = authors;
        _description = description;
        _goals = goals;
        _mappingAction = mappingAction;
    }

    public void Execute()
    {
        _memento = _learningSpace.GetMemento();

        _learningSpace.Name = _name;
        _learningSpace.Shortname = _shortname;
        _learningSpace.Authors = _authors;
        _learningSpace.Description = _description;
        _learningSpace.Goals = _goals;
        
        _mappingAction.Invoke(_learningSpace);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }
        
        _learningSpace.RestoreMemento(_memento);
        
        _mappingAction.Invoke(_learningSpace);
    }

    public void Redo() => Execute();
}