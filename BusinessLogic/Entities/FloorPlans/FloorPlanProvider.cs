using Shared;

namespace BusinessLogic.Entities.FloorPlans;

public static class FloorPlanProvider
{
    public static IFloorPlan GetFloorPlan(FloorPlanEnum floorPlanName)
    {
        return floorPlanName switch
        {
            FloorPlanEnum.L32X3110L => new L32X3110L(),
            FloorPlanEnum.R20X206L => new R20X206L(),
            FloorPlanEnum.R20X308L => new R20X308L(),
            _ => throw new ArgumentOutOfRangeException(nameof(floorPlanName), floorPlanName, null)
        };
    }
}