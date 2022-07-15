using AuthoringTool.PresentationLogic.LearningElement;

namespace AuthoringTool.PresentationLogic.LearningSpace;

public interface ILearningSpaceViewModel : IDisplayableLearningObject, ILearningElementViewModelParent, ILearningObjectViewModel
{
    string FileEnding { get; }
    ICollection<LearningElementViewModel> LearningElements { get; set; }
    int Workload { get; }
    string Name { get; set; }
    string Shortname { get; set; }
    string Authors { get; set; }
    string Description { get; set; }
    string Goals { get; set; }
    double PositionX { get; set; }
    double PositionY { get; set; }
    ILearningObjectViewModel? SelectedLearningObject { get; set; }
}