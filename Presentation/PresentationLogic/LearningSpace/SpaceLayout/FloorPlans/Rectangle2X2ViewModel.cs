namespace Presentation.PresentationLogic.LearningSpace.SpaceLayout.FloorPlans;

public class Rectangle2X2ViewModel : IFloorPlanViewModel
{
    public int Capacity => 4;

    public IEnumerable<Point> CornerPoints { get; } = new List<Point>
    {
        new() {X = 0, Y = 0},
        new() {X = 0, Y = 50},
        new() {X = 50, Y = 50},
        new() {X = 50, Y = 0}
    };

    public IList<Point> ElementSlotPositions { get; } = new List<Point>
    {
        new() {X = 10, Y = 10},
        new() {X = 10, Y = 40},
        new() {X = 40, Y = 10},
        new() {X = 40, Y = 40}
    };
}