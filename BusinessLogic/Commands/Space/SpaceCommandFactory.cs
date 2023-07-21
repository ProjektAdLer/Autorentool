using BusinessLogic.API;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;
using Shared;

namespace BusinessLogic.Commands.Space;

public class SpaceCommandFactory : ISpaceCommandFactory
{
    public ICreateLearningSpace GetCreateCommand(LearningWorld learningWorld, string name, string description,
        string goals,
        int requiredPoints, Theme theme, double positionX, double positionY, Entities.Topic? topic,
        Action<LearningWorld> mappingAction, ILogger<SpaceCommandFactory> logger) =>
        new CreateLearningSpace(learningWorld, name, description, goals, requiredPoints, theme, positionX, positionY,
            topic, mappingAction, logger);

    public ICreateLearningSpace GetCreateCommand(LearningWorld learningWorld, LearningSpace learningSpace,
        Action<LearningWorld> mappingAction, ILogger<SpaceCommandFactory> logger) =>
        new CreateLearningSpace(learningWorld, learningSpace, mappingAction, logger);

    public IDeleteLearningSpace GetDeleteCommand(LearningWorld learningWorld, LearningSpace learningSpace,
        Action<LearningWorld> mappingAction, ILogger<SpaceCommandFactory> logger) =>
        new DeleteLearningSpace(learningWorld, learningSpace, mappingAction, logger);

    public IEditLearningSpace GetEditCommand(ILearningSpace learningSpace, string name, string description,
        string goals,
        int requiredPoints, Theme theme, Entities.Topic? topic, Action<ILearningSpace> mappingAction,
        ILogger<SpaceCommandFactory> logger) =>
        new EditLearningSpace(learningSpace, name, description, goals, requiredPoints, theme, topic, mappingAction,
            logger);

    public ILoadLearningSpace GetLoadCommand(LearningWorld learningWorld, string filepath, IBusinessLogic businessLogic,
        Action<LearningWorld> mappingAction, ILogger<SpaceCommandFactory> logger) =>
        new LoadLearningSpace(learningWorld, filepath, businessLogic, mappingAction, logger);

    public ILoadLearningSpace GetLoadCommand(LearningWorld learningWorld, Stream stream, IBusinessLogic businessLogic,
        Action<LearningWorld> mappingAction, ILogger<SpaceCommandFactory> logger) =>
        new LoadLearningSpace(learningWorld, stream, businessLogic, mappingAction, logger);

    public ISaveLearningSpace GetSaveCommand(IBusinessLogic businessLogic, LearningSpace learningSpace,
        string filepath, ILogger<SpaceCommandFactory> logger) =>
        new SaveLearningSpace(businessLogic, learningSpace, filepath, logger);
}