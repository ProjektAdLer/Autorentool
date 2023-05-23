using System.Runtime.Serialization;
using Shared;

namespace PersistEntities;

[Serializable]
[DataContract]
[KnownType(typeof(LearningSpaceLayoutPe))]
[KnownType(typeof(LearningElementPe))]
public class LearningSpaceLayoutPe :  ILearningSpaceLayoutPe, IExtensibleDataObject
{
    public LearningSpaceLayoutPe(IDictionary<int, ILearningElementPe> learningElements, FloorPlanEnum floorPlanName)
    {
        LearningElements = learningElements;
        FloorPlanName = floorPlanName;
    }

    /// <summary>
    /// Constructor for serialization. DO NOT USE FOR NORMAL INITIALIZATION.
    /// </summary>
    private LearningSpaceLayoutPe()
    {
        LearningElements = new Dictionary<int, ILearningElementPe>();
    }
    
    [DataMember]
    public FloorPlanEnum FloorPlanName { get; set; }
    [DataMember]
    public IDictionary<int, ILearningElementPe> LearningElements { get; set; }

    public IEnumerable<ILearningElementPe> ContainedLearningElements => LearningElements.Values;
    
    public int Capacity { get; set; }
    

    ExtensionDataObject? IExtensibleDataObject.ExtensionData { get; set; }

    [OnDeserializing]
    private void OnDeserializing(StreamingContext context)
    {
    }
}