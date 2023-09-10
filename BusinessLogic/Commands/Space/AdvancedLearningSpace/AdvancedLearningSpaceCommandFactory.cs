using BusinessLogic.API;
using BusinessLogic.Entities;
using BusinessLogic.Entities.AdvancedLearningSpaces;
using Microsoft.Extensions.Logging;
using Shared;

namespace BusinessLogic.Commands.Space.AdvancedLearningSpace;

public class AdvancedLearningSpaceCommandFactory : IAdvancedLearningSpaceCommandFactory
{
    public AdvancedLearningSpaceCommandFactory(ILoggerFactory loggerFactory)
    {
        LoggerFactory = loggerFactory;
    }

    private ILoggerFactory LoggerFactory { get; }

    public ICreateAdvancedLearningSpace GetCreateCommand(LearningWorld learningWorld, string name, string description,
        string goals,
        int requiredPoints, Theme theme, double positionX, double positionY, Entities.Topic? topic,
        Action<LearningWorld> mappingAction) =>
        new CreateAdvancedLearningSpace(learningWorld, name, description, goals, requiredPoints, theme, positionX,
            positionY,
            topic, mappingAction, LoggerFactory.CreateLogger<CreateAdvancedLearningSpace>());
    public ICreateAdvancedLearningSpace GetCreateCommand(LearningWorld learningWorld, Entities.AdvancedLearningSpaces.AdvancedLearningSpace advancedLearningSpace,
        Action<LearningWorld> mappingAction) =>
        new CreateAdvancedLearningSpace(learningWorld, advancedLearningSpace, mappingAction,
            LoggerFactory.CreateLogger<CreateAdvancedLearningSpace>());

    public IDeleteAdvancedLearningSpace GetDeleteCommand(LearningWorld learningWorld,
        Entities.AdvancedLearningSpaces.AdvancedLearningSpace advancedLearningSpace,
        Action<LearningWorld> mappingAction) =>
        new DeleteAdvancedLearningSpace(learningWorld, advancedLearningSpace, mappingAction,
            LoggerFactory.CreateLogger<DeleteAdvancedLearningSpace>());

    public IEditAdvancedLearningSpace GetEditCommand(IAdvancedLearningSpace advancedLearningSpace, string name,
        string description,
        string goals,
        int requiredPoints, Theme theme, Entities.Topic? topic, Action<IAdvancedLearningSpace> mappingAction) =>
        new EditAdvancedLearningSpace(advancedLearningSpace, name, description, goals, requiredPoints, theme, topic,
            mappingAction,
            LoggerFactory.CreateLogger<EditAdvancedLearningSpace>());

    // public ISaveAdvancedLearningSpace GetSaveCommand(IBusinessLogic businessLogic,
    //     Entities.AdvancedLearningSpaces.AdvancedLearningSpace advancedLearningSpace,
    //     string filepath) =>
    //     new SaveAdvancedLearningSpace(businessLogic, advancedLearningSpace, filepath,
    //         LoggerFactory.CreateLogger<SaveAdvancedLearningSpace>());
}