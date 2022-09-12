using Presentation.PresentationLogic.LearningElement;

namespace Presentation.PresentationLogic.LearningSpace;

public interface ILearningSpaceViewModel : IDisplayableLearningObject, ILearningElementViewModelParent, ILearningObjectViewModel
{
    Guid Id { get; }
    ICollection<ILearningElementViewModel> LearningElements { get; set; }
    int Workload { get; }
    new string Name { get; set; }
    string Shortname { get; set; }
    string Authors { get; set; }
    string Goals { get; set; }
    ILearningObjectViewModel? SelectedLearningObject { get; set; }
}