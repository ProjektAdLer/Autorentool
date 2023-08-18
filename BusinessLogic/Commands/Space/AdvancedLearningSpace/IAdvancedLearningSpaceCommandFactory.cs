using BusinessLogic.API;
using BusinessLogic.Entities;
using BusinessLogic.Entities.AdvancedLearningSpaces;
using Shared;

namespace BusinessLogic.Commands.Space.AdvancedLearningSpace;

public interface IAdvancedLearningSpaceCommandFactory
{
    /// <summary>
    /// Creates a command to create a learning space.
    /// </summary>
    ICreateAdvancedLearningSpace GetCreateCommand(LearningWorld learningWorld, string name,
        string description, string goals, int requiredPoints, Theme theme, double positionX, double positionY,
        Entities.Topic? topic, Action<LearningWorld> mappingAction);


    /// <summary>
    /// Creates a command to delete a learning space.
    /// </summary>
    IDeleteAdvancedLearningSpace GetDeleteCommand(LearningWorld learningWorld, Entities.AdvancedLearningSpaces.AdvancedLearningSpace advancedLearningSpace,
        Action<LearningWorld> mappingAction);

    /// <summary>
    /// Creates a command to edit a learning space.
    /// </summary>
    IEditAdvancedLearningSpace GetEditCommand(IAdvancedLearningSpace advancedLearningSpace, string name,
        string description, string goals, int requiredPoints, Theme theme, Entities.Topic? topic,
        Action<IAdvancedLearningSpace> mappingAction);

    // /// <summary>
    // /// Creates a command to save a learning space.
    // /// </summary>
    // ISaveAdvancedLearningSpace GetSaveCommand(IBusinessLogic businessLogic, Entities.AdvancedLearningSpaces.AdvancedLearningSpace advancedLearningSpace, string filepath);
}