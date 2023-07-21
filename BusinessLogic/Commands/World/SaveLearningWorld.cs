using BusinessLogic.API;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.World;

public class SaveLearningWorld : ISaveLearningWorld
{
    public string Name => nameof(SaveLearningWorld);
    internal IBusinessLogic BusinessLogic { get; }
    internal LearningWorld LearningWorld { get; }
    internal string Filepath { get; }
    private ILogger<WorldCommandFactory> Logger { get; }

    public SaveLearningWorld(IBusinessLogic businessLogic, LearningWorld learningWorld, string filepath, ILogger<WorldCommandFactory> logger)
    {
        BusinessLogic = businessLogic;
        LearningWorld = learningWorld;
        Filepath = filepath;
        Logger = logger;
    }
    
    public void Execute()
    {
        BusinessLogic.SaveLearningWorld(LearningWorld, Filepath);
        Logger.LogTrace("Saved LearningWorld {name} ({id}) to {path}.", LearningWorld.Name, LearningWorld.Id, Filepath);
        ResetWorldUnsavedChangesState();
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