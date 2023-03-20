using BusinessLogic.Entities;
using Shared;

namespace BusinessLogic.Commands.Condition;

public class EditPathWayCondition : IUndoCommand
{
    public string Name => nameof(EditPathWayCondition);
    internal PathWayCondition PathWayCondition { get; }
    private readonly ConditionEnum _condition;
    private readonly Action<PathWayCondition> _mappingAction;
    private IMemento? _memento;

    public EditPathWayCondition(PathWayCondition pathWayCondition, ConditionEnum condition, Action<PathWayCondition> mappingAction)
    {
        PathWayCondition = pathWayCondition;
        _condition = condition;
        _mappingAction = mappingAction;
    }

    public void Execute()
    {
        _memento = PathWayCondition.GetMemento();

        PathWayCondition.Condition = _condition;
        
        _mappingAction.Invoke(PathWayCondition);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }
        
        PathWayCondition.RestoreMemento(_memento);
        
        _mappingAction.Invoke(PathWayCondition);
    }

    public void Redo() => Execute();
}