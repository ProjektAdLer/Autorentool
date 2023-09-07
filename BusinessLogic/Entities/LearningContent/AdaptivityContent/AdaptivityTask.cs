using BusinessLogic.Entities.LearningContent.AdaptivityContent.Question;
using Shared.Adaptivity;

namespace BusinessLogic.Entities.LearningContent.AdaptivityContent;

public class AdaptivityTask : IAdaptivityTask
{
    public AdaptivityTask(ICollection<IAdaptivityQuestion> questions, QuestionDifficulty minimumRequiredDifficulty)
    {
        Questions = questions;
        MinimumRequiredDifficulty = minimumRequiredDifficulty;
        Id = Guid.NewGuid();
    }

    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private AdaptivityTask()
    {
        Questions = null!;
        MinimumRequiredDifficulty = QuestionDifficulty.Easy;
        Id = Guid.Empty;
    }

    public ICollection<IAdaptivityQuestion> Questions { get; set; }
    public QuestionDifficulty MinimumRequiredDifficulty { get; set; }
    public Guid Id { get; set; }
}