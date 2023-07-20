using BusinessLogic.API;
using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using Microsoft.Extensions.Logging;
using Shared;

namespace BusinessLogic.Commands.Element;

public class ElementCommandFactory : IElementCommandFactory
{
    public ICreateLearningElementInSlot GetCreateInSlotCommand(LearningSpace parentSpace, int slotIndex, string name,
        ILearningContent learningContent, string description, string goals, LearningElementDifficultyEnum difficulty,
        ElementModel elementModel, int workload, int points, double positionX, double positionY,
        Action<LearningSpace> mappingAction, ILogger<ElementCommandFactory> logger) =>
        new CreateLearningElementInSlot(parentSpace, slotIndex, name, learningContent, description, goals,
            difficulty, elementModel, workload, points, positionX, positionY, mappingAction, logger);

    public ICreateLearningElementInSlot GetCreateInSlotCommand(LearningSpace parentSpace, int slotIndex,
        LearningElement learningElement, Action<LearningSpace> mappingAction, ILogger<ElementCommandFactory> logger) =>
        new CreateLearningElementInSlot(parentSpace, slotIndex, learningElement, mappingAction, logger);

    public ICreateUnplacedLearningElement GetCreateUnplacedCommand(LearningWorld learningWorld, string name,
        ILearningContent learningContent, string description, string goals, LearningElementDifficultyEnum difficulty,
        ElementModel elementModel,
        int workload, int points, double positionX, double positionY, Action<LearningWorld> mappingAction,
        ILogger<ElementCommandFactory> logger) =>
        new CreateUnplacedLearningElement(learningWorld, name, learningContent, description, goals, difficulty,
            elementModel, workload, points, positionX, positionY, mappingAction, logger);

    public IDeleteLearningElementInSpace GetDeleteInSpaceCommand(LearningElement learningElement,
        LearningSpace parentSpace, Action<LearningSpace> mappingAction, ILogger<ElementCommandFactory> logger) =>
        new DeleteLearningElementInSpace(learningElement, parentSpace, mappingAction, logger);

    public IDeleteLearningElementInWorld GetDeleteInWorldCommand(LearningElement learningElement,
        LearningWorld parentWorld, Action<LearningWorld> mappingAction, ILogger<ElementCommandFactory> logger) =>
        new DeleteLearningElementInWorld(learningElement, parentWorld, mappingAction, logger);

    public IDragLearningElement GetDragCommand(LearningElement learningElement, double oldPositionX,
        double oldPositionY,
        double newPositionX, double newPositionY, Action<LearningElement> mappingAction,
        ILogger<ElementCommandFactory> logger) =>
        new DragLearningElement(learningElement, oldPositionX, oldPositionY, newPositionX, newPositionY,
            mappingAction, logger);

    public IEditLearningElement GetEditCommand(LearningElement learningElement, LearningSpace? parentSpace, string name,
        string description, string goals, LearningElementDifficultyEnum difficulty, ElementModel elementModel,
        int workload, int points, ILearningContent learningContent, Action<LearningElement> mappingAction,
        ILogger<ElementCommandFactory> logger) =>
        new EditLearningElement(learningElement, parentSpace, name, description, goals, difficulty, elementModel,
            workload, points, learningContent, mappingAction, logger);

    public ILoadLearningElement GetLoadCommand(LearningSpace parentSpace, int slotIndex, string filepath,
        IBusinessLogic businessLogic, Action<LearningSpace> mappingAction, ILogger<ElementCommandFactory> logger) =>
        new LoadLearningElement(parentSpace, slotIndex, filepath, businessLogic, mappingAction, logger);

    public ILoadLearningElement GetLoadCommand(LearningSpace parentSpace, int slotIndex, Stream stream,
        IBusinessLogic businessLogic, Action<LearningSpace> mappingAction, ILogger<ElementCommandFactory> logger) =>
        new LoadLearningElement(parentSpace, slotIndex, stream, businessLogic, mappingAction, logger);

    public ISaveLearningElement GetSaveCommand(IBusinessLogic businessLogic, LearningElement learningElement,
        string filepath, ILogger<ElementCommandFactory> logger) =>
        new SaveLearningElement(businessLogic, learningElement, filepath, logger);
}