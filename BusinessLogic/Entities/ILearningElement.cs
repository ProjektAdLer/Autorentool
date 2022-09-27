using LearningElementDifficultyEnum = Shared.LearningElementDifficultyEnum;

namespace BusinessLogic.Entities;

public interface ILearningElement : ILearningObject
{
    new string Name { get; set; }
    string Shortname { get; set; }
    public ILearningElementParent? Parent { get; set; }
    LearningContent LearningContent { get; set; }
    string Authors { get; set; }
    new string Description { get; set; }
    string Goals { get; set; }
    int Workload { get; set; }
    int Points { get; set; }
    LearningElementDifficultyEnum Difficulty { get; set; }
}