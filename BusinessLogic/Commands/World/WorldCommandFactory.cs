using BusinessLogic.API;
using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningOutcome;
using Microsoft.Extensions.Logging;
using Shared.Theme;

namespace BusinessLogic.Commands.World;

public class WorldCommandFactory : IWorldCommandFactory
{
    public WorldCommandFactory(ILoggerFactory loggerFactory, IUnsavedChangesResetHelper unsavedChangesResetHelper)
    {
        LoggerFactory = loggerFactory;
        UnsavedChangesResetHelper = unsavedChangesResetHelper;
    }

    private ILoggerFactory LoggerFactory { get; }
    public IUnsavedChangesResetHelper UnsavedChangesResetHelper { get; }

    public ICreateLearningWorld GetCreateCommand(AuthoringToolWorkspace authoringToolWorkspace, string name,
        string shortname, string authors, string language, string description,
        LearningOutcomeCollection learningOutcomeCollection, WorldTheme worldTheme,
        string evaluationLink, string enrolmentKey, string storyStart, string storyEnd,
        Action<AuthoringToolWorkspace> mappingAction) =>
        new CreateLearningWorld(authoringToolWorkspace, name, shortname, authors, language, description,
            learningOutcomeCollection, worldTheme, evaluationLink, enrolmentKey, storyStart, storyEnd,
            mappingAction, LoggerFactory.CreateLogger<CreateLearningWorld>());

    public ICreateLearningWorld GetCreateCommand(AuthoringToolWorkspace authoringToolWorkspace,
        LearningWorld learningWorld, Action<AuthoringToolWorkspace> mappingAction) =>
        new CreateLearningWorld(authoringToolWorkspace, learningWorld, mappingAction,
            LoggerFactory.CreateLogger<CreateLearningWorld>());

    public IDeleteLearningWorld GetDeleteCommand(AuthoringToolWorkspace authoringToolWorkspace,
        LearningWorld learningWorld, Action<AuthoringToolWorkspace> mappingAction) =>
        new DeleteLearningWorld(authoringToolWorkspace, learningWorld, mappingAction,
            LoggerFactory.CreateLogger<DeleteLearningWorld>());

    public IEditLearningWorld GetEditCommand(LearningWorld learningWorld, string name, string shortname, string authors,
        string language, string description, LearningOutcomeCollection learningOutcomeCollection,
        WorldTheme worldTheme, string evaluationLink, string enrolmentKey, string storyStart, string storyEnd,
        Action<LearningWorld> mappingAction) =>
        new EditLearningWorld(learningWorld, name, shortname, authors, language, description, learningOutcomeCollection,
            worldTheme, evaluationLink, enrolmentKey, storyStart, storyEnd,
            mappingAction, LoggerFactory.CreateLogger<EditLearningWorld>());

    public ILoadLearningWorld GetLoadCommand(AuthoringToolWorkspace workspace, string filepath,
        IBusinessLogic businessLogic, Action<AuthoringToolWorkspace> mappingAction) =>
        new LoadLearningWorld(workspace, filepath, businessLogic, mappingAction,
            LoggerFactory.CreateLogger<LoadLearningWorld>());

    public ISaveLearningWorld GetSaveCommand(IBusinessLogic businessLogic, LearningWorld learningWorld,
        string filepath, Action<LearningWorld> mappingAction) =>
        new SaveLearningWorld(businessLogic, learningWorld, filepath, mappingAction,
            LoggerFactory.CreateLogger<SaveLearningWorld>(), UnsavedChangesResetHelper);
}