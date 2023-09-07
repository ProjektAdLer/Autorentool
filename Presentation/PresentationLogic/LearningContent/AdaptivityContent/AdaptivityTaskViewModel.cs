using Presentation.PresentationLogic.LearningContent.AdaptivityContent.Question;
using Shared.Adaptivity;

namespace Presentation.PresentationLogic.LearningContent.AdaptivityContent;

public class AdaptivityTaskViewModel : IAdaptivityTaskViewModel
{
    public AdaptivityTaskViewModel(ICollection<IAdaptivityQuestionViewModel> questions, QuestionDifficulty minimumRequiredDifficulty)
    {
        Questions = questions;
        MinimumRequiredDifficulty = minimumRequiredDifficulty;
        Id = Guid.NewGuid();
    }

    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private AdaptivityTaskViewModel()
    {
        Questions = null!;
        MinimumRequiredDifficulty = QuestionDifficulty.Easy;
        Id = Guid.Empty;
    }

    public ICollection<IAdaptivityQuestionViewModel> Questions { get; set; }
    public QuestionDifficulty MinimumRequiredDifficulty { get; set; }
    public Guid Id { get; set; }
}