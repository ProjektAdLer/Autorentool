using System.Runtime.Serialization;
using PersistEntities.LearningContent;

namespace PersistEntities;

[Serializable]
[DataContract]
[KnownType(typeof(H5PActivationElementPe))]
[KnownType(typeof(H5PInteractionElementPe))]
[KnownType(typeof(H5PTestElementPe))]
[KnownType(typeof(ImageTransferElementPe))]
[KnownType(typeof(PdfTransferElementPe))]
[KnownType(typeof(TextTransferElementPe))]
[KnownType(typeof(VideoActivationElementPe))]
[KnownType(typeof(VideoTransferElementPe))]
public class LearningElementPe : ILearningElementPe, IExtensibleDataObject
{
    public LearningElementPe(string name, string shortname, LearningContentPe? learningContent,
        string authors, string description, string goals, LearningElementDifficultyEnumPe difficulty, int workload = 0,
        int points = 0, double positionX = 0, double positionY = 0)
    {
        Id = Guid.NewGuid();
        Name = name;
        Shortname = shortname;
        LearningContent = learningContent;
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
    protected LearningElementPe()
    {
        Id = Guid.Empty;
        Name = "";
        Shortname = "";
        // Overridden because private automapper/serialization constructor - n.stich
        LearningContent = null!;
        Authors = "";
        Description = "";
        Goals = "";
        Difficulty = LearningElementDifficultyEnumPe.Medium;
        Workload = 0;
        Points = 0;
        PositionX = 0;
        PositionY = 0;
    }
    [IgnoreDataMember]
    public Guid Id { get; set; }
    [DataMember]
    public string Name { get; set; }
    [DataMember]
    public string Shortname { get; set; }
    [DataMember]
    public LearningContentPe LearningContent { get; set; }
    [DataMember]
    public string Authors { get; set; }
    [DataMember]
    public string Description { get; set; }
    [DataMember]
    public string Goals { get; set; }
    [DataMember]
    public int Workload { get; set; }
    [DataMember]
    public int Points { get; set; }
    [DataMember]
    public LearningElementDifficultyEnumPe Difficulty { get; set; }
    [DataMember]
    public double PositionX { get; set; }
    [DataMember]
    public double PositionY { get; set; }
    ExtensionDataObject? IExtensibleDataObject.ExtensionData { get; set; }

    [OnDeserializing]
    private void OnDeserializing(StreamingContext context)
    {
        Id = Guid.NewGuid();
    }
}

