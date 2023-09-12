using System.Runtime.Serialization;
using Shared.Adaptivity;

namespace PersistEntities.LearningContent.Question;

/// <summary>
/// Represents a question with multiple different choices but only one response (and therefore only one correct choice).
/// </summary>
[KnownType(typeof(AdaptivityRulePe))]
public class MultipleChoiceSingleResponseQuestionPe : IMultipleChoiceQuestionPe
{
    public MultipleChoiceSingleResponseQuestionPe(int expectedCompletionTime, ICollection<ChoicePe> choices,
        string text, ChoicePe correctChoice, QuestionDifficulty difficulty, ICollection<IAdaptivityRulePe> rules)
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

    [DataMember] public ChoicePe CorrectChoice { get; set; }
    [DataMember] public int ExpectedCompletionTime { get; set; }
    [DataMember] public QuestionDifficulty Difficulty { get; set; }
    [DataMember] public ICollection<IAdaptivityRulePe> Rules { get; set; }
    [DataMember] public ICollection<ChoicePe> Choices { get; set; }
    [IgnoreDataMember] public ICollection<ChoicePe> CorrectChoices => new List<ChoicePe> { CorrectChoice };
    [DataMember] public string Text { get; set; }
}