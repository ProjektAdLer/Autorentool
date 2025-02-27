using Shared.Adaptivity;

namespace BusinessLogic.Entities.LearningContent.Adaptivity.Question;

/// <summary>
/// Represents a question with multiple different choices but only one response (and therefore only one correct choice).
/// </summary>
public class MultipleChoiceSingleResponseQuestion : IMultipleChoiceQuestion
{
    public MultipleChoiceSingleResponseQuestion(int expectedCompletionTime, ICollection<Choice> choices,
        string text, Choice correctChoice, QuestionDifficulty difficulty, ICollection<IAdaptivityRule> rules)
    {
        Id = Guid.NewGuid();
        ExpectedCompletionTime = expectedCompletionTime;
        Choices = choices;
        Text = text;
        CorrectChoice = correctChoice;
        Difficulty = difficulty;
        Rules = rules;
        UnsavedChanges = true;
    }

    // ReSharper disable once UnusedMember.Local
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
        UnsavedChanges = false;
    }

    public Choice CorrectChoice { get; set; }

    // ReSharper disable once MemberCanBePrivate.Global - disabled because we need a public property so automapper will map it
    public bool InternalUnsavedChanges { get; private set; }

    public Guid Id { get; private set; }
    public int ExpectedCompletionTime { get; set; }
    public QuestionDifficulty Difficulty { get; set; }
    public ICollection<IAdaptivityRule> Rules { get; set; }

    public bool UnsavedChanges
    {
        get => InternalUnsavedChanges || Rules.Any(rule => rule.UnsavedChanges) ||
               Choices.Any(choice => choice.UnsavedChanges) || CorrectChoice.UnsavedChanges;
        set => InternalUnsavedChanges = value;
    }

    public ICollection<Choice> Choices { get; set; }
    public ICollection<Choice> CorrectChoices => new List<Choice> {CorrectChoice};
    public string Text { get; set; }

    public bool Equals(IAdaptivityQuestion? other)
    {
        if (other is not MultipleChoiceSingleResponseQuestion mcsrq)
            return false;
        return Id.Equals(mcsrq.Id) && CorrectChoice.Equals(mcsrq.CorrectChoice) &&
               ExpectedCompletionTime == mcsrq.ExpectedCompletionTime && Difficulty == mcsrq.Difficulty &&
               Rules.SequenceEqual(mcsrq.Rules) && Choices.SequenceEqual(mcsrq.Choices) && Text == mcsrq.Text;
    }

    public IMemento GetMemento()
    {
        return new MultipleChoiceSingleResponseQuestionMemento(ExpectedCompletionTime, Choices, Text,
            CorrectChoice, Difficulty, Rules, UnsavedChanges);
    }

    public void RestoreMemento(IMemento memento)
    {
        if (memento is not MultipleChoiceSingleResponseQuestionMemento multipleChoiceSingleResponseQuestionMemento)
        {
            throw new ArgumentException("Incorrect IMemento implementation", nameof(memento));
        }

        ExpectedCompletionTime = multipleChoiceSingleResponseQuestionMemento.ExpectedCompletionTime;
        Choices = multipleChoiceSingleResponseQuestionMemento.Choices;
        Text = multipleChoiceSingleResponseQuestionMemento.Text;
        CorrectChoice = multipleChoiceSingleResponseQuestionMemento.CorrectChoice;
        Difficulty = multipleChoiceSingleResponseQuestionMemento.Difficulty;
        Rules = multipleChoiceSingleResponseQuestionMemento.Rules;
        UnsavedChanges = multipleChoiceSingleResponseQuestionMemento.UnsavedChanges;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((MultipleChoiceSingleResponseQuestion) obj);
    }

    // ReSharper disable NonReadonlyMemberInGetHashCode
    public override int GetHashCode()
    {
        return HashCode.Combine(Id, CorrectChoice, ExpectedCompletionTime, (int) Difficulty, Rules, Choices, Text);
    }
    // ReSharper restore NonReadonlyMemberInGetHashCode

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

    private record MultipleChoiceSingleResponseQuestionMemento : IMemento
    {
        internal MultipleChoiceSingleResponseQuestionMemento(int expectedCompletionTime,
            ICollection<Choice> choices,
            string text, Choice correctChoice, QuestionDifficulty difficulty, ICollection<IAdaptivityRule> rules,
            bool unsavedChanges)
        {
            ExpectedCompletionTime = expectedCompletionTime;
            Choices = choices.ToList();
            Text = text;
            CorrectChoice = correctChoice;
            Difficulty = difficulty;
            Rules = rules.ToList();
            UnsavedChanges = unsavedChanges;
        }

        internal int ExpectedCompletionTime { get; }
        internal ICollection<Choice> Choices { get; }
        internal string Text { get; }
        internal Choice CorrectChoice { get; }
        internal QuestionDifficulty Difficulty { get; }
        internal ICollection<IAdaptivityRule> Rules { get; }
        internal bool UnsavedChanges { get; }
    }
}