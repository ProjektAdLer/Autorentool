using Presentation.PresentationLogic.LearningSpace;

namespace Presentation.PresentationLogic.LearningWorld;

public interface IPositioningService
{
    void CreateLearningPathWay(ILearningSpaceViewModel sourceSpace, double x, double y);
    void DeleteLearningPathWay(ILearningSpaceViewModel targetSpace);
    void SetOnHoveredLearningSpace(ILearningSpaceViewModel sourceSpace, double x, double y);
}