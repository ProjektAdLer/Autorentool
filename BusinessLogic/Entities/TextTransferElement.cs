using BusinessLogic.Entities.LearningContent;
using JetBrains.Annotations;
using Shared;

namespace BusinessLogic.Entities;

public class TextTransferElement : LearningElement
{
    /// <summary>
    /// Private Constructor for AutoMapper
    /// </summary>
    [UsedImplicitly]
    private TextTransferElement()
    {
    }
    
    internal TextTransferElement(string name, string shortname, ILearningSpace? parent,
        ILearningContent learningContent, string authors, string description, string goals,
        LearningElementDifficultyEnum difficulty, int workload, int points, double positionX, double positionY) : base(
        name, shortname, learningContent, authors, description, goals, difficulty, parent, workload, points, positionX,
        positionY)
    {
    }
}