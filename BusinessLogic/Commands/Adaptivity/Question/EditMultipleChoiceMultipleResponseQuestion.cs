using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent.Adaptivity.Question;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.Adaptivity.Question;

public class EditMultipleChoiceMultipleResponseQuestion : IEditMultipleChoiceMultipleResponseQuestion
{
    private IMemento? _memento;

    public EditMultipleChoiceMultipleResponseQuestion(MultipleChoiceMultipleResponseQuestion question, string title,
        string questionText, ICollection<Choice> choices, ICollection<Choice> correctChoices,
        int expectedCompletionTime, Action<MultipleChoiceMultipleResponseQuestion> mappingAction,
        ILogger<EditMultipleChoiceMultipleResponseQuestion> createLogger)
    {
        Question = question;
        Title = title;
        QuestionText = questionText;
        Choices = choices;
        CorrectChoices = correctChoices;
        ExpectedCompletionTime = expectedCompletionTime;
        MappingAction = mappingAction;
        Logger = createLogger;
    }

    internal MultipleChoiceMultipleResponseQuestion Question { get; }
    internal string Title { get; }
    internal string QuestionText { get; }
    internal ICollection<Choice> Choices { get; }
    internal ICollection<Choice> CorrectChoices { get; }
    internal int ExpectedCompletionTime { get; }
    internal Action<MultipleChoiceMultipleResponseQuestion> MappingAction { get; }
    private ILogger<EditMultipleChoiceMultipleResponseQuestion> Logger { get; }

    public string Name => nameof(EditMultipleChoiceMultipleResponseQuestion);

    public void Execute()
    {
        _memento = Question.GetMemento();

        Logger.LogTrace(
            "Editing MultipleChoiceMultipleResponseQuestion {QuestionTitle} ({QuestionId}). Previous Values: Title {PreviousTitle}, Text {PreviousText}, Choices {@PreviousChoices}, CorrectChoices {@PreviousCorrectChoices}, ExpectedCompletionTime {PreviousExpectedCompletionTime}",
            Question.Title, Question.Id, Question.Title, Question.Text, Question.Choices, Question.CorrectChoices,
            Question.ExpectedCompletionTime);

        Question.Title = Title;
        Question.Text = QuestionText;
        Question.Choices = Choices;
        Question.CorrectChoices = CorrectChoices;
        Question.ExpectedCompletionTime = ExpectedCompletionTime;

        Logger.LogTrace(
            "Edited MultipleChoiceMultipleResponseQuestion {QuestionTitle} ({QuestionId}). Updated Values: Title {PreviousTitle}, Text {PreviousText}, Choices {@PreviousChoices}, CorrectChoices {@PreviousCorrectChoices}, ExpectedCompletionTime {PreviousExpectedCompletionTime}",
            Question.Title, Question.Id, Question.Title, Question.Text, Question.Choices, Question.CorrectChoices,
            Question.ExpectedCompletionTime);

        MappingAction.Invoke(Question);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }

        Question.RestoreMemento(_memento);

        Logger.LogTrace(
            "Undone editing of MultipleChoiceMultipleResponseQuestion {QuestionTitle} ({QuestionId}). Restored to previous state",
            Question.Title, Question.Id);

        MappingAction.Invoke(Question);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing EditMultipleChoiceMultipleResponseQuestion");
        Execute();
    }
}