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
        UnsavedChanges = true;
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
        UnsavedChanges = false;
    }

    public Guid Id { get; private set; }
    public int ExpectedCompletionTime { get; set; }
    public QuestionDifficulty Difficulty { get; set; }
    public ICollection<IAdaptivityRule> Rules { get; set; }

    private bool InternalUnsavedChanges { get; set; }

    public bool UnsavedChanges
    {
        get => InternalUnsavedChanges || Rules.Any(rule => rule.UnsavedChanges) || Choices.Any(choice => choice.UnsavedChanges) || CorrectChoices.Any(choice => choice.UnsavedChanges);
        set => InternalUnsavedChanges = value;
    }

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

    public IMemento GetMemento()
    {
        return new MultipleChoiceMultipleResponseQuestionMemento(ExpectedCompletionTime, Choices, Text,
            CorrectChoices, Difficulty, Rules);
    }

    public void RestoreMemento(IMemento memento)
    {
        if (memento is not MultipleChoiceMultipleResponseQuestionMemento multipleChoiceMultipleResponseQuestionMemento)
        {
            throw new ArgumentException("Incorrect IMemento implementation", nameof(memento));
        }

        ExpectedCompletionTime = multipleChoiceMultipleResponseQuestionMemento.ExpectedCompletionTime;
        Choices = multipleChoiceMultipleResponseQuestionMemento.Choices;
        Text = multipleChoiceMultipleResponseQuestionMemento.Text;
        CorrectChoices = multipleChoiceMultipleResponseQuestionMemento.CorrectChoices;
        Difficulty = multipleChoiceMultipleResponseQuestionMemento.Difficulty;
        Rules = multipleChoiceMultipleResponseQuestionMemento.Rules;
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

    private record MultipleChoiceMultipleResponseQuestionMemento : IMemento
    {
        internal MultipleChoiceMultipleResponseQuestionMemento(int expectedCompletionTime,
            ICollection<Choice> choices, string text, ICollection<Choice> correctChoices, QuestionDifficulty difficulty,
            ICollection<IAdaptivityRule> rules)
        {
            ExpectedCompletionTime = expectedCompletionTime;
            Choices = choices.ToList();
            Text = text;
            CorrectChoices = correctChoices.ToList();
            Difficulty = difficulty;
            Rules = rules.ToList();
        }

        internal int ExpectedCompletionTime { get; }
        internal ICollection<Choice> Choices { get; }
        internal string Text { get; }
        internal ICollection<Choice> CorrectChoices { get; }
        internal QuestionDifficulty Difficulty { get; }
        internal ICollection<IAdaptivityRule> Rules { get; }
    }
}