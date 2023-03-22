using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using LearningElementDifficultyEnum = Shared.LearningElementDifficultyEnum;

namespace BusinessLogic.Commands.Element;

public class EditLearningElement : IUndoCommand
{
    public string Name => nameof(EditLearningElement);
    internal LearningElement LearningElement { get; }
    internal LearningSpace? ParentSpace { get; }
    private readonly string _name;
    private readonly string _shortName;
    private readonly string _authors;
    private readonly string _description;
    private readonly string _goals;
    private readonly LearningElementDifficultyEnum _difficulty;
    private readonly int _workload;
    private readonly int _points;
    internal LearningContent LearningContent { get; }
    private readonly Action<LearningElement> _mappingAction;
    private IMemento? _memento;
    
    public EditLearningElement(LearningElement learningElement, LearningSpace? parentSpace, string name,
        string shortName, string authors, string description, string goals, LearningElementDifficultyEnum difficulty,
        int workload, int points, LearningContent learningContent, Action<LearningElement> mappingAction)
    {
        LearningElement = learningElement;
        ParentSpace = parentSpace;
        _name = name;
        _shortName = shortName;
        _authors = authors;
        _description = description;
        _goals = goals;
        _difficulty = difficulty;
        _workload = workload;
        _points = points;
        LearningContent = learningContent;
        _mappingAction = mappingAction;
    }

    public void Execute()
    {
        _memento = LearningElement.GetMemento();

        LearningElement.Name = _name;
        LearningElement.Shortname = _shortName;
        LearningElement.Parent = ParentSpace;
        LearningElement.Authors = _authors;
        LearningElement.Description = _description;
        LearningElement.Goals = _goals;
        LearningElement.Difficulty = _difficulty;
        LearningElement.Workload = _workload;
        LearningElement.Points = _points;
        LearningElement.LearningContent = LearningContent;
        
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