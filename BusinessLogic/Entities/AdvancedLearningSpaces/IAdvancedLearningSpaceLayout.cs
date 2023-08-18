namespace BusinessLogic.Entities.AdvancedLearningSpaces;

public interface IAdvancedLearningSpaceLayout : IOriginator
{
    IDictionary<int, ILearningElement> LearningElements { get; set; }
    IDictionary<int, IAdvancedLearningElementSlot> AdvancedLearningElementSlots { get; set; }
    IEnumerable<ILearningElement> ContainedLearningElements { get; }
}