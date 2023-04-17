using System.Runtime.Serialization;

namespace PersistEntities;

[Serializable]
[DataContract]
[KnownType(typeof(LearningSpacePe))]
[KnownType(typeof(PathWayConditionPe))]
public class LearningWorldPe : ILearningWorldPe, IExtensibleDataObject
{
    public LearningWorldPe(string name, string shortname, string authors, string language, string description,
        string goals, List<LearningSpacePe>? learningSpaces = null, List<PathWayConditionPe>? pathWayConditions = null,
        List<LearningPathwayPe>? learningPathWays = null, List<TopicPe>? topics = null)
    {
        Id = Guid.NewGuid();
        Name = name;
        Shortname = shortname;
        Authors = authors;
        Language = language;
        Description = description;
        Goals = goals;
        LearningSpaces = learningSpaces ?? new List<LearningSpacePe>();
        PathWayConditions = pathWayConditions ?? new List<PathWayConditionPe>();
        LearningPathways = learningPathWays ?? new List<LearningPathwayPe>();
        UnplacedLearningElements = new List<ILearningElementPe>();
        Topics = topics ?? new List<TopicPe>();
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
        PathWayConditions = new List<PathWayConditionPe>();
        LearningPathways = new List<LearningPathwayPe>();
        UnplacedLearningElements = new List<ILearningElementPe>();
        Topics = new List<TopicPe>();
    }
    
    [IgnoreDataMember]
    public Guid Id { get; set; }

    [DataMember]
    public List<LearningSpacePe> LearningSpaces { get; set; }
    [DataMember]
    public List<LearningPathwayPe> LearningPathways { get; set; }
    [DataMember]
    public List<PathWayConditionPe> PathWayConditions { get; set; }
    [IgnoreDataMember]
    public List<IObjectInPathWayPe> ObjectsInPathWaysPe =>
        new List<IObjectInPathWayPe>(LearningSpaces).Concat(PathWayConditions).ToList();
[DataMember]
    public List<TopicPe> Topics { get; set; }
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
    [DataMember]
    public ICollection<ILearningElementPe> UnplacedLearningElements { get; set; }
    ExtensionDataObject? IExtensibleDataObject.ExtensionData { get; set; }

    [OnDeserialized]
    private void OnDeserialized(StreamingContext context)
    {
        Id = Guid.NewGuid();
        //rebuild InBound and OutBound on all spaces
        foreach (var learningPathwayPe in LearningPathways)
        {
            learningPathwayPe.SourceObject.OutBoundObjects.Add(learningPathwayPe.TargetObject);
            learningPathwayPe.TargetObject.InBoundObjects.Add(learningPathwayPe.SourceObject);
        }
    }
}