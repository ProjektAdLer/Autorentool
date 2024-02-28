using JetBrains.Annotations;

namespace Presentation.PresentationLogic.LearningSpace.LearningOutcomeViewModel;

public class LearningOutcomeCollectionViewModel
{
    public LearningOutcomeCollectionViewModel(List<ILearningOutcomeViewModel>? learningOutcomes = null)
    {
        LearningOutcomes = learningOutcomes ?? new List<ILearningOutcomeViewModel>();
        UnsavedChanges = true;
    }

    [UsedImplicitly]
    private LearningOutcomeCollectionViewModel()
    {
        LearningOutcomes = new List<ILearningOutcomeViewModel>();
        UnsavedChanges = false;
    }

    public List<ILearningOutcomeViewModel> LearningOutcomes { get; set; }

    public bool UnsavedChanges { get; set; }
}