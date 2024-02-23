namespace Presentation.PresentationLogic.LearningSpace.SpaceLayout.FloorPlans;

public class D40X3715LViewModel : IFloorPlanViewModel
{
    private const string Icon =
        @"<svg id=""uuid-2e36254b-03e2-4734-b7d6-fc81a62b6bd1"" data-name=""D-Form_right"" xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 2000 2000"">
            <polygon points=""1731.73 1429.08 1731.73 390.74 1235.31 89.38 47 89.38 47 1714 1238.46 1714 1731.73 1429.08"" style=""fill: #cfd8e5; stroke: #2e3a4d; stroke-miterlimit: 10; stroke-width: 51px;""/>
            <g>
                <rect x=""1668.77"" y=""734.24"" width=""114.56"" height=""368.26"" style=""fill: #2e3a4d; stroke-width: 0px;""/>
                <path d=""M1757.82,759.74v317.26h-63.56v-317.26h63.56M1808.82,708.74h-165.56v419.26h165.56v-419.26h0Z"" style=""fill: #2e3a4d; stroke-width: 0px;""/>
            </g>
            <g>
                <rect x=""317.92"" y=""1657.88"" width=""368.26"" height=""114.56"" style=""fill: #2e3a4d; stroke-width: 0px;""/>
                <path d=""M660.67,1683.38v63.56h-317.26v-63.56h317.26M711.67,1632.38h-419.26v165.56h419.26v-165.56h0Z"" style=""fill: #2e3a4d; stroke-width: 0px;""/>
            </g>
                <path d=""M589.12,1959.17l-103.61-103.61-103.61,103.61s206.23-.98,207.22,0Z"" style=""fill: #2e3a4d; fill-rule: evenodd; stroke: #2e3a4d; stroke-linecap: round; stroke-linejoin: round; stroke-width: 19px;""/>
                <path d=""M1875.39,1038.34l103.61-103.61-103.61-103.61s.98,206.23,0,207.22Z"" style=""fill: #2e3a4d; fill-rule: evenodd; stroke: #2e3a4d; stroke-linecap: round; stroke-linejoin: round; stroke-width: 19px;""/>
            <g>
                <path d=""M645.15,1280.81h-139.09v-536.87l-166.04,51.27v-112.14l290.2-103.95h14.92v701.69Z"" style=""fill: #172d4d; stroke: #172d4d; stroke-miterlimit: 10;""/>
                <path d=""M893.48,936.22l40.43-356.14h392.71v115.99h-278.65l-17.33,150.64c33.04-17.64,68.17-26.47,105.4-26.47,66.73,0,119.03,20.69,156.89,62.08,37.86,41.39,56.79,99.31,56.79,173.74,0,45.24-9.55,85.75-28.64,121.52-19.09,35.78-46.44,63.53-82.06,83.26-35.61,19.73-77.65,29.6-126.09,29.6-42.35,0-81.66-8.59-117.91-25.75-36.26-17.16-64.9-41.31-85.91-72.43-21.02-31.12-32.17-66.57-33.45-106.36h137.64c2.89,29.2,13.07,51.9,30.56,68.1,17.48,16.21,40.34,24.3,68.58,24.3,31.44,0,55.66-11.31,72.67-33.93,17-22.62,25.51-54.62,25.51-96.01s-9.79-70.27-29.36-91.44c-19.57-21.18-47.33-31.76-83.26-31.76-33.05,0-59.84,8.66-80.37,25.99l-13.48,12.51-110.69-27.43Z"" style=""fill: #172d4d; stroke: #172d4d; stroke-miterlimit: 10;""/>
            </g>
        </svg>";

    public int Capacity => 15;

    public IList<Point> CornerPoints { get; } = new List<Point>
    {
        new() { X = 0, Y = 0 },
        new() { X = 21, Y = 0 },
        new() { X = 32, Y = 10 },
        new() { X = 32, Y = 22 },
        new() { X = 21, Y = 32 },
        new() { X = 0, Y = 32 }
    };

    public IList<Point> ElementSlotPositions { get; } = new List<Point>
    {
        new() { X = 0, Y = 0 },
        new() { X = 6, Y = 0 },
        new() { X = 12, Y = 0 },
        new() { X = 17, Y = 0 },
        new() { X = 24, Y = 6 },
        new() { X = 28, Y = 12 },
        new() { X = 28, Y = 20 },
        new() { X = 24, Y = 26 },
        new() { X = 17, Y = 29 },
        new() { X = 12, Y = 29 },
        new() { X = 0, Y = 29 },
        new() { X = 0, Y = 24 },
        new() { X = 0, Y = 17 },
        new() { X = 0, Y = 12 },
        new() { X = 0, Y = 5 }
    };

    public IList<(Point, Point)> DoorPositions { get; } = new List<(Point, Point)>()
    {
        (new Point { X = 5, Y = 32 }, new Point { X = 10, Y = 32 }),
        (new Point { X = 32, Y = 14 }, new Point { X = 32, Y = 19 })
    };

    public string GetIcon => Icon;

    public string GetPreviewImage => "CustomIcons/FloorPlans/D-Example.png";
}