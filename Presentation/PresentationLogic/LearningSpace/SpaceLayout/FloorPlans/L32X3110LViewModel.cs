namespace Presentation.PresentationLogic.LearningSpace.SpaceLayout.FloorPlans;

public class L32X3110LViewModel : IFloorPlanViewModel
{
    private const string Icon =
        @"<svg id=""uuid-46a1f09d-a66f-421a-b01e-d5064bf499cb"" data-name=""L-Form_nobg"" xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 2000 2000"">
  <polygon points=""1968.74 1745.44 1002.5 1746 1001.77 1396.5 158.37 1396.5 158 272.39 1968.74 272.39 1968.74 1745.44 1968.74 1745.44"" style=""fill: #e9f2fa; stroke: #b3b3b3; stroke-miterlimit: 8; stroke-width: 51px;""/>
  <g>
    <rect x=""135.5"" y=""712.5"" width=""45"" height=""209"" style=""fill: #b3b3b3;""/>
    <polygon points=""206 687 110 687 110 947 206 947 206 687 206 687"" style=""fill: #b3b3b3;""/>
  </g>
  <g>
    <rect x=""1389.5"" y=""1723.5"" width=""209"" height=""45"" style=""fill: #b3b3b3;""/>
    <polygon points=""1624 1698 1364 1698 1364 1794 1624 1794 1624 1698 1624 1698"" style=""fill: #b3b3b3;""/>
  </g>
  <path d=""M1422.92,1817.82l72.82,72.82,72.82-72.82s-144.95.69-145.64,0Z"" style=""fill: gray; fill-rule: evenodd; stroke: gray; stroke-linecap: round; stroke-linejoin: round; stroke-width: 19px;""/>
  <path d=""M14.33,885.06l72.82-72.82-72.82-72.82s.69,144.95,0,145.64Z"" style=""fill: gray; fill-rule: evenodd; stroke: gray; stroke-linecap: round; stroke-linejoin: round; stroke-width: 19px;""/>
  <g>
    <path d=""M913.27,1326.6h-119.95v-462.99l-143.19,44.21v-96.7l250.27-89.65h12.87v605.13Z"" style=""fill: #747e8c;""/>
    <path d=""M1516.33,1076.75c0,83.57-17.3,147.48-51.88,191.75-34.59,44.27-85.23,66.41-151.9,66.41s-116.21-21.72-151.07-65.16c-34.86-43.44-52.71-105.69-53.54-186.77v-111.23c0-84.39,17.5-148.44,52.5-192.16,35-43.72,85.43-65.58,151.28-65.58s116.21,21.65,151.07,64.95c34.86,43.31,52.71,105.49,53.54,186.56v111.23ZM1396.38,954.45c0-50.13-6.85-86.62-20.54-109.47-13.7-22.85-35.07-34.27-64.12-34.27s-49.05,10.88-62.46,32.61c-13.42,21.74-20.54,55.74-21.38,101.99v147.07c0,49.3,6.71,85.93,20.13,109.89,13.42,23.96,34.93,35.93,64.54,35.93s50.49-11.49,63.5-34.48c13-22.98,19.78-58.16,20.34-105.52v-143.74Z"" style=""fill: #747e8c;""/>
  </g>
</svg>";

    public int Capacity => 10;

    public IList<Point> CornerPoints { get; } = new List<Point>
    {
        new() { X = 0, Y = 0 },
        new() { X = 32, Y = 0 },
        new() { X = 32, Y = 31 },
        new() { X = 16, Y = 31 },
        new() { X = 16, Y = 17 },
        new() { X = 0, Y = 17 }
    };

    public IList<Point> ElementSlotPositions { get; } = new List<Point>
    {
        new() { X = 4, Y = 4 },
        new() { X = 18, Y = 4 },
        new() { X = 28, Y = 4 },
        new() { X = 30, Y = 12 },
        new() { X = 30, Y = 19 },
        new() { X = 30, Y = 26 },
        new() { X = 20, Y = 26 },
        new() { X = 20, Y = 19 },
        new() { X = 14, Y = 17 },
        new() { X = 4, Y = 17 }
    };

    public IList<Point> StoryElementSlotPositions { get; } = new List<Point>
    {
        new() { X = 4, Y = 11 },
        new() { X = 25, Y = 31 }
    };

    public IList<(Point, Point)> DoorPositions { get; } = new List<(Point, Point)>()
    {
        (new Point { X = 0, Y = 12 }, new Point { X = 0, Y = 6 }),
        (new Point { X = 21, Y = 31 }, new Point { X = 27, Y = 31 })
    };
    
    public IList<IList<Point>> ArrowCornerPoints { get; } = new List<IList<Point>>
    {
        new List<Point>
        {
            //Intro
            new() { X = 0, Y = 9 },
            new() { X = -1, Y = 8 },
            new() { X = -1, Y = 10}
        },

        new List<Point>
        {
            //Outro
            new() { X = 24, Y = 32 },
            new() { X = 23, Y = 31 },
            new() { X = 25, Y = 31 }
        }
    };

    public string GetIcon => Icon;

    public string GetPreviewImage => "CustomIcons/FloorPlans/L-Floorplan-Example.png";
}