using Shared;

namespace BusinessLogic.Entities.AdvancedLearningSpaces;

public interface IAdvancedLearningSpaceLayout : IOriginator
{
    IDictionary<int, ILearningElement> LearningElements { get; set; }
    IDictionary<int, Coordinate> AdvancedLearningElementSlots { get; set; }
    IEnumerable<ILearningElement> ContainedLearningElements { get; }
    IDictionary<int, Coordinate> AdvancedDecorations { get; set; }
}