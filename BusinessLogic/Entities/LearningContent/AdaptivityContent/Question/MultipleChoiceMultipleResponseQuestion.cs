using Shared.Adaptivity;

namespace BusinessLogic.Entities.LearningContent.AdaptivityContent.Question;

/// <summary>
/// Represents a question with multiple different choices and multiple responses.
/// </summary>
public class MultipleChoiceMultipleResponseQuestion : IMultipleChoiceQuestion
{
    public MultipleChoiceMultipleResponseQuestion(int expectedCompletionTime, ICollection<Choice> choices,
        ICollection<Choice> correctChoices, ICollection<IAdaptivityRule> rules, string text, QuestionDifficulty difficulty)
    {
        Id = Guid.NewGuid();
        ExpectedCompletionTime = expectedCompletionTime;
        Choices = choices;
        CorrectChoices = correctChoices;
        Rules = rules;
        Text = text;
        Difficulty = difficulty;
    }
    
    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private MultipleChoiceMultipleResponseQuestion()
    {
        Id = Guid.Empty;
        ExpectedCompletionTime = 0;
        Choices = null!;
        CorrectChoices = null!;
        Rules = null!;
        Text = null!;
        Difficulty = QuestionDifficulty.Easy;
    }

    public Guid Id { get; private set; }
    public int ExpectedCompletionTime { get; set; }
    public QuestionDifficulty Difficulty { get; set; }
    public ICollection<IAdaptivityRule> Rules { get; set; }
    public ICollection<Choice> Choices { get; set; }
    public ICollection<Choice> CorrectChoices { get; set; }
    public string Text { get; set; }
}