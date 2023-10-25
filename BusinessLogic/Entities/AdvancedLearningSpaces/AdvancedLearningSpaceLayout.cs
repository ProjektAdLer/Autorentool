using JetBrains.Annotations;
using Shared;

namespace BusinessLogic.Entities.AdvancedLearningSpaces;

public class AdvancedLearningSpaceLayout : IAdvancedLearningSpaceLayout
{
    /// <summary>
    /// Constructor for Automapper. DO NOT USE.
    /// </summary>
    [UsedImplicitly]
    private AdvancedLearningSpaceLayout()
    {
        LearningElements = new Dictionary<int, ILearningElement>();
        AdvancedLearningElementSlots = new Dictionary<int, Coordinate>();
        AdvancedDecorations = new Dictionary<int, Coordinate>();
        AdvancedCornerPoints = new Dictionary<int, DoublePoint>();
        EntryDoorPosition = new DoublePoint();
        ExitDoorPosition = new DoublePoint();
    }

    public AdvancedLearningSpaceLayout(IDictionary<int, ILearningElement>? learningElements = null,
        IDictionary<int, Coordinate>? advancedLearningElementSlots = null,
        IDictionary<int, Coordinate>? advancedDecorations = null,
        IDictionary<int, DoublePoint>? advancedCornerPoints = null,
        DoublePoint? entryDoorPosition = null,
        DoublePoint? exitDoorPosition = null)
    {
        LearningElements = learningElements ?? new Dictionary<int, ILearningElement>();
        AdvancedLearningElementSlots = advancedLearningElementSlots ?? new Dictionary<int, Coordinate>();
        AdvancedDecorations = advancedDecorations ?? new Dictionary<int, Coordinate>();
        AdvancedCornerPoints = advancedCornerPoints ?? new Dictionary<int, DoublePoint>();
        EntryDoorPosition = entryDoorPosition ?? new DoublePoint();
        ExitDoorPosition = exitDoorPosition ?? new DoublePoint();
    }

    public IDictionary<int, ILearningElement> LearningElements { get; set; }
    public IDictionary<int, Coordinate> AdvancedLearningElementSlots { get; set; }
    public IDictionary<int, Coordinate> AdvancedDecorations { get; set; }
    public IDictionary<int, DoublePoint> AdvancedCornerPoints { get; set; }
    public DoublePoint EntryDoorPosition { get; set; }
    public DoublePoint ExitDoorPosition { get; set; }
    

    public IEnumerable<ILearningElement> ContainedLearningElements => LearningElements.Values;


    public IMemento GetMemento()
    {
        return new AdvancedLearningSpaceLayoutMemento(LearningElements, AdvancedLearningElementSlots, AdvancedDecorations, AdvancedCornerPoints);
    }

    public void RestoreMemento(IMemento memento)
    {
        if (memento is not AdvancedLearningSpaceLayoutMemento advancedLearningSpaceLayoutMemento)
        {
            throw new ArgumentException("Incorrect IMemento implementation", nameof(memento));
        }

        LearningElements = advancedLearningSpaceLayoutMemento.LearningElements;
        AdvancedLearningElementSlots = advancedLearningSpaceLayoutMemento.AdvancedLearningElementSlots;
        AdvancedDecorations = advancedLearningSpaceLayoutMemento.AdvancedDecorations;
        AdvancedCornerPoints = advancedLearningSpaceLayoutMemento.AdvancedCornerPoints;
    }

    private record AdvancedLearningSpaceLayoutMemento : IMemento
    {
        internal AdvancedLearningSpaceLayoutMemento(IDictionary<int, ILearningElement> learningElements,
            IDictionary<int, Coordinate> advancedLearningElementSlots, 
            IDictionary<int, Coordinate> advancedDecorations,
            IDictionary<int, DoublePoint> advancedCornerPoints)
        {
            //shallow copy dictionary
            LearningElements = new Dictionary<int, ILearningElement>(learningElements);
            AdvancedLearningElementSlots = new Dictionary<int, Coordinate>(advancedLearningElementSlots);
            AdvancedDecorations = new Dictionary<int, Coordinate>(advancedDecorations);
            AdvancedCornerPoints = new Dictionary<int, DoublePoint>(advancedCornerPoints);
        }

        internal IDictionary<int, ILearningElement> LearningElements { get; }
        internal IDictionary<int, Coordinate> AdvancedLearningElementSlots { get; }
        internal IDictionary<int, Coordinate> AdvancedDecorations { get; }
        internal IDictionary<int, DoublePoint> AdvancedCornerPoints { get; }
        
    }
}