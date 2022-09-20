using BusinessLogic.API;
using BusinessLogic.Entities;

namespace BusinessLogic.Commands;

public class LoadLearningSpace : IUndoCommand
{
    private readonly IBusinessLogic _businessLogic;
    
    internal LearningWorld LearningWorld { get; }
    private LearningSpace? _learningSpace;
    private readonly string _filepath;
    private readonly Action<LearningWorld> _mappingAction;
    private IMemento? _memento;

    public LoadLearningSpace(LearningWorld learningWorld, string filepath, IBusinessLogic businessLogic,
        Action<LearningWorld> mappingAction)
    {
        LearningWorld = learningWorld;
        _filepath = filepath;
        _businessLogic = businessLogic;
        _mappingAction = mappingAction;
    }
    public void Execute()
    {
        _memento = LearningWorld.GetMemento();
        
        _learningSpace ??= _businessLogic.LoadLearningSpace(_filepath);
        LearningWorld.LearningSpaces.Add(_learningSpace);
        
        _mappingAction.Invoke(LearningWorld);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }
        
        LearningWorld.RestoreMemento(_memento);
        
        _mappingAction.Invoke(LearningWorld);
    }

    public void Redo() => Execute();
}