using Shared.Adaptivity;

namespace BusinessLogic.Entities.LearningContent.AdaptivityContent.Question;

public interface IAdaptivityQuestion : IEquatable<IAdaptivityQuestion>
{
    public Guid Id { get; }
    /// <summary>
    /// Expected completion time of question in seconds.
    /// </summary>
    public int ExpectedCompletionTime { get; set; }
    /// <summary>
    /// Difficulty of the question.
    /// </summary>
    public QuestionDifficulty Difficulty { get; set; }
    /// <summary>
    /// Rules that are applied to the question.
    /// </summary>
    public ICollection<IAdaptivityRule> Rules { get; set; }
}