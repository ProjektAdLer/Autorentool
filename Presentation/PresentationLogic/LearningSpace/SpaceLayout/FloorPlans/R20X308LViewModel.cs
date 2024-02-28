namespace Presentation.PresentationLogic.LearningSpace.SpaceLayout.FloorPlans;

public class R20X308LViewModel : IFloorPlanViewModel
{
    private const string Icon =
        @"<svg id=""uuid-0ac49057-1f48-445e-b3e3-a40f0f0f7799"" data-name=""R-Form_nobg"" xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 2000 2000"">
            <g>
                <rect x=""166.5"" y=""271.4"" width=""1803.02"" height=""1474.1"" style=""fill: #cfd8e5; stroke-width: 0px;""/>
                <path d=""M1944.02,296.9v1423.1H192V296.9h1752.02M1995.02,245.9H141v1525.1h1854.02V245.9h0Z"" style=""fill: #2e3a4d; stroke-width: 0px;""/>
            </g>
            <g>
                <rect x=""145.5"" y=""913.5"" width=""45"" height=""209"" style=""fill: #2e3a4d; stroke-width: 0px;""/>
                <polygon points=""216 888 120 888 120 1148 216 1148 216 888 216 888"" style=""fill: #2e3a4d; stroke-width: 0px;""/>
            </g>
            <g>
                <rect x=""1467.5"" y=""1723.5"" width=""209"" height=""45"" style=""fill: #2e3a4d; stroke-width: 0px;""/>
                <polygon points=""1702 1698 1442 1698 1442 1794 1702 1794 1702 1698 1702 1698"" style=""fill: #2e3a4d; stroke-width: 0px;""/>
            </g>
            <path d=""M1498.48,1823.23l72.82,72.82,72.82-72.82s-144.95.69-145.64,0Z"" style=""fill: #2e3a4d; fill-rule: evenodd; stroke: #2e3a4d; stroke-linecap: round; stroke-linejoin: round; stroke-width: 19px;""/>
            <path d=""M16.98,1091.46l72.73-72.92-72.92-72.73s.88,144.95.19,145.64Z"" style=""fill: #2e3a4d; fill-rule: evenodd; stroke: #2e3a4d; stroke-linecap: round; stroke-linejoin: round; stroke-width: 19px;""/>
            <path d=""M1262.63,885.5c0,29.33-7.33,55.34-22,78.03-14.67,22.69-34.86,40.82-60.6,54.37,29.33,14.11,52.57,33.55,69.73,58.31,17.15,24.77,25.73,53.89,25.73,87.37,0,53.68-18.26,96.08-54.79,127.21-36.52,31.13-86.19,46.69-149,46.69s-112.62-15.63-149.41-46.9c-36.8-31.26-55.2-73.6-55.2-127,0-33.48,8.57-62.67,25.73-87.57,17.15-24.9,40.26-44.27,69.31-58.11-25.73-13.55-45.86-31.68-60.39-54.37-14.53-22.68-21.79-48.7-21.79-78.03,0-51.46,17.15-92.48,51.46-123.06,34.31-30.57,80.93-45.86,139.87-45.86s105.21,15.15,139.66,45.45c34.45,30.3,51.67,71.46,51.67,123.47ZM1155.13,1154.86c0-26.28-7.61-47.31-22.83-63.09-15.22-15.77-35.69-23.66-61.43-23.66s-45.8,7.82-61.01,23.45c-15.22,15.64-22.83,36.73-22.83,63.29s7.47,46.48,22.41,62.26,35.69,23.66,62.26,23.66,46.41-7.61,61.22-22.83c14.8-15.21,22.2-36.25,22.2-63.09ZM1142.68,891.31c0-23.51-6.23-42.4-18.68-56.65-12.45-14.25-30.03-21.37-52.71-21.37s-39.84,6.92-52.29,20.75c-12.45,13.84-18.68,32.93-18.68,57.28s6.23,43.44,18.68,58.11c12.45,14.67,30.02,22,52.71,22s40.19-7.33,52.5-22c12.31-14.66,18.47-34.03,18.47-58.11Z"" style=""fill: #172d4d; stroke-width: 0px;""/>
        </svg>";

    public int Capacity => 8;

    public IList<Point> CornerPoints { get; } = new List<Point>
    {
        new() { X = 0, Y = 0 },
        new() { X = 32, Y = 0 },
        new() { X = 32, Y = 20 },
        new() { X = 0, Y = 20 }
    };

    public IList<Point> ElementSlotPositions { get; } = new List<Point>
    {
        new() { X = 6, Y = 5 },
        new() { X = 14, Y = 5 },
        new() { X = 22, Y = 5 },
        new() { X = 30, Y = 5 },
        new() { X = 30, Y = 19 },
        new() { X = 22, Y = 19 },
        new() { X = 14, Y = 19 },
        new() { X = 6, Y = 19 }
    };
    
    public IList<Point> StoryElementSlotPositions { get; } = new List<Point>
    {
        new() {X = 3, Y = 12},
        new() {X = 18, Y = 26}
    };

    public IList<(Point, Point)> DoorPositions { get; } = new List<(Point, Point)>()
    {
        (new Point { X = 0, Y = 12 }, new Point { X = 0, Y = 7 }),
        (new Point { X = 22, Y = 20 }, new Point { X = 27, Y = 20 })
    };

    public string GetIcon => Icon;

    public string GetPreviewImage => "CustomIcons/FloorPlans/R8-Floorplan-Example.png";
}