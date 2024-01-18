using BusinessLogic.API;
using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent.Adaptivity;
using BusinessLogic.Entities.LearningContent.Adaptivity.Action;
using BusinessLogic.Entities.LearningContent.Adaptivity.Question;
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
        ResetWorldUnsavedChangesState();
        MappingAction.Invoke(LearningWorld);
        return;

        string GetWorldFilepath()
        {
            var basePath = ApplicationPaths.SavedWorldsFolder;
            return BusinessLogic.FindSuitableNewSavePath(basePath, LearningWorld.Name, FileEndings.WorldFileEnding,
                out _);
        }
    }

    private void ResetWorldUnsavedChangesState()
    {
        LearningWorld.UnsavedChanges = false;
        foreach (var element in LearningWorld.UnplacedLearningElements)
        {
            ResetElementUnsavedChangesState(element);
        }

        foreach (var space in LearningWorld.LearningSpaces)
        {
            space.UnsavedChanges = false;
            foreach (var element in space.ContainedLearningElements)
            {
                ResetElementUnsavedChangesState(element);
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

    private void ResetElementUnsavedChangesState(ILearningElement element)
    {
        element.UnsavedChanges = false;
        element.LearningContent.UnsavedChanges = false;
        if (element.LearningContent is AdaptivityContent ac)
            ResetUnsavedChangesAdaptivityContent(ac);
    }

    void ResetUnsavedChangesAdaptivityContent(IAdaptivityContent adaptivityContent)
    {
        foreach (var task in adaptivityContent.Tasks)
        {
            task.UnsavedChanges = false;
            foreach (var question in task.Questions)
            {
                question.UnsavedChanges = false;
                switch (question)
                {
                    case MultipleChoiceMultipleResponseQuestion mcmrq:
                        foreach (var correctChoice in mcmrq.CorrectChoices) correctChoice.UnsavedChanges = false;
                        foreach (var choice in mcmrq.Choices) choice.UnsavedChanges = false;
                        break;
                    case MultipleChoiceSingleResponseQuestion mcsrq:
                        mcsrq.CorrectChoice.UnsavedChanges = false;
                        foreach (var choice in mcsrq.Choices) choice.UnsavedChanges = false;
                        break;
                }

                foreach (var rule in question.Rules)
                {
                    rule.UnsavedChanges = false;
                    rule.Action.UnsavedChanges = false;
                    rule.Action.UnsavedChanges = false;
                    if (rule.Action is ContentReferenceAction cra)
                        cra.Content.UnsavedChanges = false;
                }
            }
        }
    }
}