using Presentation.PresentationLogic.LearningElement;

namespace Presentation.PresentationLogic.LearningSpace;

public interface ILearningSpaceViewModel : IDisplayableLearningObject
{
    Guid Id { get; }
    ICollection<ILearningElementViewModel> LearningElements { get; set; }
    ICollection<ILearningSpaceViewModel> InBoundSpaces { get; set; }
    ICollection<ILearningSpaceViewModel> OutBoundSpaces { get; set; }
    int Workload { get; }
    int Points { get; }
    int RequiredPoints { get; }
    new string Name { get; set; }
    string Description { get; set; }
    string Shortname { get; set; }
    string Authors { get; set; }
    string Goals { get; set; }
    double PositionX { get; set; }
    double PositionY { get; set; }
    ILearningElementViewModel? SelectedLearningElement { get; set; }
}