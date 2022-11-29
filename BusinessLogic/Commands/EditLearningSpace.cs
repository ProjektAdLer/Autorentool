using BusinessLogic.Entities;

namespace BusinessLogic.Commands;

public class EditLearningSpace : IUndoCommand
{
    internal LearningSpace LearningSpace { get; }
    private readonly string _name;
    private readonly string _shortname;
    private readonly string _authors;
    private readonly string _description;
    private readonly string _goals;
    private readonly int _requiredPoints;
    private readonly Action<LearningSpace> _mappingAction;
    private IMemento? _memento;

    public EditLearningSpace(LearningSpace learningSpace, string name, string shortname,
        string authors, string description, string goals, int requiredPoints, Action<LearningSpace> mappingAction)
    {
        LearningSpace = learningSpace;
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
        _memento = LearningSpace.GetMemento();

        LearningSpace.Name = _name;
        LearningSpace.Shortname = _shortname;
        LearningSpace.Authors = _authors;
        LearningSpace.Description = _description;
        LearningSpace.Goals = _goals;
        LearningSpace.RequiredPoints = _requiredPoints;
        
        _mappingAction.Invoke(LearningSpace);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }
        
        LearningSpace.RestoreMemento(_memento);
        
        _mappingAction.Invoke(LearningSpace);
    }

    public void Redo() => Execute();
}