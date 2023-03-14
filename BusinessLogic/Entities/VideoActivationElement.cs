using JetBrains.Annotations;
using Shared;

namespace BusinessLogic.Entities;

public class VideoActivationElement : LearningElement
{
    /// <summary>
    /// Private Constructor for AutoMapper
    /// </summary>
    [UsedImplicitly]
    private VideoActivationElement()
    {
    }

    public VideoActivationElement(string name, string shortname, ILearningSpace? parent,
        LearningContent.LearningContent learningContent, string authors, string description, string goals,
        LearningElementDifficultyEnum difficulty, int workload, int points, double positionX, double positionY) : base(
        name, shortname, learningContent, authors, description, goals, difficulty, parent, workload, points, positionX,
        positionY)
    {
    }
}