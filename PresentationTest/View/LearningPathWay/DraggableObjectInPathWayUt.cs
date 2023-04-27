using System;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components;
using Presentation.PresentationLogic;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.LearningPathway;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.View;
using Presentation.View.LearningPathWay;
using Shared;
using TestContext = Bunit.TestContext;

namespace PresentationTest.View.LearningPathWay;

[TestFixture]
public class DraggableObjectInPathWay
{
#pragma warning disable CS8618 //set in setup - n.stich
    private TestContext _ctx;
    private IMouseService _mouseService;
    private IMediator _mediator;
#pragma warning restore CS8618

    [SetUp]
    public void Setup()
    {
        _ctx = new TestContext();
        _ctx.ComponentFactories.AddStub<Draggable<IObjectInPathWayViewModel>>();
        _mouseService = Substitute.For<IMouseService>();
        _mediator = Substitute.For<IMediator>();
        _ctx.Services.AddSingleton(_mouseService);
        _ctx.Services.AddSingleton(_mediator);
    }
    
    [Test]
    public void Constructor_SetsParametersCorrectly_LearningSpace()
    {
        var learningSpace = new LearningSpaceViewModel("a","b","c",1);
        var onClicked = new Action<IObjectInPathWayViewModel>(_ => { });
        var onDragged = new DraggedEventArgs<IObjectInPathWayViewModel>.DraggedEventHandler((_,_) => { });
        var onDoubleClicked = new Action<IObjectInPathWayViewModel>(_ => { });
        var onRightClicked = new Action<IObjectInPathWayViewModel>(_ => { });
        var showingRightClickMenu = true;
        var onOpenLearningSpace = new Action<ILearningSpaceViewModel>(_ => { });
        var onEditLearningSpace = new Action<ILearningSpaceViewModel>(_ => { });
        var onDeleteLearningSpace = new Action<ILearningSpaceViewModel>(_ => { });
        var onRemoveLearningSpaceFromTopic = new Action<ILearningSpaceViewModel>(_ => { });
        var onCloseRightClickMenu = new Action(() => { });
        var positioningService = Substitute.For<ILearningWorldPresenter>();
        var systemUnderTest =
            GetRenderedDraggableLearningSpace(learningSpace, onClicked, onDragged, onDoubleClicked, onRightClicked,
                showingRightClickMenu, onOpenLearningSpace, onEditLearningSpace, onDeleteLearningSpace, onRemoveLearningSpaceFromTopic,
                onCloseRightClickMenu, positioningService);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.ObjectInPathWay, Is.EqualTo(learningSpace));
            //overriding nullability warning because we know target isn't null as onClicked isn't a static method but instead a lambda -n.stich
            Assert.That(systemUnderTest.Instance.OnClickedDraggable, Is.EqualTo(EventCallback.Factory.Create(onClicked.Target!, onClicked)));
            Assert.That(systemUnderTest.Instance.OnDoubleClickedDraggable, Is.EqualTo(EventCallback.Factory.Create(onDoubleClicked.Target!, onDoubleClicked)));
            Assert.That(systemUnderTest.Instance.OnRightClickedDraggable, Is.EqualTo(EventCallback.Factory.Create(onRightClicked.Target!, onRightClicked)));
            Assert.That(systemUnderTest.Instance.ShowingRightClickMenu, Is.EqualTo(showingRightClickMenu));
            Assert.That(systemUnderTest.Instance.OnOpenLearningSpace, Is.EqualTo(EventCallback.Factory.Create(onOpenLearningSpace.Target!, onOpenLearningSpace)));
            Assert.That(systemUnderTest.Instance.OnEditLearningSpace, Is.EqualTo(EventCallback.Factory.Create(onEditLearningSpace.Target!, onEditLearningSpace)));
            Assert.That(systemUnderTest.Instance.OnDeleteLearningSpace, Is.EqualTo(EventCallback.Factory.Create(onDeleteLearningSpace.Target!, onDeleteLearningSpace)));
            Assert.That(systemUnderTest.Instance.OnRemoveLearningSpaceFromTopic, Is.EqualTo(EventCallback.Factory.Create(onRemoveLearningSpaceFromTopic.Target!, onRemoveLearningSpaceFromTopic)));
            Assert.That(systemUnderTest.Instance.OnCloseRightClickMenu, Is.EqualTo(EventCallback.Factory.Create(onCloseRightClickMenu.Target!, onCloseRightClickMenu)));
            Assert.That(systemUnderTest.Instance.PositioningService, Is.EqualTo(positioningService));
        });
    }
    
    [Test]
    public void Constructor_SetsParametersCorrectly_PathWayCondition()
    {
        var pathWayCondition = new PathWayConditionViewModel(ConditionEnum.And, 2, 1);
        var onClicked = new Action<IObjectInPathWayViewModel>(_ => { });
        var onDragged = new DraggedEventArgs<IObjectInPathWayViewModel>.DraggedEventHandler((_,_) => { });
        var onDoubleClicked = new Action<IObjectInPathWayViewModel>(_ => { });
        var onRightClicked = new Action<IObjectInPathWayViewModel>(_ => { });
        var showingRightClickMenu = false;
        var onEditPathWayCondition = new Action<PathWayConditionViewModel>(_ => { });
        var onDeletePathWayCondition = new Action<PathWayConditionViewModel>(_ => { });
        var onCloseRightClickMenu = new Action(() => { });
        var positioningService = Substitute.For<ILearningWorldPresenter>();
        var systemUnderTest =
            GetRenderedDraggablePathWayCondition(pathWayCondition, onClicked, onDragged, onDoubleClicked, onRightClicked,
                showingRightClickMenu, onEditPathWayCondition, onDeletePathWayCondition, onCloseRightClickMenu , positioningService);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.ObjectInPathWay, Is.EqualTo(pathWayCondition));
            //overriding nullability warning because we know target isn't null as onClicked isn't a static method but instead a lambda -n.stich
            Assert.That(systemUnderTest.Instance.OnClickedDraggable, Is.EqualTo(EventCallback.Factory.Create(onClicked.Target!, onClicked)));
            Assert.That(systemUnderTest.Instance.OnDoubleClickedDraggable, Is.EqualTo(EventCallback.Factory.Create(onDoubleClicked.Target!, onDoubleClicked)));
            Assert.That(systemUnderTest.Instance.OnRightClickedDraggable, Is.EqualTo(EventCallback.Factory.Create(onRightClicked.Target!, onRightClicked)));
            Assert.That(systemUnderTest.Instance.ShowingRightClickMenu, Is.EqualTo(showingRightClickMenu));
            Assert.That(systemUnderTest.Instance.OnDeletePathWayCondition, Is.EqualTo(EventCallback.Factory.Create(onDeletePathWayCondition.Target!, onDeletePathWayCondition)));
            Assert.That(systemUnderTest.Instance.OnCloseRightClickMenu, Is.EqualTo(EventCallback.Factory.Create(onCloseRightClickMenu.Target!, onCloseRightClickMenu)));
            Assert.That(systemUnderTest.Instance.PositioningService, Is.EqualTo(positioningService));
        });
    }

    [Test]
    public void Constructor_PassesCorrectValuesToDraggable_LearningSpace()
    {
        var learningSpace = Substitute.For<ILearningSpaceViewModel>();
        var spaceName = "foo bar super cool name";
        learningSpace.Name.Returns(spaceName);
        var onClicked = new Action<IObjectInPathWayViewModel>(_ => { });
        var onDragged = new DraggedEventArgs<IObjectInPathWayViewModel>.DraggedEventHandler((_,_) => { });
        var onDoubleClicked = new Action<IObjectInPathWayViewModel>(_ => { });
        var onRightClicked = new Action<IObjectInPathWayViewModel>(_ => { });
        const bool showingRightClickMenu = false;
        var onOpenLearningSpace = new Action<ILearningSpaceViewModel>(_ => { });
        var onEditLearningSpace = new Action<ILearningSpaceViewModel>(_ => { });
        var onDeleteLearningSpace = new Action<ILearningSpaceViewModel>(_ => { });
        var onRemoveLearningSpaceFromTopic = new Action<ILearningSpaceViewModel>(_ => { });
        var onCloseRightClickMenu = new Action(() => { });
        var positioningService = Substitute.For<ILearningWorldPresenter>();
        var systemUnderTest =
            GetRenderedDraggableLearningSpace(learningSpace, onClicked, onDragged, onDoubleClicked, onRightClicked,
                showingRightClickMenu, onOpenLearningSpace, onEditLearningSpace, onDeleteLearningSpace, onRemoveLearningSpaceFromTopic,
                onCloseRightClickMenu, positioningService);

        Assert.That(systemUnderTest.HasComponent<Stub<Draggable<IObjectInPathWayViewModel>>>());
        var stub = systemUnderTest.FindComponent<Stub<Draggable<IObjectInPathWayViewModel>>>();
        Assert.Multiple(() =>
        {
            Assert.That(stub.Instance.Parameters[nameof(Draggable<ILearningSpaceViewModel>.LearningObject)], Is.EqualTo(learningSpace));
            //overriding nullability warning because we know target isn't null as onClicked isn't a static method but instead a lambda -n.stich
            Assert.That(stub.Instance.Parameters[nameof(Draggable<ILearningSpaceViewModel>.OnClicked)], Is.EqualTo(EventCallback.Factory.Create(onClicked.Target!, onClicked)));
            Assert.That(stub.Instance.Parameters[nameof(Draggable<ILearningSpaceViewModel>.ChildContent)], Is.Not.Null);
        });
        var childContent = _ctx.Render((RenderFragment)stub.Instance.Parameters[nameof(Draggable<ILearningSpaceViewModel>.ChildContent)]);
        childContent.MarkupMatches(
            $@"<svg width=""20%"" height=""20%"" viewBox=""-100 -100 2200 2000"" preserveAspectRatio=""xMinYMin"" version=""1.1"" xmlns=""http://www.w3.org/2000/svg"" xml:space=""preserve"" style=""fill-rule:evenodd;clip-rule:evenodd;stroke-linecap:round;stroke-linejoin:round;stroke-miterlimit:1.5;"">
              <g transform=""matrix(1.12299,-0.648358,1.29672,0.74866,-322.238,-121.233)"">
                <rect x=""-1024"" y=""1191.65"" width=""825.427"" height=""714.841"" style=""fill:rgb(185,190,198);""></rect>
                <path d=""M-183.732,1185.22C-183.732,1181.25 -186.56,1178.8 -191.152,1178.8L-1016.58,1178.8C-1021.17,1178.8 -1026.83,1181.25 -1031.42,1185.22C-1036.01,1189.2 -1038.84,1194.1 -1038.84,1198.08L-1038.84,1912.92C-1038.84,1916.89 -1036.01,1919.34 -1031.42,1919.34L-205.994,1919.34C-201.402,1919.34 -195.744,1916.89 -191.152,1912.92C-186.56,1908.94 -183.732,1904.04 -183.732,1900.07L-183.732,1185.22ZM-213.414,1204.5L-213.414,1893.64C-213.414,1893.64 -1009.16,1893.64 -1009.16,1893.64C-1009.16,1893.64 -1009.16,1204.5 -1009.16,1204.5C-1009.16,1204.5 -213.414,1204.5 -213.414,1204.5L-213.414,1204.5Z"" style=""fill:rgb(23,45,77);""></path>
              </g>
              <g transform=""matrix(1.12299,0.648358,-1.07082e-16,1.21657,2149.94,-755.81)"">
                <rect x=""-1024"" y=""1191.65"" width=""825.427"" height=""714.841"" style=""fill:rgb(227,229,232);""></rect>
                <path d=""M-183.732,1183.74C-183.732,1178.85 -186.56,1175.83 -191.152,1175.83L-1016.58,1175.83C-1021.17,1175.83 -1026.83,1178.85 -1031.42,1183.74C-1036.01,1188.63 -1038.84,1194.66 -1038.84,1199.56L-1038.84,1914.4C-1038.84,1919.3 -1036.01,1922.31 -1031.42,1922.31L-205.994,1922.31C-201.402,1922.31 -195.744,1919.3 -191.152,1914.4C-186.56,1909.51 -183.732,1903.48 -183.732,1898.58L-183.732,1183.74ZM-213.414,1207.47L-213.414,1890.67C-213.414,1890.67 -928.275,1890.67 -1009.16,1890.67C-1009.16,1890.67 -1009.16,1207.47 -1009.16,1207.47C-1009.16,1207.47 -213.414,1207.47 -213.414,1207.47L-213.414,1207.47Z"" style=""fill:rgb(23,45,77);""></path>
              </g>
              <g transform=""matrix(1.12299,-0.648358,6.99808e-17,1.21657,1222.99,-1548.47)"">
                <rect x=""-1024"" y=""1191.65"" width=""825.427"" height=""714.841"" style=""fill:rgb(143,151,163);""></rect>
                <path d=""M-183.732,1199.56C-183.732,1194.66 -186.56,1188.63 -191.152,1183.74C-195.744,1178.85 -201.402,1175.83 -205.994,1175.83L-1031.42,1175.83C-1036.01,1175.83 -1038.84,1178.85 -1038.84,1183.74L-1038.84,1898.58C-1038.84,1903.48 -1036.01,1909.51 -1031.42,1914.4C-1026.83,1919.3 -1021.17,1922.31 -1016.58,1922.31L-191.152,1922.31C-186.56,1922.31 -183.732,1919.3 -183.732,1914.4L-183.732,1199.56ZM-213.414,1207.47L-213.414,1890.67C-213.414,1890.67 -1009.16,1890.67 -1009.16,1890.67C-1009.16,1890.67 -1009.16,1284.03 -1009.16,1207.47C-1009.16,1207.47 -213.414,1207.47 -213.414,1207.47L-213.414,1207.47Z"" style=""fill:rgb(23,45,77);""></path>
              </g>
              <g id=""Avatar"" transform=""matrix(1.40843,0,0,1.40843,-562.362,-617.05)"">
                <g transform=""matrix(0.544332,-0.31427,3.59526e-17,0.747858,217.194,524.616)"">
                  <path d=""M1247.08,1216C1384.73,1216 1496.49,1343.89 1496.49,1501.41C1496.49,1658.94 1535.28,1786.82 1247.08,1786.82C974.9,1786.82 997.661,1658.94 997.661,1501.41C997.661,1343.89 1109.42,1216 1247.08,1216Z"" style=""fill:rgb(69,160,229);""></path>
                </g>
                <g transform=""matrix(0.71001,0,0,0.71001,399.283,438.112)"">
                  <path d=""M777.081,814.835C750.718,812.106 721.708,818.669 691.284,836.234C610.033,883.145 539.4,1000.18 508.43,1127.78C481.88,1208.81 475.785,1289.24 491.128,1394.18C492.167,1401.28 498.039,1468.59 523.069,1500.5C634.689,1586.59 681.061,1567.81 696.544,1564.88C723.452,1559.81 757.922,1545.87 801.802,1520.53C893.947,1467.33 943.874,1415.38 971.095,1361.63C1011.41,1282.01 1001.35,1197.57 1001.35,1095.08C1001.35,1010.81 979.448,946.409 945.254,908.381C940.456,900.982 933.524,893.327 923.907,886.18C812.094,803.089 805.695,812.477 805.695,812.477C798.345,808.234 789.276,808.288 781.978,812.619L777.081,814.835ZM883.251,905.554C898.584,941.45 907.5,987.127 907.5,1040.89C907.5,1143.38 917.56,1227.83 877.244,1307.44C850.024,1361.19 800.096,1413.15 707.951,1466.35C678.051,1483.61 652.521,1495.59 630.772,1503.07C634.46,1512.63 639.412,1520.42 646.342,1525.89C656.536,1533.93 670.821,1535.82 690.365,1532.13C714.641,1527.55 745.548,1514.52 785.135,1491.67C869.749,1442.82 916.361,1395.93 941.357,1346.57C979.365,1271.51 968.017,1191.7 968.017,1095.08C968.017,1040.14 958.371,994.497 941.182,960.916C926.718,932.658 907.034,913.261 883.251,905.554ZM845.337,902.953C846.012,904.192 846.677,905.452 847.331,906.731C864.52,940.312 874.166,985.956 874.166,1040.89C874.166,1137.51 885.514,1217.33 847.506,1292.38C822.511,1341.74 775.898,1388.63 691.284,1437.48C664.828,1452.76 642.249,1463.64 622.937,1470.55C616.647,1428.66 618.919,1373.56 618.919,1315.88C618.919,1156.03 700.131,977.986 801.801,919.287L801.802,919.287C816.892,910.574 831.476,905.151 845.337,902.953Z"" style=""fill:rgb(23,45,77);""></path>
                </g>
                <g transform=""matrix(0.71001,0,0,0.71001,399.283,474.322)"">
                  <path d=""M588.35,712.27C593.056,729.145 604.864,759.428 623.819,774.033C712.429,842.31 726.996,840.469 747.262,843.144C777.35,847.116 810.658,845.369 842.796,816.522C897.893,767.069 926.208,667.312 905.714,593.814C896.605,561.144 879.608,538.476 859.179,525.721C858.296,525.169 772.14,469.865 746.803,462.94C717.527,454.94 683.406,460.715 651.268,489.562L651.268,489.562C596.172,539.016 567.856,638.773 588.35,712.27ZM816.256,560.251C826.995,630.828 799.039,717.375 748.946,762.338C744.324,766.486 739.655,770.182 734.963,773.44C741.609,786.224 750.55,794.925 761.492,798.405C776.861,803.294 794.359,796.914 811.437,781.584C854.742,742.714 876.6,664.193 860.492,606.424C853.937,582.918 842.281,566.908 826.423,561.864C823.123,560.814 819.725,560.284 816.256,560.251ZM771.649,583.271C774.54,633.224 755.396,689.728 722.415,722.814C719.525,672.86 738.669,616.357 771.649,583.271Z"" style=""fill:rgb(23,45,77);""></path>
                </g>
                <g transform=""matrix(0.544332,-0.31427,3.59526e-17,0.747858,283.829,563.088)"">
                  <path d=""M1247.08,1216C1384.73,1216 1496.49,1343.89 1496.49,1501.41C1496.49,1658.94 1535.28,1786.82 1247.08,1786.82C974.9,1786.82 997.661,1658.94 997.661,1501.41C997.661,1343.89 1109.42,1216 1247.08,1216Z"" style=""fill:rgb(69,160,229);""></path>
                </g>
                <g transform=""matrix(0.470095,-0.271409,3.10492e-17,0.645863,395.054,667.361)"">
                  <path d=""M1247.08,1216C1384.73,1216 1496.49,1343.89 1496.49,1501.41C1496.49,1658.94 1535.28,1786.82 1247.08,1786.82C974.9,1786.82 997.661,1658.94 997.661,1501.41C997.661,1343.89 1109.42,1216 1247.08,1216Z"" style=""fill:rgb(89,172,232);""></path>
                </g>
                <g transform=""matrix(0.290838,-0.167915,1.92095e-17,0.399582,637.131,905.16)"">
                  <path d=""M1247.08,1216C1384.73,1216 1496.49,1343.89 1496.49,1501.41C1496.49,1658.94 1535.28,1786.82 1247.08,1786.82C974.9,1786.82 997.661,1658.94 997.661,1501.41C997.661,1343.89 1109.42,1216 1247.08,1216Z"" style=""fill:rgb(146,202,240);""></path>
                </g>
                <g transform=""matrix(0.17493,-0.100996,1.1554e-17,0.240337,787.185,1073.24)"">
                  <path d=""M1247.08,1216C1384.73,1216 1496.49,1343.89 1496.49,1501.41C1496.49,1658.94 1535.28,1786.82 1247.08,1786.82C974.9,1786.82 997.661,1658.94 997.661,1501.41C997.661,1343.89 1109.42,1216 1247.08,1216Z"" style=""fill:rgb(184,221,245);""></path>
                </g>
                <g transform=""matrix(0.946018,-0.849128,0.250036,0.896707,-393.158,808.37)"">
                  <ellipse cx=""1110.03"" cy=""1218.51"" rx=""65.452"" ry=""94.16"" style=""fill:rgb(69,160,229);""></ellipse>
                </g>
              </g>
              <g id=""Fernseher_rechts"">
                <g transform=""matrix(0.866025,0.5,-5.55112e-17,1.1547,0,0)"">
                  <rect x=""1360.24"" y=""-278.963"" width=""640.859"" height=""423.294"" style=""fill:rgb(143,151,163);stroke:rgb(23,45,77);stroke-width:25.24px;""></rect>
                </g>
                <g id=""Videobutton"" transform=""matrix(1.11433,0.64336,-6.18579e-17,1.28672,391.991,-1005.28)"">
                  <g transform=""matrix(1.32664,0,0,1.32664,569.679,532.734)"">
                    <circle cx=""289.99"" cy=""272"" r=""96"" style=""fill:rgb(69,160,229);""></circle>
                  </g>
                  <g transform=""matrix(6.12323e-17,1,-0.834359,5.10897e-17,1714.28,-94.2356)"">
                    <path d=""M990.236,816L1065.95,967.426L914.523,967.426L990.236,816Z"" style=""fill:rgb(23,45,79);""></path>
                  </g>
                </g>
              </g>
              <rect transform=""translate(-50,-50)"" height=""2100"" width=""2100"" rx=""100"" style=""fill:rgba(226,234,242,255);opacity:80%;stroke:rgba(61,200,229,255);stroke-width:25""></rect>
              <foreignObject x=""80"" y=""700"" width=""1800"" height=""1200"">
                <p style=""font: 400px sans-serif; font-weight:bold; user-select:none; color:rgba(9,160,229,255); text-align:center;"">{spaceName}</p>
              </foreignObject>
            </svg>
            <g  ></g>
            <g>
              <text font-size=""12"" transform=""translate(38,14)"" fill=""gray"" style=""user-select:none; cursor: pointer"">X</text>
            </g>");
    }
    
    [Test]
    public void Constructor_PassesCorrectValuesToDraggable_PathWayCondition()
    {
        var pathWayCondition = new PathWayConditionViewModel(ConditionEnum.And, 2, 1);
        var onClicked = new Action<IObjectInPathWayViewModel>(_ => { });
        var onDragged = new DraggedEventArgs<IObjectInPathWayViewModel>.DraggedEventHandler((_,_) => { });
        var onDoubleClicked = new Action<IObjectInPathWayViewModel>(_ => { });
        var onRightClicked = new Action<IObjectInPathWayViewModel>(_ => { });
        const bool showingRightClickMenu = false;
        var onEditPathWayCondition = new Action<PathWayConditionViewModel>(_ => { });
        var onDeletePathWayCondition = new Action<PathWayConditionViewModel>(_ => { });
        var onCloseRightClickMenu = new Action(() => { });
        var positioningService = Substitute.For<ILearningWorldPresenter>();
        var systemUnderTest =
            GetRenderedDraggablePathWayCondition(pathWayCondition, onClicked, onDragged, onDoubleClicked, onRightClicked,
                showingRightClickMenu, onEditPathWayCondition, onDeletePathWayCondition, onCloseRightClickMenu, positioningService);

        Assert.That(systemUnderTest.HasComponent<Stub<Draggable<IObjectInPathWayViewModel>>>());
        var stub = systemUnderTest.FindComponent<Stub<Draggable<IObjectInPathWayViewModel>>>();
        Assert.Multiple(() =>
        {
            Assert.That(stub.Instance.Parameters[nameof(Draggable<PathWayConditionViewModel>.LearningObject)], Is.EqualTo(pathWayCondition));
            //overriding nullability warning because we know target isn't null as onClicked isn't a static method but instead a lambda -n.stich
            Assert.That(stub.Instance.Parameters[nameof(Draggable<PathWayConditionViewModel>.OnClicked)], Is.EqualTo(EventCallback.Factory.Create(onClicked.Target!, onClicked)));
            Assert.That(stub.Instance.Parameters[nameof(Draggable<PathWayConditionViewModel>.ChildContent)], Is.Not.Null);
        });
        var childContent = _ctx.Render((RenderFragment)stub.Instance.Parameters[nameof(Draggable<PathWayConditionViewModel>.ChildContent)]);
        childContent.MarkupMatches(
            @"<rect x=""0"" y=""0"" width=""75"" height=""41.5"" rx=""2"" style=""fill:rgba(226,234,242,255);opacity:80%;stroke:rgba(61,200,229,255);stroke-width:1""></rect>
              <g  ></g>
              <g  >
                <text font-size=""12"" transform=""translate(65,12)"" fill=""gray"" style=""user-select:none; cursor: pointer"">X</text>
              </g>");
    }

    [Test]
    public void Constructor_SpaceNull_ThrowsException()
    {
        //Override warning for this test as we are testing exactly what happens when we break the nullability contract - n.stich
        Assert.That(
            () => GetRenderedDraggableLearningSpace(null!, _ => { }, (_, _) => { }, _ => { }, _ => { }, false, _ => { },
                _ => { }, _ => { }, _ => { } ,() => { }, null!), Throws.ArgumentNullException);
    }
    
    [Test]
    public void Constructor_ConditionNull_ThrowsException()
    {
        //Override warning for this test as we are testing exactly what happens when we break the nullability contract - n.stich
        Assert.That(
            () => GetRenderedDraggablePathWayCondition(null!, _ => { }, (_, _) => { }, _ => { }, _ => { }, false, _ => { },_ => { },
                 () => { }, null!), Throws.ArgumentNullException);
    }

    private IRenderedComponent<DraggableLearningSpace> GetRenderedDraggableLearningSpace(
        IObjectInPathWayViewModel objectViewmodel, Action<IObjectInPathWayViewModel> onClicked, 
        DraggedEventArgs<IObjectInPathWayViewModel>.DraggedEventHandler onDragged, Action<IObjectInPathWayViewModel> onDoubleClicked,
        Action<IObjectInPathWayViewModel> onRightClicked, bool showingRightClickMenu, 
        Action<ILearningSpaceViewModel> onOpenLearningSpace, Action<ILearningSpaceViewModel> onEditLearningSpace, 
        Action<ILearningSpaceViewModel> onDeleteLearningSpace, Action<ILearningSpaceViewModel> onRemoveLearningSpaceFromTopic, Action onCloseRightClickMenu,
        ILearningWorldPresenter positioningService)
    {
        return _ctx.RenderComponent<DraggableLearningSpace>(parameters => parameters
            .Add(p => p.ObjectInPathWay, objectViewmodel)
            .Add(p => p.OnClickedDraggable, onClicked)
            .Add(p => p.OnDraggedDraggable, onDragged)
            .Add(p=>p.OnDoubleClickedDraggable, onDoubleClicked)
            .Add(p=>p.OnRightClickedDraggable, onRightClicked)
            .Add(p=>p.ShowingRightClickMenu, showingRightClickMenu)
            .Add(p=>p.OnOpenLearningSpace, onOpenLearningSpace)
            .Add(p=>p.OnEditLearningSpace, onEditLearningSpace)
            .Add(p=>p.OnDeleteLearningSpace, onDeleteLearningSpace)
            .Add(p=>p.OnRemoveLearningSpaceFromTopic, onRemoveLearningSpaceFromTopic)
            .Add(p=>p.OnCloseRightClickMenu, onCloseRightClickMenu)
            .Add(p=>p.PositioningService, positioningService)
        );
    }
    
    private IRenderedComponent<DraggablePathWayCondition> GetRenderedDraggablePathWayCondition(
        IObjectInPathWayViewModel objectViewmodel, Action<IObjectInPathWayViewModel> onClicked, 
        DraggedEventArgs<IObjectInPathWayViewModel>.DraggedEventHandler onDragged, Action<IObjectInPathWayViewModel> onDoubleClicked,
        Action<IObjectInPathWayViewModel> onRightClicked, bool showingRightClickMenu,Action<PathWayConditionViewModel> onEditPathWayCondition, 
        Action<PathWayConditionViewModel> onDeletePathWayCondition, Action onCloseRightClickMenu,
        ILearningWorldPresenter positioningService)
    {
        return _ctx.RenderComponent<DraggablePathWayCondition>(parameters => parameters
            .Add(p => p.ObjectInPathWay, objectViewmodel)
            .Add(p => p.OnClickedDraggable, onClicked)
            .Add(p => p.OnDraggedDraggable, onDragged)
            .Add(p=>p.OnDoubleClickedDraggable, onDoubleClicked)
            .Add(p=>p.OnRightClickedDraggable, onRightClicked)
            .Add(p=>p.ShowingRightClickMenu, showingRightClickMenu)
            .Add(p=>p.OnDeletePathWayCondition, onDeletePathWayCondition)
            .Add(p=>p.OnCloseRightClickMenu, onCloseRightClickMenu)
            .Add(p=>p.PositioningService, positioningService)
        );
    }

}