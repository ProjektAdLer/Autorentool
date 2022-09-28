using System.ComponentModel;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningSpace;

namespace Presentation.PresentationLogic.LearningWorld;

public interface ILearningWorldViewModel : ISerializableViewModel, IDisplayableLearningObject, INotifyPropertyChanged
{
    new string FileEnding { get; }
    int Workload { get; }
    int Points { get; }
    List<ILearningSpaceViewModel> LearningSpaces { get; set; }
    IEnumerable<ILearningObjectViewModel> LearningObjects { get; }
    new string Name { get; set; }
    string Shortname { get; set; }
    string Authors { get; set; }
    string Language { get; set; }
    string Description { get; set; }
    string Goals { get; set; }
    bool UnsavedChanges { get; set; }
    ILearningSpaceViewModel? SelectedLearningSpace { get; set; }
    bool ShowingLearningSpaceView { get; set; }
}