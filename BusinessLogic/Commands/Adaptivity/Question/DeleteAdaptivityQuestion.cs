using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent.Adaptivity;
using BusinessLogic.Entities.LearningContent.Adaptivity.Question;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.Adaptivity.Question;

public class DeleteAdaptivityQuestion : IDeleteAdaptivityQuestion
{
    private IMemento? _memento;

    public DeleteAdaptivityQuestion(AdaptivityTask task, IAdaptivityQuestion question,
        Action<AdaptivityTask> mappingAction, ILogger<DeleteAdaptivityQuestion> createLogger)
    {
        Task = task;
        Question = question;
        MappingAction = mappingAction;
        Logger = createLogger;
    }

    internal AdaptivityTask Task { get; }
    internal IAdaptivityQuestion Question { get; }
    internal Action<AdaptivityTask> MappingAction { get; }
    private ILogger<DeleteAdaptivityQuestion> Logger { get; }
    public string Name => nameof(DeleteAdaptivityQuestion);

    public void Execute()
    {
        _memento = Task.GetMemento();

        var questionToDelete = Task.Questions.FirstOrDefault(x => x.Id == Question.Id);

        if (questionToDelete != null)
        {
            Task.Questions.Remove(questionToDelete);
            Task.UnsavedChanges = true;
            if (Task.MinimumRequiredDifficulty == questionToDelete.Difficulty)
            {
                Task.MinimumRequiredDifficulty = Task.Questions.Any() ? Task.Questions.Min(x => x.Difficulty) : null;
            }

            Logger.LogTrace(
                "Deleted AdaptivityQuestion {AdaptivityQuestionName} ({AdaptivityQuestionId}) in AdaptivityTask {AdaptivityTaskName}",
                Question.Difficulty, Question.Id, Task.Name);
            MappingAction.Invoke(Task);
        }
        else
        {
            Logger.LogTrace(
                "Tried to delete AdaptivityQuestion {AdaptivityQuestionName} ({AdaptivityQuestionId}) in AdaptivityTask {AdaptivityTaskName}, but it was not found. Skip deletion",
                Question.Difficulty, Question.Id, Task.Name);
        }
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }

        Task.RestoreMemento(_memento);

        Logger.LogTrace(
            "Undone deletion of AdaptivityQuestion {AdaptivityQuestionName} ({AdaptivityQuestionId}). Restored AdaptivityTask {AdaptivityTaskName} to previous state",
            Question.Difficulty, Question.Id, Task.Name);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing DeleteAdaptivityQuestion");
        Execute();
    }
}