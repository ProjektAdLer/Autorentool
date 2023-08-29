using Shared.Adaptivity;

namespace Presentation.PresentationLogic.LearningContent.AdaptivityContent.Question;

public interface IAdaptivityQuestionViewModel
{
    /// <summary>
    /// Expected completion time of question in seconds.
    /// </summary>
    public int ExpectedCompletionTime { get; set; }
    /// <summary>
    /// Difficulty of the question.
    /// </summary>
    public QuestionDifficulty Difficulty { get; set; }
}