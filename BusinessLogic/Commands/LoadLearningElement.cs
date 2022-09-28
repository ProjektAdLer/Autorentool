using BusinessLogic.API;
using BusinessLogic.Entities;

namespace BusinessLogic.Commands;

public class LoadLearningElement : IUndoCommand
{
    private readonly IBusinessLogic _businessLogic;
    
    internal LearningSpace ParentSpace { get; }
    private LearningElement? _learningElement;
    private readonly string _filepath;
    private readonly Action<LearningSpace> _mappingAction;
    private IMemento? _memento;
    
    public LoadLearningElement(LearningSpace parentSpace, string filepath, IBusinessLogic businessLogic, 
        Action<LearningSpace> mappingAction)
    {
        ParentSpace = parentSpace;
        _filepath = filepath;
        _businessLogic = businessLogic;
        _mappingAction = mappingAction;
    }
    
    public LoadLearningElement(LearningSpace parentSpace, Stream stream, IBusinessLogic businessLogic,
        Action<LearningSpace> mappingAction)
    {
        ParentSpace = parentSpace;
        _filepath = "";
        _businessLogic = businessLogic;
        _learningElement = _businessLogic.LoadLearningElement(stream);
        _mappingAction = mappingAction;
    }
    public void Execute()
    {
        _memento = ParentSpace.GetMemento();
        
        _learningElement ??= _businessLogic.LoadLearningElement(_filepath);
        _learningElement.Parent = ParentSpace;
        ParentSpace.LearningElements.Add(_learningElement);
        ParentSpace.SelectedLearningElement = _learningElement;
        
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