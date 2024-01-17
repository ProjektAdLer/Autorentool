using PersistEntities.LearningOutcome;

namespace PersistEntities;

public interface ILearningSpacePe : ISpacePe, IObjectInPathWayPe
{
    string Name { get; set; }
    string Description { get; set; }
    List<ILearningOutcomePe> LearningOutcomes { get; }
    int RequiredPoints { get; set; }
    ILearningSpaceLayoutPe LearningSpaceLayout { get; set; }
}