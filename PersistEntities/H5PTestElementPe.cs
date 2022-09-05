
namespace PersistEntities;

[Serializable]

public class H5PTestElementPe : LearningElementPe
{
    internal H5PTestElementPe(string name, string shortname, LearningContentPe? learningContent,
        string authors, string description, string goals, LearningElementDifficultyEnumPe difficulty, int workload,
        double positionX, double positionY) : base(name, shortname,learningContent, authors, description, 
        goals,difficulty,workload, positionX,positionY)
    {
    }
    /// <summary>
    /// Constructor for serialization. DO NOT USE FOR NORMAL INITIALIZATION.
    /// </summary>
    private H5PTestElementPe()
    {
    }
}