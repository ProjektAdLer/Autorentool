using Shared;

namespace BusinessLogic.Entities.FloorPlans;

public static class FloorPlanProvider
{
    public static IFloorPlan GetFloorPlan(FloorPlanEnum floorPlanName)
    {
        return floorPlanName switch
        {
            FloorPlanEnum.L_32X31_10L => new L32X3110L(),
            FloorPlanEnum.R_20X20_6L => new R20X206L(),
            FloorPlanEnum.R_20X30_8L => new R20X308L(),
            FloorPlanEnum.T_40X32_13L => new T40X3213L(),
            FloorPlanEnum.D_40X37_15L => new D40X3715L(),
            _ => throw new ArgumentOutOfRangeException(nameof(floorPlanName), floorPlanName, null)
        };
    }
}