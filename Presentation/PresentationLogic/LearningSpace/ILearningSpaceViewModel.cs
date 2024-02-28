using System.ComponentModel;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningSpace.LearningOutcomeViewModel;
using Presentation.PresentationLogic.LearningSpace.SpaceLayout;
using Presentation.PresentationLogic.Topic;
using Shared;

namespace Presentation.PresentationLogic.LearningSpace;

public interface ILearningSpaceViewModel : IDisplayableLearningObject, IObjectInPathWayViewModel, INotifyPropertyChanged
{
    IEnumerable<ILearningElementViewModel> ContainedLearningElements => LearningSpaceLayout.ContainedLearningElements;
    ILearningSpaceLayoutViewModel LearningSpaceLayout { get; set; }
    TopicViewModel? AssignedTopic { get; set; }
    int Workload { get; }
    int Points { get; }
    int RequiredPoints { get; }
    Theme Theme { get; set; }
    new string Name { get; set; }
    string Description { get; set; }
    LearningOutcomeCollectionViewModel LearningOutcomeCollection { get; set; }
    bool InternalUnsavedChanges { get; }
}