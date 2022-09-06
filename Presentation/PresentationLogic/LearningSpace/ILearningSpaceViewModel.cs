using Presentation.PresentationLogic.LearningElement;

namespace Presentation.PresentationLogic.LearningSpace;

public interface ILearningSpaceViewModel : IDisplayableLearningObject, ILearningElementViewModelParent, ILearningObjectViewModel
{
    Guid Id { get; }
    string FileEnding { get; }
    ICollection<ILearningElementViewModel> LearningElements { get; set; }
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