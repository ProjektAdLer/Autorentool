using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;
using Shared;

namespace BusinessLogic.Commands.Layout;

public class LayoutCommandFactory : ILayoutCommandFactory
{
    public IChangeLearningSpaceLayout GetChangeCommand(ILearningSpace learningSpace, LearningWorld learningWorld,
        FloorPlanEnum floorPlanName, Action<LearningWorld> mappingAction, ILogger<LayoutCommandFactory> logger) =>
        new ChangeLearningSpaceLayout(learningSpace, learningWorld, floorPlanName, mappingAction, logger);

    public IPlaceLearningElementInLayoutFromLayout GetPlaceFromLayoutCommand(LearningSpace parentSpace,
        ILearningElement learningElement, int newSlotIndex, Action<LearningSpace> mappingAction,
        ILogger<LayoutCommandFactory> logger) =>
        new PlaceLearningElementInLayoutFromLayout(parentSpace, learningElement, newSlotIndex, mappingAction, logger);

    public IPlaceLearningElementInLayoutFromUnplaced GetPlaceFromUnplacedCommand(LearningWorld learningWorld,
        LearningSpace learningSpace, ILearningElement learningElement, int newSlotIndex,
        Action<LearningWorld> mappingAction, ILogger<LayoutCommandFactory> logger) =>
        new PlaceLearningElementInLayoutFromUnplaced(learningWorld, learningSpace, learningElement, newSlotIndex,
            mappingAction, logger);

    public IRemoveLearningElementFromLayout GetRemoveCommand(LearningWorld learningWorld, LearningSpace learningSpace,
        ILearningElement learningElement, Action<LearningWorld> mappingAction, ILogger<LayoutCommandFactory> logger) =>
        new RemoveLearningElementFromLayout(learningWorld, learningSpace, learningElement, mappingAction, logger);
}