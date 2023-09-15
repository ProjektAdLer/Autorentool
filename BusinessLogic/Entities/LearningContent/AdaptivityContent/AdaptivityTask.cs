using BusinessLogic.Entities.LearningContent.AdaptivityContent.Question;
using Shared.Adaptivity;

namespace BusinessLogic.Entities.LearningContent.AdaptivityContent;

public class AdaptivityTask : IAdaptivityTask
{
    public AdaptivityTask(ICollection<IAdaptivityQuestion> questions, QuestionDifficulty minimumRequiredDifficulty)
    {
        Questions = questions;
        MinimumRequiredDifficulty = minimumRequiredDifficulty;
        Id = Guid.NewGuid();
    }

    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private AdaptivityTask()
    {
        Questions = null!;
        MinimumRequiredDifficulty = QuestionDifficulty.Easy;
        Id = Guid.Empty;
    }

    public ICollection<IAdaptivityQuestion> Questions { get; set; }
    public QuestionDifficulty MinimumRequiredDifficulty { get; set; }
    public Guid Id { get; set; }

    public bool Equals(IAdaptivityTask? other)
    {
        if (other is not AdaptivityTask)
            return false;
        return Questions.SequenceEqual(other.Questions) &&
               MinimumRequiredDifficulty == other.MinimumRequiredDifficulty && Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((AdaptivityTask)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Questions, (int)MinimumRequiredDifficulty, Id);
    }

    public static bool operator ==(AdaptivityTask? left, AdaptivityTask? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(AdaptivityTask? left, AdaptivityTask? right)
    {
        return !Equals(left, right);
    }
}