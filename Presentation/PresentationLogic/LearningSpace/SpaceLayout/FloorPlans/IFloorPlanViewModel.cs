using System.Collections;
using Presentation.PresentationLogic.LearningElement;

namespace Presentation.PresentationLogic.LearningSpace.SpaceLayout.FloorPlans;

/// <summary>
/// Holds information about the geometric properties of the space layout.
/// </summary>
public interface IFloorPlanViewModel
{
    /// <summary>
    /// The maximum number of <see cref="ILearningElementViewModel"/>s the <see cref="ILearningSpaceLayoutViewModel"/> can hold.
    /// </summary>
    int Capacity { get; }

    /// <summary>
    /// List of X/Y coordinates of the corner points of the shape.
    /// </summary>
    IList<Point> CornerPoints { get; }

    /// <summary>
    /// List of X/Y coordinates of the centers of the slots for LearningElements.
    /// </summary>
    IList<Point> ElementSlotPositions { get; }
    
    /// <summary>
    /// List of X/Y coordinates of the centers of the slots for StoryElements.
    /// </summary>
    IList<Point> StoryElementSlotPositions { get; }

    /// <summary>
    /// List of two X/Y coordinates of the door positions.
    /// </summary>
    IList<(Point, Point)> DoorPositions { get; }
    
    IList<IList<Point>> ArrowCornerPoints { get; }

    string GetIcon { get; }

    string GetPreviewImage { get; }
}