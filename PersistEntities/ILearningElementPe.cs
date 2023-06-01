

using PersistEntities.LearningContent;
using Shared;

namespace PersistEntities;

public interface ILearningElementPe
{
    Guid Id { get; set; }
    string Name { get; set; }
    ILearningContentPe LearningContent { get; set; }
    string Description { get; set; }
    string Goals { get; set; }
    int Workload { get; set; }
    int Points { get; set; }
    LearningElementDifficultyEnum Difficulty { get; set; }
    ElementModel ElementModel { get; set; }
    double PositionX { get; set; }
    double PositionY { get; set; }
}