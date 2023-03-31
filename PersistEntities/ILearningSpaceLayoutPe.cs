namespace PersistEntities;

public interface ILearningSpaceLayoutPe
{
    FloorPlanEnumPe FloorPlanName { get; set; }
    IDictionary<int, ILearningElementPe> LearningElements { get; set; }
    IEnumerable<ILearningElementPe> ContainedLearningElements { get; }
}