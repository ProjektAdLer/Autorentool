using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Element;

public class DeleteLearningElementInWorld : IDeleteLearningElementInWorld
{
    public string Name => nameof(DeleteLearningElementInWorld);
    internal LearningElement LearningElement { get; }
    internal LearningWorld ParentWorld { get; }
    internal Action<LearningWorld> MappingAction { get; }
    private IMemento? _memento;
    
    public DeleteLearningElementInWorld(LearningElement learningElement, LearningWorld parentWorld,
        Action<LearningWorld> mappingAction)
    {
        LearningElement = learningElement;
        ParentWorld = parentWorld;
        MappingAction = mappingAction;
    }
    
    public void Execute()
    {
        _memento = ParentWorld.GetMemento();

        ParentWorld.UnsavedChanges = true;
        var element = ParentWorld.UnplacedLearningElements.First(x => x.Id == LearningElement.Id);
        ParentWorld.UnplacedLearningElements.Remove(element);

        MappingAction.Invoke(ParentWorld);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }

        ParentWorld.RestoreMemento(_memento);

        MappingAction.Invoke(ParentWorld);
    }

    public void Redo() => Execute();
}