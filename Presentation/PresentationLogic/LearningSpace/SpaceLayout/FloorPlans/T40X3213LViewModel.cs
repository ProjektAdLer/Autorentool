namespace Presentation.PresentationLogic.LearningSpace.SpaceLayout.FloorPlans;

public class T40X3213LViewModel : IFloorPlanViewModel
{
    private const string Icon =
        @"<svg id=""uuid-4e6efbed-4671-4ae2-8d80-3253a488b275"" data-name=""T-Form_right"" xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 2000 2000"">
            <polygon points=""1061.83 1965.43 1061.83 1626.92 1755.51 1626.92 1755.51 409.33 1061.83 409.33 1061.83 43 250.67 43 250.67 1967 1061.83 1965.43"" style=""fill: #cfd8e5; stroke: #2e3a4d; stroke-miterlimit: 8; stroke-width: 51px;""/>
            <g>
                <rect x=""1721.67"" y=""932.83"" width=""65.17"" height=""243.2"" style=""fill: #2e3a4d; stroke-width: 0px;""/>
                <path d=""M1761.35,958.33v192.2h-14.18v-192.2h14.18M1812.35,907.33h-116.18v294.2h116.18v-294.2h0Z"" style=""fill: #2e3a4d; stroke-width: 0px;""/>
            </g>
            <g>
                <rect x=""218.5"" y=""494.46"" width=""65.17"" height=""243.2"" style=""fill: #2e3a4d; stroke-width: 0px;""/>
                <path d=""M258.18,519.96v192.2h-14.18v-192.2h14.18M309.18,468.96h-116.18v294.2h116.18v-294.2h0Z"" style=""fill: #2e3a4d; stroke-width: 0px;""/>
            </g>
            <line x1=""259.86"" y1=""1052.34"" x2=""875.01"" y2=""1052.34"" style=""fill: #45a0e5; stroke: #2e3a4d; stroke-miterlimit: 8; stroke-width: 51px;""/>
            <g>
                <path d=""M791.78,1352.92h-129.03v-498.04l-154.03,47.56v-104.03l269.22-96.44h13.84v650.94Z"" style=""fill: #172d4d; stroke: #172d4d; stroke-miterlimit: 8;""/>
                <path d=""M1134.22,970.75h68.76c32.74,0,56.99-8.18,72.77-24.56,15.77-16.37,23.66-38.1,23.66-65.18s-7.81-46.58-23.44-61.17c-15.63-14.58-37.13-21.88-64.51-21.88-24.71,0-45.39,6.77-62.06,20.31-16.67,13.55-25,31.18-25,52.91h-129.03c0-33.93,9.15-64.36,27.46-91.3,18.3-26.93,43.9-47.99,76.79-63.17,32.88-15.18,69.12-22.77,108.71-22.77,68.75,0,122.62,16.45,161.62,49.33,38.99,32.89,58.49,78.21,58.49,135.95,0,29.77-9.08,57.15-27.23,82.15-18.16,25-41.97,44.2-71.43,57.59,36.61,13.1,63.91,32.74,81.93,58.93,18,26.19,27.01,57.15,27.01,92.86,0,57.75-21.06,104.03-63.17,138.85-42.12,34.82-97.85,52.24-167.2,52.24-64.89,0-117.94-17.11-159.16-51.34-41.23-34.22-61.84-79.47-61.84-135.72h129.03c0,24.41,9.15,44.35,27.46,59.83,18.3,15.48,40.85,23.22,67.64,23.22,30.65,0,54.69-8.11,72.1-24.33,17.41-16.22,26.12-37.73,26.12-64.51,0-64.88-35.72-97.33-107.15-97.33h-68.31v-100.9Z"" style=""fill: #172d4d; stroke: #172d4d; stroke-miterlimit: 8;""/>
            </g>
            <path d=""M18.35,725.55l113.04-113.04-113.04-113.04s1.07,225.01,0,226.09Z"" style=""fill: #2e3a4d; fill-rule: evenodd; stroke: #2e3a4d; stroke-linecap: round; stroke-linejoin: round; stroke-width: 19px;""/>
            <path d=""M1868.35,1167.55l113.04-113.04-113.04-113.04s1.07,225.01,0,226.09Z"" style=""fill: #2e3a4d; fill-rule: evenodd; stroke: #2e3a4d; stroke-linecap: round; stroke-linejoin: round; stroke-width: 19px;""/>
        </svg>";

    public int Capacity => 13;

    public IList<Point> CornerPoints { get; } = new List<Point>
    {
        new() { X = 0, Y = 0 },
        new() { X = 18, Y = 0 },
        new() { X = 18, Y = 6 },
        new() { X = 32, Y = 6 },
        new() { X = 32, Y = 26 },
        new() { X = 18, Y = 26 },
        new() { X = 18, Y = 32 },
        new() { X = 0, Y = 32 }
    };

    public IList<Point> ElementSlotPositions { get; } = new List<Point>
    {
        new() { X = 1, Y = 1 },
        new() { X = 12, Y = 9 },
        new() { X = 20, Y = 1 },
        new() { X = 1, Y = 11 },
        new() { X = 12, Y = 12 },
        new() { X = 27, Y = 7 },
        new() { X = 16, Y = 14 },
        new() { X = 27, Y = 22 },
        new() { X = 13, Y = 28 },
        new() { X = 12, Y = 29 },
        new() { X = 1, Y = 28 },
        new() { X = 1, Y = 18 },
        new() { X = 12, Y = 17 }
    };

    public IList<(Point, Point)> DoorPositions { get; } = new List<(Point, Point)>()
    {
        (new Point { X = 32, Y = 13 }, new Point { X = 32, Y = 18 }),
        (new Point { X = 0, Y = 5 }, new Point { X = 0, Y = 10 })
    };

    public string GetIcon => Icon;

    public string GetPreviewImage => "CustomIcons/FloorPlans/T-Example.png";
}