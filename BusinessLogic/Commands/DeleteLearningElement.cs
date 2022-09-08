using BusinessLogic.Entities;

namespace BusinessLogic.Commands;

public class DeleteLearningElement : IUndoCommand
{
    private readonly LearningElement _learningElement;
    private readonly ILearningElementParent _elementParent;
    private readonly Action<ILearningElementParent> _mappingAction;
    private IMemento? _memento;

    public DeleteLearningElement(LearningElement learningElement, ILearningElementParent elementParent,
        Action<ILearningElementParent> mappingAction)
    {
        _learningElement = learningElement;
        _elementParent = elementParent;
        _mappingAction = mappingAction;
    }
    
    public void Execute()
    {
        _memento = _elementParent.GetMemento();

        var element = _elementParent.LearningElements.First(x => x.Id == _learningElement.Id);

        _elementParent.LearningElements.Remove(element);
        
        _mappingAction.Invoke(_elementParent);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }
        
        _elementParent.RestoreMemento(_memento);
        
        _mappingAction.Invoke(_elementParent);
    }

    public void Redo() => Execute();
}