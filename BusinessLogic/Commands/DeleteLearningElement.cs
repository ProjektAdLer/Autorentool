using BusinessLogic.Entities;

namespace BusinessLogic.Commands;

public class DeleteLearningElement : IUndoCommand
{
    internal LearningElement LearningElement { get; }
    internal ILearningElementParent ElementParent { get; }
    private readonly Action<ILearningElementParent> _mappingAction;
    private IMemento? _memento;

    public DeleteLearningElement(LearningElement learningElement, ILearningElementParent elementParent,
        Action<ILearningElementParent> mappingAction)
    {
        LearningElement = learningElement;
        ElementParent = elementParent;
        _mappingAction = mappingAction;
    }
    
    public void Execute()
    {
        _memento = ElementParent.GetMemento();

        var element = ElementParent.LearningElements.First(x => x.Id == LearningElement.Id);

        ElementParent.LearningElements.Remove(element);
        
        _mappingAction.Invoke(ElementParent);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }
        
        ElementParent.RestoreMemento(_memento);
        
        _mappingAction.Invoke(ElementParent);
    }

    public void Redo() => Execute();
}