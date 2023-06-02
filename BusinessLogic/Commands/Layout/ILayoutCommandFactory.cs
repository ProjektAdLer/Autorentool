using BusinessLogic.Entities;
using Shared;

namespace BusinessLogic.Commands.Layout;

/// <summary>
/// Factory for creating commands relating to learning space layouts.
/// </summary>
public interface ILayoutCommandFactory
{
    /// <summary>
    /// Creates a command to change the layout of a learning space.
    /// </summary>
    IChangeLearningSpaceLayout GetChangeCommand(ILearningSpace learningSpace, LearningWorld learningWorld,
        FloorPlanEnum floorPlanName, Action<LearningWorld> mappingAction);

    /// <summary>
    /// Creates a command to place a learning element in a layout from another layout.
    /// </summary>
    IPlaceLearningElementInLayoutFromLayout GetPlaceFromLayoutCommand(LearningSpace parentSpace,
        ILearningElement learningElement, int newSlotIndex,
        Action<LearningSpace> mappingAction);

    /// <summary>
    /// Creates a command to place a learning element in a layout from the unplaced elements in the world.
    /// </summary>
    IPlaceLearningElementInLayoutFromUnplaced GetPlaceFromUnplacedCommand(LearningWorld learningWorld,
        LearningSpace learningSpace,
        ILearningElement learningElement, int newSlotIndex, Action<LearningWorld> mappingAction);

    /// <summary>
    /// Creates a command to remove a learning element from a layout.
    /// </summary>
    IRemoveLearningElementFromLayout GetRemoveCommand(LearningWorld learningWorld, LearningSpace learningSpace,
        ILearningElement learningElement, Action<LearningWorld> mappingAction);
}