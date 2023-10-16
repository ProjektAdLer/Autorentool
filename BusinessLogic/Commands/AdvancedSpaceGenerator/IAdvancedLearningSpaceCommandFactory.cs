using BusinessLogic.API;
using BusinessLogic.Commands.AdvancedSpace.AdvancedLayout;
using BusinessLogic.Commands.AdvancedSpace.Elements;
using BusinessLogic.Entities;
using BusinessLogic.Entities.AdvancedLearningSpaces;
using BusinessLogic.Entities.LearningContent;
using Shared;

namespace BusinessLogic.Commands.Space.AdvancedLearningSpace;

public interface IAdvancedLearningSpaceCommandFactory
{
    /// <summary>
    /// Creates a command to create a learning space.
    /// </summary>
    ICreateAdvancedLearningSpace GetCreateAdvancedLearningSpaceCommand(LearningWorld learningWorld, string name,
        string description, string goals, int requiredPoints, Theme theme, double positionX, double positionY,
        Entities.Topic? topic, Action<LearningWorld> mappingAction);

    /// <summary>
    /// Creates a command to create a learning space.
    /// </summary>
    ICreateAdvancedLearningSpace GetCreateAdvancedLearningSpaceCommand(LearningWorld learningWorld,
        Entities.AdvancedLearningSpaces.AdvancedLearningSpace advancedLearningSpace,
        Action<LearningWorld> mappingAction);

    /// <summary>
    /// Creates a command to delete a learning space.
    /// </summary>
    IDeleteAdvancedLearningSpace GetDeleteAdvancedLearningSpaceCommand(LearningWorld learningWorld,
        Entities.AdvancedLearningSpaces.AdvancedLearningSpace advancedLearningSpace,
        Action<LearningWorld> mappingAction);

    /// <summary>
    /// Creates a command to edit a learning space.
    /// </summary>
    IEditAdvancedLearningSpace GetEditAdvancedLearningSpaceCommand(IAdvancedLearningSpace advancedLearningSpace,
        string name,
        string description, string goals, int requiredPoints, Theme theme, Entities.Topic? topic,
        Action<IAdvancedLearningSpace> mappingAction);


    /// <summary>
    /// Creates a command to place learning elements from unplaced
    /// </summary>
    IPlaceLearningElementInAdvancedLayoutFromUnplaced GetPlaceFromUnplacedCommand(LearningWorld learningWorld,
        Entities.AdvancedLearningSpaces.AdvancedLearningSpace advancedLearningSpace, ILearningElement learningElement,
        int newSlotIndex,
        Action<LearningWorld> mappingAction);

    /// <summary>
    /// Creates a command to place learning elements from advanced layout
    /// </summary>
    IPlaceLearningElementInAdvancedLayoutFromAdvancedLayout GetPlaceFromAdvancedLayoutCommand(
        Entities.AdvancedLearningSpaces.AdvancedLearningSpace parentSpace, ILearningElement learningElement,
        int newSlotIndex,
        Action<IAdvancedLearningSpace> mappingAction);

    /// <summary>
    /// Creates a command to create a learning element in an advanced slot
    /// </summary>
    ICreateLearningElementInAdvancedSlot GetCreateElementInSlotCommand(
        Entities.AdvancedLearningSpaces.AdvancedLearningSpace parentSpace, int slotIndex, string name,
        ILearningContent learningContent, string description, string goals, LearningElementDifficultyEnum difficulty,
        ElementModel elementModel, int workload, int points, double positionX, double positionY,
        Action<IAdvancedLearningSpace> mappingAction);
    
    /// <summary>
    /// Creates a command to create a learning element in an advanced slot
    /// </summary>
    ICreateLearningElementInAdvancedSlot GetCreateElementInSlotCommand(
        Entities.AdvancedLearningSpaces.AdvancedLearningSpace parentSpace, int slotIndex, LearningElement learningElement,
        Action<IAdvancedLearningSpace> mappingAction);

        /// <summary>
    /// Creates a command to delete a learning element from an advanced space
    /// </summary>
    IDeleteLearningElementInAdvancedSpace GetDeleteElementInSlotCommand(
        LearningElement learningElement, Entities.AdvancedLearningSpaces.AdvancedLearningSpace parentSpace, int slotIndex,
        Action<IAdvancedLearningSpace> mappingAction);
}