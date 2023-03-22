using System.Runtime.Serialization;
using PersistEntities.LearningContent;

namespace PersistEntities;

[Serializable]
[DataContract]
public class TextTransferElementPe : LearningElementPe
{
    internal TextTransferElementPe(string name, string shortname, ILearningContentPe? learningContent,
        string authors, string description, string goals, LearningElementDifficultyEnumPe difficulty, int workload,
        int points, double positionX, double positionY) : base(name, shortname, learningContent, authors, description, 
        goals, difficulty, workload, points, positionX,positionY)
    {
    }
    /// <summary>
    /// Constructor for serialization. DO NOT USE FOR NORMAL INITIALIZATION.
    /// </summary>
    private TextTransferElementPe()
    {
    }
}