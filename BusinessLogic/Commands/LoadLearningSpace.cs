using BusinessLogic.API;
using BusinessLogic.Entities;

namespace BusinessLogic.Commands;

public class LoadLearningSpace : IUndoCommand
{
    private readonly IBusinessLogic _businessLogic;
    
    private readonly LearningWorld _learningWorld;
    private LearningSpace? _learningSpace;
    private readonly string _filepath;
    private readonly Action<LearningWorld> _mappingAction;
    private IMemento? _memento;

    public LoadLearningSpace(LearningWorld learningWorld, string filepath, IBusinessLogic businessLogic,
        Action<LearningWorld> mappingAction)
    {
        _learningWorld = learningWorld;
        _filepath = filepath;
        _businessLogic = businessLogic;
        _mappingAction = mappingAction;
    }
    public void Execute()
    {
        _memento = _learningWorld.GetMemento();
        
        _learningSpace = _businessLogic.LoadLearningSpace(_filepath);
        _learningWorld.LearningSpaces.Add(_learningSpace);
        
        _mappingAction.Invoke(_learningWorld);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }
        
        _learningWorld.RestoreMemento(_memento);
        
        _mappingAction.Invoke(_learningWorld);
    }

    public void Redo()
    {
        _memento = _learningWorld.GetMemento();
        
        if (_learningSpace != null) _learningWorld.LearningSpaces.Add(_learningSpace);

        _mappingAction.Invoke(_learningWorld);
    }
}