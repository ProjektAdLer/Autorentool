using System.Runtime.Serialization;
using JetBrains.Annotations;
using Shared.Adaptivity;

namespace PersistEntities.LearningContent.Question;

/// <summary>
/// Represents a question with multiple different choices and multiple responses.
/// </summary>
[KnownType(typeof(AdaptivityRulePe))]
[DataContract(IsReference = true)]
public class MultipleChoiceMultipleResponseQuestionPe : IMultipleChoiceQuestionPe
{
    public MultipleChoiceMultipleResponseQuestionPe(string title, int expectedCompletionTime,
        ICollection<ChoicePe> choices, ICollection<ChoicePe> correctChoices, ICollection<IAdaptivityRulePe> rules,
        string text, QuestionDifficulty difficulty)
    {
        Id = Guid.NewGuid();
        Title = title;
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
        Id = Guid.Empty;
        Title = null!;
        ExpectedCompletionTime = 0;
        Choices = null!;
        CorrectChoices = null!;
        Rules = null!;
        Text = null!;
        Difficulty = QuestionDifficulty.Easy;
    }

    [IgnoreDataMember] public Guid Id { get; private set; }
    [DataMember] public string Title { get; set; }
    [DataMember] public int ExpectedCompletionTime { get; set; }
    [DataMember] public QuestionDifficulty Difficulty { get; set; }
    [DataMember] public ICollection<IAdaptivityRulePe> Rules { get; set; }
    [DataMember] public ICollection<ChoicePe> Choices { get; set; }
    [DataMember] public ICollection<ChoicePe> CorrectChoices { get; set; }
    [DataMember] public string Text { get; set; }

    [OnDeserializing]
    [UsedImplicitly]
    private void OnDeserializing(StreamingContext context)
    {
        Id = Guid.NewGuid();
    }
}