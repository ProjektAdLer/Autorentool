using BusinessLogic.API;
using BusinessLogic.Commands.AdvancedSpace.AdvancedLayout;
using BusinessLogic.Commands.AdvancedSpace.Elements;
using BusinessLogic.Commands.Layout;
using BusinessLogic.Entities;
using BusinessLogic.Entities.AdvancedLearningSpaces;
using BusinessLogic.Entities.LearningContent;
using Microsoft.Extensions.Logging;
using Shared;

namespace BusinessLogic.Commands.Space.AdvancedLearningSpace;

public class AdvancedLearningSpaceCommandFactory : IAdvancedLearningSpaceCommandFactory
{
    public AdvancedLearningSpaceCommandFactory(ILoggerFactory loggerFactory)
    {
        LoggerFactory = loggerFactory;
    }

    private ILoggerFactory LoggerFactory { get; }

    #region AdvancedLearningSpace

    public ICreateAdvancedLearningSpace GetCreateAdvancedLearningSpaceCommand(LearningWorld learningWorld, string name,
        string description,
        string goals,
        int requiredPoints, Theme theme, double positionX, double positionY, Entities.Topic? topic,
        Action<LearningWorld> mappingAction) =>
        new CreateAdvancedLearningSpace(learningWorld, name, description, goals, requiredPoints, theme, positionX,
            positionY,
            topic, mappingAction, LoggerFactory.CreateLogger<CreateAdvancedLearningSpace>());

    public ICreateAdvancedLearningSpace GetCreateAdvancedLearningSpaceCommand(LearningWorld learningWorld,
        Entities.AdvancedLearningSpaces.AdvancedLearningSpace advancedLearningSpace,
        Action<LearningWorld> mappingAction) =>
        new CreateAdvancedLearningSpace(learningWorld, advancedLearningSpace, mappingAction,
            LoggerFactory.CreateLogger<CreateAdvancedLearningSpace>());

    public IDeleteAdvancedLearningSpace GetDeleteAdvancedLearningSpaceCommand(LearningWorld learningWorld,
        Entities.AdvancedLearningSpaces.AdvancedLearningSpace advancedLearningSpace,
        Action<LearningWorld> mappingAction) =>
        new DeleteAdvancedLearningSpace(learningWorld, advancedLearningSpace, mappingAction,
            LoggerFactory.CreateLogger<DeleteAdvancedLearningSpace>());

    public IEditAdvancedLearningSpace GetEditAdvancedLearningSpaceCommand(IAdvancedLearningSpace advancedLearningSpace,
        string name,
        string description,
        string goals,
        int requiredPoints, Theme theme, Entities.Topic? topic, Action<IAdvancedLearningSpace> mappingAction) =>
        new EditAdvancedLearningSpace(advancedLearningSpace, name, description, goals, requiredPoints, theme, topic,
            mappingAction,
            LoggerFactory.CreateLogger<EditAdvancedLearningSpace>());

    #endregion

    #region AdvancedLearningSpaceLayout

    public IPlaceLearningElementInAdvancedLayoutFromUnplaced GetPlaceFromUnplacedCommand(LearningWorld learningWorld,
        Entities.AdvancedLearningSpaces.AdvancedLearningSpace advancedLearningSpace, ILearningElement learningElement,
        int newSlotIndex,
        Action<LearningWorld> mappingAction) =>
        new PlaceLearningElementInAdvancedLayoutFromUnplaced(learningWorld, advancedLearningSpace, learningElement,
            newSlotIndex,
            mappingAction, LoggerFactory.CreateLogger<PlaceLearningElementInAdvancedLayoutFromUnplaced>());

    public IPlaceLearningElementInAdvancedLayoutFromAdvancedLayout GetPlaceFromAdvancedLayoutCommand(
        Entities.AdvancedLearningSpaces.AdvancedLearningSpace parentSpace,
        ILearningElement learningElement, int newSlotIndex, Action<IAdvancedLearningSpace> mappingAction) =>
        new PlaceLearningElementInAdvancedLayoutFromAdvancedLayout(parentSpace, learningElement, newSlotIndex,
            mappingAction,
            LoggerFactory.CreateLogger<PlaceLearningElementInAdvancedLayoutFromAdvancedLayout>());

    #endregion


    #region Elements

    public ICreateLearningElementInAdvancedSlot GetCreateElementInSlotCommand(
        Entities.AdvancedLearningSpaces.AdvancedLearningSpace parentSpace, int slotIndex,
        string name, ILearningContent learningContent, string description, string goals,
        LearningElementDifficultyEnum difficulty, ElementModel elementModel, int workload, int points, double positionX,
        double positionY, Action<IAdvancedLearningSpace> mappingAction) =>
        new CreateLearningElementInAdvancedSlot(parentSpace, slotIndex, name, learningContent, description, goals,
            difficulty, elementModel, workload, points, positionX, positionY, mappingAction,
            LoggerFactory.CreateLogger<CreateLearningElementInAdvancedSlot>());


    public ICreateLearningElementInAdvancedSlot GetCreateElementInSlotCommand(
        Entities.AdvancedLearningSpaces.AdvancedLearningSpace parentSpace, int slotIndex,
        LearningElement learningElement, Action<IAdvancedLearningSpace> mappingAction) =>
        new CreateLearningElementInAdvancedSlot(parentSpace, slotIndex, learningElement, mappingAction,
            LoggerFactory.CreateLogger<CreateLearningElementInAdvancedSlot>());


    public IDeleteLearningElementInAdvancedSpace GetDeleteElementInSlotCommand(LearningElement learningElement,
        Entities.AdvancedLearningSpaces.AdvancedLearningSpace parentSpace, int slotIndex,
        Action<IAdvancedLearningSpace> mappingAction) =>
        new DeleteLearningElementInAdvancedSpace(learningElement, parentSpace, mappingAction,
            LoggerFactory.CreateLogger<DeleteLearningElementInAdvancedSpace>());

    #endregion
}