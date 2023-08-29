using BusinessLogic.Entities.LearningContent.AdaptivityContent.Question;

namespace BusinessLogic.Entities.LearningContent.AdaptivityContent;

public interface IAdaptivityTask
{
    /// <summary>
    /// The main questions this task contains.
    /// </summary>
    public IEnumerable<IAdaptivityQuestion> Questions { get; set; }
    /// <summary>
    /// The minimum required difficulty of question required to answer to complete this task.
    /// </summary>
    public QuestionDifficulty MinimumRequiredDifficulty { get; set; }
}