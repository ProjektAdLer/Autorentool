using BusinessLogic.Entities.LearningContent.Adaptivity.Question;
using Shared.Adaptivity;

namespace BusinessLogic.Entities.LearningContent.Adaptivity;

public interface IAdaptivityTask : IEquatable<IAdaptivityTask>, IOriginator
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
    bool UnsavedChanges { get; set; }
}