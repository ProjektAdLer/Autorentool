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
    
    public LoadLearningElement(ILearningElementParent elementParent, Stream stream, IBusinessLogic businessLogic,
        Action<ILearningElementParent> mappingAction)
    {
        ElementParent = elementParent;
        _filepath = "";
        _businessLogic = businessLogic;
        _learningElement = _businessLogic.LoadLearningElement(stream);
        _mappingAction = mappingAction;
    }
    public void Execute()
    {
        _memento = ElementParent.GetMemento();
        
        _learningElement ??= _businessLogic.LoadLearningElement(_filepath);
        _learningElement.Parent = ElementParent;
        ElementParent.LearningElements.Add(_learningElement);
        ElementParent.SelectedLearningObject = _learningElement;
        
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