using BusinessLogic.API;
using BusinessLogic.Entities;

namespace BusinessLogic.Commands.Space;

/// <summary>
/// Factory for creating commands relating to learning spaces.
/// </summary>
public interface ISpaceCommandFactory
{
    /// <summary>
    /// Creates a command to create a learning space.
    /// </summary>
    ICreateLearningSpace GetCreateCommand(LearningWorld learningWorld, string name,
        string description, string goals, int requiredPoints, double positionX, double positionY, Entities.Topic? topic,
        Action<LearningWorld> mappingAction);

    /// <summary>
    /// Creates a command to create a learning space.
    /// </summary>
    ICreateLearningSpace GetCreateCommand(LearningWorld learningWorld, LearningSpace learningSpace,
        Action<LearningWorld> mappingAction);

    /// <summary>
    /// Creates a command to delete a learning space.
    /// </summary>
    IDeleteLearningSpace GetDeleteCommand(LearningWorld learningWorld, LearningSpace learningSpace,
        Action<LearningWorld> mappingAction);

    /// <summary>
    /// Creates a command to edit a learning space.
    /// </summary>
    IEditLearningSpace GetEditCommand(LearningSpace learningSpace, string name,
        string description, string goals, int requiredPoints, Entities.Topic? topic,
        Action<LearningSpace> mappingAction);

    /// <summary>
    /// Creates a command to load a learning space.
    /// </summary>
    ILoadLearningSpace GetLoadCommand(LearningWorld learningWorld, string filepath, IBusinessLogic businessLogic,
        Action<LearningWorld> mappingAction);

    /// <summary>
    /// Creates a command to load a learning space.
    /// </summary>
    ILoadLearningSpace GetLoadCommand(LearningWorld learningWorld, Stream stream, IBusinessLogic businessLogic,
        Action<LearningWorld> mappingAction);

    /// <summary>
    /// Creates a command to save a learning space.
    /// </summary>
    ISaveLearningSpace GetSaveCommand(IBusinessLogic businessLogic, LearningSpace learningSpace, string filepath);
}