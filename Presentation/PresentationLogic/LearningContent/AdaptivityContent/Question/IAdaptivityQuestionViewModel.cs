using Shared.Adaptivity;

namespace Presentation.PresentationLogic.LearningContent.AdaptivityContent.Question;

public interface IAdaptivityQuestionViewModel
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
    public ICollection<IAdaptivityRuleViewModel> Rules { get; set; }
}