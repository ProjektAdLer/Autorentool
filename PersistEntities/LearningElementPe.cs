using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace PersistEntities;

[XmlInclude(typeof(ImageTransferElementPe))]
[XmlInclude(typeof(VideoTransferElementPe))]
[XmlInclude(typeof(PdfTransferElementPe))]
[XmlInclude(typeof(TextTransferElementPe))]
[XmlInclude(typeof(VideoActivationElementPe))]
[XmlInclude(typeof(H5PActivationElementPe))]
[XmlInclude(typeof(H5PInteractionElementPe))]
[XmlInclude(typeof(H5PTestElementPe))]
[Serializable]
public class LearningElementPe : ILearningElementPe, IExtensibleDataObject
{
    public LearningElementPe(string name, string shortname, LearningContentPe? learningContent,
        string authors, string description, string goals, LearningElementDifficultyEnumPe difficulty, int workload = 0,
        int points = 0, double positionX = 0, double positionY = 0)
    {
        Name = name;
        Shortname = shortname;
        LearningContent = learningContent ?? new LearningContentPe();
        Authors = authors;
        Description = description;
        Goals = goals;
        Difficulty = difficulty;
        Workload = workload;
        Points = points;
        PositionX = positionX;
        PositionY = positionY;
    }
    /// <summary>
    /// Constructor for serialization. DO NOT USE FOR NORMAL INITIALIZATION.
    /// </summary>
    internal LearningElementPe()
    {
        Name = "";
        Shortname = "";
        LearningContent = new LearningContentPe();
        Authors = "";
        Description = "";
        Goals = "";
        Difficulty = LearningElementDifficultyEnumPe.Medium;
        Workload = 0;
        Points = 0;
        PositionX = 0;
        PositionY = 0;
    }


    public string Name { get; set; }
    public string Shortname { get; set; }
    public LearningContentPe LearningContent { get; set; }
    public string Authors { get; set; }
    public string Description { get; set; }
    public string Goals { get; set; }
    public int Workload { get; set; }
    public int Points { get; set; }
    public LearningElementDifficultyEnumPe Difficulty { get; set; }
    public double PositionX { get; set; }
    public double PositionY { get; set; }
    public ExtensionDataObject? ExtensionData { get; set; }
}

