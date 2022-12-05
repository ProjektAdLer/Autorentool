using Presentation.PresentationLogic.LearningElement;

namespace Presentation.PresentationLogic.LearningSpace;

/// <summary>
/// Holds information about the floor plan of the space and where which element should go.
/// </summary>
public interface ILearningSpaceLayout
{
    /// <summary>
    /// Floor plan of the current <see cref="ILearningSpaceLayout"/>.
    /// </summary>
    IFloorPlan FloorPlan { get; }
    /// <summary>
    /// The maximum number of <see cref="ILearningElementViewModel"/>s the <see cref="ILearningSpaceLayout"/> can hold.
    /// </summary>
    int Capacity { get; }
    /// <summary>
    /// The current number of <see cref="ILearningElementViewModel"/>s in this layout.
    /// </summary>
    int Count { get; }
    /// <summary>
    /// The indices currently in use.
    /// </summary>
    IEnumerable<int> UsedIndices { get; }
    /// <summary>
    /// List of currently held <see cref="ILearningElementViewModel"/>.
    /// </summary>
    IEnumerable<ILearningElementViewModel> LearningElements { get; }
    /// <summary>
    /// Gets the element at the specified index.
    /// </summary>
    /// <param name="index">The index of the element.</param>
    /// <returns>The element if there was one at the specified index, otherwise null.</returns>
    ILearningElementViewModel? GetElement(int index);
    /// <summary>
    /// Put the element into the layout at the specified index.
    /// </summary>
    /// <param name="index">The index of where to put the element.</param>
    /// <param name="element">The element to put at the index.</param>
    void PutElement(int index, ILearningElementViewModel element);
    /// <summary>
    /// Remove any element from the specified index.
    /// </summary>
    /// <param name="index">The index from which to remove the element.</param>
    void RemoveElement(int index);
}

/// <summary>
/// Holds information about the geometric properties of the space layout.
/// </summary>
public interface IFloorPlan
{
    /// <summary>
    /// The maximum number of <see cref="ILearningElementViewModel"/>s the <see cref="ILearningSpaceLayout"/> can hold.
    /// </summary>
    int Capacity { get; }
    /// <summary>
    /// List of X/Y coordinates of the corner points of the shape.
    /// </summary>
    IEnumerable<Point> CornerPoints { get; }
    /// <summary>
    /// List of X/Y coordinates of the centers of the slots for LearningElements.
    /// </summary>
    IEnumerable<Point> ElementSlotPositions { get; }
}

public struct Point
{
    public int X { get; init; }
    public int Y { get; init; }
}