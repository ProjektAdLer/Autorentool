using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent.Adaptivity;
using BusinessLogic.Entities.LearningContent.Adaptivity.Question;
using Microsoft.Extensions.Logging;
using Shared.Adaptivity;

namespace BusinessLogic.Commands.Adaptivity.Question;

public class CreateMultipleChoiceSingleResponseQuestion : ICreateMultipleChoiceSingleResponseQuestion
{
    private IMemento? _memento;

    public CreateMultipleChoiceSingleResponseQuestion(AdaptivityTask adaptivityTask, QuestionDifficulty difficulty,
        string questionText, ICollection<Choice> choices, Choice correctChoice, int expectedCompletionTime,
        Action<AdaptivityTask> mappingAction, ILogger<CreateMultipleChoiceSingleResponseQuestion> logger)
    {
        AdaptivityTask = adaptivityTask;
        Question = new MultipleChoiceSingleResponseQuestion(expectedCompletionTime, choices, questionText,
            correctChoice, difficulty, new List<IAdaptivityRule>());
        MappingAction = mappingAction;
        Logger = logger;
    }

    internal AdaptivityTask AdaptivityTask { get; }
    internal MultipleChoiceSingleResponseQuestion Question { get; }
    internal Action<AdaptivityTask> MappingAction { get; }
    private ILogger<CreateMultipleChoiceSingleResponseQuestion> Logger { get; }
    public string Name => nameof(CreateMultipleChoiceSingleResponseQuestion);

    public void Execute()
    {
        _memento = AdaptivityTask.GetMemento();

        AdaptivityTask.Questions.Add(Question);
        AdaptivityTask.UnsavedChanges = true;
        if (AdaptivityTask.Questions.Count == 1)
        {
            AdaptivityTask.MinimumRequiredDifficulty = Question.Difficulty;
        }

        Logger.LogTrace(
            "Created MultipleChoicesSingleResponseQuestion '{QuestionText}' in AdaptivityTask {AdaptivityTaskName}",
            Question.Text, AdaptivityTask.Name);

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
            "Undone creation of MultipleChoicesSingleResponseQuestion '{QuestionText}'. Restored AdaptivityTask {AdaptivityTaskName} to previous state",
            Question.Text, AdaptivityTask.Name);

        MappingAction.Invoke(AdaptivityTask);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing CreateMultipleChoicesSingleResponseQuestion");
        Execute();
    }
}