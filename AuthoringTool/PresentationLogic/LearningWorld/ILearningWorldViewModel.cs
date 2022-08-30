using System.ComponentModel;
using AuthoringTool.PresentationLogic.LearningElement;
using AuthoringTool.PresentationLogic.LearningSpace;

namespace AuthoringTool.PresentationLogic.LearningWorld;

public interface ILearningWorldViewModel : ILearningElementViewModelParent, ISerializableViewModel, IDisplayableLearningObject, INotifyPropertyChanged
{
    new string FileEnding { get; }
    List<ILearningElementViewModel> LearningElements { get; set; }
    int Workload { get; }
    List<ILearningSpaceViewModel> LearningSpaces { get; set; }
    IEnumerable<ILearningObjectViewModel> LearningObjects { get; }
    new string Name { get; set; }
    string Shortname { get; set; }
    string Authors { get; set; }
    string Language { get; set; }
    string Description { get; set; }
    string Goals { get; set; }
    bool UnsavedChanges { get; set; }
    ILearningObjectViewModel? SelectedLearningObject { get; set; }
    bool ShowingLearningSpaceView { get; set; }
}