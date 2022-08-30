

namespace AuthoringTool.Entities;

public class H5PActivationElement : LearningElement
{
    /// <summary>
    /// Private Constructor for AutoMapper
    /// </summary>
    private H5PActivationElement() : base()
    {
    }

    internal H5PActivationElement(string name, string shortname, LearningContent? learningContent,
        string authors, string description, string goals, LearningElementDifficultyEnum difficulty,
        ILearningElementParent? parent, int workload,
        double positionX, double positionY) : base(name, shortname,learningContent, authors, description, 
        goals,difficulty,parent, workload,positionX, positionY)
    {
    }
}