using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Condition;

public class DeletePathWayCondition : IUndoCommand
{
    public string Name => nameof(DeletePathWayCondition);
    internal LearningWorld LearningWorld { get; }
    internal PathWayCondition PathWayCondition { get; }
    private readonly Action<LearningWorld> _mappingAction;
    private IMemento? _memento;

    public DeletePathWayCondition(LearningWorld learningWorld, PathWayCondition pathWayCondition,
        Action<LearningWorld> mappingAction)
    {
        LearningWorld = learningWorld;
        PathWayCondition = pathWayCondition;
        _mappingAction = mappingAction;
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

        if (pathWayCondition == LearningWorld.SelectedLearningObject || LearningWorld.SelectedLearningObject == null)
        {
            LearningWorld.SelectedLearningObject = LearningWorld.SelectableWorldObjects.LastOrDefault();
        }

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