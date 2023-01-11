using Shared;

namespace BusinessLogic.Entities;

public interface ILearningSpaceLayout : IOriginator
{
    ILearningElement?[] LearningElements { get; set; }
    FloorPlanEnum FloorPlanName { get; set; }
    IEnumerable<ILearningElement> ContainedLearningElements { get; }
}