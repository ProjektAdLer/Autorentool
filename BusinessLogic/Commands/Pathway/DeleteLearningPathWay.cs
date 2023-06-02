using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Pathway;

public class DeleteLearningPathWay : IDeleteLearningPathWay
{
    public string Name => nameof(DeleteLearningPathWay);
    internal LearningWorld LearningWorld { get; }
    internal LearningPathway LearningPathway { get; }
    internal Action<LearningWorld> MappingAction { get; }
    private IMemento? _memento;
    public DeleteLearningPathWay(LearningWorld learningWorld, LearningPathway learningPathway,
        Action<LearningWorld> mappingAction)
    {
        LearningWorld = learningWorld;
        LearningPathway = learningWorld.LearningPathways.Single(x => x.Id == learningPathway.Id);
        MappingAction = mappingAction;
    }
    public void Execute()
    {
        _memento = LearningWorld.GetMemento();
        
        LearningWorld.UnsavedChanges = true;
        LearningWorld.LearningPathways.Remove(LearningPathway);
        
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