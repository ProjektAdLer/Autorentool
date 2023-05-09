using BusinessLogic.API;
using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Space;

public interface ISpaceCommandFactory
{
    ICreateLearningSpace GetCreateCommand(LearningWorld learningWorld, string name,
        string description, string goals, int requiredPoints, double positionX, double positionY, Entities.Topic? topic,
        Action<LearningWorld> mappingAction);

    ICreateLearningSpace GetCreateCommand(LearningWorld learningWorld, LearningSpace learningSpace,
        Action<LearningWorld> mappingAction);

    IDeleteLearningSpace GetDeleteCommand(LearningWorld learningWorld, LearningSpace learningSpace,
        Action<LearningWorld> mappingAction);

    IEditLearningSpace GetEditCommand(LearningSpace learningSpace, string name,
        string description, string goals, int requiredPoints, Entities.Topic? topic,
        Action<LearningSpace> mappingAction);

    ILoadLearningSpace GetLoadCommand(LearningWorld learningWorld, string filepath, IBusinessLogic businessLogic,
        Action<LearningWorld> mappingAction);

    ILoadLearningSpace GetLoadCommand(LearningWorld learningWorld, Stream stream, IBusinessLogic businessLogic,
        Action<LearningWorld> mappingAction);

    ISaveLearningSpace GetSaveCommand(IBusinessLogic businessLogic, LearningSpace learningSpace, string filepath);
}