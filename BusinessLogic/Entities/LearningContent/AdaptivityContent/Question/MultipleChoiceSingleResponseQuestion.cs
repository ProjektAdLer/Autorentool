using Shared.Adaptivity;

namespace BusinessLogic.Entities.LearningContent.AdaptivityContent.Question;

/// <summary>
/// Represents a question with multiple different choices but only one response (and therefore only one correct choice).
/// </summary>
public class MultipleChoiceSingleResponseQuestion : IMultipleChoiceQuestion
{
    public MultipleChoiceSingleResponseQuestion(int expectedCompletionTime, ICollection<Choice> choices, string text, Choice correctChoice, QuestionDifficulty difficulty, ICollection<IAdaptivityRule> rules)
    {
        ExpectedCompletionTime = expectedCompletionTime;
        Choices = choices;
        Text = text;
        CorrectChoice = correctChoice;
        Difficulty = difficulty;
        Rules = rules;
    }

    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private MultipleChoiceSingleResponseQuestion()
    {
        ExpectedCompletionTime = 0;
        Choices = null!;
        Text = null!;
        CorrectChoice = null!;
        Difficulty = QuestionDifficulty.Easy;
        Rules = null!;
    }
    
    public Choice CorrectChoice { get; set; }
    public int ExpectedCompletionTime { get; set; }
    public QuestionDifficulty Difficulty { get; set; }
    public ICollection<IAdaptivityRule> Rules { get; set; }
    public ICollection<Choice> Choices { get; set; }
    public ICollection<Choice> CorrectChoices => new List<Choice> { CorrectChoice };
    public string Text { get; set; }
}