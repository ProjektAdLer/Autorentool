using Presentation.PresentationLogic.Element;
using Presentation.PresentationLogic.Space.SpaceLayout.FloorPlans;
using Shared;

namespace Presentation.PresentationLogic.Space.SpaceLayout;

/// <summary>
/// Holds information about the floor plan of the space and where which element should go.
/// </summary>
public interface ISpaceLayoutViewModel
{
    IElementViewModel?[] Elements { get; set; }
    FloorPlanEnum FloorPlanName { get; set; }
    /// <summary>
    /// Floor plan of the current <see cref="ISpaceLayoutViewModel"/>.
    /// </summary>
    IFloorPlanViewModel FloorPlanViewModel { get; }
    /// <summary>
    /// The maximum number of <see cref="IElementViewModel"/>s the <see cref="ISpaceLayoutViewModel"/> can hold.
    /// </summary>
    int Capacity { get; }
    /// <summary>
    /// The current number of <see cref="IElementViewModel"/>s in this layout.
    /// </summary>
    int Count { get; }
    /// <summary>
    /// The indices currently in use.
    /// </summary>
    IEnumerable<int> UsedIndices { get; }
    /// <summary>
    /// List of currently held <see cref="IElementViewModel"/>.
    /// </summary>
    IEnumerable<IElementViewModel> ContainedElements { get; }
    /// <summary>
    /// Gets the element at the specified index.
    /// </summary>
    /// <param name="index">The index of the element.</param>
    /// <returns>The element if there was one at the specified index, otherwise null.</returns>
    IElementViewModel? GetElement(int index);
    /// <summary>
    /// Put the element into the layout at the specified index.
    /// </summary>
    /// <param name="index">The index of where to put the element.</param>
    /// <param name="element">The element to put at the index.</param>
    void PutElement(int index, IElementViewModel element);
    /// <summary>
    /// Remove any element from the specified index.
    /// </summary>
    /// <param name="index">The index from which to remove the element.</param>
    void RemoveElement(int index);
    /// <summary>
    /// Overrides all elements in this layout with null.
    /// </summary>
    void ClearAllElements();
    /// <summary>
    /// Changes the current floor plan to the specified one.
    /// </summary>
    /// <param name="floorPlanName">Name of the new floor plan</param>
    void ChangeFloorPlan(FloorPlanEnum floorPlanName);
}