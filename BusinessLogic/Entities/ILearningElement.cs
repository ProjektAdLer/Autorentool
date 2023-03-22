using BusinessLogic.Entities.LearningContent;
using LearningElementDifficultyEnum = Shared.LearningElementDifficultyEnum;

namespace BusinessLogic.Entities;

public interface ILearningElement
{
    Guid Id { get; }
    string Name { get; set; }
    string Shortname { get; set; }
    public ILearningSpace? Parent { get; set; }
    ILearningContent LearningContent { get; set; }
    string Authors { get; set; }
    string Description { get; set; }
    string Goals { get; set; }
    int Workload { get; set; }
    int Points { get; set; }
    LearningElementDifficultyEnum Difficulty { get; set; }
}