namespace Presentation.PresentationLogic.LearningSpace.SpaceLayout.FloorPlans;

public class R20X206LViewModel : IFloorPlanViewModel
{
    public int Capacity => 6;

    public IList<Point> CornerPoints { get; } = new List<Point>
    {
        new() {X = 0, Y = 0},
        new() {X = 0, Y = 60},
        new() {X = 60, Y = 60},
        new() {X = 60, Y = 0}
    };

    public IList<Point> ElementSlotPositions { get; } = new List<Point>
    {
        new() {X = 9, Y = 9},
        new() {X = 30, Y = 9},
        new() {X = 51, Y = 9},
        new() {X = 51, Y = 30},
        new() {X = 51, Y = 51},
        new () {X = 9, Y = 51}
    };

    public string GetIcon => Icon;


    private const string Icon =
        @"<svg id=""R_20x20_6L-nobg"" xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 2000 2000"">
            <defs>
            <style>
            .R_20x20_6L-1 {
                fill: #45a0e5;
                stroke: #45a0e5;
                stroke-miterlimit: 10;
            }

            .R_20x20_6L-2 {
                fill: #172d4d;
            }

            .R_20x20_6L-3 {
                fill: #cfd8e5;
            }
            </style>
            </defs>
            <g>
            <rect class=""R_20x20_6L-3"" x=""310.5"" y=""332.5"" width=""1382"" height=""1313""/>
            <path class=""R_20x20_6L-2"" d=""m1667,358v1262H336V358h1331m51-51H285v1364h1433V307h0Z""/>
            </g>
            <g>
            <rect class=""R_20x20_6L-2"" x=""272.5"" y=""852.5"" width=""88"" height=""301""/>
            <path class=""R_20x20_6L-2"" d=""m335,878v250h-37v-250h37m51-51h-139v352h139v-352h0Z""/>
            </g>
            <g>
            <rect class=""R_20x20_6L-2"" x=""864.5"" y=""1601.5"" width=""301"" height=""88""/>
            <path class=""R_20x20_6L-2"" d=""m1140,1627v37h-250v-37h250m51-51h-352v139h352v-139h0Z""/>
            </g>
            <path class=""R_20x20_6L-1"" d=""m1089.53,730.22v78.52h-11.72c-53.12.78-95.58,15.37-127.34,43.75-31.78,28.39-50.65,68.62-56.64,120.7,30.47-32.03,69.4-48.05,116.8-48.05,50.26,0,89.9,17.71,118.95,53.12,29.03,35.42,43.55,81.12,43.55,137.11s-16.99,104.63-50.98,140.43c-33.98,35.81-78.58,53.71-133.79,53.71s-102.8-20.96-138.09-62.89c-35.29-41.92-52.93-96.88-52.93-164.84v-32.42c0-99.74,24.28-177.86,72.85-234.38,48.57-56.51,118.42-84.77,209.57-84.77h9.77Zm-101.95,271.48c-20.84,0-39.91,5.86-57.23,17.58-17.32,11.72-30.02,27.34-38.09,46.88v28.91c0,42.19,8.85,75.72,26.56,100.59,17.71,24.87,40.62,37.3,68.75,37.3s50.39-10.55,66.8-31.64,24.61-48.83,24.61-83.2-8.34-62.37-25-83.98c-16.67-21.61-38.81-32.42-66.41-32.42Z""/>
        </svg>";
    
}