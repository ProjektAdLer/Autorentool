using BusinessLogic.Entities.LearningContent.Adaptivity.Question;
using Shared.Adaptivity;

namespace BusinessLogic.Entities.LearningContent.Adaptivity;

public class AdaptivityTask : IAdaptivityTask
{
    public AdaptivityTask(ICollection<IAdaptivityQuestion> questions, QuestionDifficulty? minimumRequiredDifficulty,
        string name)
    {
        Questions = questions;
        MinimumRequiredDifficulty = minimumRequiredDifficulty;
        Name = name;
        Id = Guid.NewGuid();
        UnsavedChanges = true;
    }

    // ReSharper disable once UnusedMember.Local
    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private AdaptivityTask()
    {
        Questions = null!;
        MinimumRequiredDifficulty = QuestionDifficulty.Easy;
        Name = "";
        Id = Guid.Empty;
        UnsavedChanges = false;
    }

    // ReSharper disable once MemberCanBePrivate.Global - disabled because we need a public property so automapper will map it
    public bool InternalUnsavedChanges { get; private set; }

    public ICollection<IAdaptivityQuestion> Questions { get; set; }
    public QuestionDifficulty? MinimumRequiredDifficulty { get; set; }
    public string Name { get; set; }

    public bool UnsavedChanges
    {
        get => InternalUnsavedChanges || Questions.Any(question => question.UnsavedChanges);
        set => InternalUnsavedChanges = value;
    }

    public Guid Id { get; set; }

    public bool Equals(IAdaptivityTask? other)
    {
        if (other is not AdaptivityTask)
            return false;
        return Questions.SequenceEqual(other.Questions) &&
               MinimumRequiredDifficulty == other.MinimumRequiredDifficulty && Id.Equals(other.Id);
    }

    public IMemento GetMemento()
    {
        return new AdaptivityTaskMemento(Questions, MinimumRequiredDifficulty, Name, UnsavedChanges);
    }

    public void RestoreMemento(IMemento memento)
    {
        if (memento is not AdaptivityTaskMemento adaptivityTaskMemento)
        {
            throw new ArgumentException("Incorrect IMemento implementation", nameof(memento));
        }

        Questions = adaptivityTaskMemento.Questions;
        MinimumRequiredDifficulty = adaptivityTaskMemento.MinimumRequiredDifficulty;
        Name = adaptivityTaskMemento.Name;
        UnsavedChanges = adaptivityTaskMemento.UnsavedChanges;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((AdaptivityTask) obj);
    }

    // ReSharper disable NonReadonlyMemberInGetHashCode
    public override int GetHashCode()
    {
        return HashCode.Combine(Questions,
            MinimumRequiredDifficulty.HasValue ? (int) MinimumRequiredDifficulty.Value : -1, Id);
    }
    // ReSharper restore NonReadonlyMemberInGetHashCode

    public static bool operator ==(AdaptivityTask? left, AdaptivityTask? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(AdaptivityTask? left, AdaptivityTask? right)
    {
        return !Equals(left, right);
    }

    private record AdaptivityTaskMemento : IMemento
    {
        internal AdaptivityTaskMemento(ICollection<IAdaptivityQuestion> questions,
            QuestionDifficulty? minimumRequiredDifficulty, string name, bool unsavedChanges)
        {
            Questions = questions.ToList();
            MinimumRequiredDifficulty = minimumRequiredDifficulty;
            Name = name;
            UnsavedChanges = unsavedChanges;
        }

        internal ICollection<IAdaptivityQuestion> Questions { get; set; }
        internal QuestionDifficulty? MinimumRequiredDifficulty { get; set; }
        internal string Name { get; set; }
        internal bool UnsavedChanges { get; set; }
    }
}