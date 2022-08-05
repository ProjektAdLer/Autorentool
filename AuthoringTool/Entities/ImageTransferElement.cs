using AuthoringTool.PresentationLogic.LearningElement;

namespace AuthoringTool.Entities;

[Serializable]

public class ImageTransferElement : LearningElement
{
    internal ImageTransferElement(string name, string shortname,  string? parentName, LearningContent? content,
        string authors, string description, string goals, LearningElementDifficultyEnum difficulty, int workload,
        double positionX, double positionY) : base(name, shortname,parentName,content, authors, description, 
        goals,difficulty,workload, positionX,positionY)
    {
    }
    /// <summary>
    /// Constructor for serialization. DO NOT USE FOR NORMAL INITIALIZATION.
    /// </summary>
    private ImageTransferElement()
    {
        Name = "";
        Shortname = "";
        Content = new LearningContent();
        Authors = "";
        Description = "";
        Goals = "";
        Difficulty = LearningElementDifficultyEnum.Medium;
        Workload = 0;
        PositionX = 0;
        PositionY = 0;
        ParentName = null;
    }
}