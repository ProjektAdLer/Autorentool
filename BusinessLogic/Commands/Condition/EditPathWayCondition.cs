using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;
using Shared;

namespace BusinessLogic.Commands.Condition;

public class EditPathWayCondition : IEditPathWayCondition
{
    private IMemento? _memento;

    public EditPathWayCondition(PathWayCondition pathWayCondition, ConditionEnum condition,
        Action<PathWayCondition> mappingAction, ILogger<EditPathWayCondition> logger)
    {
        PathWayCondition = pathWayCondition;
        Condition = condition;
        MappingAction = mappingAction;
        Logger = logger;
    }

    internal PathWayCondition PathWayCondition { get; }
    internal ConditionEnum Condition { get; }
    internal Action<PathWayCondition> MappingAction { get; }
    private ILogger<EditPathWayCondition> Logger { get; }
    public string Name => nameof(EditPathWayCondition);

    public void Execute()
    {
        _memento = PathWayCondition.GetMemento();

        PathWayCondition.Condition = Condition;

        Logger.LogTrace("Edited PathWayCondition {PathWayConditionId} to condition {Condition}", PathWayCondition.Id,
            Condition);

        MappingAction.Invoke(PathWayCondition);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }

        PathWayCondition.RestoreMemento(_memento);

        Logger.LogTrace("Undone edit of PathWayCondition {PathWayConditionId}. Restored to previous condition",
            PathWayCondition.Id);

        MappingAction.Invoke(PathWayCondition);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing EditPathWayCondition");
        Execute();
    }
}