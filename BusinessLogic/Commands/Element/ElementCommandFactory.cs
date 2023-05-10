using BusinessLogic.API;
using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using Shared;

namespace BusinessLogic.Commands.Element;

public class ElementCommandFactory : IElementCommandFactory
{
    public ICreateLearningElementInSlot GetCreateInSlotCommand(LearningSpace parentSpace, int slotIndex, string name,
        ILearningContent learningContent, string description, string goals, LearningElementDifficultyEnum difficulty,
        int workload, int points, double positionX, double positionY, Action<LearningSpace> mappingAction) =>
        new CreateLearningElementInSlot(parentSpace, slotIndex, name, learningContent, description, goals,
            difficulty, workload, points, positionX, positionY, mappingAction);

    public ICreateLearningElementInSlot GetCreateInSlotCommand(LearningSpace parentSpace, int slotIndex,
        LearningElement learningElement, Action<LearningSpace> mappingAction) =>
        new CreateLearningElementInSlot(parentSpace, slotIndex, learningElement, mappingAction);

    public ICreateUnplacedLearningElement GetCreateUnplacedCommand(LearningWorld learningWorld, string name,
        ILearningContent learningContent, string description, string goals, LearningElementDifficultyEnum difficulty,
        int workload, int points, double positionX, double positionY, Action<LearningWorld> mappingAction) =>
        new CreateUnplacedLearningElement(learningWorld, name, learningContent, description, goals, difficulty,
            workload, points, positionX, positionY, mappingAction);

    public IDeleteLearningElementInSpace GetDeleteInSpaceCommand(LearningElement learningElement,
        LearningSpace parentSpace, Action<LearningSpace> mappingAction) =>
        new DeleteLearningElementInSpace(learningElement, parentSpace, mappingAction);

    public IDeleteLearningElementInWorld GetDeleteInWorldCommand(LearningElement learningElement,
        LearningWorld parentWorld, Action<LearningWorld> mappingAction) =>
        new DeleteLearningElementInWorld(learningElement, parentWorld, mappingAction);

    public IDragLearningElement GetDragCommand(LearningElement learningElement, double oldPositionX,
        double oldPositionY,
        double newPositionX, double newPositionY, Action<LearningElement> mappingAction) =>
        new DragLearningElement(learningElement, oldPositionX, oldPositionY, newPositionX, newPositionY,
            mappingAction);

    public IEditLearningElement GetEditCommand(LearningElement learningElement, LearningSpace? parentSpace, string name,
        string description, string goals, LearningElementDifficultyEnum difficulty, int workload, int points,
        ILearningContent learningContent, Action<LearningElement> mappingAction) =>
        new EditLearningElement(learningElement, parentSpace, name, description, goals, difficulty, workload,
            points, learningContent, mappingAction);

    public ILoadLearningElement GetLoadCommand(LearningSpace parentSpace, int slotIndex, string filepath,
        IBusinessLogic businessLogic, Action<LearningSpace> mappingAction) =>
        new LoadLearningElement(parentSpace, slotIndex, filepath, businessLogic, mappingAction);

    public ILoadLearningElement GetLoadCommand(LearningSpace parentSpace, int slotIndex, Stream stream,
        IBusinessLogic businessLogic, Action<LearningSpace> mappingAction) =>
        new LoadLearningElement(parentSpace, slotIndex, stream, businessLogic, mappingAction);

    public ISaveLearningElement GetSaveCommand(IBusinessLogic businessLogic, LearningElement learningElement,
        string filepath) => new SaveLearningElement(businessLogic, learningElement, filepath);
}