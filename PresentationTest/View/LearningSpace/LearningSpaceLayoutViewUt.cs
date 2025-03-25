using System;
using System.Collections.Generic;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using NSubstitute;
using NUnit.Framework;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningSpace.SpaceLayout.FloorPlans;
using Presentation.PresentationLogic.SelectedViewModels;
using Presentation.View.LearningSpace;
using TestContext = Bunit.TestContext;

namespace PresentationTest.View.LearningSpace;

[TestFixture]
public class LearningSpaceLayoutViewUt
{
    [SetUp]
    public void Setup()
    {
        _ctx = new TestContext();
        _learningSpacePresenter = Substitute.For<ILearningSpacePresenter>();
        _ctx.Services.AddSingleton(_learningSpacePresenter);
        _selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        _ctx.Services.AddSingleton(_selectedViewModelsProvider);
        _dimensions = new LearningSpaceLayoutView.Dimensions
        {
            Width = 1920,
            Height = 1080
        };
        _jsRuntime = Substitute.For<IJSRuntime>();
        _jsRuntime.InvokeAsync<LearningSpaceLayoutView.Dimensions>("getScreenDimensions")
            .Returns(_dimensions);
        _ctx.Services.AddSingleton(_jsRuntime);
    }

    [TearDown]
    public void TearDown()
    {
        _ctx.Dispose();
    }

    private TestContext _ctx = null!;
    private ILearningSpacePresenter _learningSpacePresenter = null!;
    private ISelectedViewModelsProvider _selectedViewModelsProvider = null!;
    private IJSRuntime _jsRuntime;
    private LearningSpaceLayoutView.Dimensions _dimensions;

    private const string BackgroundOpen =
        @"<div class=""w-full min-h-[265px] bg-adlergreybright border-2 border-b-adlerdeactivated"">";

    private const string ShadowOpen = @"<div class=""drop-shadow-xl w-full h-full"">";

    private const string FloorPlanClass =
        @"<div class=""flex flex-col w-full h-full""><div class=""h-full relative"" style=""background-image: url('data:image/svg+xml;utf8,";

    private const string FloorPlanStyleOpen =
        @"&lt;svg xmlns=&quot;http://www.w3.org/2000/svg&quot; preserveAspectRatio=&quot;xMidYMid meet&quot; viewBox=&quot;-1 -1 34 34&quot;&gt;&lt;polygon points=&quot;";

    private const string FloorPlanStyleMid =
        @"&quot; style=&quot;fill:%23e9f2fa; stroke:rgb(204,204,204); stroke-width:0.2&quot; />";

    private const string FloorPlanStyleClose =
        @"</svg>'); background-size: 100% 100%; background-repeat: no-repeat; background-position: center; "">";

    private const string DivClose = @"</div></div>";


    [Test]
    public void Render_LearningSpaceLayoutWithZeroCapacity_RendersFrame()
    {
        var systemUnderTest = GetRenderedLearningSpaceLayoutView();

        // These two MarkupMatches calls are equivalent
        systemUnderTest.MarkupMatches(
            @"<div class=""flex flex-col w-full h-full"">
                  <div class=""h-full relative"" style=""background-image: url('data:image/svg+xml;utf8,<svg xmlns=&quot;http://www.w3.org/2000/svg&quot; preserveAspectRatio=&quot;xMidYMid meet&quot; viewBox=&quot;-1 -1 34 34&quot;><polygon points=&quot;&quot; style=&quot;fill:%23e9f2fa; stroke:rgb(204,204,204); stroke-width:0.2&quot; /></svg>'); background-size: 100% 100%; background-repeat: no-repeat; background-position: center;""></div>
              </div>"
        );
    }

    [Test]
    public void Render_CornersAndDoors()
    {
        // Arrange
        var floorPlanViewModel = Substitute.For<IFloorPlanViewModel>();
        floorPlanViewModel.CornerPoints.Returns(new List<Point>
            { new() { X = 0, Y = 1 }, new() { X = 2, Y = 3 }, new() { X = 4, Y = 5 }, new() { X = 6, Y = 7 } });
        const string expectedCorners = "0,1 2,3 4,5 6,7 ";
        floorPlanViewModel.DoorPositions.Returns(new List<(Point, Point)>
            { (new Point { X = 8, Y = 9 }, new Point { X = 10, Y = 11 }) });
        const string expectedDoors =
            "<line x1=&quot;8&quot; y1=&quot;9&quot; x2=&quot;10&quot; y2=&quot;11&quot; style=&quot;stroke:rgb(204,204,204);stroke-width:0.5&quot; />";
        var learningSpaceViewModel = Substitute.For<ILearningSpaceViewModel>();
        learningSpaceViewModel.LearningSpaceLayout.FloorPlanViewModel.Returns(floorPlanViewModel);

        // Act
        var systemUnderTest = GetRenderedLearningSpaceLayoutView(learningSpaceViewModel);

        // Assert
        var floorPlan = systemUnderTest.Find("div.w-full");
        floorPlan.MarkupMatches(FloorPlanClass + FloorPlanStyleOpen + expectedCorners + FloorPlanStyleMid +
                                expectedDoors + FloorPlanStyleClose + DivClose);
    }

