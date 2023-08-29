using Presentation.PresentationLogic.LearningContent.AdaptivityContent.Question;
using Shared.Adaptivity;

namespace Presentation.PresentationLogic.LearningContent.AdaptivityContent;

class AdaptivityTaskViewModel : IAdaptivityTaskViewModel
{
    public AdaptivityTaskViewModel(IEnumerable<IAdaptivityQuestionViewModel> questions, QuestionDifficulty minimumRequiredDifficulty)
    {
        Questions = questions;
        MinimumRequiredDifficulty = minimumRequiredDifficulty;
    }

    public IEnumerable<IAdaptivityQuestionViewModel> Questions { get; set; }
    public QuestionDifficulty MinimumRequiredDifficulty { get; set; }
}