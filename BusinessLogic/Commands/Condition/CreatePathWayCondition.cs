using BusinessLogic.Entities;
using Shared;

namespace BusinessLogic.Commands.Condition;

public class CreatePathWayCondition : IUndoCommand
{
    internal LearningWorld LearningWorld { get; }
    internal PathWayCondition PathWayCondition { get; }
    internal IObjectInPathWay? SourceObject { get; }
    internal LearningSpace? TargetObject { get; }
    private readonly Action<LearningWorld> _mappingAction;
    private IMemento? _memento;

    public CreatePathWayCondition(LearningWorld learningWorld, ConditionEnum condition, double positionX,
        double positionY, Action<LearningWorld> mappingAction)
    {
        PathWayCondition = new PathWayCondition(condition, positionX, positionY);
        LearningWorld = learningWorld;
        _mappingAction = mappingAction;
    }

    public CreatePathWayCondition(LearningWorld learningWorld, ConditionEnum condition, IObjectInPathWay sourceObject,
        LearningSpace targetObject, Action<LearningWorld> mappingAction)
    {
        LearningWorld = learningWorld;
        SourceObject = LearningWorld.ObjectsInPathWays.First(x => x.Id == sourceObject.Id);
        TargetObject = LearningWorld.LearningSpaces.First(x => x.Id == targetObject.Id);
        PathWayCondition = new PathWayCondition(condition, TargetObject.PositionX +42, TargetObject.PositionY - 60);
        _mappingAction = mappingAction;
    }

    public void Execute()
    {
        _memento = LearningWorld.GetMemento();

        LearningWorld.PathWayConditions.Add(PathWayCondition);
        LearningWorld.SelectedLearningObject = PathWayCondition;
        
        if (SourceObject != null && TargetObject != null)
        {
            var previousInBoundObject = TargetObject.InBoundObjects.FirstOrDefault();
            var previousPathWay = LearningWorld.LearningPathways.FirstOrDefault(pw =>
                pw.TargetObject.Id == TargetObject.Id);
            if(previousPathWay == null)
                throw new ApplicationException("Previous pathway is null");
            if(previousInBoundObject == null)
                throw new ApplicationException("Previous in bound object is null");
                
            LearningWorld.LearningPathways.Remove(previousPathWay);
            LearningWorld.LearningPathways.Add(new LearningPathway(SourceObject, PathWayCondition));
            LearningWorld.LearningPathways.Add(new LearningPathway(previousInBoundObject, PathWayCondition));
            LearningWorld.LearningPathways.Add(new LearningPathway(PathWayCondition, TargetObject));
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