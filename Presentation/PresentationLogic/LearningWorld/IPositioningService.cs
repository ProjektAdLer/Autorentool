namespace Presentation.PresentationLogic.LearningWorld;

public interface IPositioningService
{
    /// <summary>
    /// Creates a learning pathway from the given source space to the target space on the given position.
    /// Does nothing when there is no learning space on the given position.
    /// </summary>
    /// <param name="sourceObject">The learning space from which the path starts.</param>
    /// <param name="x">The x-coordinate of the target space</param>
    /// <param name="y">The y-coordinate of the target space</param>
    void CreateLearningPathWay(IObjectInPathWayViewModel sourceObject, double x, double y);

    /// <summary>
    /// Deletes the last created learning pathway leading to the target space.
    /// </summary>
    /// <param name="targetObject">The learning space where the path ends.</param>
    void DeleteLearningPathWay(IObjectInPathWayViewModel targetObject);

    /// <summary>
    /// Sets the on hovered learning object of the learning world to the target object on the given position.
    /// When there is no learning object on the given position, the on hovered learning object is set to null.
    /// </summary>
    /// <param name="sourceObject">The learning object from which the path starts.</param>
    /// <param name="x">The x-coordinate of the target object.</param>
    /// <param name="y">The y-coordinate of the target object.</param>
    void SetOnHoveredObjectInPathWay(IObjectInPathWayViewModel sourceObject, double x, double y);
}