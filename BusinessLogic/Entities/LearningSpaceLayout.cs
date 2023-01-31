using JetBrains.Annotations;
using Shared;

namespace BusinessLogic.Entities;

public class LearningSpaceLayout : ILearningSpaceLayout, IOriginator
{
    /// <summary>
    /// Constructor for Automapper. DO NOT USE.
    /// </summary>
    [UsedImplicitly]
    internal LearningSpaceLayout()
    {
        LearningElements = Array.Empty<ILearningElement>();
        FloorPlanName = FloorPlanEnum.NoFloorPlan;
    }
    
    public LearningSpaceLayout(ILearningElement?[] learningElements, FloorPlanEnum floorPlanName)
    {
        LearningElements = learningElements;
        FloorPlanName = floorPlanName;
    }
    
    public ILearningElement?[] LearningElements { get; set; }
    public FloorPlanEnum FloorPlanName { get; set; }
    public IEnumerable<ILearningElement> ContainedLearningElements => LearningElements.Where(e => e != null)!;


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
        internal LearningSpaceLayoutMemento(ILearningElement?[] learningElements, FloorPlanEnum floorPlanName)
        {
            LearningElements = learningElements.ToArray();
            FloorPlanName = floorPlanName;
        }
        
        internal ILearningElement?[] LearningElements { get; }
        internal FloorPlanEnum FloorPlanName { get; }
    }
}