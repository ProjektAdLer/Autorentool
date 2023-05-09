using BusinessLogic.API;
using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using Shared;

namespace BusinessLogic.Commands.Element;

public interface IElementCommandFactory
{
    ICreateLearningElementInSlot GetCreateInSlotCommand(LearningSpace parentSpace, int slotIndex, string name,
        ILearningContent learningContent, string description, string goals,
        LearningElementDifficultyEnum difficulty, int workload, int points, double positionX, double positionY,
        Action<LearningSpace> mappingAction);

    ICreateLearningElementInSlot GetCreateInSlotCommand(LearningSpace parentSpace, int slotIndex,
        LearningElement learningElement,
        Action<LearningSpace> mappingAction);

    ICreateUnplacedLearningElement GetCreateUnplacedCommand(LearningWorld learningWorld, string name,
        ILearningContent learningContent, string description, string goals,
        LearningElementDifficultyEnum difficulty, int workload, int points, double positionX, double positionY,
        Action<LearningWorld> mappingAction);

    IDeleteLearningElementInSpace GetDeleteInSpaceCommand(LearningElement learningElement, LearningSpace parentSpace,
        Action<LearningSpace> mappingAction);

    IDeleteLearningElementInWorld GetDeleteInWorldCommand(LearningElement learningElement, LearningWorld parentWorld,
        Action<LearningWorld> mappingAction);

    IDragLearningElement GetDragCommand(LearningElement learningElement, double oldPositionX, double oldPositionY,
        double newPositionX, double newPositionY, Action<LearningElement> mappingAction);

    IEditLearningElement GetEditCommand(LearningElement learningElement, LearningSpace? parentSpace, string name,
        string description, string goals, LearningElementDifficultyEnum difficulty,
        int workload, int points, ILearningContent learningContent, Action<LearningElement> mappingAction);

    ILoadLearningElement GetLoadCommand(LearningSpace parentSpace, int slotIndex, string filepath,
        IBusinessLogic businessLogic,
        Action<LearningSpace> mappingAction);

    ILoadLearningElement GetLoadCommand(LearningSpace parentSpace, int slotIndex, Stream stream,
        IBusinessLogic businessLogic,
        Action<LearningSpace> mappingAction);

    ISaveLearningElement GetSaveCommand(IBusinessLogic businessLogic, LearningElement learningElement, string filepath);
}