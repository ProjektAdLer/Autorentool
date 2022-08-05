using AuthoringTool.PresentationLogic.LearningElement;

namespace AuthoringTool.Entities;

[Serializable]

public class H5PActivationElement : LearningElement
{
    internal H5PActivationElement(string name, string shortname,  string? parentName, LearningContent? content,
        string authors, string description, string goals, LearningElementDifficultyEnum difficulty, int workload,
        double positionX, double positionY) : base(name, shortname,parentName,content, authors, description, 
        goals,difficulty,workload, positionX,positionY)
    {
    }
    /// <summary>
    /// Constructor for serialization. DO NOT USE FOR NORMAL INITIALIZATION.
    /// </summary>
    private H5PActivationElement()
    {
    }
}