using LearningElementDifficultyEnum = Shared.LearningElementDifficultyEnum;

namespace BusinessLogic.Entities;

public interface ILearningElement
{
    string Shortname { get; set; }
    public ILearningElementParent? Parent { get; set; }
    LearningContent LearningContent { get; set; }
    string Authors { get; set; }
    string Goals { get; set; }
    int Workload { get; set; }
    LearningElementDifficultyEnum Difficulty { get; set; }
}