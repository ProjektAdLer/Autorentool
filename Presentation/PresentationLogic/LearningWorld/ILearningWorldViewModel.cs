using System.ComponentModel;
using Presentation.PresentationLogic.LearningPathway;
using Presentation.PresentationLogic.LearningSpace;

namespace Presentation.PresentationLogic.LearningWorld;

public interface ILearningWorldViewModel : ISerializableViewModel, IDisplayableLearningObject, INotifyPropertyChanged
{
    new string FileEnding { get; }
    int Workload { get; }
    int Points { get; }
    ICollection<ILearningSpaceViewModel> LearningSpaces { get; set; }
    ICollection<ILearningPathWayViewModel> LearningPathWays { get; set; }
    new string Name { get; set; }
    string Shortname { get; set; }
    string Authors { get; set; }
    string Language { get; set; }
    string Description { get; set; }
    string Goals { get; set; }
    bool UnsavedChanges { get; set; }
    ILearningSpaceViewModel? SelectedLearningSpace { get; set; }
    ILearningSpaceViewModel? OnHoveredLearningSpace { get; set; }
    bool ShowingLearningSpaceView { get; set; }
}