using BusinessLogic.API;
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
        string name,
        ILearningContent contentEntity, string description, string goals, LearningElementDifficultyEnum difficulty,
        ElementModel elementModel, int workload, int points, double positionX, double positionY, Action<object> action) =>
        new CreateStoryElementInSlot(parentSpaceEntity, slotIndex, name, contentEntity, description, goals,
            difficulty, elementModel, workload, points, positionX, positionY, action, LoggerFactory.CreateLogger<CreateStoryElementInSlot>());

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

    public IDeleteLearningElementInWorld GetDeleteInWorldCommand(LearningElement learningElement,
        LearningWorld parentWorld, Action<LearningWorld> mappingAction) =>
        new DeleteLearningElementInWorld(learningElement, parentWorld, mappingAction,
            LoggerFactory.CreateLogger<DeleteLearningElementInWorld>());

    public IDragLearningElement GetDragCommand(LearningElement learningElement, double oldPositionX,
        double oldPositionY,
        double newPositionX, double newPositionY, Action<LearningElement> mappingAction) =>
        new DragLearningElement(learningElement, oldPositionX, oldPositionY, newPositionX, newPositionY,
            mappingAction, LoggerFactory.CreateLogger<DragLearningElement>());

    public IEditLearningElement GetEditCommand(LearningElement learningElement, LearningSpace? parentSpace, string name,
        string description, string goals, LearningElementDifficultyEnum difficulty, ElementModel elementModel,
        int workload, int points, ILearningContent learningContent, Action<LearningElement> mappingAction) =>
        new EditLearningElement(learningElement, parentSpace, name, description, goals, difficulty, elementModel,
            workload, points, learningContent, mappingAction, LoggerFactory.CreateLogger<EditLearningElement>());

    public ILoadLearningElement GetLoadCommand(LearningSpace parentSpace, int slotIndex, string filepath,
        IBusinessLogic businessLogic, Action<LearningSpace> mappingAction) =>
        new LoadLearningElement(parentSpace, slotIndex, filepath, businessLogic, mappingAction,
            LoggerFactory.CreateLogger<LoadLearningElement>());

    public ILoadLearningElement GetLoadCommand(LearningSpace parentSpace, int slotIndex, Stream stream,
        IBusinessLogic businessLogic, Action<LearningSpace> mappingAction) =>
        new LoadLearningElement(parentSpace, slotIndex, stream, businessLogic, mappingAction,
            LoggerFactory.CreateLogger<LoadLearningElement>());

    public ISaveLearningElement GetSaveCommand(IBusinessLogic businessLogic, LearningElement learningElement,
        string filepath) =>
        new SaveLearningElement(businessLogic, learningElement, filepath,
            LoggerFactory.CreateLogger<SaveLearningElement>());
}