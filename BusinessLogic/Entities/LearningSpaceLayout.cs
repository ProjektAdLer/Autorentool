using JetBrains.Annotations;
using Shared;

namespace BusinessLogic.Entities;

public class LearningSpaceLayout : ILearningSpaceLayout
{
    /// <summary>
    /// Constructor for Automapper. DO NOT USE.
    /// </summary>
    [UsedImplicitly]
    private LearningSpaceLayout()
    {
        LearningElements = new Dictionary<int, ILearningElement>();
        StoryElements = new Dictionary<int, ILearningElement>();
        FloorPlanName = FloorPlanEnum.R_20X30_8L;
    }

    public LearningSpaceLayout(IDictionary<int, ILearningElement> learningElements,
        IDictionary<int, ILearningElement> storyElements, FloorPlanEnum floorPlanName)
    {
        LearningElements = learningElements;
        StoryElements = storyElements;
        FloorPlanName = floorPlanName;
    }

    public IDictionary<int, ILearningElement> LearningElements { get; set; }
    public IDictionary<int, ILearningElement> StoryElements { get; set; }
    public FloorPlanEnum FloorPlanName { get; set; }
    public IEnumerable<ILearningElement> ContainedLearningElements => LearningElements.Values;


    public IMemento GetMemento()
    {
        return new LearningSpaceLayoutMemento(LearningElements, StoryElements, FloorPlanName);
    }

    public void RestoreMemento(IMemento memento)
    {
        if (memento is not LearningSpaceLayoutMemento learningSpaceLayoutMemento)
        {
            throw new ArgumentException("Incorrect IMemento implementation", nameof(memento));
        }

        LearningElements = learningSpaceLayoutMemento.LearningElements;
        StoryElements = learningSpaceLayoutMemento.StoryElements;
        FloorPlanName = learningSpaceLayoutMemento.FloorPlanName;
    }

    private record LearningSpaceLayoutMemento : IMemento
    {
        internal LearningSpaceLayoutMemento(IDictionary<int, ILearningElement> learningElements,
            IDictionary<int, ILearningElement> storyElements,
            FloorPlanEnum floorPlanName)
        {
            //shallow copy dictionary
            LearningElements = new Dictionary<int, ILearningElement>(learningElements);
            StoryElements = new Dictionary<int, ILearningElement>(storyElements);
            FloorPlanName = floorPlanName;
        }

        internal IDictionary<int, ILearningElement> LearningElements { get; }
        internal IDictionary<int, ILearningElement> StoryElements { get; }
        internal FloorPlanEnum FloorPlanName { get; }
    }
}