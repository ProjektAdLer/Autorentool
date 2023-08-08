using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;
using Shared;

namespace BusinessLogic.Commands.Condition;

public class CreatePathWayCondition : ICreatePathWayCondition
{
    internal readonly Action<LearningWorld> MappingAction;
    private IMemento? _memento;

    public CreatePathWayCondition(LearningWorld learningWorld, ConditionEnum condition, double positionX,
        double positionY, Action<LearningWorld> mappingAction, ILogger<CreatePathWayCondition> logger)
    {
        PathWayCondition = new PathWayCondition(condition, positionX, positionY);
        LearningWorld = learningWorld;
        MappingAction = mappingAction;
        Logger = logger;
    }

    public CreatePathWayCondition(LearningWorld learningWorld, ConditionEnum condition, IObjectInPathWay sourceObject,
        ISelectableObjectInWorld targetObject, Action<LearningWorld> mappingAction,
        ILogger<CreatePathWayCondition> logger)
    {
        LearningWorld = learningWorld;
        SourceObject = LearningWorld.ObjectsInPathWays.First(x => x.Id == sourceObject.Id);
        TargetObject = LearningWorld.LearningSpaces.First(x => x.Id == targetObject.Id);
        PathWayCondition = new PathWayCondition(condition, TargetObject.PositionX + 42, TargetObject.PositionY - 60);
        MappingAction = mappingAction;
        Logger = logger;
    }

    internal LearningWorld LearningWorld { get; }
    internal PathWayCondition PathWayCondition { get; }
    internal IObjectInPathWay? SourceObject { get; }
    internal ILearningSpace? TargetObject { get; }
    private ILogger<CreatePathWayCondition> Logger { get; }
    public string Name => nameof(CreatePathWayCondition);

    public void Execute()
    {
        _memento = LearningWorld.GetMemento();

        LearningWorld.PathWayConditions.Add(PathWayCondition);

        Logger.LogTrace(
            "Created PathWayCondition {PathWayCondition} ({Id}) in LearningWorld {LearningWorldName} ({LearningWorldId})",
            PathWayCondition.Condition, PathWayCondition.Id, LearningWorld.Name, LearningWorld.Id);

        if (SourceObject != null && TargetObject != null)
        {
            var previousInBoundObject = TargetObject.InBoundObjects.FirstOrDefault();
            var previousPathWay = LearningWorld.LearningPathways.FirstOrDefault(pw =>
                pw.TargetObject.Id == TargetObject.Id);
            if (previousPathWay == null)
                throw new ApplicationException("Previous pathway is null");
            if (previousInBoundObject == null)
                throw new ApplicationException("Previous in bound object is null");

            LearningWorld.LearningPathways.Remove(previousPathWay);
            LearningWorld.LearningPathways.Add(new LearningPathway(SourceObject, PathWayCondition));
            LearningWorld.LearningPathways.Add(new LearningPathway(previousInBoundObject, PathWayCondition));
            LearningWorld.LearningPathways.Add(new LearningPathway(PathWayCondition, TargetObject));
            Logger.LogTrace("Removed LearningPathway from {Target} to {Source}", previousPathWay.SourceObject.Id,
                previousPathWay.TargetObject.Id);
            Logger.LogTrace("Created LearningPathway from {Source} to {Target}", SourceObject.Id, PathWayCondition.Id);
            Logger.LogTrace("Created LearningPathway from {Source} to {Target}", previousInBoundObject.Id,
                PathWayCondition.Id);
            Logger.LogTrace("Created LearningPathway from {Source} to {Target}", PathWayCondition.Id, TargetObject.Id);
        }

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
            "CreatePathWayCondition undone. Restored LearningWorld {LearningWorldName} ({LearningWorldId}) to previous state without PathWayCondition {PathWayCondition} ({PathWayConditionId})"
            , LearningWorld.Name, LearningWorld.Id, PathWayCondition.Condition, PathWayCondition.Id);

        MappingAction.Invoke(LearningWorld);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing CreatePathWayCondition");
        Execute();
    }
}