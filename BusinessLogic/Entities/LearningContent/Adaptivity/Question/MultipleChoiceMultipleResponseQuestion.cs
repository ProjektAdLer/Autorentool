using Shared.Adaptivity;

namespace BusinessLogic.Entities.LearningContent.Adaptivity.Question;

/// <summary>
/// Represents a question with multiple different choices and multiple responses.
/// </summary>
public class MultipleChoiceMultipleResponseQuestion : IMultipleChoiceQuestion
{
    public MultipleChoiceMultipleResponseQuestion(int expectedCompletionTime, ICollection<Choice> choices,
        ICollection<Choice> correctChoices, ICollection<IAdaptivityRule> rules, string text,
        QuestionDifficulty difficulty)
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

    public bool Equals(IAdaptivityQuestion? other)
    {
        if (other is not MultipleChoiceMultipleResponseQuestion mcmrq)
            return false;
        return Id.Equals(mcmrq.Id) && ExpectedCompletionTime == mcmrq.ExpectedCompletionTime &&
               Difficulty == mcmrq.Difficulty && Rules.SequenceEqual(mcmrq.Rules) &&
               Choices.SequenceEqual(mcmrq.Choices) &&
               CorrectChoices.SequenceEqual(mcmrq.CorrectChoices) && Text == mcmrq.Text;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((MultipleChoiceMultipleResponseQuestion) obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, ExpectedCompletionTime, (int) Difficulty, Rules, Choices, CorrectChoices, Text);
    }

    public static bool operator ==(MultipleChoiceMultipleResponseQuestion? left,
        MultipleChoiceMultipleResponseQuestion? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(MultipleChoiceMultipleResponseQuestion? left,
        MultipleChoiceMultipleResponseQuestion? right)
    {
        return !Equals(left, right);
    }
}