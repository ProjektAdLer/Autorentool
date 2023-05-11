using BusinessLogic.Entities;
using Shared;

namespace BusinessLogic.Commands.Condition;

public class EditPathWayCondition : IEditPathWayCondition
{
    public string Name => nameof(EditPathWayCondition);
    internal PathWayCondition PathWayCondition { get; }
    internal readonly ConditionEnum Condition;
    internal readonly Action<PathWayCondition> MappingAction;
    private IMemento? _memento;

    public EditPathWayCondition(PathWayCondition pathWayCondition, ConditionEnum condition, Action<PathWayCondition> mappingAction)
    {
        PathWayCondition = pathWayCondition;
        Condition = condition;
        MappingAction = mappingAction;
    }

    public void Execute()
    {
        _memento = PathWayCondition.GetMemento();

        PathWayCondition.Condition = Condition;
        
        MappingAction.Invoke(PathWayCondition);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }
        
        PathWayCondition.RestoreMemento(_memento);
        
        MappingAction.Invoke(PathWayCondition);
    }

    public void Redo() => Execute();
}