﻿namespace Presentation.PresentationLogic.LearningSpace.SpaceLayout.FloorPlans;

public class Rectangle2X2ViewModel : IFloorPlanViewModel
{
    public int Capacity => 4;

    public IList<Point> CornerPoints { get; } = new List<Point>
    {
        new() {X = 0, Y = 0},
        new() {X = 0, Y = 50},
        new() {X = 50, Y = 50},
        new() {X = 50, Y = 0}
    };

    public IList<Point> ElementSlotPositions { get; } = new List<Point>
    {
        new() {X = 10, Y = 10},
        new() {X = 10, Y = 40},
        new() {X = 40, Y = 10},
        new() {X = 40, Y = 40}
    };

    public string GetIcon => Icon;


    private const string Icon =
        @"<svg width=""100%"" height=""100%"" viewBox=""0 0 2000 2000"" version=""1.1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" xml:space=""preserve"" xmlns:serif=""http://www.serif.com/"" style=""fill-rule:evenodd;clip-rule:evenodd;"">
            <rect id=""h5p_icon"" x=""0"" y=""0"" width=""2000"" height=""2000"" style=""fill:none;""/>
            <g id=""h5p_icon1"" serif:id=""h5p_icon"">
                <g transform=""matrix(2.36128,0,0,1.7452,-1303.42,-719.023)"">
                    <path d=""M1399,457.84C1399,432.54 1383.82,412 1365.12,412L585.88,412C567.181,412 552,432.54 552,457.84L552,1512.16C552,1537.46 567.181,1558 585.88,1558L1365.12,1558C1383.82,1558 1399,1537.46 1399,1512.16L1399,457.84Z"" style=""fill:rgb(230,230,230);""/>
                </g>
                <g transform=""matrix(1,0,0,1,1000,1000)"">
                    <g transform=""matrix(1,0,0,1,-1000,-1000)"">
                        <g transform=""matrix(1.01662,0,0,1.06791,-18.1232,-69.908)"">
                            <path d=""M1959.39,160.04C1959.39,119.728 1925.01,87 1882.67,87L117.334,87C74.988,87 40.609,119.728 40.609,160.04L40.609,1839.96C40.609,1880.27 74.988,1913 117.334,1913L1882.67,1913C1925.01,1913 1959.39,1880.27 1959.39,1839.96L1959.39,160.04Z"" style=""fill:rgb(69,160,229);""/>
                        </g>
                        <g id=""H5P"" transform=""matrix(1,0,0,1,2.27374e-13,-1)"">
                            <g transform=""matrix(1.62791,0,0,1.62791,-631.472,-627.962)"">
                                <path d=""M445.74,1000.3L445.74,756.99L596.59,756.99L596.59,966.24L759.61,966.24L759.61,756.99L881.36,756.99L880.053,762.465C877.514,773.1 832.982,970.235 827.563,994.825C824.54,1008.54 822.659,1020.36 823.382,1021.08C824.104,1021.8 853.896,1026.42 889.587,1031.34L954.478,1040.29L964.373,1028.71C992.31,996.026 1039.79,995.47 1067.21,1027.51C1103.15,1069.49 1071.94,1135.05 1016.13,1134.8C996.332,1134.71 980.476,1127.42 965.588,1111.56L953.804,1099.01L888.998,1108.2C853.354,1113.25 823.647,1117.93 822.981,1118.59C820.441,1121.13 833.481,1153.12 843.955,1170.04C863.199,1201.13 886.391,1220.89 920.32,1235.11C928.949,1238.72 936.466,1242.11 937.023,1242.64C937.58,1243.17 897.891,1243.6 848.823,1243.6L759.61,1243.6L759.61,1063.55L596.59,1063.55L596.59,1243.6L445.74,1243.6L445.74,1000.3Z"" style=""fill:white;fill-rule:nonzero;stroke:white;stroke-width:2.43px;""/>
                            </g>
                            <g transform=""matrix(1.62791,0,0,1.62791,-631.472,-627.962)"">
                                <path d=""M1113.7,1241.5C1114.97,1240.33 1121.6,1237.55 1128.43,1235.34C1186.66,1216.46 1231.51,1144.37 1231.56,1069.57C1231.59,1026.38 1218.57,992.789 1190.21,962.939C1170.95,942.668 1150.48,929.762 1123.31,920.761C1105.45,914.846 1100.43,914.299 1062.48,914.132C1023.69,913.962 1019.79,914.371 999.804,920.727C988.082,924.454 976.847,928.134 974.837,928.905C972.477,929.811 971.563,929.135 972.257,926.995C972.847,925.173 976.786,908.628 981.01,890.228L988.689,856.773L1209.68,856.773L1209.68,756.423L1323.43,757.635C1429.47,758.766 1438.5,759.198 1456.64,764.018C1507.09,777.421 1536.81,804.42 1552.02,850.687C1557.63,867.77 1558.44,874.319 1558.62,904.215C1558.77,929.643 1557.73,942.288 1554.53,954.093C1540.61,1005.5 1503.53,1041.54 1450.55,1055.16C1431.08,1060.17 1384.97,1063.34 1329.5,1063.48L1287.53,1063.59L1287.53,1243.63L1199.45,1243.63C1148.83,1243.63 1112.36,1242.73 1113.69,1241.5L1113.7,1241.5ZM1375.33,959.89C1392.29,954.853 1407.52,940.171 1411.63,924.878C1419.36,896.165 1403.36,867.126 1375.42,859.183C1370.58,857.807 1348.82,855.982 1327.08,855.127L1287.54,853.574L1287.54,967.014L1324.86,965.405C1347.41,964.432 1367.38,962.25 1375.33,959.891L1375.33,959.89Z"" style=""fill:white;fill-rule:nonzero;stroke:white;stroke-width:2.43px;""/>
                            </g>
                        </g>
                    </g>
                </g>
            </g>
        </svg>";
}