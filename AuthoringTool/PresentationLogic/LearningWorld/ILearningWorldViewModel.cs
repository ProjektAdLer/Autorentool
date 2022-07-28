using AuthoringTool.PresentationLogic.LearningElement;
using AuthoringTool.PresentationLogic.LearningSpace;

namespace AuthoringTool.PresentationLogic.LearningWorld;

public interface ILearningWorldViewModel : ILearningElementViewModelParent, ISerializableViewModel, IDisplayableLearningObject
{
    string FileEnding { get; }
    ICollection<ILearningElementViewModel> LearningElements { get; set; }
    int Workload { get; }
    ICollection<ILearningSpaceViewModel> LearningSpaces { get; set; }
    IEnumerable<ILearningObjectViewModel> LearningObjects { get; }
    string Name { get; set; }
    string Shortname { get; set; }
    string Authors { get; set; }
    string Language { get; set; }
    string Description { get; set; }
    string Goals { get; set; }
    bool UnsavedChanges { get; set; }
    ILearningObjectViewModel? SelectedLearningObject { get; set; }
    bool ShowingLearningSpaceView { get; set; }
}