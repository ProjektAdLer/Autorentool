using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent.Adaptivity;
using BusinessLogic.Entities.LearningContent.Adaptivity.Question;
using Microsoft.Extensions.Logging;
using Shared.Adaptivity;

namespace BusinessLogic.Commands.Adaptivity.Question;

public class CreateMultipleChoiceMultipleResponseQuestion : ICreateMultipleChoiceMultipleResponseQuestion
{
    private IMemento? _memento;

    public CreateMultipleChoiceMultipleResponseQuestion(AdaptivityTask adaptivityTask, QuestionDifficulty difficulty,
        string questionText, ICollection<Choice> choices, ICollection<Choice> correctChoices,
        int expectedCompletionTime, Action<AdaptivityTask> mappingAction,
        ILogger<CreateMultipleChoiceMultipleResponseQuestion> logger)
    {
        AdaptivityTask = adaptivityTask;
        Question = new MultipleChoiceMultipleResponseQuestion(expectedCompletionTime, choices, correctChoices,
            new List<IAdaptivityRule>(), questionText, difficulty);
        MappingAction = mappingAction;
        Logger = logger;
    }

    internal AdaptivityTask AdaptivityTask { get; }
    internal MultipleChoiceMultipleResponseQuestion Question { get; }
    internal Action<AdaptivityTask> MappingAction { get; }
    private ILogger<CreateMultipleChoiceMultipleResponseQuestion> Logger { get; }
    public string Name => nameof(CreateMultipleChoiceMultipleResponseQuestion);

    public void Execute()
    {
        _memento = AdaptivityTask.GetMemento();

        AdaptivityTask.Questions.Add(Question);

        Logger.LogTrace(
            "Created MultipleChoiceMultipleResponseQuestion '{QuestionText}' in AdaptivityTask {AdaptivityTaskName}",
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
            "Undone creation of MultipleChoiceMultipleResponseQuestion '{QuestionText}'. Restored AdaptivityTask {AdaptivityTaskName} to previous state",
            Question.Text, AdaptivityTask.Name);

        MappingAction.Invoke(AdaptivityTask);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing CreateMultipleChoiceMultipleResponseQuestion");
        Execute();
    }
}