using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.Condition;

public class DeletePathWayCondition : IDeletePathWayCondition
{
    private IMemento? _memento;

    public DeletePathWayCondition(LearningWorld learningWorld, PathWayCondition pathWayCondition,
        Action<LearningWorld> mappingAction, ILogger<DeletePathWayCondition> logger)
    {
        LearningWorld = learningWorld;
        PathWayCondition = pathWayCondition;
        MappingAction = mappingAction;
        Logger = logger;
    }

    internal LearningWorld LearningWorld { get; }
    internal PathWayCondition PathWayCondition { get; }
    internal Action<LearningWorld> MappingAction { get; }
    private ILogger<DeletePathWayCondition> Logger { get; }
    public string Name => nameof(DeletePathWayCondition);

    public void Execute()
    {
        _memento = LearningWorld.GetMemento();

        var pathWayCondition = LearningWorld.PathWayConditions.First(x => x.Id == PathWayCondition.Id);

        foreach (var inBoundSpace in pathWayCondition.InBoundObjects)
        {
            LearningWorld.LearningPathways
                .Where(x => x.SourceObject.Id == inBoundSpace.Id && x.TargetObject.Id == pathWayCondition.Id)
                .ToList().ForEach(x =>
                {
                    LearningWorld.LearningPathways.Remove(x);
                    Logger.LogTrace("Removed LearningPathway from {Source} to {Target}", x.SourceObject.Id,
                        x.TargetObject.Id);
                });
        }

        foreach (var outBoundSpace in pathWayCondition.OutBoundObjects)
        {
            LearningWorld.LearningPathways
                .Where(x => x.SourceObject.Id == pathWayCondition.Id && x.TargetObject.Id == outBoundSpace.Id)
                .ToList().ForEach(x =>
                {
                    LearningWorld.LearningPathways.Remove(x);
                    Logger.LogTrace("Removed LearningPathway from {Source} to {Target}", x.SourceObject.Id,
                        x.TargetObject.Id);
                });
        }

        LearningWorld.PathWayConditions.Remove(pathWayCondition);

        Logger.LogTrace("Deleted PathWayCondition {PathWayCondition} ({Id})", PathWayCondition.Condition,
            PathWayCondition.Id);

        MappingAction.Invoke(LearningWorld);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }

        LearningWorld.RestoreMemento(_memento);

        Logger.LogTrace(
            "DeletePathWayCondition undone. Restored LearningWorld {LearningWorldName} ({LearningWorldId}) to previous state",
            LearningWorld.Name, LearningWorld.Id);

        MappingAction.Invoke(LearningWorld);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing DeletePathWayCondition");
        Execute();
    }
}