﻿namespace Presentation.PresentationLogic.LearningSpace.SpaceLayout.FloorPlans;

public class R20X308LViewModel : IFloorPlanViewModel
{
    public int Capacity => 8;

    public IList<Point> CornerPoints { get; } = new List<Point>
    {
        new() {X = 0, Y = 0},
        new() {X = 0, Y = 60},
        new() {X = 90, Y = 60},
        new() {X = 90, Y = 0}
    };

    public IList<Point> ElementSlotPositions { get; } = new List<Point>
    {
        new() {X = 12, Y = 15},
        new() {X = 36, Y = 15},
        new() {X = 60, Y = 15},
        new() {X = 84, Y = 15},
        new() {X = 84, Y = 45},
        new() {X = 60, Y = 45},
        new() {X = 36, Y = 45},
        new() {X = 12, Y = 45}
    };

    public string GetIcon => Icon;


    private const string Icon =
        @"<svg id=""R_20x30_8L-nobg"" xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 2000 2000"">
            <defs>
            <style>
            .R_20x30_8L-1 {
                fill: #172d4d;
            }

            .R_20x30_8L-2 {
                fill: #45a0e5;
                stroke: #45a0e5;
                stroke-miterlimit: 8;
            }

            .R_20x30_8L-3 {
                fill: #cfd8e5;
            }
            </style>
            </defs>
            <g>
            <rect class=""R_20x30_8L-3"" x=""82.5"" y=""343.5"" width=""1833"" height=""1313""/>
            <path class=""R_20x30_8L-1"" d=""m1890,369v1262H108V369h1782m51-51H57v1364h1884V318h0Z""/>
            </g>
            <g>
            <rect class=""R_20x30_8L-1"" x=""1870.5"" y=""850.5"" width=""88"" height=""301""/>
            <path class=""R_20x30_8L-1"" d=""m1933,876v250h-37v-250h37m51-51h-139v352h139v-352h0Z""/>
            </g>
            <g>
            <rect class=""R_20x30_8L-1"" x=""41.5"" y=""849.5"" width=""88"" height=""301""/>
            <path class=""R_20x30_8L-1"" d=""m104,875v250h-37v-250h37m51-51H16v352h139v-352h0Z""/>
            </g>
            <path class=""R_20x30_8L-2"" d=""m1175.25,881.5c0,27.87-7.17,52.67-21.48,74.41-14.33,21.75-33.86,38.87-58.59,51.37,29.69,13.81,52.54,32.68,68.55,56.64,16.02,23.96,24.02,51.04,24.02,81.25,0,50-16.93,89.65-50.78,118.95-33.86,29.3-78.91,43.95-135.16,43.95s-101.76-14.71-135.74-44.14c-33.98-29.42-50.98-69.01-50.98-118.75,0-30.47,8.07-57.81,24.22-82.03,16.14-24.22,38.8-42.83,67.97-55.86-24.48-12.5-43.82-29.62-58.01-51.37-14.2-21.74-21.29-46.55-21.29-74.41,0-48.44,15.62-86.85,46.88-115.23,31.25-28.38,73.44-42.58,126.56-42.58s95.7,14.2,126.95,42.58c31.25,28.39,46.88,66.8,46.88,115.23Zm-82.42,258.98c0-28.12-8.4-50.84-25.2-68.16-16.8-17.32-39-25.98-66.6-25.98s-49.68,8.59-66.21,25.78c-16.54,17.19-24.8,39.98-24.8,68.36s8.14,50.13,24.41,66.8c16.27,16.67,38.73,25,67.38,25s50.98-8.07,66.99-24.22c16.02-16.14,24.02-38.67,24.02-67.58Zm-12.11-255.47c0-24.74-7.17-45.12-21.48-61.13-14.33-16.02-33.59-24.02-57.81-24.02s-43.36,7.62-57.42,22.85-21.09,36-21.09,62.3,7.09,46.75,21.29,62.11c14.19,15.37,33.4,23.05,57.62,23.05s43.42-7.68,57.62-23.05c14.19-15.36,21.29-36.07,21.29-62.11Z""/>
        </svg>";
}