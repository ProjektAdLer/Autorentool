using System.ComponentModel;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningSpace.SpaceLayout;
using Presentation.PresentationLogic.Topic;

namespace Presentation.PresentationLogic.LearningSpace;

public interface ILearningSpaceViewModel : IDisplayableLearningObject, IObjectInPathWayViewModel, INotifyPropertyChanged
{
    IEnumerable<ILearningElementViewModel> ContainedLearningElements => LearningSpaceLayout.ContainedLearningElements;
    ILearningSpaceLayoutViewModel LearningSpaceLayout { get; set; }
    TopicViewModel? AssignedTopic { get; set; }
    int Workload { get; }
    int Points { get; }
    int RequiredPoints { get; }
    new string Name { get; set; }
    string Description { get; set; }
    string Goals { get; set; }
    ILearningElementViewModel? SelectedLearningElement { get; set; }
    event EventHandler<EventArgs>? SelectedElementChanged;
}