using JetBrains.Annotations;
using Shared;

namespace BusinessLogic.Entities;

public class LearningSpaceLayout : ILearningSpaceLayout, IOriginator
{
    /// <summary>
    /// Constructor for Automapper. DO NOT USE.
    /// </summary>
    [UsedImplicitly]
    private LearningSpaceLayout()
    {
        LearningElements = new Dictionary<int, ILearningElement>();
        FloorPlanName = FloorPlanEnum.R20X308L;
    }
    
    public LearningSpaceLayout(IDictionary<int, ILearningElement> learningElements, FloorPlanEnum floorPlanName)
    {
        LearningElements = learningElements;
        FloorPlanName = floorPlanName;
    }
    
    public IDictionary<int, ILearningElement> LearningElements { get; set; }
    public FloorPlanEnum FloorPlanName { get; set; }
    public IEnumerable<ILearningElement> ContainedLearningElements => LearningElements.Values;


    public IMemento GetMemento()
    {
        return new LearningSpaceLayoutMemento(LearningElements, FloorPlanName);
    }

    public void RestoreMemento(IMemento memento)
    {
        if (memento is not LearningSpaceLayoutMemento learningSpaceLayoutMemento)
        {
            throw new ArgumentException("Incorrect IMemento implementation", nameof(memento));
        }
        LearningElements = learningSpaceLayoutMemento.LearningElements;
        FloorPlanName = learningSpaceLayoutMemento.FloorPlanName;
    }

    private record LearningSpaceLayoutMemento : IMemento
    {
        internal LearningSpaceLayoutMemento(IDictionary<int, ILearningElement> learningElements, FloorPlanEnum floorPlanName)
        {
            //shallow copy dictionary
            LearningElements = new Dictionary<int, ILearningElement>(learningElements);
            FloorPlanName = floorPlanName;
        }
        
        internal IDictionary<int, ILearningElement> LearningElements { get; }
        internal FloorPlanEnum FloorPlanName { get; }
    }
}