using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using Microsoft.Extensions.Logging;
using Shared;

namespace BusinessLogic.Commands.Element;

public class ElementCommandFactory : IElementCommandFactory
{
    public ElementCommandFactory(ILoggerFactory loggerFactory)
    {
        LoggerFactory = loggerFactory;
    }

    private ILoggerFactory LoggerFactory { get; }

    public ICreateLearningElementInSlot GetCreateInSlotCommand(LearningSpace parentSpace, int slotIndex, string name,
        ILearningContent learningContent, string description, string goals, LearningElementDifficultyEnum difficulty,
        ElementModel elementModel, int workload, int points, double positionX, double positionY,
        Action<LearningSpace> mappingAction) =>
        new CreateLearningElementInSlot(parentSpace, slotIndex, name, learningContent, description, goals,
            difficulty, elementModel, workload, points, positionX, positionY, mappingAction,
            LoggerFactory.CreateLogger<CreateLearningElementInSlot>());

    public ICreateStoryElementInSlot GetCreateStoryInSlotCommand(LearningSpace parentSpaceEntity, int slotIndex,
        string name, ILearningContent contentEntity, string description, string goals,
        LearningElementDifficultyEnum difficulty, ElementModel elementModel, int workload, int points, double positionX,
        double positionY, Action<LearningSpace> action) =>
        new CreateStoryElementInSlot(parentSpaceEntity, slotIndex, name, contentEntity, description, goals,
            difficulty, elementModel, workload, points, positionX, positionY, action,
            LoggerFactory.CreateLogger<CreateStoryElementInSlot>());

    public ICreateLearningElementInSlot GetCreateInSlotCommand(LearningSpace parentSpace, int slotIndex,
        LearningElement learningElement, Action<LearningSpace> mappingAction) =>
        new CreateLearningElementInSlot(parentSpace, slotIndex, learningElement, mappingAction,
            LoggerFactory.CreateLogger<CreateLearningElementInSlot>());

    public ICreateUnplacedLearningElement GetCreateUnplacedCommand(LearningWorld learningWorld, string name,
        ILearningContent learningContent, string description, string goals, LearningElementDifficultyEnum difficulty,
        ElementModel elementModel,
        int workload, int points, double positionX, double positionY, Action<LearningWorld> mappingAction) =>
        new CreateUnplacedLearningElement(learningWorld, name, learningContent, description, goals, difficulty,
            elementModel, workload, points, positionX, positionY, mappingAction,
            LoggerFactory.CreateLogger<CreateUnplacedLearningElement>());

    public IDeleteLearningElementInSpace GetDeleteInSpaceCommand(LearningElement learningElement,
        LearningSpace parentSpace, Action<LearningSpace> mappingAction) =>
        new DeleteLearningElementInSpace(learningElement, parentSpace, mappingAction,
            LoggerFactory.CreateLogger<DeleteLearningElementInSpace>());

    public IDeleteStoryElementInSpace GetDeleteStoryInSpaceCommand(LearningElement learningElement,
        LearningSpace parentSpace, Action<LearningSpace> mappingAction) =>
        new DeleteStoryElementInSpace(learningElement, parentSpace, mappingAction,
            LoggerFactory.CreateLogger<DeleteStoryElementInSpace>());

    public IDeleteLearningElementInWorld GetDeleteInWorldCommand(LearningElement learningElement,
        LearningWorld parentWorld, Action<LearningWorld> mappingAction) =>
        new DeleteLearningElementInWorld(learningElement, parentWorld, mappingAction,
            LoggerFactory.CreateLogger<DeleteLearningElementInWorld>());

    public IEditLearningElement GetEditCommand(LearningElement learningElement, LearningSpace? parentSpace, string name,
        string description, string goals, LearningElementDifficultyEnum difficulty, ElementModel elementModel,
        int workload, int points, ILearningContent learningContent, Action<LearningElement> mappingAction) =>
        new EditLearningElement(learningElement, parentSpace, name, description, goals, difficulty, elementModel,
            workload, points, learningContent, mappingAction, LoggerFactory.CreateLogger<EditLearningElement>());
}