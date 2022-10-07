using JetBrains.Annotations;
using LearningElementDifficultyEnum = Shared.LearningElementDifficultyEnum;

namespace BusinessLogic.Entities;

public class H5PInteractionElement : LearningElement
{
    /// <summary>
    /// Private Constructor for AutoMapper
    /// </summary>
    [UsedImplicitly]
    private H5PInteractionElement()
    {
    }

    public H5PInteractionElement(string name, string shortname, ILearningSpace? parent,
        LearningContent learningContent, string url, string authors, string description, string goals,
        LearningElementDifficultyEnum difficulty, int workload, int points, double positionX, double positionY) : base(
        name, shortname, learningContent, url, authors, description, goals, difficulty, parent, workload, points, positionX,
        positionY)
    {
    }
}