using System.Runtime.Serialization;

namespace PersistEntities;

[Serializable]
[DataContract]
public class PdfTransferElementPe : LearningElementPe
{
    public PdfTransferElementPe(string name, string shortname, LearningContentPe? learningContent, string url,
        string authors, string description, string goals, LearningElementDifficultyEnumPe difficulty, int workload,
        int points, double positionX, double positionY) : base(name, shortname, learningContent, url, authors, description,
        goals, difficulty, workload, points, positionX, positionY)
    {
    }

    /// <summary>
    /// Constructor for serialization. DO NOT USE FOR NORMAL INITIALIZATION.
    /// </summary>
    private PdfTransferElementPe()
    {
    }
}