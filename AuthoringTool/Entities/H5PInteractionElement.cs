using AuthoringTool.PresentationLogic.LearningElement;

namespace AuthoringTool.Entities;

[Serializable]

public class H5PInteractionElement : LearningElement
{
    internal H5PInteractionElement(string name, string shortname,  string? parentName, LearningContent? content,
        string authors, string description, string goals, LearningElementDifficultyEnum difficulty, int workload,
        double positionX, double positionY) : base(name, shortname,parentName,content, authors, description, 
        goals,difficulty,workload, positionX,positionY)
    {
    }
    /// <summary>
    /// Constructor for serialization. DO NOT USE FOR NORMAL INITIALIZATION.
    /// </summary>
    private H5PInteractionElement() : base()
    {
    }
}