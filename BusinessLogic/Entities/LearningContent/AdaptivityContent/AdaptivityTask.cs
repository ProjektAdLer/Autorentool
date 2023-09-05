using BusinessLogic.Entities.LearningContent.AdaptivityContent.Question;
using Shared.Adaptivity;

namespace BusinessLogic.Entities.LearningContent.AdaptivityContent;

public class AdaptivityTask : IAdaptivityTask
{
    public AdaptivityTask(IEnumerable<IAdaptivityQuestion> questions, QuestionDifficulty minimumRequiredDifficulty)
    {
        Questions = questions;
        MinimumRequiredDifficulty = minimumRequiredDifficulty;
    }

    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private AdaptivityTask()
    {
        Questions = null!;
        MinimumRequiredDifficulty = QuestionDifficulty.Easy;
    }

    public IEnumerable<IAdaptivityQuestion> Questions { get; set; }
    public QuestionDifficulty MinimumRequiredDifficulty { get; set; }
}