using System.Collections.ObjectModel;
using JetBrains.Annotations;

namespace Presentation.PresentationLogic.LearningOutcome;

public class LearningOutcomeCollectionViewModel : ILearningOutcomeCollectionViewModel
{
    public LearningOutcomeCollectionViewModel(ObservableCollection<ILearningOutcomeViewModel>? learningOutcomes = null)
    {
        LearningOutcomes = learningOutcomes ?? new ObservableCollection<ILearningOutcomeViewModel>();
        UnsavedChanges = true;
    }

    [UsedImplicitly]
    private LearningOutcomeCollectionViewModel()
    {
        LearningOutcomes = new ObservableCollection<ILearningOutcomeViewModel>();
        UnsavedChanges = false;
    }

    public ObservableCollection<ILearningOutcomeViewModel> LearningOutcomes { get; set; }

    public bool UnsavedChanges { get; set; }
}