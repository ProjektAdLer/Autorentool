using System.Runtime.Serialization;

namespace PersistEntities;

[Serializable]
[DataContract]
[KnownType(typeof(SpacePe))]
[KnownType(typeof(PathWayConditionPe))]
public class WorldPe : IWorldPe, IExtensibleDataObject
{
    public WorldPe(string name, string shortname, string authors, string language, string description,
        string goals, List<SpacePe>? spaces = null, List<PathWayConditionPe>? pathWayConditions = null, List<PathwayPe>? pathWays = null)
    {
        Id = Guid.NewGuid();
        Name = name;
        Shortname = shortname;
        Authors = authors;
        Language = language;
        Description = description;
        Goals = goals;
        Spaces = spaces ?? new List<SpacePe>();
        PathWayConditions = pathWayConditions ?? new List<PathWayConditionPe>();
        Pathways = pathWays ?? new List<PathwayPe>();
    }

    /// <summary>
    /// Constructor for serialization. DO NOT USE FOR NORMAL INITIALIZATION.
    /// </summary>
    private WorldPe()
    {
        Id = Guid.Empty;
        Name = "";
        Shortname = "";
        Authors = "";
        Language = "";
        Description = "";
        Goals = "";
        Spaces = new List<SpacePe>();
        PathWayConditions = new List<PathWayConditionPe>();
        Pathways = new List<PathwayPe>();
    }
    
    [IgnoreDataMember]
    public Guid Id { get; set; }

    [DataMember]
    public List<SpacePe> Spaces { get; set; }
    [DataMember]
    public List<PathwayPe> Pathways { get; set; }
    [DataMember]
    public List<PathWayConditionPe> PathWayConditions { get; set; }
    [IgnoreDataMember]
    public List<IObjectInPathWayPe> ObjectsInPathWaysPe => new List<IObjectInPathWayPe>(Spaces).Concat(PathWayConditions).ToList();
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
        foreach (var pathwayPe in Pathways)
        {
            pathwayPe.SourceObject.OutBoundObjects.Add(pathwayPe.TargetObject);
            pathwayPe.TargetObject.InBoundObjects.Add(pathwayPe.SourceObject);
        }
    }
    
}