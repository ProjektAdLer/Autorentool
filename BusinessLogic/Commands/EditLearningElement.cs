using BusinessLogic.Entities;
using LearningElementDifficultyEnum = Shared.LearningElementDifficultyEnum;

namespace BusinessLogic.Commands;

public class EditLearningElement : IUndoCommand
{
    private readonly LearningElement _learningElement;
    private readonly ILearningElementParent _elementParent;
    private readonly string _name;
    private readonly string _shortName;
    private readonly string _authors;
    private readonly string _description;
    private readonly string _goals;
    private readonly LearningElementDifficultyEnum _difficulty;
    private readonly int _workload;
    private readonly int _points;
    private readonly Action<LearningElement> _mappingAction;
    private IMemento? _memento;
    
    public EditLearningElement(LearningElement learningElement, ILearningElementParent elementParent, string name,
        string shortName, string authors, string description, string goals, LearningElementDifficultyEnum difficulty,
        int workload, int points, Action<LearningElement> mappingAction)
    {
        _learningElement = learningElement;
        _elementParent = elementParent;
        _name = name;
        _shortName = shortName;
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
        _memento = _learningElement.GetMemento();

        _learningElement.Name = _name;
        _learningElement.Shortname = _shortName;
        _learningElement.Parent = _elementParent;
        _learningElement.Authors = _authors;
        _learningElement.Description = _description;
        _learningElement.Goals = _goals;
        _learningElement.Difficulty = _difficulty;
        _learningElement.Workload = _workload;
        _learningElement.Points = _points;
        
        _mappingAction.Invoke(_learningElement);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }
        
        _learningElement.RestoreMemento(_memento);

        _mappingAction.Invoke(_learningElement);
    }

    public void Redo() => Execute();
}