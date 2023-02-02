using System.Runtime.Serialization;

namespace PersistEntities;

[Serializable]
[DataContract]
public class TextTransferElementPe : ElementPe
{
    internal TextTransferElementPe(string name, string shortname, ContentPe? content, string url,
        string authors, string description, string goals, ElementDifficultyEnumPe difficulty, int workload,
        int points, double positionX, double positionY) : base(name, shortname,content, url, authors, description, 
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