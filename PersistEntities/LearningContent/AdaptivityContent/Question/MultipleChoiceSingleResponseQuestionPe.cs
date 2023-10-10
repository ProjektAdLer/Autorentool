using System.Runtime.Serialization;
using JetBrains.Annotations;
using Shared.Adaptivity;

namespace PersistEntities.LearningContent.Question;

/// <summary>
/// Represents a question with multiple different choices but only one response (and therefore only one correct choice).
/// </summary>
[KnownType(typeof(AdaptivityRulePe))]
public class MultipleChoiceSingleResponseQuestionPe : IMultipleChoiceQuestionPe
{
    public MultipleChoiceSingleResponseQuestionPe(string title, int expectedCompletionTime,
        ICollection<ChoicePe> choices, string text, ChoicePe correctChoice, QuestionDifficulty difficulty,
        ICollection<IAdaptivityRulePe> rules)
    {
        Id = Guid.NewGuid();
        Title = title;
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
        Id = Guid.Empty;
        Title = null!;
        ExpectedCompletionTime = 0;
        Choices = null!;
        Text = null!;
        CorrectChoice = null!;
        Difficulty = QuestionDifficulty.Easy;
        Rules = null!;
    }

    [DataMember] public ChoicePe CorrectChoice { get; set; }

    [IgnoreDataMember] public Guid Id { get; private set; }
    [DataMember] public string Title { get; set; }
    [DataMember] public int ExpectedCompletionTime { get; set; }
    [DataMember] public QuestionDifficulty Difficulty { get; set; }
    [DataMember] public ICollection<IAdaptivityRulePe> Rules { get; set; }
    [DataMember] public ICollection<ChoicePe> Choices { get; set; }
    [IgnoreDataMember] public ICollection<ChoicePe> CorrectChoices => new List<ChoicePe> {CorrectChoice};
    [DataMember] public string Text { get; set; }

    [OnDeserializing]
    [UsedImplicitly]
    private void OnDeserializing(StreamingContext context)
    {
        Id = Guid.NewGuid();
    }
}