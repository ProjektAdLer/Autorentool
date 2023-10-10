using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent.Adaptivity;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.Adaptivity.Task;

public class DeleteAdaptivityTask : IDeleteAdaptivityTask
{
    private IMemento? _memento;

    public DeleteAdaptivityTask(AdaptivityContent adaptivityContent, AdaptivityTask adaptivityTask,
        Action<AdaptivityContent> mappingAction, ILogger<DeleteAdaptivityTask> logger)
    {
        AdaptivityContent = adaptivityContent;
        AdaptivityTask = adaptivityTask;
        MappingAction = mappingAction;
        Logger = logger;
    }

    internal AdaptivityContent AdaptivityContent { get; }
    internal AdaptivityTask AdaptivityTask { get; }
    internal Action<AdaptivityContent> MappingAction { get; }
    private ILogger<DeleteAdaptivityTask> Logger { get; }
    public string Name => nameof(DeleteAdaptivityTask);

    public void Execute()
    {
        _memento = AdaptivityContent.GetMemento();

        var taskToDelete = AdaptivityContent.Tasks.FirstOrDefault(x => x.Id == AdaptivityTask.Id);

        if (taskToDelete != null)
        {
            AdaptivityContent.Tasks.Remove(taskToDelete);

            Logger.LogTrace(
                "Deleted AdaptivityTask {AdaptivityTaskName} ({AdaptivityTaskId}) in AdaptivityContent {AdaptivityContentName}",
                AdaptivityTask.Name, AdaptivityTask.Id, AdaptivityContent.Name);
            MappingAction.Invoke(AdaptivityContent);
        }
        else
        {
            Logger.LogTrace(
                "Tried to delete AdaptivityTask {AdaptivityTaskName} ({AdaptivityTaskId}) in AdaptivityContent {AdaptivityContentName}, but it was not found. Skip deletion",
                AdaptivityTask.Name, AdaptivityTask.Id, AdaptivityContent.Name);
        }
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }

        AdaptivityContent.RestoreMemento(_memento);

        Logger.LogTrace(
            "Undone deletion of AdaptivityTask {AdaptivityTaskName} ({AdaptivityTaskId}). Restored AdaptivityContent {AdaptivityContentName} to previous state",
            AdaptivityTask.Name, AdaptivityTask.Id, AdaptivityContent.Name);

        MappingAction.Invoke(AdaptivityContent);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing DeleteAdaptivityTask");
        Execute();
    }
}