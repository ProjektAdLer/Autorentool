using Shared;

namespace Presentation.PresentationLogic.Space.SpaceLayout.FloorPlans;

public static class FloorPlanViewModelProvider
{
    public static IFloorPlanViewModel GetFloorPlan(FloorPlanEnum floorPlanName)
    {
        return floorPlanName switch
        {
            FloorPlanEnum.NoFloorPlan => new NoFloorPlanViewModel(),
            FloorPlanEnum.LShape3L2 => new LShape3L2ViewModel(),
            FloorPlanEnum.Rectangle2X2 => new Rectangle2X2ViewModel(),
            FloorPlanEnum.Rectangle2X3 => new Rectangle2X3ViewModel(),
            _ => throw new ArgumentOutOfRangeException(nameof(floorPlanName), floorPlanName, null)
        };
    }
}