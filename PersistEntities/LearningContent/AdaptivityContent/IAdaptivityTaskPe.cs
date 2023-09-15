using PersistEntities.LearningContent.Question;
using Shared.Adaptivity;

namespace PersistEntities.LearningContent;

public interface IAdaptivityTaskPe
{
    /// <summary>
    /// The main questions this task contains.
    /// </summary>
    public ICollection<IAdaptivityQuestionPe> Questions { get; set; }
    /// <summary>
    /// The minimum required difficulty of question required to answer to complete this task.
    /// </summary>
    public QuestionDifficulty MinimumRequiredDifficulty { get; set; }

    Guid Id { get; set; }
}