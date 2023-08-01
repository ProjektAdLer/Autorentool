using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;
using Shared;

namespace BusinessLogic.Commands.Layout;

public class LayoutCommandFactory : ILayoutCommandFactory
{
    public LayoutCommandFactory(ILoggerFactory loggerFactory)
    {
        LoggerFactory = loggerFactory;
    }

    private ILoggerFactory LoggerFactory { get; }

    public IChangeLearningSpaceLayout GetChangeCommand(ILearningSpace learningSpace, LearningWorld learningWorld,
        FloorPlanEnum floorPlanName, Action<LearningWorld> mappingAction) =>
        new ChangeLearningSpaceLayout(learningSpace, learningWorld, floorPlanName, mappingAction,
            LoggerFactory.CreateLogger<ChangeLearningSpaceLayout>());

    public IPlaceLearningElementInLayoutFromLayout GetPlaceFromLayoutCommand(LearningSpace parentSpace,
        ILearningElement learningElement, int newSlotIndex, Action<LearningSpace> mappingAction) =>
        new PlaceLearningElementInLayoutFromLayout(parentSpace, learningElement, newSlotIndex, mappingAction,
            LoggerFactory.CreateLogger<PlaceLearningElementInLayoutFromLayout>());

    public IPlaceLearningElementInLayoutFromUnplaced GetPlaceFromUnplacedCommand(LearningWorld learningWorld,
        LearningSpace learningSpace, ILearningElement learningElement, int newSlotIndex,
        Action<LearningWorld> mappingAction) =>
        new PlaceLearningElementInLayoutFromUnplaced(learningWorld, learningSpace, learningElement, newSlotIndex,
            mappingAction, LoggerFactory.CreateLogger<PlaceLearningElementInLayoutFromUnplaced>());

    public IRemoveLearningElementFromLayout GetRemoveCommand(LearningWorld learningWorld, LearningSpace learningSpace,
        ILearningElement learningElement, Action<LearningWorld> mappingAction) =>
        new RemoveLearningElementFromLayout(learningWorld, learningSpace, learningElement, mappingAction,
            LoggerFactory.CreateLogger<RemoveLearningElementFromLayout>());
}