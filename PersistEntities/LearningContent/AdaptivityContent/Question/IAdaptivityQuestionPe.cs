using Shared.Adaptivity;

namespace PersistEntities.LearningContent.Question;

public interface IAdaptivityQuestionPe
{
    public Guid Id { get; }

    /// <summary>
    /// Title of the question.
    /// </summary>
    public string Title { get; set; }

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
    public ICollection<IAdaptivityRulePe> Rules { get; set; }
}