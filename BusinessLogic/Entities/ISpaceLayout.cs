using Shared;

namespace BusinessLogic.Entities;

public interface ISpaceLayout : IOriginator
{
    IElement?[] Elements { get; set; }
    FloorPlanEnum FloorPlanName { get; set; }
    IEnumerable<IElement> ContainedElements { get; }
}