using Shared.Adaptivity;

namespace Presentation.PresentationLogic.LearningContent.AdaptivityContent.Question;

/// <summary>
/// Represents a question with multiple different choices and multiple responses.
/// </summary>
public class MultipleChoiceMultipleResponseQuestionViewModel : IMultipleChoiceQuestionViewModel
{
    public MultipleChoiceMultipleResponseQuestionViewModel(int expectedCompletionTime, IEnumerable<Choice> choices,
        IEnumerable<Choice> correctChoices, string text, QuestionDifficulty difficulty)
    {
        ExpectedCompletionTime = expectedCompletionTime;
        Choices = choices;
        CorrectChoices = correctChoices;
        Text = text;
        Difficulty = difficulty;
    }

    public int ExpectedCompletionTime { get; set; }
    public QuestionDifficulty Difficulty { get; set; }
    public IEnumerable<Choice> Choices { get; set; }
    public IEnumerable<Choice> CorrectChoices { get; set; }
    public string Text { get; set; }
}