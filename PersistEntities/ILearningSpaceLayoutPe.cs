namespace PersistEntities;

public interface ILearningSpaceLayoutPe
{
    FloorPlanEnumPe FloorPlanName { get; set; }
    ILearningElementPe?[] LearningElements { get; set; }
    IEnumerable<LearningElementPe> ContainedLearningElements { get; }
}