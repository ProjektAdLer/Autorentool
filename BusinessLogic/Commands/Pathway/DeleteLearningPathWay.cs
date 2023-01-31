using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Pathway;

public class DeleteLearningPathWay : IUndoCommand
{
    internal LearningWorld LearningWorld { get; }
    internal LearningPathway LearningPathway { get; }
    private readonly Action<LearningWorld> _mappingAction;
    private IMemento? _memento;
    public DeleteLearningPathWay(LearningWorld learningWorld, LearningPathway learningPathway,
        Action<LearningWorld> mappingAction)
    {
        LearningWorld = learningWorld;
        LearningPathway = learningWorld.LearningPathways.Single(x => x.Id == learningPathway.Id);
        _mappingAction = mappingAction;
    }
    public void Execute()
    {
        _memento = LearningWorld.GetMemento();
        
        LearningWorld.LearningPathways.Remove(LearningPathway);
        
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