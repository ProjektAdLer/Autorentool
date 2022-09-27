using Presentation.PresentationLogic.LearningContent;
using Shared;

namespace Presentation.PresentationLogic.LearningElement;

public interface ILearningElementViewModel : ILearningObjectViewModel, IDisplayableLearningObject
{
    new string Name { get; set; }
    string Shortname { get; set; }
    ILearningElementViewModelParent? Parent { get; set; }
    LearningContentViewModel LearningContent { get; set; }
    string Authors { get; set; }
    string Goals { get; set; }
    LearningElementDifficultyEnum Difficulty { get; set; }
    int Workload { get; set; }
    int Points { get; set; }
}