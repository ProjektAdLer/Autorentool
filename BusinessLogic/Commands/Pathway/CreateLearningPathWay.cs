using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Pathway;

public class CreateLearningPathWay : ICreateLearningPathWay
{
    public string Name => nameof(CreateLearningPathWay);
    public bool HasError { get; private set; }
    internal LearningWorld LearningWorld { get; }
    internal LearningPathway LearningPathway { get; }
    internal Action<LearningWorld> MappingAction { get; }
    private IMemento? _memento;

    public CreateLearningPathWay(LearningWorld learningWorld, IObjectInPathWay sourceObject, IObjectInPathWay targetObject,
        Action<LearningWorld> mappingAction)
    {
        LearningPathway = new LearningPathway(sourceObject, targetObject);
        LearningWorld = learningWorld;
        MappingAction = mappingAction;
    }

    
    public void Execute()
    {
        if( LearningWorld.LearningPathways.
                Any(x => x.SourceObject == LearningPathway.SourceObject && x.TargetObject == LearningPathway.TargetObject) 
            || LearningPathway.SourceObject.Id == LearningPathway.TargetObject.Id || IsCircular(LearningPathway)
            || (LearningPathway.TargetObject is LearningSpace && LearningWorld.LearningPathways.Any(x => x.TargetObject.Id == LearningPathway.TargetObject.Id)))
        {
            HasError = true;
            return;
        }
            
        _memento = LearningWorld.GetMemento();
        
        LearningWorld.UnsavedChanges = true;
        LearningWorld.LearningPathways.Add(LearningPathway); 
        
        MappingAction.Invoke(LearningWorld);
    }
    
    private bool IsCircular(LearningPathway learningPathway)
    {
        var isCircular = false;
        var sourceObject = LearningWorld.ObjectsInPathWays.First(x => x.Id == learningPathway.SourceObject.Id);
        var targetObject = LearningWorld.ObjectsInPathWays.First(x => x.Id == learningPathway.TargetObject.Id);

        if(!targetObject.OutBoundObjects.Any())
        {
            return false;
        }
        var outBoundObject = targetObject.OutBoundObjects;
        var outBoundObjectSum = new List<IObjectInPathWay>();
        
        while (isCircular == false)
        {
            foreach (var space in outBoundObject)
            {
                if(space.Id == sourceObject.Id)
                {
                    isCircular = true;
                }
                outBoundObjectSum.AddRange(space.OutBoundObjects);
            }
            if (!outBoundObject.Any())
            {
                break;
            }
            outBoundObject  = outBoundObjectSum;
            outBoundObjectSum = new List<IObjectInPathWay>();
        }
        return isCircular;
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