using BusinessLogic.API;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.Space;

public class LoadLearningSpace : ILoadLearningSpace
{
    private IMemento? _memento;
    internal LearningSpace? LearningSpace;

    public LoadLearningSpace(LearningWorld learningWorld, string filepath, IBusinessLogic businessLogic,
        Action<LearningWorld> mappingAction, ILogger<LoadLearningSpace> logger)
    {
        LearningWorld = learningWorld;
        Filepath = filepath;
        BusinessLogic = businessLogic;
        MappingAction = mappingAction;
        Logger = logger;
    }

    public LoadLearningSpace(LearningWorld learningWorld, Stream stream, IBusinessLogic businessLogic,
        Action<LearningWorld> mappingAction, ILogger<LoadLearningSpace> logger)
    {
        LearningWorld = learningWorld;
        Filepath = "";
        BusinessLogic = businessLogic;
        LearningSpace = BusinessLogic.LoadLearningSpace(stream);
        MappingAction = mappingAction;
        Logger = logger;
    }

    internal IBusinessLogic BusinessLogic { get; }

    internal LearningWorld LearningWorld { get; }
    internal string Filepath { get; }
    internal Action<LearningWorld> MappingAction { get; }
    private ILogger<LoadLearningSpace> Logger { get; }
    public string Name => nameof(LoadLearningSpace);

    public void Execute()
    {
        _memento = LearningWorld.GetMemento();

        LearningSpace ??= BusinessLogic.LoadLearningSpace(Filepath);
        LearningWorld.LearningSpaces.Add(LearningSpace);

        Logger.LogTrace("Loaded LearningSpace {LearningSpaceName} ({LearningSpaceId}) from file {FilePath}",
            LearningSpace.Name, LearningSpace.Id, Filepath);

        MappingAction.Invoke(LearningWorld);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }

        LearningWorld.RestoreMemento(_memento);

        Logger.LogTrace("Undone loading of LearningSpace");

        MappingAction.Invoke(LearningWorld);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing LoadLearningSpace");
        Execute();
    }
}