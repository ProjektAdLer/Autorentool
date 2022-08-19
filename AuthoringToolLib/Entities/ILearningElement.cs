using AuthoringToolLib.PresentationLogic.LearningElement;

namespace AuthoringToolLib.Entities;

public interface ILearningElement
{
    string Name { get; set; }
    string Shortname { get; set; }
    LearningContent? Content { get; set; }
    string Authors { get; set; }
    string Description { get; set; }
    string Goals { get; set; }
    int Workload { get; set; }
    LearningElementDifficultyEnum Difficulty { get; set; }
    double PositionX { get; set; }
    double PositionY { get; set; }
    string? ParentName { get; set; }
}