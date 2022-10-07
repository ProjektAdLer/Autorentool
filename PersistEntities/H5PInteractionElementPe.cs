
namespace PersistEntities;

[Serializable]

public class H5PInteractionElementPe : LearningElementPe
{
    public H5PInteractionElementPe(string name, string shortname, LearningContentPe? learningContent, string? url,
        string authors, string description, string goals, LearningElementDifficultyEnumPe difficulty, int workload,
        int points, double positionX, double positionY) : base(name, shortname, learningContent, url, authors, description, 
        goals, difficulty, workload, points, positionX,positionY)
    {
    }
    /// <summary>
    /// Constructor for serialization. DO NOT USE FOR NORMAL INITIALIZATION.
    /// </summary>
    private H5PInteractionElementPe()
    {
    }
}