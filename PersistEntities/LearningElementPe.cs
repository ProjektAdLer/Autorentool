using System.Runtime.Serialization;
using PersistEntities.LearningContent;

namespace PersistEntities;

[Serializable]
[DataContract]
[KnownType(typeof(FileContentPe))]
[KnownType(typeof(LinkContentPe))]
public class LearningElementPe : ILearningElementPe, IExtensibleDataObject
{
    public LearningElementPe(string name, ILearningContentPe? learningContent,
        string description, string goals, LearningElementDifficultyEnumPe difficulty, int workload = 0,
        int points = 0, double positionX = 0, double positionY = 0)
    {
        Id = Guid.NewGuid();
        Name = name;
        LearningContent = learningContent;
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
        // Overridden because private automapper/serialization constructor - n.stich
        LearningContent = null!;
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
    public ILearningContentPe LearningContent { get; set; }
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

