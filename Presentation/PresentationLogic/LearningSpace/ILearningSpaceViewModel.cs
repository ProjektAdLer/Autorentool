using System.ComponentModel;
using Presentation.PresentationLogic.LearningElement;
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
    bool AdvancedMode { get; }
    new string Name { get; set; }
    string Description { get; set; }
    string Goals { get; set; }
    bool UnsavedChanges { get; set; }
    bool InternalUnsavedChanges { get; }
}