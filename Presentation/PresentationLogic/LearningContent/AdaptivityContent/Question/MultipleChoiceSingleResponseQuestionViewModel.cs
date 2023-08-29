using Shared.Adaptivity;

namespace Presentation.PresentationLogic.LearningContent.AdaptivityContent.Question;

/// <summary>
/// Represents a question with multiple different choices but only one response (and therefore only one correct choice).
/// </summary>
public class MultipleChoiceSingleResponseQuestionViewModel : IMultipleChoiceQuestionViewModel
{
    public MultipleChoiceSingleResponseQuestionViewModel(int expectedCompletionTime, IEnumerable<Choice> choices, string text, Choice correctChoice, QuestionDifficulty difficulty)
    {
        ExpectedCompletionTime = expectedCompletionTime;
        Choices = choices;
        Text = text;
        CorrectChoice = correctChoice;
        Difficulty = difficulty;
    }
    
    public Choice CorrectChoice { get; set; }
    public int ExpectedCompletionTime { get; set; }
    public QuestionDifficulty Difficulty { get; set; }
    public IEnumerable<Choice> Choices { get; set; }
    public IEnumerable<Choice> CorrectChoices => new List<Choice> { CorrectChoice };
    public string Text { get; set; }
}