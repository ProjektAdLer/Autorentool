namespace Presentation.PresentationLogic.World;

public interface IPositioningService
{
    void CreatePathWay(IObjectInPathWayViewModel sourceObject, double x, double y);
    void DeletePathWay(IObjectInPathWayViewModel targetObject);
    void SetOnHoveredObjectInPathWay(IObjectInPathWayViewModel sourceSpace, double x, double y);
}