using BusinessLogic.API;
using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Space;

public class SpaceCommandFactory : ISpaceCommandFactory
{
    public ICreateLearningSpace GetCreateCommand(LearningWorld learningWorld, string name, string description,
        string goals,
        int requiredPoints, double positionX, double positionY, Entities.Topic? topic,
        Action<LearningWorld> mappingAction) =>
        new CreateLearningSpace(learningWorld, name, description, goals, requiredPoints, positionX, positionY,
            topic, mappingAction);

    public ICreateLearningSpace GetCreateCommand(LearningWorld learningWorld, LearningSpace learningSpace,
        Action<LearningWorld> mappingAction) => new CreateLearningSpace(learningWorld, learningSpace, mappingAction);

    public IDeleteLearningSpace GetDeleteCommand(LearningWorld learningWorld, LearningSpace learningSpace,
        Action<LearningWorld> mappingAction) => new DeleteLearningSpace(learningWorld, learningSpace, mappingAction);

    public IEditLearningSpace GetEditCommand(LearningSpace learningSpace, string name, string description, string goals,
        int requiredPoints, Entities.Topic? topic, Action<LearningSpace> mappingAction) =>
        new EditLearningSpace(learningSpace, name, description, goals, requiredPoints, topic, mappingAction);

    public ILoadLearningSpace GetLoadCommand(LearningWorld learningWorld, string filepath, IBusinessLogic businessLogic,
        Action<LearningWorld> mappingAction) =>
        new LoadLearningSpace(learningWorld, filepath, businessLogic, mappingAction);

    public ILoadLearningSpace GetLoadCommand(LearningWorld learningWorld, Stream stream, IBusinessLogic businessLogic,
        Action<LearningWorld> mappingAction) =>
        new LoadLearningSpace(learningWorld, stream, businessLogic, mappingAction);

    public ISaveLearningSpace GetSaveCommand(IBusinessLogic businessLogic, LearningSpace learningSpace,
        string filepath) => new SaveLearningSpace(businessLogic, learningSpace, filepath);
}