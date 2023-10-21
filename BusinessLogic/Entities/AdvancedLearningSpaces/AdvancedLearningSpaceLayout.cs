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
    }

    public AdvancedLearningSpaceLayout(IDictionary<int, ILearningElement>? learningElements = null,
        IDictionary<int, Coordinate>? advancedLearningElementSlots = null,
        IDictionary<int, Coordinate>? advancedDecorations = null)
    {
        LearningElements = learningElements ?? new Dictionary<int, ILearningElement>();
        AdvancedLearningElementSlots = advancedLearningElementSlots ?? new Dictionary<int, Coordinate>();
        AdvancedDecorations = advancedDecorations ?? new Dictionary<int, Coordinate>();
    }

    public IDictionary<int, Coordinate> AdvancedLearningElementSlots { get; set; }
    public IDictionary<int, Coordinate> AdvancedDecorations { get; set; }

    public IDictionary<int, ILearningElement> LearningElements { get; set; }
    public IEnumerable<ILearningElement> ContainedLearningElements => LearningElements.Values;


    public IMemento GetMemento()
    {
        return new AdvancedLearningSpaceLayoutMemento(LearningElements, AdvancedLearningElementSlots);
    }

    public void RestoreMemento(IMemento memento)
    {
        if (memento is not AdvancedLearningSpaceLayoutMemento advancedLearningSpaceLayoutMemento)
        {
            throw new ArgumentException("Incorrect IMemento implementation", nameof(memento));
        }

        LearningElements = advancedLearningSpaceLayoutMemento.LearningElements;
        AdvancedLearningElementSlots = advancedLearningSpaceLayoutMemento.AdvancedLearningElementSlots;
    }

    private record AdvancedLearningSpaceLayoutMemento : IMemento
    {
        internal AdvancedLearningSpaceLayoutMemento(IDictionary<int, ILearningElement> learningElements,
            IDictionary<int, Coordinate> advancedLearningElementSlots)
        {
            //shallow copy dictionary
            LearningElements = new Dictionary<int, ILearningElement>(learningElements);
            AdvancedLearningElementSlots = new Dictionary<int, Coordinate>(advancedLearningElementSlots);
        }

        internal IDictionary<int, ILearningElement> LearningElements { get; }
        internal IDictionary<int, Coordinate> AdvancedLearningElementSlots { get; }
    }
}