using Shared;

namespace BusinessLogic.Entities.AdvancedLearningSpaces;

public interface IAdvancedLearningSpace : IOriginator, ILearningSpace
{
    IAdvancedLearningSpaceLayout AdvancedLearningSpaceLayout { get; set; }
}