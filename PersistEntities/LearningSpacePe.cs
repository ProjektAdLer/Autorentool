﻿using System.Runtime.Serialization;
using PersistEntities.LearningOutcome;
using Shared;
using Shared.Theme;

namespace PersistEntities;

[Serializable]
[DataContract]
[KnownType(typeof(PathWayConditionPe))]
[KnownType(typeof(LearningSpacePe))]
[KnownType(typeof(LearningSpaceLayoutPe))]
public class LearningSpacePe : ILearningSpacePe, IExtensibleDataObject
{
    public LearningSpacePe(string name, string description,
        int requiredPoints, SpaceTheme spaceTheme, LearningOutcomeCollectionPe? learningOutcomes = null,
        ILearningSpaceLayoutPe? learningSpaceLayout = null, double positionX = 0, double positionY = 0,
        List<IObjectInPathWayPe>? inBoundObjects = null, List<IObjectInPathWayPe>? outBoundObjects = null,
        TopicPe? assignedTopic = null)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        LearningOutcomeCollection = learningOutcomes ?? new LearningOutcomeCollectionPe();
        RequiredPoints = requiredPoints;
        SpaceTheme = spaceTheme;
        LearningSpaceLayout = learningSpaceLayout ??
                              new LearningSpaceLayoutPe(new Dictionary<int, ILearningElementPe>(),
                                  new Dictionary<int, ILearningElementPe>(),
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
        LearningOutcomeCollection = new LearningOutcomeCollectionPe();
        RequiredPoints = 0;
        //overriding nullability as serializer must set value
        LearningSpaceLayout = null!;
        InBoundObjects = new List<IObjectInPathWayPe>();
        OutBoundObjects = new List<IObjectInPathWayPe>();
        AssignedTopic = null;
        PositionX = 0;
        PositionY = 0;
    }

    [DataMember] public SpaceTheme SpaceTheme { get; set; }

    [DataMember] public TopicPe? AssignedTopic { get; set; }

    [DataMember]
    [Obsolete(
        "The 'Goals' field is deprecated as of version 2.0.0 and has been replaced by 'LearningOutcomeCollection'. Use 'LearningOutcomeCollection' for new developments. 'Goals' is retained only for compatibility with LearningWorlds created in or before version 2.0.0.")]
    public string? Goals { get; set; }

    ExtensionDataObject? IExtensibleDataObject.ExtensionData { get; set; }

    [IgnoreDataMember] public Guid Id { get; set; }

    [DataMember] public string Name { get; set; }

    [DataMember] public string Description { get; set; }

    [DataMember] public LearningOutcomeCollectionPe LearningOutcomeCollection { get; set; }

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