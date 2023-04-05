using Shared;

namespace BusinessLogic.Entities.FloorPlans;

public static class FloorPlanProvider
{
    public static IFloorPlan GetFloorPlan(FloorPlanEnum floorPlanName)
    {
        return floorPlanName switch
        {
            FloorPlanEnum.LShape3L2 => new LShape3L2(),
            FloorPlanEnum.Rectangle2X2 => new Rectangle2X2(),
            FloorPlanEnum.Rectangle2X3 => new Rectangle2X3(),
            _ => throw new ArgumentOutOfRangeException(nameof(floorPlanName), floorPlanName, null)
        };
    }
}