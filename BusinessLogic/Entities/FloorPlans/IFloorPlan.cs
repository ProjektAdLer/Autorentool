namespace BusinessLogic.Entities.FloorPlans;

/// <summary>
/// Holds information about the Capacity of the space layout.
/// </summary>
public interface IFloorPlan
{
    /// <summary>
    /// The maximum number of <see cref="IElement"/>s the <see cref="ISpaceLayout"/> can hold.
    /// </summary>
    int Capacity { get; }
}