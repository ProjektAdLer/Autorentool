using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent.Adaptivity.Question;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.Adaptivity.Question;

public class EditMultipleChoiceSingleResponseQuestion : IEditMultipleChoiceSingleResponseQuestion
{
    private IMemento? _memento;

    public EditMultipleChoiceSingleResponseQuestion(MultipleChoiceSingleResponseQuestion question, string questionText,
        ICollection<Choice> choices, Choice correctChoice, int expectedCompletionTime,
        Action<MultipleChoiceSingleResponseQuestion> mappingAction,
        ILogger<EditMultipleChoiceSingleResponseQuestion> createLogger)
    {
        Question = question;
        QuestionText = questionText;
        Choices = choices;
        CorrectChoice = correctChoice;
        ExpectedCompletionTime = expectedCompletionTime;
        MappingAction = mappingAction;
        Logger = createLogger;
    }

    internal MultipleChoiceSingleResponseQuestion Question { get; }
    internal string QuestionText { get; }
    internal ICollection<Choice> Choices { get; }
    internal Choice CorrectChoice { get; }
    internal int ExpectedCompletionTime { get; }
    internal Action<MultipleChoiceSingleResponseQuestion> MappingAction { get; }
    private ILogger<EditMultipleChoiceSingleResponseQuestion> Logger { get; }

    public string Name => nameof(EditMultipleChoiceSingleResponseQuestion);

    public void Execute()
    {
        _memento = Question.GetMemento();

        Logger.LogTrace(
            "Editing MultipleChoiceSingleResponseQuestion {QuestionName} ({QuestionId}). Previous Values: Text {PreviousText}, Choices {@PreviousChoices}, CorrectChoice {@PreviousCorrectChoice}, ExpectedCompletionTime {PreviousExpectedCompletionTime}",
            Question.Text, Question.Id, Question.Text, Question.Choices, Question.CorrectChoice,
            Question.ExpectedCompletionTime);

        Question.Text = QuestionText;
        Question.Choices = Choices;
        Question.CorrectChoice = CorrectChoice;
        Question.ExpectedCompletionTime = ExpectedCompletionTime;

        Logger.LogTrace(
            "Edited MultipleChoiceSingleResponseQuestion {QuestionName} ({QuestionId}). Updated Values: Text {PreviousText}, Choices {@PreviousChoices}, CorrectChoice {@PreviousCorrectChoice}, ExpectedCompletionTime {PreviousExpectedCompletionTime}",
            Question.Text, Question.Id, Question.Text, Question.Choices, Question.CorrectChoice,
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
            "Undone editing of MultipleChoiceSingleResponseQuestion {QuestionName} ({QuestionId}). Restored to previous state. Previous Values: Text {PreviousText}, Choices {@PreviousChoices}, CorrectChoice {@PreviousCorrectChoice}, ExpectedCompletionTime {PreviousExpectedCompletionTime}",
            Question.Text, Question.Id, Question.Text, Question.Choices, Question.CorrectChoice,
            Question.ExpectedCompletionTime);

        MappingAction.Invoke(Question);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing EditMultipleChoiceSingleResponseQuestion");
        Execute();
    }
}