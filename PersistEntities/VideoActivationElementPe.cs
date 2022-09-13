namespace PersistEntities;

[Serializable]

public class VideoActivationElementPe : LearningElementPe
{
    internal VideoActivationElementPe(string name, string shortname, LearningContentPe? learningContent,
        string authors, string description, string goals, LearningElementDifficultyEnumPe difficulty, int workload,
        int points, double positionX, double positionY) : base(name, shortname, learningContent, authors, description, 
        goals, difficulty, workload, points, positionX, positionY)
    {
    }
    /// <summary>
    /// Constructor for serialization. DO NOT USE FOR NORMAL INITIALIZATION.
    /// </summary>
    private VideoActivationElementPe()
    {
    }
}