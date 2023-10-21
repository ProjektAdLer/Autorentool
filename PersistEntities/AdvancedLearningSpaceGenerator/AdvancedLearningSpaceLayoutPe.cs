using System.Runtime.Serialization;
using Presentation.PresentationLogic.AdvancedLearningSpaceEditor;
using Shared;

namespace PersistEntities.AdvancedLearningSpaceGenerator;

[Serializable]
[DataContract]
[KnownType(typeof(AdvancedLearningSpaceLayoutPe))]
[KnownType(typeof(LearningElementPe))]
public class AdvancedLearningSpaceLayoutPe : IAdvancedLearningSpaceLayoutPe, IExtensibleDataObject
{
    public AdvancedLearningSpaceLayoutPe(IDictionary<int, ILearningElementPe> learningElements, IDictionary<int, IAdvancedLearningElementSlotPe> advancedLearningElementSlots,
        IDictionary<int, IAdvancedDecorationPe> advancedDecorations, IDictionary<int, DoublePoint> advancedCornerPoints, DoublePoint entryDoorPosition, DoublePoint exitDoorPosition)
    {
        LearningElements = learningElements;
        AdvancedLearningElementSlots = advancedLearningElementSlots;
        AdvancedDecorations = advancedDecorations;
        AdvancedCornerPoints = advancedCornerPoints;
        EntryDoorPosition = entryDoorPosition;
        ExitDoorPosition = exitDoorPosition;
    }

    /// <summary>
    /// Constructor for serialization. DO NOT USE FOR NORMAL INITIALIZATION.
    /// </summary>
    private AdvancedLearningSpaceLayoutPe()
    {
        LearningElements = new Dictionary<int, ILearningElementPe>();
        AdvancedLearningElementSlots = new Dictionary<int, IAdvancedLearningElementSlotPe>();
        AdvancedDecorations = new Dictionary<int, IAdvancedDecorationPe>();
        AdvancedCornerPoints = new Dictionary<int, DoublePoint>();
        EntryDoorPosition = new DoublePoint();
        ExitDoorPosition = new DoublePoint();
    }
    

    [DataMember]
    public IDictionary<int, ILearningElementPe> LearningElements { get; set; }
    [DataMember]
    public IDictionary<int, IAdvancedLearningElementSlotPe> AdvancedLearningElementSlots { get; set; }
    [DataMember]
    public IDictionary<int, IAdvancedDecorationPe> AdvancedDecorations { get; set; }
    [DataMember]
    public IDictionary<int, DoublePoint> AdvancedCornerPoints { get; set; }
    [DataMember]
    public DoublePoint EntryDoorPosition { get; set; }
    [DataMember]
    public DoublePoint ExitDoorPosition { get; set; }

    public IEnumerable<ILearningElementPe> ContainedLearningElements => LearningElements.Values;
    public IEnumerable<IAdvancedLearningElementSlotPe> ContainedAdvancedLearningElementSlots => AdvancedLearningElementSlots.Values;
    public IEnumerable<IAdvancedDecorationPe> ContainedAdvancedDecorations => AdvancedDecorations.Values;
    public IEnumerable<DoublePoint> ContainedAdvancedCornerPoints => AdvancedCornerPoints.Values;
    

    ExtensionDataObject? IExtensibleDataObject.ExtensionData { get; set; }

    [OnDeserializing]
    private void OnDeserializing(StreamingContext context)
    {
    }
    
}