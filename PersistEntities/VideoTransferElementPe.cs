namespace PersistEntities;

[Serializable]

public class VideoTransferElementPe : LearningElementPe
{
    public VideoTransferElementPe(string name, string shortname, LearningContentPe? learningContent, string url,
        string authors, string description, string goals, LearningElementDifficultyEnumPe difficulty, int workload,
        int points, double positionX, double positionY) : base(name, shortname,learningContent, url, authors, description, 
        goals, difficulty, workload, points, positionX,positionY)
    {
    }
    /// <summary>
    /// Constructor for serialization. DO NOT USE FOR NORMAL INITIALIZATION.
    /// </summary>
    private VideoTransferElementPe()
    {
    }
}