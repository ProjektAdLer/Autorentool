using BusinessLogic.Entities.LearningContent.AdaptivityContent.Question;
using Shared.Adaptivity;

namespace BusinessLogic.Entities.LearningContent.AdaptivityContent;

public interface IAdaptivityTask : IEquatable<IAdaptivityTask>
{
    /// <summary>
    /// The main questions this task contains.
    /// </summary>
    public ICollection<IAdaptivityQuestion> Questions { get; set; }
    /// <summary>
    /// The minimum required difficulty of question required to answer to complete this task.
    /// </summary>
    public QuestionDifficulty? MinimumRequiredDifficulty { get; set; }

    Guid Id { get; set; }
    string Name { get; set; }
}