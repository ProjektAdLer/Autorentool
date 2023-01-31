using System.Runtime.Serialization;

namespace PersistEntities;

[Serializable]
[DataContract]
[KnownType(typeof(LearningSpaceLayoutPe))]
[KnownType(typeof(LearningElementPe))]
public class LearningSpaceLayoutPe :  ILearningSpaceLayoutPe, IExtensibleDataObject
{
    public LearningSpaceLayoutPe(ILearningElementPe?[]? learningElements, FloorPlanEnumPe? floorPlanName)
    {
        LearningElements = learningElements ?? Array.Empty<ILearningElementPe?>();
        FloorPlanName = floorPlanName ?? FloorPlanEnumPe.NoFloorPlan;
    }

    /// <summary>
    /// Constructor for serialization. DO NOT USE FOR NORMAL INITIALIZATION.
    /// </summary>
    private LearningSpaceLayoutPe()
    {
        LearningElements = Array.Empty<ILearningElementPe>();
        FloorPlanName = FloorPlanEnumPe.NoFloorPlan;
    }
    
    [DataMember]
    public FloorPlanEnumPe FloorPlanName { get; set; }
    [DataMember]
    public ILearningElementPe?[] LearningElements { get; set; }

    public IEnumerable<LearningElementPe> ContainedLearningElements => LearningElements.Where(x => x != null).Cast<LearningElementPe>()!;
    

    ExtensionDataObject? IExtensibleDataObject.ExtensionData { get; set; }

    [OnDeserializing]
    private void OnDeserializing(StreamingContext context)
    {
    }
}