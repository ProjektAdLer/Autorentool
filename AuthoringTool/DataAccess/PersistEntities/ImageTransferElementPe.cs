using AuthoringTool.PresentationLogic.LearningElement;

namespace AuthoringTool.DataAccess.PersistEntities;

[Serializable]

public class ImageTransferElement : LearningElementPe
{
    internal ImageTransferElement(string name, string shortname,  string? parentName, LearningContentPe? content,
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
        Content = new LearningContentPe();
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