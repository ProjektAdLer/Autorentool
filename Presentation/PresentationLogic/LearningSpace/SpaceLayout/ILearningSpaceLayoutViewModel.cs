using System.ComponentModel;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningSpace.SpaceLayout.FloorPlans;
using Shared;

namespace Presentation.PresentationLogic.LearningSpace.SpaceLayout;

/// <summary>
/// Holds information about the floor plan of the space and where which element should go.
/// </summary>
public interface ILearningSpaceLayoutViewModel : INotifyPropertyChanged
{
    IDictionary<int, ILearningElementViewModel> LearningElements { get; set; }
    FloorPlanEnum FloorPlanName { get; set; }

    /// <summary>
    /// Floor plan of the current <see cref="ILearningSpaceLayoutViewModel"/>.
    /// </summary>
    IFloorPlanViewModel FloorPlanViewModel { get; }

    /// <summary>
    /// The maximum number of <see cref="ILearningElementViewModel"/>s the <see cref="ILearningSpaceLayoutViewModel"/> can hold.
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
    IEnumerable<ILearningElementViewModel> ContainedLearningElements { get; }

    IDictionary<int, ILearningElementViewModel> StoryElements { get; set; }

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
    /// Put the story element into the layout at the specified index.
    /// </summary>
    /// <param name="index">The index of where to put the element.</param>
    /// <param name="element">The element to put at the index.</param>
    void PutStoryElement(int index, ILearningElementViewModel element);

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