namespace Presentation.PresentationLogic.LearningWorld;

public interface IPositioningService
{
    void CreateLearningPathWay(IObjectInPathWayViewModel sourceObject, double x, double y);
    void DeleteLearningPathWay(IObjectInPathWayViewModel targetObject);
    void SetOnHoveredObjectInPathWay(IObjectInPathWayViewModel sourceSpace, double x, double y);
}