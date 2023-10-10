using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent.Adaptivity;
using Microsoft.Extensions.Logging;
using Shared.Adaptivity;

namespace BusinessLogic.Commands.Adaptivity.Task;

public class EditAdaptivityTask : IEditAdaptivityTask
{
    private IMemento? _memento;

    public EditAdaptivityTask(AdaptivityTask adaptivityTask, string name, QuestionDifficulty? minimumRequiredDifficulty,
        Action<AdaptivityTask> mappingAction, ILogger<EditAdaptivityTask> logger)
    {
        AdaptivityTask = adaptivityTask;
        AdaptivityTaskName = name;
        MinimumRequiredDifficulty = minimumRequiredDifficulty;
        MappingAction = mappingAction;
        Logger = logger;
    }

    internal AdaptivityTask AdaptivityTask { get; }
    internal string AdaptivityTaskName { get; }
    internal QuestionDifficulty? MinimumRequiredDifficulty { get; }
    internal Action<AdaptivityTask> MappingAction { get; }
    private ILogger<EditAdaptivityTask> Logger { get; }
    public string Name => nameof(EditAdaptivityTask);

    public void Execute()
    {
        _memento = AdaptivityTask.GetMemento();

        Logger.LogTrace(
            "Editing AdaptivityTask {AdaptivityTaskName} ({AdaptivityTaskId}). Previous Values: Name {PreviousName}, MinimumRequiredDifficulty {PreviousMinimumRequiredDifficulty}",
            AdaptivityTask.Name, AdaptivityTask.Id, AdaptivityTask.Name, AdaptivityTask.MinimumRequiredDifficulty);

        AdaptivityTask.Name = AdaptivityTaskName;
        AdaptivityTask.MinimumRequiredDifficulty = MinimumRequiredDifficulty;

        Logger.LogTrace(
            "Edited AdaptivityTask {AdaptivityTaskName} ({AdaptivityTaskId}). Updated Values: Name {PreviousName}, MinimumRequiredDifficulty {PreviousMinimumRequiredDifficulty}",
            AdaptivityTask.Name, AdaptivityTask.Id, AdaptivityTask.Name, AdaptivityTask.MinimumRequiredDifficulty);


        MappingAction.Invoke(AdaptivityTask);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }

        AdaptivityTask.RestoreMemento(_memento);

        Logger.LogTrace(
            "Undone editing of AdaptivityTask {AdaptivityTaskName} ({AdaptivityTaskId})",
            AdaptivityTask.Name, AdaptivityTask.Id);

        MappingAction.Invoke(AdaptivityTask);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing EditAdaptivityTask");
        Execute();
    }
}