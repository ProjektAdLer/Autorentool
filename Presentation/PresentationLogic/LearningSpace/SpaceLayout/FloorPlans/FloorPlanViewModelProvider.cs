using Shared;

namespace Presentation.PresentationLogic.LearningSpace.SpaceLayout.FloorPlans;

public static class FloorPlanViewModelProvider
{
    public static IFloorPlanViewModel GetFloorPlan(FloorPlanEnum floorPlanName)
    {
        return floorPlanName switch
        {
            FloorPlanEnum.L32X3110L => new L32X3110LViewModel(),
            FloorPlanEnum.R20X206L => new R20X206LViewModel(),
            FloorPlanEnum.R20X308L => new R20X308LViewModel(),
            _ => throw new ArgumentOutOfRangeException(nameof(floorPlanName), floorPlanName, null)
        };
    }
}