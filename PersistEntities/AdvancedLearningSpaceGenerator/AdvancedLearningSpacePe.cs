using System.Runtime.Serialization;
using Presentation.PresentationLogic.AdvancedLearningSpaceEditor;
using Shared;

namespace PersistEntities.AdvancedLearningSpaceGenerator;

[Serializable]
[DataContract]
[KnownType(typeof(AdvancedLearningSpacePe))]
[KnownType(typeof(LearningElementPe))]
public class AdvancedLearningSpacePe : IAdvancedLearningSpacePe, IExtensibleDataObject
{
    public AdvancedLearningSpacePe(string name, string description, string goals,
        int requiredPoints, Theme theme, IAdvancedLearningSpaceLayoutPe? advancedLearningSpaceLayout = null,
        double positionX = 0, double positionY = 0,
        List<IObjectInPathWayPe>? inBoundObjects = null, List<IObjectInPathWayPe>? outBoundObjects = null,
        TopicPe? assignedTopic = null)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        Goals = goals;
        RequiredPoints = requiredPoints;
        Theme = theme;
        LearningSpaceLayout = new LearningSpaceLayoutPe(new Dictionary<int, ILearningElementPe>(),
            FloorPlanEnum.R_20X20_6L);
        AdvancedLearningSpaceLayout = advancedLearningSpaceLayout ??
                                      new AdvancedLearningSpaceLayoutPe(new Dictionary<int, ILearningElementPe>(),
                                          new Dictionary<int, Coordinate>(),
                                          new Dictionary<int, Coordinate>(),
                                          new Dictionary<int, DoublePoint>(),
                                          new DoublePoint(), new DoublePoint());
        InBoundObjects = inBoundObjects ?? new List<IObjectInPathWayPe>();
        OutBoundObjects = outBoundObjects ?? new List<IObjectInPathWayPe>();
        AssignedTopic = assignedTopic;
        PositionX = positionX;
        PositionY = positionY;
    }

    /// <summary>
    /// Constructor for serialization. DO NOT USE FOR NORMAL INITIALIZATION.
    /// </summary>
    private AdvancedLearningSpacePe()
    {
        Id = Guid.Empty;
        Name = "";
        Description = "";
        Goals = "";
        //overriding nullability as serializer must set value
        LearningSpaceLayout = null!;
        RequiredPoints = 0;
        AdvancedLearningSpaceLayout = null!;
        InBoundObjects = new List<IObjectInPathWayPe>();
        OutBoundObjects = new List<IObjectInPathWayPe>();
        AssignedTopic = null;
        PositionX = 0;
        PositionY = 0;
    }

    [IgnoreDataMember] public Guid Id { get; set; }
    [DataMember] public string Name { get; set; }
    [DataMember] public string Description { get; set; }
    [DataMember] public string Goals { get; set; }
    [DataMember] public int RequiredPoints { get; set; }
    [DataMember] public Theme Theme { get; set; }
    [DataMember] public IAdvancedLearningSpaceLayoutPe AdvancedLearningSpaceLayout { get; set; }
    [DataMember] public ILearningSpaceLayoutPe LearningSpaceLayout { get; set; }
    [IgnoreDataMember] public List<IObjectInPathWayPe> InBoundObjects { get; set; }
    [IgnoreDataMember] public List<IObjectInPathWayPe> OutBoundObjects { get; set; }
    [DataMember] public TopicPe? AssignedTopic { get; set; }
    [DataMember] public double PositionX { get; set; }
    [DataMember] public double PositionY { get; set; }
    ExtensionDataObject? IExtensibleDataObject.ExtensionData { get; set; }

    [OnDeserializing]
    private void OnDeserializing(StreamingContext context)
    {
        Id = Guid.NewGuid();
        InBoundObjects = new List<IObjectInPathWayPe>();
        OutBoundObjects = new List<IObjectInPathWayPe>();
    }
}