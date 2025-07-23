using BusinessLogic.API;
using BusinessLogic.Entities;
using Shared.Theme;

namespace BusinessLogic.Commands.World;

/// <summary>
/// Factory for creating commands relating to learning worlds.
/// </summary>
public interface IWorldCommandFactory
{
    /// <summary>
    /// Creates a command to create a learning world.
    /// </summary>
    ICreateLearningWorld GetCreateCommand(AuthoringToolWorkspace authoringToolWorkspace, string name, string shortname,
        string authors, string language, string description, string goals, WorldTheme worldTheme, string evaluationLink,
        string evaluationLinkName, string evaluationLinkText, string enrolmentKey,
        string storyStart, string storyEnd,
        Action<AuthoringToolWorkspace> mappingAction);

    /// <summary>
    /// Creates a command to create a learning world.
    /// </summary>
    ICreateLearningWorld GetCreateCommand(AuthoringToolWorkspace authoringToolWorkspace, LearningWorld learningWorld,
        Action<AuthoringToolWorkspace> mappingAction);

    /// <summary>
    /// Creates a command to delete a learning world.
    /// </summary>
    IDeleteLearningWorld GetDeleteCommand(AuthoringToolWorkspace authoringToolWorkspace, LearningWorld learningWorld,
        Action<AuthoringToolWorkspace> mappingAction);

    /// <summary>
    /// Creates a command to edit a learning world.
    /// </summary>
    IEditLearningWorld GetEditCommand(LearningWorld learningWorld, string name, string shortname, string authors,
        string language, string description, string goals, WorldTheme worldTheme, string evaluationLink, string evaluationLinkName,
        string evaluationLinkText, string enrolmentKey,
        string storyStart, string storyEnd,
        Action<LearningWorld> mappingAction);

    /// <summary>
    /// Creates a command to load a learning world.
    /// </summary>
    ILoadLearningWorld GetLoadCommand(AuthoringToolWorkspace workspace, string filepath, IBusinessLogic businessLogic,
        Action<AuthoringToolWorkspace> mappingAction);

    /// <summary>
    /// Creates a command to save a learning world.
    /// </summary>
    ISaveLearningWorld GetSaveCommand(IBusinessLogic businessLogic, LearningWorld learningWorld, string filepath,
        Action<LearningWorld> mappingAction);
}