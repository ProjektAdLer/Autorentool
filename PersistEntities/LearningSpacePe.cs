using System.Runtime.Serialization;

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
public class LearningSpacePe : ILearningSpacePe, IExtensibleDataObject
{
    public LearningSpacePe(string name, string shortname, string authors, string description, string goals,
        int requiredPoints, List<LearningElementPe>? learningElements = null, double positionX = 0, double positionY = 0,
        List<LearningSpacePe>? inBoundSpaces = null, List<LearningSpacePe>? outBoundSpaces = null)
    {
        Name = name;
        Shortname = shortname;
        Authors = authors;
        Description = description;
        Goals = goals;
        RequiredPoints = requiredPoints;
        LearningElements = learningElements ?? new List<LearningElementPe>();
        InBoundSpaces = inBoundSpaces ?? new List<LearningSpacePe>();
        OutBoundSpaces = outBoundSpaces ?? new List<LearningSpacePe>();
        PositionX = positionX;
        PositionY = positionY;
    }

    /// <summary>
    /// Constructor for serialization. DO NOT USE FOR NORMAL INITIALIZATION.
    /// </summary>
    private LearningSpacePe()
    {
        Id = Guid.Empty;
        Name = "";
        Shortname = "";
        Authors = "";
        Description = "";
        Goals = "";
        RequiredPoints = 0;
        LearningElements = new List<LearningElementPe>();
        InBoundSpaces = new List<LearningSpacePe>();
        OutBoundSpaces = new List<LearningSpacePe>();
        PositionX = 0;
        PositionY = 0;
    }

    [DataMember]
    public Guid Id { get; set; }
    [DataMember]
    public string Name { get; set; }
    [DataMember]
    public string Description { get; set; }
    [DataMember]
    public string Shortname { get; set; }
    [DataMember]
    public string Authors { get; set; }
    [DataMember]
    public string Goals { get; set; }
    [DataMember]
    public int RequiredPoints { get; set; }
    [DataMember]
    public List<LearningElementPe> LearningElements { get; set; }
    [IgnoreDataMember]
    public List<LearningSpacePe> InBoundSpaces { get; set; }
    [IgnoreDataMember]
    public List<LearningSpacePe> OutBoundSpaces { get; set; }
    [DataMember]
    public double PositionX { get; set; }
    [DataMember]
    public double PositionY { get; set; }
    public ExtensionDataObject? ExtensionData { get; set; }

    [OnDeserializing]
    private void OnDeserializing(StreamingContext context)
    {
        InBoundSpaces = new List<LearningSpacePe>();
        OutBoundSpaces = new List<LearningSpacePe>();
    }
}