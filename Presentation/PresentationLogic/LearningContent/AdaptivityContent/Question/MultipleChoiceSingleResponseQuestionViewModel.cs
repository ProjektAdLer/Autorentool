using Shared.Adaptivity;

namespace Presentation.PresentationLogic.LearningContent.AdaptivityContent.Question;

/// <summary>
/// Represents a question with multiple different choices but only one response (and therefore only one correct choice).
/// </summary>
public class MultipleChoiceSingleResponseQuestionViewModel : IMultipleChoiceQuestionViewModel
{
    public MultipleChoiceSingleResponseQuestionViewModel(int expectedCompletionTime, IEnumerable<ChoiceViewModel> choices, string text, ChoiceViewModel correctChoice, QuestionDifficulty difficulty)
    {
        ExpectedCompletionTime = expectedCompletionTime;
        Choices = choices;
        Text = text;
        CorrectChoice = correctChoice;
        Difficulty = difficulty;
    }

    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private MultipleChoiceSingleResponseQuestionViewModel()
    {
        ExpectedCompletionTime = 0;
        Choices = null!;
        Text = null!;
        CorrectChoice = null!;
        Difficulty = QuestionDifficulty.Easy;
    }
    
    public ChoiceViewModel CorrectChoice { get; set; }
    public int ExpectedCompletionTime { get; set; }
    public QuestionDifficulty Difficulty { get; set; }
    public IEnumerable<ChoiceViewModel> Choices { get; set; }
    public IEnumerable<ChoiceViewModel> CorrectChoices => new List<ChoiceViewModel> { CorrectChoice };
    public string Text { get; set; }
}