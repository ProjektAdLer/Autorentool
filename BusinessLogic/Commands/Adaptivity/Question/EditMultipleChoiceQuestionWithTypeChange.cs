using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent.Adaptivity;
using BusinessLogic.Entities.LearningContent.Adaptivity.Question;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.Adaptivity.Question;

public class EditMultipleChoiceQuestionWithTypeChange : IEditMultipleChoiceQuestionWithTypeChange
{
    private IMemento? _memento;

    public EditMultipleChoiceQuestionWithTypeChange(AdaptivityTask task, IMultipleChoiceQuestion question,
        bool isSingleResponse, string title, string text, ICollection<Choice> choices,
        ICollection<Choice> correctChoices, int expectedCompletionTime, Action<AdaptivityTask> mappingAction,
        ILogger<EditMultipleChoiceQuestionWithTypeChange> createLogger)
    {
        switch (question)
        {
            case MultipleChoiceMultipleResponseQuestion when isSingleResponse:
                SingleResponseQuestion = new MultipleChoiceSingleResponseQuestion(title, expectedCompletionTime,
                    choices, text, correctChoices.First(), question.Difficulty, question.Rules);
                break;
            case MultipleChoiceSingleResponseQuestion when !isSingleResponse:
                MultipleResponseQuestion = new MultipleChoiceMultipleResponseQuestion(title, expectedCompletionTime,
                    choices, correctChoices, question.Rules, text, question.Difficulty);
                break;
            default:
                throw new InvalidOperationException(
                    $"Cannot change type of question {question.Id} from {question.GetType().Name} to {isSingleResponse}. The question has already this type.");
        }

        Task = task;
        Question = question;
        IsSingleResponse = isSingleResponse;
        MappingAction = mappingAction;
        Logger = createLogger;
    }

    internal AdaptivityTask Task { get; }
    internal IMultipleChoiceQuestion Question { get; }
    internal bool IsSingleResponse { get; }
    internal MultipleChoiceSingleResponseQuestion? SingleResponseQuestion { get; }
    internal MultipleChoiceMultipleResponseQuestion? MultipleResponseQuestion { get; }
    internal Action<AdaptivityTask> MappingAction { get; }
    private ILogger<EditMultipleChoiceQuestionWithTypeChange> Logger { get; }

    public string Name => nameof(EditMultipleChoiceQuestionWithTypeChange);

    public void Execute()
    {
        _memento = Task.GetMemento();

        Logger.LogTrace(
            "Editing MultipleChoiceQuestion {QuestionTitle} ({QuestionId}). Previous Values: Type {PreviousType} Title {PreviousTitle}, Text {PreviousText}, Choices {@PreviousChoices}, CorrectChoices {@PreviousCorrectChoices}, ExpectedCompletionTime {PreviousExpectedCompletionTime}",
            Question.Title, Question.Id, Question.GetType().Name, Question.Title, Question.Text, Question.Choices,
            Question.CorrectChoices, Question.ExpectedCompletionTime);

        var questionToDelete = Task.Questions.FirstOrDefault(q => q.Id == Question.Id);
        if (questionToDelete == null)
        {
            throw new InvalidOperationException(
                $"Cannot change type of question {Question.Id} from {Question.GetType().Name} to {IsSingleResponse}. The question does not exist in the Task.");
        }

        if (IsSingleResponse)
        {
            Task.Questions.Remove(questionToDelete);
            Task.Questions.Add(SingleResponseQuestion!);
        }
        else if (!IsSingleResponse)
        {
            Task.Questions.Remove(questionToDelete);
            Task.Questions.Add(MultipleResponseQuestion!);
        }


        Logger.LogTrace(
            "Edited MultipleChoiceQuestion {QuestionTitle} ({QuestionId}). Updated Values: Title {PreviousTitle}, Text {PreviousText}, Choices {@PreviousChoices}, CorrectChoices {@PreviousCorrectChoices}, ExpectedCompletionTime {PreviousExpectedCompletionTime}",
            Question.Title, Question.Id, Question.Title, Question.Text, Question.Choices, Question.CorrectChoices,
            Question.ExpectedCompletionTime);

        MappingAction.Invoke(Task);
    }

    public void Undo()
    {
        if (_memento == null)
        {
            throw new InvalidOperationException("_memento is null");
        }

        Task.RestoreMemento(_memento);

        Logger.LogTrace(
            "Undoing edit of MultipleChoiceQuestion {QuestionTitle} ({QuestionId}). Restored Values: Title {PreviousTitle}, Text {PreviousText}, Choices {@PreviousChoices}, CorrectChoices {@PreviousCorrectChoices}, ExpectedCompletionTime {PreviousExpectedCompletionTime}",
            Question.Title, Question.Id, Question.Title, Question.Text, Question.Choices, Question.CorrectChoices,
            Question.ExpectedCompletionTime);

        MappingAction.Invoke(Task);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing EditMultipleChoiceQuestionWithTypeChange");
        Execute();
    }
}