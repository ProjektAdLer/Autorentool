using BusinessLogic.Entities;

namespace BusinessLogic.Commands;

public class CreateLearningPathWay : IUndoCommand, ICommandWithError
{
    public bool HasError { get; private set; }
    internal LearningWorld LearningWorld { get; }
    internal LearningPathway LearningPathway { get; }
    private readonly Action<LearningWorld> _mappingAction;
    private IMemento? _memento;

    public CreateLearningPathWay(LearningWorld learningWorld, LearningSpace sourceSpace, LearningSpace targetSpace,
        Action<LearningWorld> mappingAction)
    {
        LearningPathway = new LearningPathway(sourceSpace, targetSpace);
        LearningWorld = learningWorld;
        _mappingAction = mappingAction;
    }
    public void Execute()
    {
        if(LearningWorld.LearningPathways.Contains(LearningPathway ) ||
           LearningPathway.SourceSpace.Id == LearningPathway.TargetSpace.Id || IsCircular(LearningPathway))
        {
            HasError = true;
            return;
        }
            
        _memento = LearningWorld.GetMemento();
        
        LearningWorld.LearningPathways.Add(LearningPathway); 
        
        _mappingAction.Invoke(LearningWorld);
    }
    
    private bool IsCircular(LearningPathway learningPathway)
    {
        var isCircular = false;
        var sourceSpace = LearningWorld.LearningSpaces.First(x => x.Id == learningPathway.SourceSpace.Id);
        var targetSpace = LearningWorld.LearningSpaces.First(x => x.Id == learningPathway.TargetSpace.Id);

        if(!targetSpace.OutBoundSpaces.Any())
        {
            return false;
        }
        var outBoundSpaces = targetSpace.OutBoundSpaces;
        var outBoundSpacesSum = new List<LearningSpace>();
        
        while (isCircular == false)
        {
            foreach (var space in outBoundSpaces)
            {
                if(space.Id == sourceSpace.Id)
                {
                    isCircular = true;
                }
                outBoundSpacesSum.AddRange(space.OutBoundSpaces);
            }
            if (!outBoundSpaces.Any())
            {
                break;
            }
            outBoundSpaces  = outBoundSpacesSum;
            outBoundSpacesSum = new List<LearningSpace>();
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
        
        _mappingAction.Invoke(LearningWorld);
    }

    public void Redo() => Execute();
}