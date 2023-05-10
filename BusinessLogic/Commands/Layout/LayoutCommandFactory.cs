using BusinessLogic.Entities;
using Shared;

namespace BusinessLogic.Commands.Layout;

public class LayoutCommandFactory : ILayoutCommandFactory
{
    public IChangeLearningSpaceLayout GetChangeCommand(LearningSpace learningSpace, LearningWorld learningWorld,
        FloorPlanEnum floorPlanName, Action<LearningWorld> mappingAction) =>
        new ChangeLearningSpaceLayout(learningSpace, learningWorld, floorPlanName, mappingAction);

    public IPlaceLearningElementInLayoutFromLayout GetPlaceFromLayoutCommand(LearningSpace parentSpace,
        ILearningElement learningElement, int newSlotIndex, Action<LearningSpace> mappingAction) =>
        new PlaceLearningElementInLayoutFromLayout(parentSpace, learningElement, newSlotIndex, mappingAction);

    public IPlaceLearningElementInLayoutFromUnplaced GetPlaceFromUnplacedCommand(LearningWorld learningWorld,
        LearningSpace learningSpace, ILearningElement learningElement, int newSlotIndex,
        Action<LearningWorld> mappingAction) =>
        new PlaceLearningElementInLayoutFromUnplaced(learningWorld, learningSpace, learningElement, newSlotIndex,
            mappingAction);

    public IRemoveLearningElementFromLayout GetRemoveCommand(LearningWorld learningWorld, LearningSpace learningSpace,
        ILearningElement learningElement, Action<LearningWorld> mappingAction) =>
        new RemoveLearningElementFromLayout(learningWorld, learningSpace, learningElement, mappingAction);
}