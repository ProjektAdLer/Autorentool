using System.Runtime.Serialization;
using PersistEntities.LearningOutcome;

namespace PersistEntities;

[Serializable]
[DataContract]
[KnownType(typeof(LearningSpacePe))]
[KnownType(typeof(PathWayConditionPe))]
public class LearningWorldPe : ILearningWorldPe, IExtensibleDataObject
{
    public LearningWorldPe(string name, string shortname, string authors, string language, string description,
        string goals, string evaluationLink, string enrolmentKey, string savePath,
        List<LearningSpacePe>? learningSpaces = null,
        List<PathWayConditionPe>? pathWayConditions = null,
        List<LearningPathwayPe>? learningPathWays = null, List<TopicPe>? topics = null)
    {
        Id = Guid.NewGuid();
        Name = name;
        Shortname = shortname;
        Authors = authors;
        Language = language;
        Description = description;
        Goals = goals;
        EvaluationLink = evaluationLink;
        EnrolmentKey = enrolmentKey;
        SavePath = savePath;
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
        EvaluationLink = "";
        EnrolmentKey = "";
        SavePath = "";
        LearningSpaces = new List<LearningSpacePe>();
        PathWayConditions = new List<PathWayConditionPe>();
        LearningPathways = new List<LearningPathwayPe>();
        UnplacedLearningElements = new List<ILearningElementPe>();
        Topics = new List<TopicPe>();
    }

    [IgnoreDataMember] public Guid Id { get; set; }

    [DataMember] public List<LearningPathwayPe> LearningPathways { get; set; }

    [DataMember] public List<PathWayConditionPe> PathWayConditions { get; set; }

    [IgnoreDataMember]
    public List<IObjectInPathWayPe> ObjectsInPathWaysPe =>
        new List<IObjectInPathWayPe>(LearningSpaces).Concat(PathWayConditions).ToList();

    [DataMember] public List<TopicPe> Topics { get; set; }

    ExtensionDataObject? IExtensibleDataObject.ExtensionData { get; set; }

    [DataMember] public List<LearningSpacePe> LearningSpaces { get; set; }

    [DataMember] public string Name { get; set; }

    [DataMember] public string Shortname { get; set; }

    [DataMember] public string Authors { get; set; }

    [DataMember] public string Language { get; set; }

    [DataMember] public string Description { get; set; }

    [DataMember] public string Goals { get; set; }

    [DataMember] public string EvaluationLink { get; set; }

    [DataMember] public string EnrolmentKey { get; set; }

    [DataMember] public string SavePath { get; set; }

    [DataMember] public ICollection<ILearningElementPe> UnplacedLearningElements { get; set; }

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

        //LearningWorlds created in or before Version 2.0.0 have Goals instead of LearningOutcomeCollection
        //To ensure compatibility, we convert Goals to ManualLearningOutcomePe and add them to LearningOutcomeCollection - m.ho
#pragma warning disable
        if (!LearningSpaces.Any(x => x.LearningOutcomeCollection == null)) return;
        foreach (var space in LearningSpaces)
        {
            space.LearningOutcomeCollection = new LearningOutcomeCollectionPe();
            if (!string.IsNullOrEmpty(space.Goals))
                space.LearningOutcomeCollection.LearningOutcomes.Add(new ManualLearningOutcomePe(space.Goals));
        }
    }
#pragma warning restore
}