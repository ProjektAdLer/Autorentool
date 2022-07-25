using AuthoringTool.PresentationLogic.LearningContent;

namespace AuthoringTool.PresentationLogic.LearningElement;

public interface ILearningElementViewModel : ILearningObjectViewModel
{
    string FileEnding { get; }
    string Name { get; set; }
    string Shortname { get; set; }
    ILearningElementViewModelParent? Parent { get; set; }
    LearningContentViewModel LearningContent { get; set; }
    string Authors { get; set; }
    string Description { get; set; }
    string Goals { get; set; }
    LearningElementDifficultyEnum Difficulty { get; set; }
    int Workload { get; set; }
    double PositionX { get; set; }
    double PositionY { get; set; }
}