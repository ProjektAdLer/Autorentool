using BusinessLogic.Entities;

namespace BusinessLogic.Commands;

public class DeleteLearningElement : IUndoCommand
{
    internal LearningElement LearningElement { get; }
    internal LearningSpace ParentSpace { get; }
    private readonly Action<LearningSpace> _mappingAction;
    private IMemento? _memento;

    public DeleteLearningElement(LearningElement learningElement, LearningSpace parentSpace,
        Action<LearningSpace> mappingAction)
    {
        LearningElement = learningElement;
        ParentSpace = parentSpace;
        _mappingAction = mappingAction;
    }
    
    public void Execute()
    {
        _memento = ParentSpace.GetMemento();

        var element = ParentSpace.LearningElements.First(x => x.Id == LearningElement.Id);

        ParentSpace.LearningElements.Remove(element);

        ParentSpace.SelectedLearningElement = ParentSpace.LearningElements.LastOrDefault();

        _mappingAction.Invoke(ParentSpace);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }
        
        ParentSpace.RestoreMemento(_memento);
        
        _mappingAction.Invoke(ParentSpace);
    }

    public void Redo() => Execute();
}