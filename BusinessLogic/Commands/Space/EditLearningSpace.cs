using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Space;

public class EditLearningSpace : IUndoCommand
{
    public string Name => nameof(EditLearningSpace);
    internal LearningSpace LearningSpace { get; }
    private readonly string _name;
    private readonly string _description;
    private readonly string _goals;
    private readonly Topic? _topic;
    private readonly int _requiredPoints;
    private readonly Action<LearningSpace> _mappingAction;
    private IMemento? _memento;

    public EditLearningSpace(LearningSpace learningSpace, string name,
        string description, string goals, int requiredPoints, Topic? topic, Action<LearningSpace> mappingAction)
    {
        LearningSpace = learningSpace;
        _name = name;
        _description = description;
        _goals = goals;
        _topic = topic;
        _requiredPoints = requiredPoints;
        _mappingAction = mappingAction;
    }

    public void Execute()
    {
        _memento = LearningSpace.GetMemento();

        LearningSpace.Name = _name;
        LearningSpace.Description = _description;
        LearningSpace.Goals = _goals;
        LearningSpace.AssignedTopic = _topic;
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