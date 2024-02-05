using JetBrains.Annotations;

namespace Presentation.PresentationLogic.LearningSpace.LearningOutcomeViewModel;

public class LearningOutcomeCollectionViewModel
{
    public LearningOutcomeCollectionViewModel(List<ILearningOutcomeViewModel>? learningOutcomes = null)
    {
        LearningOutcomes = learningOutcomes ?? new List<ILearningOutcomeViewModel>();
    }

    [UsedImplicitly]
    private LearningOutcomeCollectionViewModel()
    {
        LearningOutcomes = new List<ILearningOutcomeViewModel>();
    }

    public List<ILearningOutcomeViewModel> LearningOutcomes { get; set; }
}