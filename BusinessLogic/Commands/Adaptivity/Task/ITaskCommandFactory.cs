using BusinessLogic.Entities.LearningContent.Adaptivity;
using Shared.Adaptivity;

namespace BusinessLogic.Commands.Adaptivity.Task;

/// <summary>
/// Factory for creating commands relating to adaptivity tasks.
/// </summary>
public interface ITaskCommandFactory
{
    /// <summary>
    /// Creates a command to create a adaptivity task.
    /// </summary>
    ICreateAdaptivityTask GetCreateCommand(AdaptivityContent adaptivityContent, string name,
        Action<AdaptivityContent> mappingAction);

    /// <summary>
    /// Creates a command to edit a adaptivity task.
    /// </summary>
    IEditAdaptivityTask GetEditCommand(AdaptivityTask adaptivityTask, string name,
        QuestionDifficulty? minimumRequiredDifficulty, Action<AdaptivityTask> mappingAction);

    /// <summary>
    /// Creates a command to delete a adaptivity task.
    /// </summary>
    IDeleteAdaptivityTask GetDeleteCommand(AdaptivityContent adaptivityContent, AdaptivityTask adaptivityTask,
        Action<AdaptivityContent> mappingAction);
}