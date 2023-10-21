using System.Runtime.Serialization;

namespace PersistEntities.AdvancedLearningSpaceGenerator;

[Serializable]
[DataContract]
[KnownType(typeof(AdvancedDecorationPe))]
[KnownType(typeof(LearningElementPe))]
public class AdvancedDecorationPe : IAdvancedDecorationPe, IExtensibleDataObject
{
    public AdvancedDecorationPe(Guid spaceId, int decorationKey, double positionX, double positionY, double rotation)
    {
        SpaceId = spaceId;
        DecorationKey = decorationKey;
        PositionX = positionX;
        PositionY = positionY;
        Rotation = rotation;
    }
    /// <summary>
    /// Constructor for serialization. DO NOT USE FOR NORMAL INITIALIZATION.
    /// </summary>
    private AdvancedDecorationPe()
    {
        SpaceId = Guid.Empty;
        DecorationKey = 0;
        PositionX = 0;
        PositionY = 0;
        Rotation = 0;
    }
    [DataMember]
    public Guid SpaceId { get; set; }
    [DataMember]
    public int DecorationKey { get; set; }
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