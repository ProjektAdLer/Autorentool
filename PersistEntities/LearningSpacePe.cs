using System.Runtime.Serialization;
using PersistEntities.LearningOutcome;
using Shared;

namespace PersistEntities;

[Serializable]
[DataContract]
[KnownType(typeof(PathWayConditionPe))]
[KnownType(typeof(LearningSpacePe))]
[KnownType(typeof(LearningSpaceLayoutPe))]
public class LearningSpacePe : ILearningSpacePe, IExtensibleDataObject
{
    public LearningSpacePe(string name, string description,
        int requiredPoints, Theme theme, List<ILearningOutcomePe>? learningOutcomes = null,
        ILearningSpaceLayoutPe? learningSpaceLayout = null, double positionX = 0, double positionY = 0,
        List<IObjectInPathWayPe>? inBoundObjects = null, List<IObjectInPathWayPe>? outBoundObjects = null,
        TopicPe? assignedTopic = null)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        LearningOutcomes = learningOutcomes ?? new List<ILearningOutcomePe>();
        RequiredPoints = requiredPoints;
        Theme = theme;
        LearningSpaceLayout = learningSpaceLayout ??
                              new LearningSpaceLayoutPe(new Dictionary<int, ILearningElementPe>(),
                                  FloorPlanEnum.R_20X20_6L);
        InBoundObjects = inBoundObjects ?? new List<IObjectInPathWayPe>();
        OutBoundObjects = outBoundObjects ?? new List<IObjectInPathWayPe>();
        AssignedTopic = assignedTopic;
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
        Description = "";
        LearningOutcomes = new List<ILearningOutcomePe>();
        RequiredPoints = 0;
        //overriding nullability as serializer must set value
        LearningSpaceLayout = null!;
        InBoundObjects = new List<IObjectInPathWayPe>();
        OutBoundObjects = new List<IObjectInPathWayPe>();
        AssignedTopic = null;
        PositionX = 0;
        PositionY = 0;
    }

    [DataMember] public Theme Theme { get; set; }

    [DataMember] public TopicPe? AssignedTopic { get; set; }

    ExtensionDataObject? IExtensibleDataObject.ExtensionData { get; set; }

    [IgnoreDataMember] public Guid Id { get; set; }

    [DataMember] public string Name { get; set; }

    [DataMember] public string Description { get; set; }

    [DataMember] public List<ILearningOutcomePe> LearningOutcomes { get; set; }

    [DataMember] public int RequiredPoints { get; set; }

    [DataMember] public ILearningSpaceLayoutPe LearningSpaceLayout { get; set; }

    [IgnoreDataMember] public List<IObjectInPathWayPe> InBoundObjects { get; set; }

    [IgnoreDataMember] public List<IObjectInPathWayPe> OutBoundObjects { get; set; }

    [DataMember] public double PositionX { get; set; }

    [DataMember] public double PositionY { get; set; }

    [OnDeserializing]
    private void OnDeserializing(StreamingContext context)
    {
        Id = Guid.NewGuid();
        InBoundObjects = new List<IObjectInPathWayPe>();
        OutBoundObjects = new List<IObjectInPathWayPe>();
    }
}