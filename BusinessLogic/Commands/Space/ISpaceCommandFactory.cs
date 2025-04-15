using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningOutcome;
using Shared;
using Shared.Theme;

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
        string description, LearningOutcomeCollection learningOutcomeCollection, int requiredPoints, Theme theme,
        double positionX,
        double positionY,
        Entities.Topic? topic, Action<LearningWorld> mappingAction);

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
    IEditLearningSpace GetEditCommand(ILearningSpace learningSpace, string name,
        string description, int requiredPoints, Theme theme, Entities.Topic? topic,
        Action<ILearningSpace> mappingAction);
}