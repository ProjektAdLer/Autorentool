using BusinessLogic.Entities.LearningContent;
using Shared;
using LearningElementDifficultyEnum = Shared.LearningElementDifficultyEnum;

namespace BusinessLogic.Entities;

public interface ILearningElement
{
    Guid Id { get; }
    string Name { get; set; }
    public ILearningSpace? Parent { get; set; }
    ILearningContent LearningContent { get; set; }
    string Description { get; set; }
    string Goals { get; set; }
    ElementModel ElementModel { get; set; }
    int Workload { get; set; }
    int Points { get; set; }
    bool UnsavedChanges { get; set; }
    bool InternalUnsavedChanges { get; }
    LearningElementDifficultyEnum Difficulty { get; set; }
}