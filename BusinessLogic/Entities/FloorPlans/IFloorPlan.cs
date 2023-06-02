namespace BusinessLogic.Entities.FloorPlans;

/// <summary>
/// Holds information about the Capacity of the space layout.
/// </summary>
public interface IFloorPlan
{
    /// <summary>
    /// The maximum number of <see cref="ILearningElement"/>s the <see cref="ILearningSpaceLayout"/> can hold.
    /// </summary>
    int Capacity { get; }
}