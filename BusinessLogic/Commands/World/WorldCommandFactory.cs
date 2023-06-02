using BusinessLogic.API;
using BusinessLogic.Entities;

namespace BusinessLogic.Commands.World;

public class WorldCommandFactory : IWorldCommandFactory
{
    public ICreateLearningWorld GetCreateCommand(AuthoringToolWorkspace authoringToolWorkspace, string name,
        string shortname, string authors, string language, string description, string goals,
        Action<AuthoringToolWorkspace> mappingAction) =>
        new CreateLearningWorld(authoringToolWorkspace, name, shortname, authors, language, description, goals,
            mappingAction);

    public ICreateLearningWorld GetCreateCommand(AuthoringToolWorkspace authoringToolWorkspace,
        LearningWorld learningWorld, Action<AuthoringToolWorkspace> mappingAction) =>
        new CreateLearningWorld(authoringToolWorkspace, learningWorld, mappingAction);

    public IDeleteLearningWorld GetDeleteCommand(AuthoringToolWorkspace authoringToolWorkspace,
        LearningWorld learningWorld, Action<AuthoringToolWorkspace> mappingAction) =>
        new DeleteLearningWorld(authoringToolWorkspace, learningWorld, mappingAction);

    public IEditLearningWorld GetEditCommand(LearningWorld learningWorld, string name, string shortname, string authors,
        string language, string description, string goals, Action<LearningWorld> mappingAction) =>
        new EditLearningWorld(learningWorld, name, shortname, authors, language, description, goals,
            mappingAction);

    public ILoadLearningWorld GetLoadCommand(AuthoringToolWorkspace workspace, string filepath,
        IBusinessLogic businessLogic, Action<AuthoringToolWorkspace> mappingAction) =>
        new LoadLearningWorld(workspace, filepath, businessLogic, mappingAction);

    public ILoadLearningWorld GetLoadCommand(AuthoringToolWorkspace workspace, Stream stream,
        IBusinessLogic businessLogic, Action<AuthoringToolWorkspace> mappingAction) =>
        new LoadLearningWorld(workspace, stream, businessLogic, mappingAction);

    public ISaveLearningWorld GetSaveCommand(IBusinessLogic businessLogic, LearningWorld learningWorld,
        string filepath) =>
        new SaveLearningWorld(businessLogic, learningWorld, filepath);
}