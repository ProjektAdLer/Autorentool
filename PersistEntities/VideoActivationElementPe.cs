namespace PersistEntities;

[Serializable]

public class VideoActivationElementPe : LearningElementPe
{
    public VideoActivationElementPe(string name, string shortname,  string? parentName, LearningContentPe? content,
        string authors, string description, string goals, LearningElementDifficultyEnumPe difficulty, int workload,
        double positionX, double positionY) : base(name, shortname,parentName,content, authors, description, 
        goals,difficulty,workload, positionX,positionY)
    {
    }
    /// <summary>
    /// Constructor for serialization. DO NOT USE FOR NORMAL INITIALIZATION.
    /// </summary>
    private VideoActivationElementPe()
    {
    }
}