using AuthoringTool.PresentationLogic.LearningElement;

namespace AuthoringTool.DataAccess.PersistEntities;

[Serializable]

public class H5PInteractionElementPe : LearningElementPe
{
    internal H5PInteractionElementPe(string name, string shortname,  string? parentName, LearningContentPe? content,
        string authors, string description, string goals, LearningElementDifficultyEnum difficulty, int workload,
        double positionX, double positionY) : base(name, shortname,parentName,content, authors, description, 
        goals,difficulty,workload, positionX,positionY)
    {
    }
    /// <summary>
    /// Constructor for serialization. DO NOT USE FOR NORMAL INITIALIZATION.
    /// </summary>
    private H5PInteractionElementPe()
    {
    }
}