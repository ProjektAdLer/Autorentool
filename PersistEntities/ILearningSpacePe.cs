using PersistEntities.LearningOutcome;

namespace PersistEntities;

public interface ILearningSpacePe : ISpacePe, IObjectInPathWayPe
{
    string Name { get; set; }
    string Description { get; set; }
    LearningOutcomeCollectionPe LearningOutcomeCollection { get; }
    int RequiredPoints { get; set; }
    ILearningSpaceLayoutPe LearningSpaceLayout { get; set; }
}