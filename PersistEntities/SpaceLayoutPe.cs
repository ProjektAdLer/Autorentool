using System.Runtime.Serialization;

namespace PersistEntities;

[Serializable]
[DataContract]
[KnownType(typeof(SpaceLayoutPe))]
[KnownType(typeof(ElementPe))]
public class SpaceLayoutPe :  ISpaceLayoutPe, IExtensibleDataObject
{
    public SpaceLayoutPe(IElementPe?[]? elements, FloorPlanEnumPe? floorPlanName)
    {
        Elements = elements ?? Array.Empty<IElementPe?>();
        FloorPlanName = floorPlanName ?? FloorPlanEnumPe.NoFloorPlan;
    }

    /// <summary>
    /// Constructor for serialization. DO NOT USE FOR NORMAL INITIALIZATION.
    /// </summary>
    private SpaceLayoutPe()
    {
        Elements = Array.Empty<IElementPe>();
        FloorPlanName = FloorPlanEnumPe.NoFloorPlan;
    }
    
    [DataMember]
    public FloorPlanEnumPe FloorPlanName { get; set; }
    [DataMember]
    public IElementPe?[] Elements { get; set; }

    public IEnumerable<ElementPe> ContainedElements => Elements.Where(x => x != null).Cast<ElementPe>()!;
    

    ExtensionDataObject? IExtensibleDataObject.ExtensionData { get; set; }

    [OnDeserializing]
    private void OnDeserializing(StreamingContext context)
    {
    }
}