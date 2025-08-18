using System.ComponentModel;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningOutcome;
using Presentation.PresentationLogic.LearningPathway;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.Topic;
using Shared.Theme;

namespace Presentation.PresentationLogic.LearningWorld;

public interface ILearningWorldViewModel : ISerializableViewModel, IDisplayableLearningObject, INotifyPropertyChanged
{
    Guid Id { get; }
    new string FileEnding { get; }
    int Workload { get; }
    int Points { get; }
    int NumberOfElements { get; }
    int NumberOfRequiredElements { get; }
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
    LearningOutcomeCollectionViewModel LearningOutcomeCollection { get; set; }
    WorldTheme WorldTheme { get; set; }
    string SavePath { get; set; }
    bool UnsavedChanges { get; set; }
    IObjectInPathWayViewModel? OnHoveredObjectInPathWay { get; set; }
    ICollection<ILearningElementViewModel> UnplacedLearningElements { get; set; }
    bool InternalUnsavedChanges { get; }
    IEnumerable<ILearningElementViewModel> AllLearningElements { get; }
    string EvaluationLink { get; set; }
    string EvaluationLinkName { get; set; }
    string EvaluationLinkText { get; set; }
    string EnrolmentKey { get; set; }
    IEnumerable<ILearningElementViewModel> AllStoryElements { get; }
    string StoryStart { get; set; }
    string StoryEnd { get; set; }
}