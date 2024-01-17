using BusinessLogic.API;
using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningOutcome;
using Microsoft.Extensions.Logging;
using Shared;

namespace BusinessLogic.Commands.Space;

public class SpaceCommandFactory : ISpaceCommandFactory
{
    public SpaceCommandFactory(ILoggerFactory loggerFactory)
    {
        LoggerFactory = loggerFactory;
    }

    private ILoggerFactory LoggerFactory { get; }

    public ICreateLearningSpace GetCreateCommand(LearningWorld learningWorld, string name, string description,
        List<ILearningOutcome> learningOutcomes,
        int requiredPoints, Theme theme, double positionX, double positionY, Entities.Topic? topic,
        Action<LearningWorld> mappingAction) =>
        new CreateLearningSpace(learningWorld, name, description, learningOutcomes, requiredPoints, theme, positionX,
            positionY,
            topic, mappingAction, LoggerFactory.CreateLogger<CreateLearningSpace>());

    public ICreateLearningSpace GetCreateCommand(LearningWorld learningWorld, LearningSpace learningSpace,
        Action<LearningWorld> mappingAction) =>
        new CreateLearningSpace(learningWorld, learningSpace, mappingAction,
            LoggerFactory.CreateLogger<CreateLearningSpace>());

    public IDeleteLearningSpace GetDeleteCommand(LearningWorld learningWorld, LearningSpace learningSpace,
        Action<LearningWorld> mappingAction) =>
        new DeleteLearningSpace(learningWorld, learningSpace, mappingAction,
            LoggerFactory.CreateLogger<DeleteLearningSpace>());

    public IEditLearningSpace GetEditCommand(ILearningSpace learningSpace, string name, string description,
        int requiredPoints, Theme theme, Entities.Topic? topic, Action<ILearningSpace> mappingAction) =>
        new EditLearningSpace(learningSpace, name, description, requiredPoints, theme, topic, mappingAction,
            LoggerFactory.CreateLogger<EditLearningSpace>());

    public ILoadLearningSpace GetLoadCommand(LearningWorld learningWorld, string filepath, IBusinessLogic businessLogic,
        Action<LearningWorld> mappingAction) =>
        new LoadLearningSpace(learningWorld, filepath, businessLogic, mappingAction,
            LoggerFactory.CreateLogger<LoadLearningSpace>());

    public ILoadLearningSpace GetLoadCommand(LearningWorld learningWorld, Stream stream, IBusinessLogic businessLogic,
        Action<LearningWorld> mappingAction) =>
        new LoadLearningSpace(learningWorld, stream, businessLogic, mappingAction,
            LoggerFactory.CreateLogger<LoadLearningSpace>());

    public ISaveLearningSpace GetSaveCommand(IBusinessLogic businessLogic, LearningSpace learningSpace,
        string filepath) =>
        new SaveLearningSpace(businessLogic, learningSpace, filepath, LoggerFactory.CreateLogger<SaveLearningSpace>());
}