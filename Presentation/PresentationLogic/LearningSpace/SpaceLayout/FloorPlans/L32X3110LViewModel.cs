namespace Presentation.PresentationLogic.LearningSpace.SpaceLayout.FloorPlans;

public class L32X3110LViewModel : IFloorPlanViewModel
{
    private const string Icon =
        @"<svg id=""L_32x31_10L-nobg"" xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 2000 2000"">
          <defs>
          <style>
          .L_32x31_10L-1 {
            fill: #cfd8e5;
            stroke: #172d4d;
            stroke-width: 51px;
          }

          .L_32x31_10L-1, .L_32x31_10L-2 {
            stroke-miterlimit: 8;
          }

          .L_32x31_10L-3 {
            fill: #172d4d;
          }

          .L_32x31_10L-2 {
            fill: #45a0e5;
            stroke: #45a0e5;
          }
          </style>
        </defs>
        <polygon class=""L_32x31_10L-1"" points=""1940 1660 1054.5 1660.5 1053.5 1281.5 107.5 1281.5 107 348 1940 348 1940 1660 1940 1660""/>
        <g>
        <rect class=""L_32x31_10L-3"" x=""61.5"" y=""673.5"" width=""88"" height=""301""/>
        <path class=""L_32x31_10L-3"" d=""m124,699v250h-37v-250h37m51-51H36v352h139v-352h0Z""/>
        </g>
        <g>
        <rect class=""L_32x31_10L-3"" x=""1357.5"" y=""1615.5"" width=""301"" height=""88""/>
        <path class=""L_32x31_10L-3"" d=""m1633,1641v37h-250v-37h250m51-51h-352v139h352v-139h0Z""/>
        </g>
        <path class=""L_32x31_10L-2"" d=""m1413.28,856.55c0,81.78-15.3,143.43-45.9,184.96-30.6,41.54-77.15,62.3-139.65,62.3s-107.75-20.25-138.87-60.74c-31.12-40.49-47.07-100.45-47.85-179.88v-98.44c0-81.77,15.29-143.1,45.9-183.98,30.6-40.88,77.28-61.33,140.04-61.33s108.65,19.92,139.26,59.77c30.6,39.84,46.29,99.48,47.07,178.91v98.44Zm-94.92-106.25c0-53.38-7.29-92.51-21.88-117.38-14.59-24.87-37.76-37.3-69.53-37.3s-53.78,11.79-68.36,35.35c-14.59,23.57-22.27,60.35-23.05,110.35v128.91c0,53.12,7.42,92.71,22.27,118.75,14.84,26.04,38.15,39.06,69.92,39.06s52.99-12.04,67.58-36.13c14.58-24.08,22.27-61.78,23.05-113.09v-128.52Z""/>
        <path class=""L_32x31_10L-2"" d=""m844.92,1096h-94.53v-456.92l-139.45,47.54v-79.69l221.88-81.64h12.11v570.7Z""/>
       </svg>";

    public int Capacity => 10;

    public IList<Point> CornerPoints { get; } = new List<Point>
    {
        new() {X = 0, Y = 0},
        new() {X = 32, Y = 0},
        new() {X = 32, Y = 31},
        new() {X = 16, Y = 31},
        new() {X = 16, Y = 17},
        new() {X = 0, Y = 17}
    };

    public IList<Point> ElementSlotPositions { get; } = new List<Point>
    {
        new() {X = 8, Y = 5},
        new() {X = 18, Y = 5},
        new() {X = 28, Y = 5},
        new() {X = 30, Y = 14},
        new() {X = 30, Y = 22},
        new() {X = 30, Y = 30},
        new() {X = 22, Y = 30},
        new() {X = 22, Y = 22},
        new() {X = 15, Y = 17},
        new() {X = 6, Y = 17}
    };

    public IList<(Point, Point)> DoorPositions { get; } = new List<(Point, Point)>()
    {
        (new Point {X = 0, Y = 11}, new Point {X = 0, Y = 6}),
        (new Point {X = 22, Y = 31}, new Point {X = 27, Y = 31})
    };

    public string GetIcon => Icon;

    public string GetPreviewImage => "CustomIcons/FloorPlans/L-Floorplan-Example.png";
}