using Shared.Adaptivity;

namespace Presentation.PresentationLogic.LearningContent.AdaptivityContent.Question;

/// <summary>
/// Represents a question with multiple different choices and multiple responses.
/// </summary>
public class MultipleChoiceMultipleResponseQuestionViewModel : IMultipleChoiceQuestionViewModel
{
    public MultipleChoiceMultipleResponseQuestionViewModel(int expectedCompletionTime, IEnumerable<ChoiceViewModel> choices,
        IEnumerable<ChoiceViewModel> correctChoices, string text, QuestionDifficulty difficulty)
    {
        ExpectedCompletionTime = expectedCompletionTime;
        Choices = choices;
        CorrectChoices = correctChoices;
        Text = text;
        Difficulty = difficulty;
    }

    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private MultipleChoiceMultipleResponseQuestionViewModel()
    {
        ExpectedCompletionTime = 0;
        Choices = null!;
        CorrectChoices = null!;
        Text = null!;
        Difficulty = QuestionDifficulty.Easy;
    }

    public int ExpectedCompletionTime { get; set; }
    public QuestionDifficulty Difficulty { get; set; }
    public IEnumerable<ChoiceViewModel> Choices { get; set; }
    public IEnumerable<ChoiceViewModel> CorrectChoices { get; set; }
    public string Text { get; set; }
}