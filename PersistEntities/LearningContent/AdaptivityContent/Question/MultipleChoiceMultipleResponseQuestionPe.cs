using Shared.Adaptivity;

namespace PersistEntities.LearningContent.Question;

/// <summary>
/// Represents a question with multiple different choices and multiple responses.
/// </summary>
public class MultipleChoiceMultipleResponseQuestionPe : IMultipleChoiceQuestionPe
{
    public MultipleChoiceMultipleResponseQuestionPe(int expectedCompletionTime, ICollection<ChoicePe> choices,
        ICollection<ChoicePe> correctChoices, ICollection<IAdaptivityRulePe> rules, string text, QuestionDifficulty difficulty)
    {
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
    private MultipleChoiceMultipleResponseQuestionPe()
    {
        ExpectedCompletionTime = 0;
        Choices = null!;
        CorrectChoices = null!;
        Rules = null!;
        Text = null!;
        Difficulty = QuestionDifficulty.Easy;
    }

    public int ExpectedCompletionTime { get; set; }
    public QuestionDifficulty Difficulty { get; set; }
    public ICollection<IAdaptivityRulePe> Rules { get; set; }
    public ICollection<ChoicePe> Choices { get; set; }
    public ICollection<ChoicePe> CorrectChoices { get; set; }
    public string Text { get; set; }
}