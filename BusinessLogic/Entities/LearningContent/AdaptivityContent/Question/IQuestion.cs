namespace BusinessLogic.Entities.LearningContent.AdaptivityContent.Question;

public interface IQuestion
{
    /// <summary>
    /// Expected completion time of question in seconds
    /// </summary>
    public int ExpectedCompletionTime { get; set; }
}