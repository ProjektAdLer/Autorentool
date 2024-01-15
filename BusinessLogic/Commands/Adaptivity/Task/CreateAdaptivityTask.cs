using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent.Adaptivity;
using BusinessLogic.Entities.LearningContent.Adaptivity.Question;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.Adaptivity.Task;

public class CreateAdaptivityTask : ICreateAdaptivityTask
{
    private IMemento? _memento;

    public CreateAdaptivityTask(AdaptivityContent adaptivityContent, string name,
        Action<AdaptivityContent> mappingAction, ILogger<CreateAdaptivityTask> logger)
    {
        AdaptivityContent = adaptivityContent;
        AdaptivityTask = new AdaptivityTask(new List<IAdaptivityQuestion>(), null, name);
        MappingAction = mappingAction;
        Logger = logger;
    }

    internal AdaptivityContent AdaptivityContent { get; }
    internal AdaptivityTask AdaptivityTask { get; }
    internal Action<AdaptivityContent> MappingAction { get; }
    private ILogger<CreateAdaptivityTask> Logger { get; }
    public string Name => nameof(CreateAdaptivityTask);

    public void Execute()
    {
        _memento = AdaptivityContent.GetMemento();

        AdaptivityContent.Tasks.Add(AdaptivityTask);
        AdaptivityContent.UnsavedChanges = true;

        Logger.LogTrace(
            "Created AdaptivityTask {AdaptivityTaskName} ({AdaptivityTaskId}) in AdaptivityContent {AdaptivityContentName}",
            AdaptivityTask.Name, AdaptivityTask.Id, AdaptivityContent.Name);

        MappingAction.Invoke(AdaptivityContent);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }

        AdaptivityContent.RestoreMemento(_memento);

        Logger.LogTrace(
            "Undone creation of AdaptivityTask {AdaptivityTaskName} ({AdaptivityTaskId}). Restored AdaptivityContent {AdaptivityContentName} to previous state",
            AdaptivityTask.Name, AdaptivityTask.Id, AdaptivityContent.Name);

        MappingAction.Invoke(AdaptivityContent);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing CreateAdaptivityTask");
        Execute();
    }
}