using BusinessLogic.API;
using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Space;

public class LoadLearningSpace : IUndoCommand
{
    public string Name => nameof(LoadLearningSpace);
    private readonly IBusinessLogic _businessLogic;
    
    internal LearningWorld LearningWorld { get; }
    internal LearningSpace? LearningSpace;
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
    
    public LoadLearningSpace(LearningWorld learningWorld, Stream stream, IBusinessLogic businessLogic,
        Action<LearningWorld> mappingAction)
    {
        LearningWorld = learningWorld;
        _filepath = "";
        _businessLogic = businessLogic;
        LearningSpace = _businessLogic.LoadLearningSpace(stream);
        _mappingAction = mappingAction;
    }
    public void Execute()
    {
        _memento = LearningWorld.GetMemento();
        
        LearningSpace ??= _businessLogic.LoadLearningSpace(_filepath);
        LearningWorld.LearningSpaces.Add(LearningSpace);

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