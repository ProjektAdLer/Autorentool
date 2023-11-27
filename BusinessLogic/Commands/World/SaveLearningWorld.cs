using BusinessLogic.API;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.Configuration;

namespace BusinessLogic.Commands.World;

public class SaveLearningWorld : ISaveLearningWorld
{
    public SaveLearningWorld(IBusinessLogic businessLogic, LearningWorld learningWorld, string filepath,
        Action<LearningWorld> mappingAction, ILogger<SaveLearningWorld> logger)
    {
        BusinessLogic = businessLogic;
        LearningWorld = learningWorld;
        Filepath = filepath;
        MappingAction = mappingAction;
        Logger = logger;
    }

    internal IBusinessLogic BusinessLogic { get; }
    internal LearningWorld LearningWorld { get; }
    internal string Filepath { get; private set; }
    internal Action<LearningWorld> MappingAction { get; }
    private ILogger<SaveLearningWorld> Logger { get; }
    public string Name => nameof(SaveLearningWorld);

    public void Execute()
    {
        if (string.IsNullOrWhiteSpace(Filepath))
        {
            Filepath = GetWorldFilepath();
        }
        if (LearningWorld.SavePath != Filepath)
        {
            LearningWorld.SavePath = Filepath;
        }
        BusinessLogic.SaveLearningWorld(LearningWorld, Filepath);
        Logger.LogTrace("Saved LearningWorld {Name} ({Id}) to {Path}", LearningWorld.Name, LearningWorld.Id, Filepath);
        ResetWorldUnsavedChangesState();
        MappingAction.Invoke(LearningWorld);
        return;

        string GetWorldFilepath()
        {
            var basePath = ApplicationPaths.SavedWorldsFolder;
            return BusinessLogic.FindSuitableNewSavePath(basePath, LearningWorld.Name, FileEndings.WorldFileEnding);
        }
    }

    private void ResetWorldUnsavedChangesState()
    {
        LearningWorld.UnsavedChanges = false;
        foreach (var element in LearningWorld.UnplacedLearningElements)
        {
            element.UnsavedChanges = false;
        }

        foreach (var space in LearningWorld.LearningSpaces)
        {
            space.UnsavedChanges = false;
            foreach (var element in space.ContainedLearningElements)
            {
                element.UnsavedChanges = false;
            }
        }

        foreach (var condition in LearningWorld.PathWayConditions)
        {
            condition.UnsavedChanges = false;
        }

        foreach (var topic in LearningWorld.Topics)
        {
            topic.UnsavedChanges = false;
        }
    }
}