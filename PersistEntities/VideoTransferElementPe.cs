namespace Generator.PersistEntities;

[Serializable]

public class VideoTransferElementPe : Generator.PersistEntities.LearningElementPe
{
    public VideoTransferElementPe(string name, string shortname,  string? parentName, Generator.PersistEntities.LearningContentPe? content,
        string authors, string description, string goals, Generator.PersistEntities.LearningElementDifficultyEnumPe difficulty, int workload,
        double positionX, double positionY) : base(name, shortname,parentName,content, authors, description, 
        goals,difficulty,workload, positionX,positionY)
    {
    }
    /// <summary>
    /// Constructor for serialization. DO NOT USE FOR NORMAL INITIALIZATION.
    /// </summary>
    private VideoTransferElementPe()
    {
    }
}