using JetBrains.Annotations;
using LearningElementDifficultyEnum = Shared.LearningElementDifficultyEnum;

namespace BusinessLogic.Entities;

public class H5PActivationElement : LearningElement
{
    /// <summary>
    /// Private Constructor for AutoMapper
    /// </summary>
    [UsedImplicitly]
    private H5PActivationElement()
    {
    }

    public H5PActivationElement(string name, string shortname, ILearningSpace? parent,
        LearningContent.LearningContent learningContent, string authors, string description, string goals,
        LearningElementDifficultyEnum difficulty, int workload = 0, int points = 0, double positionX = 0,
        double positionY = 0) : base(name, shortname, learningContent, authors, description, goals, 
        difficulty, parent, workload, points, positionX, positionY)
    {
    }
}