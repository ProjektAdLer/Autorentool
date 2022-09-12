using BusinessLogic.API;
using BusinessLogic.Entities;

namespace BusinessLogic.Commands;

public class LoadLearningElement : IUndoCommand
{
    private readonly IBusinessLogic _businessLogic;
    
    private readonly ILearningElementParent _elementParent;
    private LearningElement? _learningElement;
    private readonly string _filepath;
    private readonly Action<ILearningElementParent> _mappingAction;
    private IMemento? _memento;
    
    public LoadLearningElement(ILearningElementParent elementParent, string filepath, IBusinessLogic businessLogic, 
        Action<ILearningElementParent> mappingAction)
    {
        _elementParent = elementParent;
        _filepath = filepath;
        _businessLogic = businessLogic;
        _mappingAction = mappingAction;
    }
    public void Execute()
    {
        _memento = _elementParent.GetMemento();
        
        _learningElement = _businessLogic.LoadLearningElement(_filepath);
        _elementParent.LearningElements.Add(_learningElement);
        
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

    public void Redo()
    {
        _memento = _elementParent.GetMemento();
        
        if(_learningElement != null) _elementParent.LearningElements.Add(_learningElement);
        
        _mappingAction.Invoke(_elementParent);
    }
}