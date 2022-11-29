using System.Runtime.Serialization;

namespace PersistEntities;

[Serializable]
[DataContract]
public class LearningWorldPe : ILearningWorldPe, IExtensibleDataObject
{
    public LearningWorldPe(string name, string shortname, string authors, string language, string description,
        string goals, List<LearningSpacePe>? learningSpaces = null,  List<LearningPathwayPe>? learningPathWays = null)
    {
        Id = Guid.NewGuid();
        Name = name;
        Shortname = shortname;
        Authors = authors;
        Language = language;
        Description = description;
        Goals = goals;
        LearningSpaces = learningSpaces ?? new List<LearningSpacePe>();
        LearningPathways = learningPathWays ?? new List<LearningPathwayPe>();
    }

    /// <summary>
    /// Constructor for serialization. DO NOT USE FOR NORMAL INITIALIZATION.
    /// </summary>
    private LearningWorldPe()
    {
        Id = Guid.Empty;
        Name = "";
        Shortname = "";
        Authors = "";
        Language = "";
        Description = "";
        Goals = "";
        LearningSpaces = new List<LearningSpacePe>();
        LearningPathways = new List<LearningPathwayPe>();
    }
    
    [IgnoreDataMember]
    public Guid Id { get; set; }

    [DataMember]
    public List<LearningSpacePe> LearningSpaces { get; set; }
    [DataMember]
    public List<LearningPathwayPe> LearningPathways { get; set; }
    [DataMember]
    public string Name { get; set; }
    [DataMember]
    public string Shortname { get; set; }
    [DataMember]
    public string Authors { get; set; }
    [DataMember]
    public string Language { get; set; }
    [DataMember]
    public string Description { get; set; }
    [DataMember]
    public string Goals { get; set; }
    ExtensionDataObject? IExtensibleDataObject.ExtensionData { get; set; }

    [OnDeserialized]
    private void OnDeserialized(StreamingContext context)
    {
        Id = Guid.NewGuid();
        //rebuild InBound and OutBound on all spaces
        foreach (var learningPathwayPe in LearningPathways)
        {
            learningPathwayPe.SourceSpace.OutBoundSpaces.Add(learningPathwayPe.TargetSpace);
            learningPathwayPe.TargetSpace.InBoundSpaces.Add(learningPathwayPe.SourceSpace);
        }
    }
    
}