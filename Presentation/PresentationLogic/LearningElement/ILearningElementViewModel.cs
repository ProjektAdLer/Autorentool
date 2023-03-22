using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningSpace;
using Shared;

namespace Presentation.PresentationLogic.LearningElement;

public interface ILearningElementViewModel : IDisplayableLearningObject
{
    Guid Id { get; }
    new string Name { get; set; }
    string Description { get; set; }
    string Shortname { get; set; }
    ILearningSpaceViewModel? Parent { get; set; }
    ILearningContentViewModel LearningContent { get; set; }
    string Authors { get; set; }
    string Goals { get; set; }
    LearningElementDifficultyEnum Difficulty { get; set; }
    int Workload { get; set; }
    int Points { get; set; }
    double PositionX { get; set; }
    double PositionY { get; set; }
}