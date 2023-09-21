using BusinessLogic.Entities.LearningContent.Adaptivity;
using Microsoft.Extensions.Logging;
using Shared.Adaptivity;

namespace BusinessLogic.Commands.Adaptivity.Task;

public class TaskCommandFactory : ITaskCommandFactory
{
    public TaskCommandFactory(ILoggerFactory loggerFactory)
    {
        LoggerFactory = loggerFactory;
    }

    private ILoggerFactory LoggerFactory { get; }

    public ICreateAdaptivityTask GetCreateCommand(AdaptivityContent adaptivityContent, string name,
        Action<AdaptivityContent> mappingAction)
    {
        return new CreateAdaptivityTask(adaptivityContent, name, mappingAction,
            LoggerFactory.CreateLogger<CreateAdaptivityTask>());
    }

    public IEditAdaptivityTask GetEditCommand(AdaptivityTask adaptivityTask, string name,
        QuestionDifficulty? minimumRequiredDifficulty,
        Action<AdaptivityTask> mappingAction)
    {
        return new EditAdaptivityTask(adaptivityTask, name, minimumRequiredDifficulty, mappingAction,
            LoggerFactory.CreateLogger<EditAdaptivityTask>());
    }

    public IDeleteAdaptivityTask GetDeleteCommand(AdaptivityContent adaptivityContent, AdaptivityTask adaptivityTask,
        Action<AdaptivityContent> mappingAction)
    {
        return new DeleteAdaptivityTask(adaptivityContent, adaptivityTask, mappingAction,
            LoggerFactory.CreateLogger<DeleteAdaptivityTask>());
    }
}