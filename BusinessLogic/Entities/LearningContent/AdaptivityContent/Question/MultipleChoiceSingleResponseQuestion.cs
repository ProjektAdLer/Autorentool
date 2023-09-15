using Shared.Adaptivity;

namespace BusinessLogic.Entities.LearningContent.AdaptivityContent.Question;

/// <summary>
/// Represents a question with multiple different choices but only one response (and therefore only one correct choice).
/// </summary>
public class MultipleChoiceSingleResponseQuestion : IMultipleChoiceQuestion
{
    public MultipleChoiceSingleResponseQuestion(int expectedCompletionTime, ICollection<Choice> choices, string text,
        Choice correctChoice, QuestionDifficulty difficulty, ICollection<IAdaptivityRule> rules)
    {
        Id = Guid.NewGuid();
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
        Id = Guid.Empty;
        ExpectedCompletionTime = 0;
        Choices = null!;
        Text = null!;
        CorrectChoice = null!;
        Difficulty = QuestionDifficulty.Easy;
        Rules = null!;
    }

    public Guid Id { get; private set; }
    public Choice CorrectChoice { get; set; }
    public int ExpectedCompletionTime { get; set; }
    public QuestionDifficulty Difficulty { get; set; }
    public ICollection<IAdaptivityRule> Rules { get; set; }
    public ICollection<Choice> Choices { get; set; }
    public ICollection<Choice> CorrectChoices => new List<Choice> { CorrectChoice };
    public string Text { get; set; }

    public bool Equals(IAdaptivityQuestion? other)
    {
        if (other is not MultipleChoiceSingleResponseQuestion mcsrq)
            return false;
        return Id.Equals(mcsrq.Id) && CorrectChoice.Equals(mcsrq.CorrectChoice) &&
               ExpectedCompletionTime == mcsrq.ExpectedCompletionTime && Difficulty == mcsrq.Difficulty &&
               Rules.SequenceEqual(mcsrq.Rules) && Choices.SequenceEqual(mcsrq.Choices) && Text == mcsrq.Text;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((MultipleChoiceSingleResponseQuestion)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, CorrectChoice, ExpectedCompletionTime, (int)Difficulty, Rules, Choices, Text);
    }

    public static bool operator ==(MultipleChoiceSingleResponseQuestion? left,
        MultipleChoiceSingleResponseQuestion? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(MultipleChoiceSingleResponseQuestion? left,
        MultipleChoiceSingleResponseQuestion? right)
    {
        return !Equals(left, right);
    }
}