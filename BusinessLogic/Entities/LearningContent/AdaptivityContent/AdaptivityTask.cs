using BusinessLogic.Entities.LearningContent.AdaptivityContent.Question;
using Shared.Adaptivity;

namespace BusinessLogic.Entities.LearningContent.AdaptivityContent;

class AdaptivityTask : IAdaptivityTask
{
    public AdaptivityTask(IEnumerable<IAdaptivityQuestion> questions, QuestionDifficulty minimumRequiredDifficulty)
    {
        Questions = questions;
        MinimumRequiredDifficulty = minimumRequiredDifficulty;
    }

    public IEnumerable<IAdaptivityQuestion> Questions { get; set; }
    public QuestionDifficulty MinimumRequiredDifficulty { get; set; }
}