namespace BusinessLogic.Entities.LearningContent.AdaptivityContent.Question;

public interface IAdaptivityQuestion
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