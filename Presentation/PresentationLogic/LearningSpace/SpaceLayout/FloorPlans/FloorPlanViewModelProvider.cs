using Shared;

namespace Presentation.PresentationLogic.LearningSpace.SpaceLayout.FloorPlans;

public static class FloorPlanViewModelProvider
{
    public static IFloorPlanViewModel GetFloorPlan(FloorPlanEnum floorPlanName)
    {
        return floorPlanName switch
        {
            FloorPlanEnum.L_32X31_10L => new L32X3110LViewModel(),
            FloorPlanEnum.R_20X20_6L => new R20X206LViewModel(),
            FloorPlanEnum.R_20X30_8L => new R20X308LViewModel(),
            _ => throw new ArgumentOutOfRangeException(nameof(floorPlanName), floorPlanName, null)
        };
    }
}