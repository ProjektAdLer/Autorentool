using System.ComponentModel;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningOutcome;
using Presentation.PresentationLogic.LearningSpace.SpaceLayout;
using Presentation.PresentationLogic.Topic;
using Shared.Theme;

namespace Presentation.PresentationLogic.LearningSpace;

public interface ILearningSpaceViewModel : IDisplayableLearningObject, IObjectInPathWayViewModel, INotifyPropertyChanged
{
    IEnumerable<ILearningElementViewModel> ContainedLearningElements { get; }
    ILearningSpaceLayoutViewModel LearningSpaceLayout { get; set; }
    TopicViewModel? AssignedTopic { get; set; }
    int Workload { get; }
    int Points { get; }
    int NumberOfElements { get; }
    int NumberOfRequiredElements { get; }
    int RequiredPoints { get; }
    SpaceTheme SpaceTheme { get; set; }
    new string Name { get; set; }
    string Description { get; set; }
    LearningOutcomeCollectionViewModel LearningOutcomeCollection { get; set; }
    bool InternalUnsavedChanges { get; }
}