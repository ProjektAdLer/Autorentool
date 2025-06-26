using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningOutcome;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.Theme;

namespace BusinessLogic.Commands.Space;

public class SpaceCommandFactory : ISpaceCommandFactory
{
    public SpaceCommandFactory(ILoggerFactory loggerFactory)
    {
        LoggerFactory = loggerFactory;
    }

    private ILoggerFactory LoggerFactory { get; }

    public ICreateLearningSpace GetCreateCommand(LearningWorld learningWorld, string name, string description,
        LearningOutcomeCollection learningOutcomeCollection,
        int requiredPoints, SpaceTheme spaceTheme, double positionX, double positionY, Entities.Topic? topic,
        Action<LearningWorld> mappingAction) =>
        new CreateLearningSpace(learningWorld, name, description, learningOutcomeCollection, requiredPoints, spaceTheme,
            positionX,
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
        int requiredPoints, SpaceTheme spaceTheme, Entities.Topic? topic, Action<ILearningSpace> mappingAction) =>
        new EditLearningSpace(learningSpace, name, description, requiredPoints, spaceTheme, topic, mappingAction,
            LoggerFactory.CreateLogger<EditLearningSpace>());
}