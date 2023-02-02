namespace Presentation.PresentationLogic.Space.SpaceLayout.FloorPlans;

public class LShape3L2ViewModel : IFloorPlanViewModel
{
    public int Capacity { get; } = 5;

    public IEnumerable<Point> CornerPoints { get; } = new List<Point>
    {
        new() {X = 0, Y = 0},
        new() {X = 0, Y = 100},
        new() {X = 100, Y = 100},
        new() {X = 100, Y = 75},
        new() {X = 25, Y = 75},
        new() {X = 25, Y = 0}
    };

    public IList<Point> ElementSlotPositions { get; } = new List<Point>
    {
        new() {X = 10, Y = 10},
        new() {X = 10, Y = 50},
        new() {X = 10, Y = 90},
        new() {X = 50, Y = 90},
        new() {X = 90, Y = 90}
    };
}