    [Test]
    public void Render_Corners()
    {
        // Arrange
        var floorPlanViewModel = Substitute.For<IFloorPlanViewModel>();
        floorPlanViewModel.CornerPoints.Returns(new List<Point>
            { new() { X = 0, Y = 1 }, new() { X = 2, Y = 3 }, new() { X = 4, Y = 5 }, new() { X = 6, Y = 7 } });
        const string expectedCorners = "0,1 2,3 4,5 6,7 ";
        var learningSpaceViewModel = Substitute.For<ILearningSpaceViewModel>();
        learningSpaceViewModel.LearningSpaceLayout.FloorPlanViewModel.Returns(floorPlanViewModel);

        // Act
        var systemUnderTest = GetRenderedLearningSpaceLayoutView(learningSpaceViewModel);

        // Assert
        var floorPlan = systemUnderTest.Find("div.w-full");
        floorPlan.MarkupMatches(FloorPlanClass + FloorPlanStyleOpen + expectedCorners + FloorPlanStyleMid +
                                FloorPlanStyleClose + DivClose);
    }

    [Test]
    public void Render_Doors()
    {
        // Arrange
        var floorPlanViewModel = Substitute.For<IFloorPlanViewModel>();
        floorPlanViewModel.DoorPositions.Returns(new List<(Point, Point)>
            { (new Point { X = 0, Y = 1 }, new Point { X = 2, Y = 3 }) });
        const string expectedDoors =
            "<line x1=&quot;0&quot; y1=&quot;1&quot; x2=&quot;2&quot; y2=&quot;3&quot; style=&quot;stroke:rgb(204,204,204);stroke-width:0.5&quot; />";
        var learningSpaceViewModel = Substitute.For<ILearningSpaceViewModel>();
        learningSpaceViewModel.LearningSpaceLayout.FloorPlanViewModel.Returns(floorPlanViewModel);

        // Act
        var systemUnderTest = GetRenderedLearningSpaceLayoutView(learningSpaceViewModel);

        // Assert
        var floorPlan = systemUnderTest.Find("div.w-full");
        floorPlan.MarkupMatches(FloorPlanClass + FloorPlanStyleOpen + FloorPlanStyleMid + expectedDoors +
                                FloorPlanStyleClose + DivClose);
    }

    [Test]
    [TestCase(2560)]
    [TestCase(1920)]
    [TestCase(1280)]
    [TestCase(800)]
    public void Render_LearningElementSlots(int screenWidth)
    {
        // Arrange
        var testDimensions = new LearningSpaceLayoutView.Dimensions
        {
            Width = screenWidth,
            Height = 1080
        };
        _jsRuntime.InvokeAsync<LearningSpaceLayoutView.Dimensions>("getScreenDimensions")
            .Returns(testDimensions);
        var floorPlanViewModel = Substitute.For<IFloorPlanViewModel>();
        floorPlanViewModel.ElementSlotPositions.Returns(new List<Point>
        {
            new() { X = 0, Y = 1 }, new() { X = 2, Y = 3 }, new() { X = 4, Y = 5 }, new() { X = 6, Y = 7 }
        });
        var learningSpaceViewModel = Substitute.For<ILearningSpaceViewModel>();
        learningSpaceViewModel.LearningSpaceLayout.FloorPlanViewModel.Returns(floorPlanViewModel);
        learningSpaceViewModel.LearningSpaceLayout.Capacity.Returns(4);
        var expectedId = Guid.NewGuid();
        learningSpaceViewModel.Id.Returns(expectedId);

        // Act
        var systemUnderTest = GetRenderedLearningSpaceLayoutView(learningSpaceViewModel);
        // Assert
        var dropZones = systemUnderTest.FindAll("div.mud-drop-zone");
        Assert.That(dropZones, Has.Count.EqualTo(4));
        for (var i = 0; i < 4; i++)
        {
            Assert.Multiple(() =>
            {
                Assert.That(dropZones[i].Attributes["identifier"]?.Value,
                    Is.EqualTo($"{expectedId.ToString()}_ele_{i}"));
                if (screenWidth < 1536)
                {
                    Assert.That(dropZones[i].Attributes["style"]?.Value,
                        Is.EqualTo(
                            $"position: absolute; " +
                            $"top: {GetSlotPositionPercentValue(floorPlanViewModel.ElementSlotPositions[i].Y, false, screenWidth)}%; " +
                            $"left: {GetSlotPositionPercentValue(floorPlanViewModel.ElementSlotPositions[i].X, true, screenWidth)}%; " +
                            $"transform: translate(-50%, -30%); " +
                            "scale: 0.6;"));
                }
                else
                {
                    Assert.That(dropZones[i].Attributes["style"]?.Value,
                        Is.EqualTo(
                            $"position: absolute; " +
                            $"top: {GetSlotPositionPercentValue(floorPlanViewModel.ElementSlotPositions[i].Y, false, screenWidth)}%; " +
                            $"left: {GetSlotPositionPercentValue(floorPlanViewModel.ElementSlotPositions[i].X, true, screenWidth)}%; " +
                            $"transform: translate(180%, -60%); " +
                            "scale: 0.7;"));
                }
            });
        }
    }

    private static string GetSlotPositionPercentValue(int abs, bool isXAxis, int screenWidth)
    {
        var smallWidth = screenWidth < 1536;

        var viewportWidth = smallWidth ? 34.0 : 50.0;
        var scaleFactor = Math.Min(screenWidth / viewportWidth, 0.6);
        var adjustedWidth = 20.0 * scaleFactor;
        var factor = smallWidth ? isXAxis ? 35 : 33 : isXAxis ? 22 : 35;
        
        return ((int)((abs - 1) / adjustedWidth * factor)).ToString();
    }

    private IRenderedComponent<LearningSpaceLayoutView> GetRenderedLearningSpaceLayoutView(
        ILearningSpaceViewModel? learningSpaceViewModel = null)
    {
        learningSpaceViewModel ??= Substitute.For<ILearningSpaceViewModel>();

        return _ctx.RenderComponent<LearningSpaceLayoutView>(parameters => parameters
            .Add(p => p.LearningSpace, learningSpaceViewModel)
        );
    }
}