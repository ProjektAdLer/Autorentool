using BusinessLogic.Entities;
using Shared;

namespace BusinessLogic.Commands;

public class CreatePathWayCondition : IUndoCommand
{
    internal LearningWorld LearningWorld { get; }
    internal PathWayCondition PathWayCondition { get; }
    private readonly Action<LearningWorld> _mappingAction;
    private IMemento? _memento;

    public CreatePathWayCondition(LearningWorld learningWorld, ConditionEnum condition, double positionX,
        double positionY, Action<LearningWorld> mappingAction)
    {
        PathWayCondition = new PathWayCondition(condition, positionX, positionY);
        LearningWorld = learningWorld;
        _mappingAction = mappingAction;
    }

    public void Execute()
    {
        _memento = LearningWorld.GetMemento();

        LearningWorld.PathWayConditions.Add(PathWayCondition);
        LearningWorld.SelectedLearningObject = PathWayCondition;
        
        _mappingAction.Invoke(LearningWorld);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }
        
        LearningWorld.RestoreMemento(_memento);
        
        _mappingAction.Invoke(LearningWorld);
    }

    public void Redo() => Execute();
}