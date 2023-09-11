using Shared.Adaptivity;

namespace PersistEntities.LearningContent.Question;

/// <summary>
/// Represents a question with multiple different choices but only one response (and therefore only one correct choice).
/// </summary>
public class MultipleChoiceSingleResponseQuestionPe : IMultipleChoiceQuestionPe
{
    public MultipleChoiceSingleResponseQuestionPe(int expectedCompletionTime, ICollection<ChoicePe> choices, string text, ChoicePe correctChoice, QuestionDifficulty difficulty, ICollection<IAdaptivityRulePe> rules)
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
    private MultipleChoiceSingleResponseQuestionPe()
    {
        ExpectedCompletionTime = 0;
        Choices = null!;
        Text = null!;
        CorrectChoice = null!;
        Difficulty = QuestionDifficulty.Easy;
        Rules = null!;
    }
    
    public ChoicePe CorrectChoice { get; set; }
    public int ExpectedCompletionTime { get; set; }
    public QuestionDifficulty Difficulty { get; set; }
    public ICollection<IAdaptivityRulePe> Rules { get; set; }
    public ICollection<ChoicePe> Choices { get; set; }
    public ICollection<ChoicePe> CorrectChoices => new List<ChoicePe> { CorrectChoice };
    public string Text { get; set; }
}