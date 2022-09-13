using BusinessLogic.API;
using BusinessLogic.Entities;

namespace BusinessLogic.Commands;

public class LoadLearningElement : IUndoCommand
{
    private readonly IBusinessLogic _businessLogic;
    
    internal ILearningElementParent ElementParent { get; }
    private LearningElement? _learningElement;
    private readonly string _filepath;
    private readonly Action<ILearningElementParent> _mappingAction;
    private IMemento? _memento;
    
    public LoadLearningElement(ILearningElementParent elementParent, string filepath, IBusinessLogic businessLogic, 
        Action<ILearningElementParent> mappingAction)
    {
        ElementParent = elementParent;
        _filepath = filepath;
        _businessLogic = businessLogic;
        _mappingAction = mappingAction;
    }
    public void Execute()
    {
        _memento = ElementParent.GetMemento();
        
        _learningElement ??= _businessLogic.LoadLearningElement(_filepath);
        ElementParent.LearningElements.Add(_learningElement);
        
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