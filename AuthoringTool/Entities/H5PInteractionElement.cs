namespace AuthoringTool.Entities;

public class H5PInteractionElement : LearningElement
{
    /// <summary>
    /// Private Constructor for AutoMapper
    /// </summary>
    private H5PInteractionElement() : base()
    {
    }

    internal H5PInteractionElement(string name, string shortname,  ILearningElementParent? parentName, LearningContent? learningContent,
        string authors, string description, string goals, LearningElementDifficultyEnum difficulty, int workload,
        double positionX, double positionY) : base(name, shortname,learningContent, authors, description, 
        goals,difficulty,parentName, workload,positionX, positionY)
    {
    }
}