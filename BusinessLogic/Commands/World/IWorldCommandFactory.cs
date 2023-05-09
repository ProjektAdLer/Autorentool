using BusinessLogic.API;
using BusinessLogic.Entities;

namespace BusinessLogic.Commands.World;

public interface IWorldCommandFactory
{
    ICreateLearningWorld GetCreateCommand(AuthoringToolWorkspace authoringToolWorkspace, string name, string shortname,
        string authors, string language, string description, string goals,
        Action<AuthoringToolWorkspace> mappingAction);

    ICreateLearningWorld GetCreateCommand(AuthoringToolWorkspace authoringToolWorkspace, LearningWorld learningWorld,
        Action<AuthoringToolWorkspace> mappingAction);

    IDeleteLearningWorld GetDeleteCommand(AuthoringToolWorkspace authoringToolWorkspace, LearningWorld learningWorld,
        Action<AuthoringToolWorkspace> mappingAction);

    IEditLearningWorld GetEditCommand(LearningWorld learningWorld, string name, string shortname, string authors,
        string language, string description, string goals, Action<LearningWorld> mappingAction);

    ILoadLearningWorld GetLoadCommand(AuthoringToolWorkspace workspace, string filepath, IBusinessLogic businessLogic,
        Action<AuthoringToolWorkspace> mappingAction);

    ILoadLearningWorld GetLoadCommand(AuthoringToolWorkspace workspace, Stream stream, IBusinessLogic businessLogic,
        Action<AuthoringToolWorkspace> mappingAction);
    
    ISaveLearningWorld GetSaveCommand(IBusinessLogic businessLogic, LearningWorld learningWorld, string filepath);
}