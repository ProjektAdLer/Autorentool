using BusinessLogic.Entities;
using Shared;

namespace BusinessLogic.Commands.Layout;

public interface ILayoutCommandFactory
{
    IChangeLearningSpaceLayout GetChangeCommand(LearningSpace learningSpace, LearningWorld learningWorld,
        FloorPlanEnum floorPlanName, Action<LearningWorld> mappingAction);

    IPlaceLearningElementInLayoutFromLayout GetPlaceFromLayoutCommand(LearningSpace parentSpace,
        ILearningElement learningElement, int newSlotIndex,
        Action<LearningSpace> mappingAction);

    IPlaceLearningElementInLayoutFromUnplaced GetPlaceFromUnplacedCommand(LearningWorld learningWorld,
        LearningSpace learningSpace,
        ILearningElement learningElement, int newSlotIndex, Action<LearningWorld> mappingAction);

    IRemoveLearningElementFromLayout GetRemoveCommand(LearningWorld learningWorld, LearningSpace learningSpace,
        ILearningElement learningElement, Action<LearningWorld> mappingAction);
}