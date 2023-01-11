namespace Presentation.PresentationLogic.LearningSpace.SpaceLayout.FloorPlans;

public class NoFloorPlanViewModel : IFloorPlanViewModel
{
    public int Capacity { get; } = 0;
    public IEnumerable<Point> CornerPoints { get; } = new List<Point>();
    public IEnumerable<Point> ElementSlotPositions { get; } = new List<Point>();
}