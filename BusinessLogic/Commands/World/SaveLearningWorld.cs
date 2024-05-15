using BusinessLogic.API;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.Configuration;

namespace BusinessLogic.Commands.World;

public class SaveLearningWorld : ISaveLearningWorld
{
    public SaveLearningWorld(IBusinessLogic businessLogic, LearningWorld learningWorld, string filepath,
        Action<LearningWorld> mappingAction, ILogger<SaveLearningWorld> logger,
        IUnsavedChangesResetHelper unsavedChangesResetHelper)
    {
        BusinessLogic = businessLogic;
        LearningWorld = learningWorld;
        Filepath = filepath;
        MappingAction = mappingAction;
        Logger = logger;
        UnsavedChangesResetHelper = unsavedChangesResetHelper;
    }

    internal IBusinessLogic BusinessLogic { get; }
    internal LearningWorld LearningWorld { get; }
    internal string Filepath { get; private set; }
    internal Action<LearningWorld> MappingAction { get; }
    private ILogger<SaveLearningWorld> Logger { get; }
    public IUnsavedChangesResetHelper UnsavedChangesResetHelper { get; }
    public string Name => nameof(SaveLearningWorld);

    public void Execute()
    {
        //get suitable file path for world
        if (string.IsNullOrWhiteSpace(Filepath))
        {
            Filepath = GetWorldFilepath();
        }

        //update world save path if necessary
        if (LearningWorld.SavePath != Filepath)
        {
            LearningWorld.SavePath = Filepath;
        }

        BusinessLogic.SaveLearningWorld(LearningWorld, Filepath);
        Logger.LogTrace("Saved LearningWorld {Name} ({Id}) to {Path}", LearningWorld.Name, LearningWorld.Id, Filepath);
        UnsavedChangesResetHelper.ResetWorldUnsavedChangesState(LearningWorld);
        MappingAction.Invoke(LearningWorld);
        return;

        string GetWorldFilepath()
        {
            var basePath = ApplicationPaths.SavedWorldsFolder;
            return BusinessLogic.FindSuitableNewSavePath(basePath, LearningWorld.Name, FileEndings.WorldFileEnding,
                out _);
        }
    }
}