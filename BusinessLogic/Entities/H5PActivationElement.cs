using LearningElementDifficultyEnum = Shared.LearningElementDifficultyEnum;

namespace BusinessLogic.Entities;

public class H5PActivationElement : LearningElement
{
    public H5PActivationElement(string name, string shortname, LearningContent learningContent,
        string authors, string description, string goals, LearningElementDifficultyEnum difficulty,
        ILearningElementParent? parent, int workload,
        double positionX, double positionY) : base(name, shortname, learningContent, authors, description, 
        goals, difficulty, parent, workload,positionX, positionY)
    {
    }
}