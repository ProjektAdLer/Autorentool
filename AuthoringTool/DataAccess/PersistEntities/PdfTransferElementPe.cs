namespace AuthoringTool.DataAccess.PersistEntities;

[Serializable]
public class PdfTransferElementPe : LearningElementPe
{
    internal PdfTransferElementPe(string name, string shortname, string? parentName, LearningContentPe? content,
        string authors, string description, string goals, LearningElementDifficultyEnumPe difficulty, int workload,
        double positionX, double positionY) : base(name, shortname, parentName, content, authors, description,
        goals, difficulty, workload, positionX, positionY)
    {
    }

    /// <summary>
    /// Constructor for serialization. DO NOT USE FOR NORMAL INITIALIZATION.
    /// </summary>
    private PdfTransferElementPe()
    {
    }
}