using BusinessLogic.Entities;

namespace BusinessLogic.Commands;

public class DeleteLearningPathWay : IUndoCommand
{
    internal LearningWorld LearningWorld { get; set; }
    internal LearningSpace TargetSpace { get; set; }
    private readonly Action<LearningWorld> _mappingAction;
    private IMemento? _memento;
    public DeleteLearningPathWay(LearningWorld learningWorld, LearningSpace targetSpace,
        Action<LearningWorld> mappingAction)
    {
        LearningWorld = learningWorld;
        TargetSpace = targetSpace;
        _mappingAction = mappingAction;
    }
    public void Execute()
    {
        _memento = LearningWorld.GetMemento();
        
        var learningPathWay = LearningWorld.LearningPathways.Last(x => x.TargetSpace.Id == TargetSpace.Id);
        LearningWorld.LearningPathways.Remove(learningPathWay);
        
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