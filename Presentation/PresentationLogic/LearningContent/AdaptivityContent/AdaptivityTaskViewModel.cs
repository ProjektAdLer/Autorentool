using Presentation.PresentationLogic.LearningContent.AdaptivityContent.Question;
using Shared.Adaptivity;

namespace Presentation.PresentationLogic.LearningContent.AdaptivityContent;

public class AdaptivityTaskViewModel : IAdaptivityTaskViewModel
{
    public AdaptivityTaskViewModel(IEnumerable<IAdaptivityQuestionViewModel> questions, QuestionDifficulty minimumRequiredDifficulty)
    {
        Questions = questions;
        MinimumRequiredDifficulty = minimumRequiredDifficulty;
    }

    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private AdaptivityTaskViewModel()
    {
        Questions = null!;
        MinimumRequiredDifficulty = QuestionDifficulty.Easy;
    }

    public IEnumerable<IAdaptivityQuestionViewModel> Questions { get; set; }
    public QuestionDifficulty MinimumRequiredDifficulty { get; set; }
}