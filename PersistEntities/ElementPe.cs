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
public class ElementPe : IElementPe, IExtensibleDataObject
{
    public ElementPe(string name, string shortname, ContentPe? content, string url,
        string authors, string description, string goals, ElementDifficultyEnumPe difficulty, int workload = 0,
        int points = 0, double positionX = 0, double positionY = 0)
    {
        Id = Guid.NewGuid();
        Name = name;
        Shortname = shortname;
        Content = content ?? new ContentPe();
        Url = url ?? "";
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
    internal ElementPe()
    {
        Id = Guid.Empty;
        Name = "";
        Shortname = "";
        Content = new ContentPe();
        Url = "";
        Authors = "";
        Description = "";
        Goals = "";
        Difficulty = ElementDifficultyEnumPe.Medium;
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
    public ContentPe Content { get; set; }
    [DataMember]
    public string Url { get; set; }
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
    public ElementDifficultyEnumPe Difficulty { get; set; }
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

