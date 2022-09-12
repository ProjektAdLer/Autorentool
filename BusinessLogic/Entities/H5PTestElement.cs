using JetBrains.Annotations;
using Shared;

namespace BusinessLogic.Entities;

public class H5PTestElement : LearningElement
{
    /// <summary>
    /// Private Constructor for AutoMapper
    /// </summary>
    [UsedImplicitly]
    private H5PTestElement()
    {
    }

    public H5PTestElement(string name, string shortname,  ILearningElementParent? parent, LearningContent learningContent,
        string authors, string description, string goals, LearningElementDifficultyEnum difficulty, int workload,
        double positionX, double positionY) : base(name, shortname, learningContent, authors, description, 
        goals, difficulty, parent, workload, positionX, positionY)
    {
    }
}