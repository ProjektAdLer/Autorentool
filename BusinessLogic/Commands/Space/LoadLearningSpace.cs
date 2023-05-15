using BusinessLogic.API;
using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Space;

public class LoadLearningSpace : ILoadLearningSpace
{
    public string Name => nameof(LoadLearningSpace);
    internal IBusinessLogic BusinessLogic { get; }
    
    internal LearningWorld LearningWorld { get; }
    internal LearningSpace? LearningSpace;
    internal string Filepath { get; }
    internal Action<LearningWorld> MappingAction { get; }
    private IMemento? _memento;

    public LoadLearningSpace(LearningWorld learningWorld, string filepath, IBusinessLogic businessLogic,
        Action<LearningWorld> mappingAction)
    {
        LearningWorld = learningWorld;
        Filepath = filepath;
        BusinessLogic = businessLogic;
        MappingAction = mappingAction;
    }
    
    public LoadLearningSpace(LearningWorld learningWorld, Stream stream, IBusinessLogic businessLogic,
        Action<LearningWorld> mappingAction)
    {
        LearningWorld = learningWorld;
        Filepath = "";
        BusinessLogic = businessLogic;
        LearningSpace = BusinessLogic.LoadLearningSpace(stream);
        MappingAction = mappingAction;
    }
    public void Execute()
    {
        _memento = LearningWorld.GetMemento();
        
        LearningSpace ??= BusinessLogic.LoadLearningSpace(Filepath);
        LearningWorld.LearningSpaces.Add(LearningSpace);

        MappingAction.Invoke(LearningWorld);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }
        
        LearningWorld.RestoreMemento(_memento);
        
        MappingAction.Invoke(LearningWorld);
    }

    public void Redo() => Execute();
}