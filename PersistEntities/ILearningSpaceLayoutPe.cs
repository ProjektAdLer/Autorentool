using Shared;

namespace PersistEntities;

public interface ILearningSpaceLayoutPe
{
    FloorPlanEnum FloorPlanName { get; set; }
    IDictionary<int, ILearningElementPe> LearningElements { get; set; }
    IEnumerable<ILearningElementPe> ContainedLearningElements { get; }
}