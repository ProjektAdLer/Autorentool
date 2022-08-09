using System.Xml.Serialization;
using AuthoringTool.PresentationLogic.LearningElement;

namespace AuthoringTool.DataAccess.PersistEntities;

[XmlInclude(typeof(ImageTransferElementPe))]
[XmlInclude(typeof(VideoTransferElementPe))]
[XmlInclude(typeof(PdfTransferElementPe))]
[XmlInclude(typeof(VideoActivationElementPe))]
[XmlInclude(typeof(H5PActivationElementPe))]
[XmlInclude(typeof(H5PInteractionElementPe))]
[XmlInclude(typeof(H5PTestElementPe))]
[Serializable]
public class LearningElementPe : ILearningElementPe
{
    internal LearningElementPe(string name, string shortname,  string? parentName, LearningContentPe? content,
        string authors, string description, string goals, LearningElementDifficultyEnum difficulty, int workload = 0,
        double positionX = 0, double positionY = 0)
    {
        Name = name;
        Shortname = shortname;
        Content = content ?? new LearningContentPe();
        Authors = authors;
        Description = description;
        Goals = goals;
        Difficulty = difficulty;
        Workload = workload;
        PositionX = positionX;
        PositionY = positionY;
        ParentName = parentName;
    }
    /// <summary>
    /// Constructor for serialization. DO NOT USE FOR NORMAL INITIALIZATION.
    /// </summary>
    internal LearningElementPe()
    {
        Name = "";
        Shortname = "";
        Content = new LearningContentPe();
        Authors = "";
        Description = "";
        Goals = "";
        Difficulty = LearningElementDifficultyEnum.Medium;
        Workload = 0;
        PositionX = 0;
        PositionY = 0;
        ParentName = null;
    }


    public string Name { get; set; }
    public string Shortname { get; set; }
    public LearningContentPe Content { get; set; }
    public string Authors { get; set; }
    public string Description { get; set; }
    public string Goals { get; set; }
    public int Workload { get; set; }
    public LearningElementDifficultyEnum Difficulty { get; set; }
    public double PositionX { get; set; }
    public double PositionY { get; set; }
    public string? ParentName { get; set; }
}

