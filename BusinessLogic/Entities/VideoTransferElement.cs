using JetBrains.Annotations;
using Shared;

namespace BusinessLogic.Entities;

public class VideoTransferElement : LearningElement
{
    /// <summary>
    /// Private Constructor for AutoMapper
    /// </summary>
    [UsedImplicitly]
    private VideoTransferElement()
    {
    }

    public VideoTransferElement(string name, string shortname, ILearningSpace? parent,
        LearningContent learningContent, string authors, string description, string goals,
        LearningElementDifficultyEnum difficulty, int workload, int points, double positionX, double positionY) : base(
        name, shortname, learningContent, authors, description, goals, difficulty, parent, workload, points, positionX,
        positionY)
    {
    }
}