namespace AuthoringTool.Entities;

public class VideoActivationElement : LearningElement
{
    /// <summary>
    /// Private Constructor for AutoMapper
    /// </summary>
    private VideoActivationElement() : base()
    {
    }

    internal VideoActivationElement(string name, string shortname,  ILearningElementParent? parentName, LearningContent? learningContent,
        string authors, string description, string goals, LearningElementDifficultyEnum difficulty, int workload,
        double positionX, double positionY) : base(name, shortname,learningContent, authors, description, 
        goals,difficulty,parentName, workload,positionX, positionY)
    {
    }
}