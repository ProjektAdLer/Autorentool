using BusinessLogic.API;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.World;

public class WorldCommandFactory : IWorldCommandFactory
{
    public ICreateLearningWorld GetCreateCommand(AuthoringToolWorkspace authoringToolWorkspace, string name,
        string shortname, string authors, string language, string description, string goals,
        Action<AuthoringToolWorkspace> mappingAction, ILogger<WorldCommandFactory> logger) =>
        new CreateLearningWorld(authoringToolWorkspace, name, shortname, authors, language, description, goals,
            mappingAction, logger);

    public ICreateLearningWorld GetCreateCommand(AuthoringToolWorkspace authoringToolWorkspace,
        LearningWorld learningWorld, Action<AuthoringToolWorkspace> mappingAction,
        ILogger<WorldCommandFactory> logger) =>
        new CreateLearningWorld(authoringToolWorkspace, learningWorld, mappingAction, logger);

    public IDeleteLearningWorld GetDeleteCommand(AuthoringToolWorkspace authoringToolWorkspace,
        LearningWorld learningWorld, Action<AuthoringToolWorkspace> mappingAction,
        ILogger<WorldCommandFactory> logger) =>
        new DeleteLearningWorld(authoringToolWorkspace, learningWorld, mappingAction, logger);

    public IEditLearningWorld GetEditCommand(LearningWorld learningWorld, string name, string shortname, string authors,
        string language, string description, string goals, Action<LearningWorld> mappingAction,
        ILogger<WorldCommandFactory> logger) =>
        new EditLearningWorld(learningWorld, name, shortname, authors, language, description, goals,
            mappingAction, logger);

    public ILoadLearningWorld GetLoadCommand(AuthoringToolWorkspace workspace, string filepath,
        IBusinessLogic businessLogic, Action<AuthoringToolWorkspace> mappingAction,
        ILogger<WorldCommandFactory> logger) =>
        new LoadLearningWorld(workspace, filepath, businessLogic, mappingAction, logger);

    public ILoadLearningWorld GetLoadCommand(AuthoringToolWorkspace workspace, Stream stream,
        IBusinessLogic businessLogic, Action<AuthoringToolWorkspace> mappingAction,
        ILogger<WorldCommandFactory> logger) =>
        new LoadLearningWorld(workspace, stream, businessLogic, mappingAction, logger);

    public ISaveLearningWorld GetSaveCommand(IBusinessLogic businessLogic, LearningWorld learningWorld,
        string filepath, ILogger<WorldCommandFactory> logger) =>
        new SaveLearningWorld(businessLogic, learningWorld, filepath, logger);
}