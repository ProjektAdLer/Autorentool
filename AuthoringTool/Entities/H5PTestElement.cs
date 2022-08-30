namespace AuthoringTool.Entities;

public class H5PTestElement : LearningElement
{
    /// <summary>
    /// Private Constructor for AutoMapper
    /// </summary>
    private H5PTestElement() : base()
    {
    }

    internal H5PTestElement(string name, string shortname,  ILearningElementParent? parentName, LearningContent? learningContent,
        string authors, string description, string goals, LearningElementDifficultyEnum difficulty, int workload,
        double positionX, double positionY) : base(name, shortname,learningContent, authors, description, 
        goals,difficulty,parentName, workload,positionX, positionY)
    {
    }
}