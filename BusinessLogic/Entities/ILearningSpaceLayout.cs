using Shared;

namespace BusinessLogic.Entities;

public interface ILearningSpaceLayout : IOriginator
{
    IDictionary<int, ILearningElement> LearningElements { get; set; }
    FloorPlanEnum FloorPlanName { get; set; }
    IEnumerable<ILearningElement> ContainedLearningElements { get; }
    IDictionary<int, ILearningElement> StoryElements { get; set; }
}