namespace Presentation.PresentationLogic.LearningSpace.SpaceLayout.FloorPlans;

public class D40X3715LViewModel : IFloorPlanViewModel
{
    private const string Icon =
        @"<svg id=""uuid-3948e83b-b593-4639-82ed-5134f9f59bb5"" data-name=""D-Form_nobg"" xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 2000 2000"">
  <polygon points=""1841 1399.5 1841 627.5 1354.1 271 36.5 271 36.5 1748 1356.54 1748 1841 1399.5"" style=""fill: #e9f2fa; stroke: #b3b3b3; stroke-miterlimit: 10; stroke-width: 51px;""/>
  <g>
    <rect x=""1816.5"" y=""920.5"" width=""45"" height=""209"" style=""fill: #b3b3b3;""/>
    <polygon points=""1887 895 1791 895 1791 1155 1887 1155 1887 895 1887 895"" style=""fill: #b3b3b3;""/>
  </g>
  <g>
    <rect x=""438.5"" y=""1726.5"" width=""209"" height=""45"" style=""fill: #b3b3b3;""/>
    <polygon points=""673 1701 413 1701 413 1797 673 1797 673 1701 673 1701"" style=""fill: #b3b3b3;""/>
  </g>
  <path d=""M617.33,1895.71l-72.82-72.82-72.82,72.82s144.95-.69,145.64,0Z"" style=""fill: gray; fill-rule: evenodd; stroke: gray; stroke-linecap: round; stroke-linejoin: round; stroke-width: 19px;""/>
  <path d=""M1915.67,1086.54l72.82-72.82-72.82-72.82s.69,144.95,0,145.64Z"" style=""fill: gray; fill-rule: evenodd; stroke: gray; stroke-linecap: round; stroke-linejoin: round; stroke-width: 19px;""/>
  <g>
    <path d=""M768.78,1328.25h-119.95v-462.99l-143.19,44.21v-96.7l250.27-89.65h12.87v605.13Z"" style=""fill: #747e8c;""/>
    <path d=""M982.94,1031.08l34.86-307.13h338.67v100.02h-240.31l-14.94,129.91c28.49-15.21,58.79-22.83,90.89-22.83,57.55,0,102.65,17.85,135.3,53.54,32.65,35.69,48.97,85.64,48.97,149.83,0,39.01-8.24,73.95-24.7,104.8-16.46,30.86-40.05,54.79-70.76,71.8-30.71,17.02-66.96,25.53-108.74,25.53-36.52,0-70.42-7.41-101.68-22.21-31.27-14.8-55.97-35.62-74.09-62.46-18.12-26.83-27.74-57.41-28.84-91.72h118.7c2.49,25.18,11.27,44.76,26.35,58.73,15.08,13.97,34.79,20.96,59.14,20.96,27.11,0,48-9.75,62.67-29.26,14.66-19.51,22-47.11,22-82.8s-8.44-60.6-25.32-78.86c-16.88-18.26-40.82-27.39-71.8-27.39-28.5,0-51.61,7.47-69.31,22.41l-11.62,10.79-95.46-23.66Z"" style=""fill: #747e8c;""/>
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
        new() { X = 4, Y = 32 },
        new() { X = 4, Y = 26 },
        new() { X = 4, Y = 20 },
        new() { X = 4, Y = 15 },
        new() { X = 4, Y = 9 },
        new() { X = 4, Y = 4 },
        new() { X = 9, Y = 4 },
        new() { X = 14, Y = 4 },
        new() { X = 20, Y = 4 },
        new() { X = 26, Y = 9 },
        new() { X = 30, Y = 14 },
        new() { X = 30, Y = 23 },
        new() { X = 26, Y = 27 },
        new() { X = 20, Y = 32 },
        new() { X = 14, Y = 32 }
    };

    public IList<Point> StoryElementSlotPositions { get; } = new List<Point>
    {
        new() { X = 9, Y = 32 },
        new() { X = 30, Y = 18 }
    };

    public IList<(Point, Point)> DoorPositions { get; } = new List<(Point, Point)>()
    {
        (new Point { X = 5, Y = 32 }, new Point { X = 11, Y = 32 }),
        (new Point { X = 32, Y = 13 }, new Point { X = 32, Y = 19 })
    };
    
    public IList<IList<Point>> ArrowCornerPoints { get; } = new List<IList<Point>>
    {
        new List<Point>
        {
            //Intro
            new() { X = 8, Y = 32 },
            new() { X = 7, Y = 33 },
            new() { X = 9, Y = 33 }
        },

        new List<Point>
        {
            //Outro
            new() { X = 33, Y = 16 },
            new() { X = 32, Y = 15 },
            new() { X = 32, Y = 17 }
        }
    };

    public string GetIcon => Icon;

    public string GetPreviewImage => "CustomIcons/FloorPlans/D-Example.png";
}