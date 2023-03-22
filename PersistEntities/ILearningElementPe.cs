

using PersistEntities.LearningContent;

namespace PersistEntities;

public interface ILearningElementPe
{
    string Name { get; set; }
    string Shortname { get; set; }
    ILearningContentPe LearningContent { get; set; }
    string Authors { get; set; }
    string Description { get; set; }
    string Goals { get; set; }
    int Workload { get; set; }
    int Points { get; set; }
    LearningElementDifficultyEnumPe Difficulty { get; set; }
    double PositionX { get; set; }
    double PositionY { get; set; }
}