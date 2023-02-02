namespace Presentation.PresentationLogic.Space.SpaceLayout.FloorPlans;

public class Rectangle2X3ViewModel : IFloorPlanViewModel
{
    public int Capacity => 6;

    public IEnumerable<Point> CornerPoints { get; } = new List<Point>
    {
        new() {X = 0, Y = 0},
        new() {X = 0, Y = 50},
        new() {X = 75, Y = 50},
        new() {X = 75, Y = 0}
    };

    public IList<Point> ElementSlotPositions { get; } = new List<Point>
    {
        new() {X = 10, Y = 10},
        new() {X = 37, Y = 10},
        new() {X = 65, Y = 10},
        new() {X = 10, Y = 40},
        new() {X = 37, Y = 40},
        new() {X = 65, Y = 40}
    };
}