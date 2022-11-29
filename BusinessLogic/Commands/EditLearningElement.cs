using System.ComponentModel.DataAnnotations;
using BusinessLogic.Entities;
using LearningElementDifficultyEnum = Shared.LearningElementDifficultyEnum;

namespace BusinessLogic.Commands;

public class EditLearningElement : IUndoCommand
{
    internal LearningElement LearningElement { get; }
    internal LearningSpace ParentSpace { get; }
    private readonly string _name;
    private readonly string _shortName;
    private readonly string _url;
    private readonly string _authors;
    private readonly string _description;
    private readonly string _goals;
    private readonly LearningElementDifficultyEnum _difficulty;
    private readonly int _workload;
    private readonly int _points;
    private readonly Action<LearningElement> _mappingAction;
    private IMemento? _memento;
    
    public EditLearningElement(LearningElement learningElement, LearningSpace parentSpace, string name,
        string shortName, string url, string authors, string description, string goals, LearningElementDifficultyEnum difficulty,
        int workload, int points, Action<LearningElement> mappingAction)
    {
        LearningElement = learningElement;
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
        _memento = LearningElement.GetMemento();

        LearningElement.Name = _name;
        LearningElement.Shortname = _shortName;
        LearningElement.Parent = ParentSpace;
        LearningElement.Url = _url;
        LearningElement.Authors = _authors;
        LearningElement.Description = _description;
        LearningElement.Goals = _goals;
        LearningElement.Difficulty = _difficulty;
        LearningElement.Workload = _workload;
        LearningElement.Points = _points;
        
        _mappingAction.Invoke(LearningElement);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }
        
        LearningElement.RestoreMemento(_memento);

        _mappingAction.Invoke(LearningElement);
    }

    public void Redo() => Execute();
}