using System.Runtime.Serialization;

namespace PersistEntities.AdvancedLearningSpaceGenerator;

[Serializable]
[DataContract]
[KnownType(typeof(AdvancedLearningElementSlotPe))]
[KnownType(typeof(LearningElementPe))]
public class AdvancedLearningElementSlotPe : IAdvancedLearningElementSlotPe, IExtensibleDataObject
{
    public AdvancedLearningElementSlotPe(Guid spaceId, int slotKey, double positionX, double positionY, double rotation)
    {
        SpaceId = spaceId;
        SlotKey = slotKey;
        PositionX = positionX;
        PositionY = positionY;
        Rotation = rotation;
    }

    /// <summary>
    /// Constructor for serialization. DO NOT USE FOR NORMAL INITIALIZATION.
    /// </summary>
    private AdvancedLearningElementSlotPe()
    {
        SpaceId = Guid.Empty;
        SlotKey = 0;
        PositionX = 0;
        PositionY = 0;
        Rotation = 0;
    }
    [DataMember]
    public Guid SpaceId { get; set; }
    [DataMember]
    public int SlotKey { get; set; }
    [DataMember]
    public double PositionX { get; set; }
    [DataMember]
    public double PositionY { get; set; }
    [DataMember]
    public double Rotation { get; set; }
    
    
    ExtensionDataObject? IExtensibleDataObject.ExtensionData { get; set; }

    [OnDeserializing]
    private void OnDeserializing(StreamingContext context)
    {
    }
}