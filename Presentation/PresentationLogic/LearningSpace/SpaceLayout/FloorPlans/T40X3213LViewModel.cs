namespace Presentation.PresentationLogic.LearningSpace.SpaceLayout.FloorPlans;

public class T40X3213LViewModel : IFloorPlanViewModel
{
    private const string Icon =
        @"<svg id=""uuid-26806155-9e0f-4f39-bd93-b1455ea85227"" data-name=""T-Form_nobg"" xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 2000 2000"">
            <polygon points=""1111.19 1743.79 1111.19 1484.03 1840.06 1484.03 1840.06 549.7 1111.19 549.7 1111.19 268.59 165.66 268.59 165.66 1745 1111.19 1743.79"" style=""fill: #cfd8e5; stroke: #2e3a4d; stroke-miterlimit: 8; stroke-width: 51px;""/>
            <line x1=""171.58"" y1=""1008.79"" x2=""966.85"" y2=""1008.79"" style=""fill: #45a0e5; stroke: #2e3a4d; stroke-miterlimit: 8; stroke-width: 51px;""/>
            <g>
                <rect x=""1815.5"" y=""915.73"" width=""45"" height=""209"" style=""fill: #2e3a4d; stroke-width: 0px;""/>
                <polygon points=""1886 890.23 1790 890.23 1790 1150.23 1886 1150.23 1886 890.23 1886 890.23"" style=""fill: #2e3a4d; stroke-width: 0px;""/>
            </g>
            <g>
                <rect x=""142.5"" y=""534.5"" width=""45"" height=""209"" style=""fill: #2e3a4d; stroke-width: 0px;""/>
                <polygon points=""213 509 117 509 117 769 213 769 213 509 213 509"" style=""fill: #2e3a4d; stroke-width: 0px;""/>
            </g>
                <path d=""M15.07,709.54l72.82-72.82-72.82-72.82s.69,144.95,0,145.64Z"" style=""fill: #2e3a4d; fill-rule: evenodd; stroke: #2e3a4d; stroke-linecap: round; stroke-linejoin: round; stroke-width: 19px;""/>
                <path d=""M1914.74,1094.74l72.82-72.82-72.82-72.82s.69,144.95,0,145.64Z"" style=""fill: #2e3a4d; fill-rule: evenodd; stroke: #2e3a4d; stroke-linecap: round; stroke-linejoin: round; stroke-width: 19px;""/>
            <g>
                <path d=""M849.28,1327.18h-119.95v-462.99l-143.19,44.21v-96.7l250.27-89.65h12.87v605.13Z"" style=""fill: #172d4d; stroke-width: 0px;""/>
                <path d=""M1167.61,971.9h63.92c30.43,0,52.98-7.61,67.65-22.83,14.66-15.21,22-35.41,22-60.6s-7.26-43.3-21.79-56.86c-14.53-13.55-34.52-20.34-59.97-20.34-22.97,0-42.2,6.3-57.69,18.88-15.5,12.59-23.24,28.99-23.24,49.18h-119.95c0-31.54,8.51-59.83,25.53-84.88,17.02-25.04,40.81-44.62,71.39-58.73,30.57-14.11,64.26-21.17,101.06-21.17,63.92,0,113.99,15.29,150.24,45.86,36.24,30.58,54.37,72.7,54.37,126.38,0,27.67-8.44,53.12-25.32,76.37-16.88,23.24-39.01,41.09-66.41,53.54,34.03,12.18,59.42,30.44,76.16,54.79,16.74,24.35,25.11,53.12,25.11,86.33,0,53.68-19.58,96.7-58.73,129.08-39.16,32.37-90.97,48.56-155.43,48.56-60.32,0-109.64-15.91-147.96-47.73-38.33-31.82-57.48-73.88-57.48-126.17h119.95c0,22.69,8.51,41.23,25.52,55.62,17.02,14.39,37.98,21.58,62.88,21.58,28.5,0,50.84-7.54,67.03-22.62,16.19-15.08,24.28-35.07,24.28-59.97,0-60.32-33.2-90.48-99.61-90.48h-63.5v-93.8Z"" style=""fill: #172d4d; stroke-width: 0px;""/>
            </g>
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
        new() { X = 5, Y = 5 },
        new() { X = 11, Y = 5 },
        new() { X = 17, Y = 5 },
        new() { X = 5, Y = 16 },
        new() { X = 11, Y = 16 },
        new() { X = 30, Y = 12 },
        new() { X = 17, Y = 19 },
        new() { X = 30, Y = 26 },
        new() { X = 17, Y = 33 },
        new() { X = 11, Y = 33 },
        new() { X = 5, Y = 33 },
        new() { X = 5, Y = 22 },
        new() { X = 11, Y = 22 }
    };

    public IList<Point> StoryElementSlotPositions { get; } = new List<Point>
    {
        new() { X = 2, Y = 9 },
        new() { X = 33, Y = 17 }
    };

    public IList<(Point, Point)> DoorPositions { get; } = new List<(Point, Point)>()
    {
        (new Point { X = 0, Y = 5 }, new Point { X = 0, Y = 10 }),
        (new Point { X = 32, Y = 13 }, new Point { X = 32, Y = 18 })
    };

    public string GetIcon => Icon;

    public string GetPreviewImage => "CustomIcons/FloorPlans/T-Example.png";
}