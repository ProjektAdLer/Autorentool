using Shared.Adaptivity;

namespace BusinessLogic.Entities.LearningContent.AdaptivityContent.Question;

/// <summary>
/// Represents a question with multiple different choices and multiple responses.
/// </summary>
public class MultipleChoiceMultipleResponseQuestion : IMultipleChoiceQuestion
{
    public MultipleChoiceMultipleResponseQuestion(int expectedCompletionTime, IEnumerable<Choice> choices,
        IEnumerable<Choice> correctChoices, string text, QuestionDifficulty difficulty)
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
    private MultipleChoiceMultipleResponseQuestion()
    {
        ExpectedCompletionTime = 0;
        Choices = null!;
        CorrectChoices = null!;
        Text = null!;
        Difficulty = QuestionDifficulty.Easy;
    }

    public int ExpectedCompletionTime { get; set; }
    public QuestionDifficulty Difficulty { get; set; }
    public IEnumerable<Choice> Choices { get; set; }
    public IEnumerable<Choice> CorrectChoices { get; set; }
    public string Text { get; set; }
}