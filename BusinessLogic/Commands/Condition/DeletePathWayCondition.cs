using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Condition;

public class DeletePathWayCondition : IDeletePathWayCondition
{
    public string Name => nameof(DeletePathWayCondition);
    internal LearningWorld LearningWorld { get; }
    internal PathWayCondition PathWayCondition { get; }
    internal Action<LearningWorld> MappingAction { get; }
    private IMemento? _memento;

    public DeletePathWayCondition(LearningWorld learningWorld, PathWayCondition pathWayCondition,
        Action<LearningWorld> mappingAction)
    {
        LearningWorld = learningWorld;
        PathWayCondition = pathWayCondition;
        MappingAction = mappingAction;
    }

    public void Execute()
    {
        _memento = LearningWorld.GetMemento();

        var pathWayCondition = LearningWorld.PathWayConditions.First(x => x.Id == PathWayCondition.Id);

        foreach (var inBoundSpace in pathWayCondition.InBoundObjects)
        {
            LearningWorld.LearningPathways
                .Where(x => x.SourceObject.Id == inBoundSpace.Id && x.TargetObject.Id == pathWayCondition.Id)
                .ToList().ForEach(x => LearningWorld.LearningPathways.Remove(x));
        }
        foreach (var outBoundSpace in pathWayCondition.OutBoundObjects)
        {
            LearningWorld.LearningPathways
                .Where(x => x.SourceObject.Id == pathWayCondition.Id && x.TargetObject.Id == outBoundSpace.Id)
                .ToList().ForEach(x => LearningWorld.LearningPathways.Remove(x));
        }
        LearningWorld.PathWayConditions.Remove(pathWayCondition);

        MappingAction.Invoke(LearningWorld);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }

        LearningWorld.RestoreMemento(_memento);

        MappingAction.Invoke(LearningWorld);
    }

    public void Redo() => Execute();
}