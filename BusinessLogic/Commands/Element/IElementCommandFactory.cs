using BusinessLogic.API;
using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using Shared;

namespace BusinessLogic.Commands.Element;

/// <summary>
/// Factory for creating commands relating to learning elements.
/// </summary>
public interface IElementCommandFactory
{
    /// <summary>
    /// Creates a command to create a learning element in a slot.
    /// </summary>
    ICreateLearningElementInSlot GetCreateInSlotCommand(LearningSpace parentSpace, int slotIndex, string name,
        ILearningContent learningContent, string description, string goals, LearningElementDifficultyEnum difficulty,
        ElementModel elementModel, int workload, int points, double positionX, double positionY,
        Action<LearningSpace> mappingAction);

    /// <summary>
    /// Creates a command to create a learning element in a slot.
    /// </summary>
    ICreateLearningElementInSlot GetCreateInSlotCommand(LearningSpace parentSpace, int slotIndex,
        LearningElement learningElement, Action<LearningSpace> mappingAction);

    /// <summary>
    /// Creates a command to create a learning element unplaced in a world.
    /// </summary>
    ICreateUnplacedLearningElement GetCreateUnplacedCommand(LearningWorld learningWorld, string name,
        ILearningContent learningContent, string description, string goals, LearningElementDifficultyEnum difficulty,
        ElementModel elementModel, int workload, int points, double positionX, double positionY,
        Action<LearningWorld> mappingAction);

    /// <summary>
    /// Creates a command to delete a learning element from a space.
    /// </summary>
    IDeleteLearningElementInSpace GetDeleteInSpaceCommand(LearningElement learningElement, LearningSpace parentSpace,
        Action<LearningSpace> mappingAction);

    /// <summary>
    /// Creates a command to delete a learning element from a world.
    /// </summary>
    IDeleteLearningElementInWorld GetDeleteInWorldCommand(LearningElement learningElement, LearningWorld parentWorld,
        Action<LearningWorld> mappingAction);

    /// <summary>
    /// Creates a command to drag a learning element.
    /// </summary>
    IDragLearningElement GetDragCommand(LearningElement learningElement, double oldPositionX, double oldPositionY,
        double newPositionX, double newPositionY, Action<LearningElement> mappingAction);

    /// <summary>
    /// Creates a command to edit a learning element.
    /// </summary>
    IEditLearningElement GetEditCommand(LearningElement learningElement, LearningSpace? parentSpace, string name,
        string description, string goals, LearningElementDifficultyEnum difficulty, ElementModel elementModel,
        int workload, int points, ILearningContent learningContent, Action<LearningElement> mappingAction);

    /// <summary>
    /// Creates a command to load a learning element from a file.
    /// </summary>
    ILoadLearningElement GetLoadCommand(LearningSpace parentSpace, int slotIndex, string filepath,
        IBusinessLogic businessLogic, Action<LearningSpace> mappingAction);

    /// <summary>
    /// Creates a command to load a learning element from a stream.
    /// </summary>
    ILoadLearningElement GetLoadCommand(LearningSpace parentSpace, int slotIndex, Stream stream,
        IBusinessLogic businessLogic, Action<LearningSpace> mappingAction);

    /// <summary>
    /// Creates a command to save a learning element to a file.
    /// </summary>
    ISaveLearningElement GetSaveCommand(IBusinessLogic businessLogic, LearningElement learningElement, string filepath);
}