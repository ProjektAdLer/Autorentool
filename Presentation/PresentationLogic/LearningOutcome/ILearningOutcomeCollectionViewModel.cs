using System.Collections.ObjectModel;

namespace Presentation.PresentationLogic.LearningOutcome;

public interface ILearningOutcomeCollectionViewModel
{
    ObservableCollection<ILearningOutcomeViewModel> LearningOutcomes { get; set; }
    bool UnsavedChanges { get; set; }
}