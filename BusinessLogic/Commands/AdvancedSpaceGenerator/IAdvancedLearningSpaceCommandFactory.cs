using BusinessLogic.Commands.AdvancedSpaceGenerator.AdvancedLayout;
using BusinessLogic.Commands.AdvancedSpaceGenerator.AdvancedSpace;
using BusinessLogic.Commands.AdvancedSpaceGenerator.Elements;
using BusinessLogic.Entities;
using BusinessLogic.Entities.AdvancedLearningSpaces;
using BusinessLogic.Entities.LearningContent;
using Shared;

namespace BusinessLogic.Commands.AdvancedSpaceGenerator;

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
        AdvancedLearningSpace advancedLearningSpace,
        Action<LearningWorld> mappingAction);

    /// <summary>
    /// Creates a command to delete a learning space.
    /// </summary>
    IDeleteAdvancedLearningSpace GetDeleteAdvancedLearningSpaceCommand(LearningWorld learningWorld,
        AdvancedLearningSpace advancedLearningSpace,
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
        AdvancedLearningSpace advancedLearningSpace, ILearningElement learningElement,
        int newSlotIndex,
        Action<LearningWorld> mappingAction);

    /// <summary>
    /// Creates a command to place learning elements from advanced layout
    /// </summary>
    IPlaceLearningElementInAdvancedLayoutFromAdvancedLayout GetPlaceFromAdvancedLayoutCommand(
        AdvancedLearningSpace parentSpace, ILearningElement learningElement,
        int newSlotIndex,
        Action<IAdvancedLearningSpace> mappingAction);
    
    /// <summary>
    /// Creates a command to remove element from AdvancedSpace and place into unplaced.
    /// </summary>
    IRemoveLearningElementFromAdvancedLayout GetRemoveFromAdvancedLayoutCommand(
        LearningWorld learningWorld, AdvancedLearningSpace parentSpace, ILearningElement learningElement,
        Action<ILearningWorld> mappingAction);

    /// <summary>
    /// Creates a command to create a learning element in an advanced slot
    /// </summary>
    ICreateLearningElementInAdvancedSlot GetCreateElementInSlotCommand(
        AdvancedLearningSpace parentSpace, int slotIndex, string name,
        ILearningContent learningContent, string description, string goals, LearningElementDifficultyEnum difficulty,
        ElementModel elementModel, int workload, int points, 
        Action<IAdvancedLearningSpace> mappingAction);
    
    /// <summary>
    /// Creates a command to create a learning element in an advanced slot
    /// </summary>
    ICreateLearningElementInAdvancedSlot GetCreateElementInSlotCommand(
        AdvancedLearningSpace parentSpace, int slotIndex, LearningElement learningElement,
        Action<IAdvancedLearningSpace> mappingAction);

        /// <summary>
    /// Creates a command to delete a learning element from an advanced space
    /// </summary>
    IDeleteLearningElementInAdvancedSpace GetDeleteElementInSlotCommand(
        LearningElement learningElement, AdvancedLearningSpace parentSpace, int slotIndex,
        Action<IAdvancedLearningSpace> mappingAction);
}