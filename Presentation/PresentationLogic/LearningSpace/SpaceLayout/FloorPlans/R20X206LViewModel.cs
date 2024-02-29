namespace Presentation.PresentationLogic.LearningSpace.SpaceLayout.FloorPlans;

public class R20X206LViewModel : IFloorPlanViewModel
{
    private const string Icon =
        @"<svg id=""uuid-18860a4c-8515-45be-94be-540113902268"" data-name=""Rsquare-Form_nobg"" xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 2000 2000"">
            <g>
                <rect x=""276.49"" y=""270.5"" width=""1552.2"" height=""1475"" style=""fill: #cfd8e5; stroke-width: 0px;""/>
                <path d=""M1803.19,296v1424H301.99V296h1501.2M1854.19,245H250.99v1526h1603.2V245h0Z"" style=""fill: #2e3a4d; stroke-width: 0px;""/>
            </g>
            <g>
                <rect x=""255.5"" y=""908.5"" width=""45"" height=""209"" style=""fill: #2e3a4d; stroke-width: 0px;""/>
                <polygon points=""326 883 230 883 230 1143 326 1143 326 883 326 883"" style=""fill: #2e3a4d; stroke-width: 0px;""/>
            </g>
            <g>
                <rect x=""933.5"" y=""1725.5"" width=""209"" height=""45"" style=""fill: #2e3a4d; stroke-width: 0px;""/>
                <polygon points=""1168 1700 908 1700 908 1796 1168 1796 1168 1700 1168 1700"" style=""fill: #2e3a4d; stroke-width: 0px;""/>
            </g>
            <path d=""M962.52,1817.74l72.82,72.82,72.82-72.82s-144.95.69-145.64,0Z"" style=""fill: #2e3a4d; fill-rule: evenodd; stroke: #2e3a4d; stroke-linecap: round; stroke-linejoin: round; stroke-width: 19px;""/>
            <path d=""M123.75,1085.31l72.73-72.92-72.92-72.73s.88,144.95.19,145.64Z"" style=""fill: #2e3a4d; fill-rule: evenodd; stroke: #2e3a4d; stroke-linecap: round; stroke-linejoin: round; stroke-width: 19px;""/>
            <path d=""M1181.64,718.65v98.78h-11.62c-54.23.83-97.88,14.94-130.94,42.33-33.07,27.39-52.92,65.44-59.56,114.14,32.09-32.65,72.63-48.97,121.61-48.97,52.57,0,94.35,18.82,125.34,56.45,30.99,37.63,46.48,87.16,46.48,148.58,0,39.29-8.51,74.85-25.52,106.67-17.02,31.82-41.09,56.59-72.22,74.29-31.13,17.71-66.34,26.56-105.63,26.56-63.64,0-115.04-22.13-154.19-66.41-39.16-44.27-58.73-103.34-58.73-177.22v-43.16c0-65.58,12.38-123.47,37.15-173.69,24.76-50.22,60.32-89.09,106.66-116.63,46.34-27.53,100.09-41.43,161.24-41.71h19.92ZM1064.6,1021.63c-19.37,0-36.94,5.05-52.71,15.12s-27.39,23.4-34.86,39.97v36.45c0,40.05,7.89,71.33,23.66,93.83,15.77,22.51,37.9,33.76,66.41,33.76,25.73,0,46.55-10.15,62.46-30.45,15.91-20.3,23.86-46.6,23.86-78.92s-8.03-59.38-24.07-79.54c-16.05-20.16-37.63-30.24-64.75-30.24Z"" style=""fill: #172d4d; stroke-width: 0px;""/>
        </svg>";

    public int Capacity => 6;

    public IList<Point> CornerPoints { get; } = new List<Point>
    {
        new() { X = 4, Y = 0 },
        new() { X = 28, Y = 0 },
        new() { X = 28, Y = 22 },
        new() { X = 4, Y = 22 }
    };

    public IList<Point> ElementSlotPositions { get; } = new List<Point>
    {
        new() { X = 10, Y = 5 },
        new() { X = 18, Y = 5 },
        new() { X = 26, Y = 5 },
        new() { X = 26, Y = 14 },
        new() { X = 26, Y = 22 },
        new() { X = 10, Y = 22 }
    };
    
    public IList<Point> StoryElementSlotPositions { get; } = new List<Point>
    {
        new() {X = 3, Y = 12},
        new() {X = 18, Y = 26}
    };

    public IList<(Point, Point)> DoorPositions { get; } = new List<(Point, Point)>()
    {
        (new Point { X = 4, Y = 13 }, new Point { X = 4, Y = 8 }),
        (new Point { X = 14, Y = 22 }, new Point { X = 19, Y = 22 })
    };

    public string GetIcon => Icon;

    public string GetPreviewImage => "CustomIcons/FloorPlans/R6-Floorplan-Example.png";
}