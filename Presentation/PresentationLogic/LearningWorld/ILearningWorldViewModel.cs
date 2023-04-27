using System.ComponentModel;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningPathway;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.Topic;

namespace Presentation.PresentationLogic.LearningWorld;

public interface ILearningWorldViewModel : ISerializableViewModel, IDisplayableLearningObject, INotifyPropertyChanged
{
    Guid Id { get; }
    new string FileEnding { get; }
    int Workload { get; }
    int Points { get; }
    ICollection<ILearningSpaceViewModel> LearningSpaces { get; }
    ICollection<PathWayConditionViewModel> PathWayConditions { get; }
    IEnumerable<IObjectInPathWayViewModel> ObjectsInPathWays { get; }
    ICollection<ILearningPathWayViewModel> LearningPathWays { get; }
    ICollection<TopicViewModel> Topics { get; }
    new string Name { get; set; }
    string Shortname { get; set; }
    string Authors { get; set; }
    string Language { get; set; }
    string Description { get; set; }
    string Goals { get; set; }
    string SavePath { get; set; }
    bool UnsavedChanges { get; set; }
    IObjectInPathWayViewModel? OnHoveredObjectInPathWay { get; set; }
    bool ShowingLearningSpaceView { get; set; }
    ICollection<ILearningElementViewModel> UnplacedLearningElements { get; set; }
}