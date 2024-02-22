using System;
using System.Collections.Generic;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
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
    }

    [TearDown]
    public void TearDown()
    {
        _ctx.Dispose();
    }

    private TestContext _ctx = null!;
    private ILearningSpacePresenter _learningSpacePresenter = null!;
    private ISelectedViewModelsProvider _selectedViewModelsProvider = null!;

    private const string BackgroundOpen =
        @"<div class=""w-full min-h-[400px] bg-adlergreybright border-2 border-b-adlerdeactivated"">";

    private const string ShadowOpen = @"<div class=""drop-shadow-xl w-full h-full"">";

    private const string FloorPlanClass =
        @"<div class=""mt-15 mx-auto w-[98%] h-[265px] 2xl:h-[420px] 1080p:h-[645px] 2500p:h-[1000px] 3000p:h-[1150px] 3700p:h-[1675px]""";

    private const string FloorPlanStyleOpen =
        @"style=""background-image: url('data:image/svg+xml;utf8,<svg xmlns=&quot;http://www.w3.org/2000/svg&quot; preserveAspectRatio=&quot;none&quot; viewBox=&quot;-1 -1 34 34&quot;><polygon points=&quot;";

    private const string FloorPlanStyleMid =
        @"&quot; style=&quot;fill:%23e9f2fa; stroke:rgba(204,204,204); stroke-width:0.2&quot; />";

    private const string FloorPlanStyleClose =
        @"</svg>'); background-size: 100% 100%; background-repeat: no-repeat; background-position: center; "">";

    private const string DivClose = @"</div>";


    [Test]
    public void Render_LearningSpaceLayoutWithZeroCapacity_RendersFrame()
    {
        var systemUnderTest = GetRenderedLearningSpaceLayoutView();

        // These two MarkupMatches calls are equivalent
        systemUnderTest.MarkupMatches(
            @"<div class=""w-full min-h-[265px] bg-adlergreybright border-2 border-b-adlerdeactivated"">
                  <div class=""drop-shadow-xl w-full h-full"">
                      <div class=""mt-15 mx-auto w-[98%] h-[265px] 2xl:h-[420px] 1080p:h-[645px] 2500p:h-[1000px] 3000p:h-[1150px] 3700p:h-[1675px]""
                           style=""background-image: url('data:image/svg+xml;utf8,<svg xmlns=&quot;http://www.w3.org/2000/svg&quot; preserveAspectRatio=&quot;none&quot; viewBox=&quot;-1 -1 34 34&quot;><polygon points=&quot;&quot; style=&quot;fill:%23e9f2fa; stroke:rgba(204,204,204); stroke-width:0.2&quot; /></svg>'); background-size: 100% 100%; background-repeat: no-repeat; background-position: center; "">
                      </div>
                  </div>
              </div>"
        );
    }

    [Test]
    public void Render_CornersAndDoors()
    {
        // Arrange
        var floorPlanViewModel = Substitute.For<IFloorPlanViewModel>();
        floorPlanViewModel.CornerPoints.Returns(new List<Point>
            {new() {X = 0, Y = 1}, new() {X = 2, Y = 3}, new() {X = 4, Y = 5}, new() {X = 6, Y = 7}});
        const string expectedCorners = "0,1 2,3 4,5 6,7 ";
        floorPlanViewModel.DoorPositions.Returns(new List<(Point, Point)>
            {(new Point {X = 8, Y = 9}, new Point {X = 10, Y = 11})});
        const string expectedDoors =
            "<line x1=&quot;8&quot; y1=&quot;9&quot; x2=&quot;10&quot; y2=&quot;11&quot; style=&quot;stroke:rgba(204,204,204);stroke-width:0.5&quot; />";
        var learningSpaceViewModel = Substitute.For<ILearningSpaceViewModel>();
        learningSpaceViewModel.LearningSpaceLayout.FloorPlanViewModel.Returns(floorPlanViewModel);

        // Act
        var systemUnderTest = GetRenderedLearningSpaceLayoutView(learningSpaceViewModel);

        // Assert
        var floorPlan = systemUnderTest.Find("div.mt-15");
        floorPlan.MarkupMatches(FloorPlanClass + FloorPlanStyleOpen + expectedCorners + FloorPlanStyleMid +
                                expectedDoors + FloorPlanStyleClose + DivClose);
    }

    [Test]
    public void Render_Corners()
    {
        // Arrange
        var floorPlanViewModel = Substitute.For<IFloorPlanViewModel>();
        floorPlanViewModel.CornerPoints.Returns(new List<Point>
            {new() {X = 0, Y = 1}, new() {X = 2, Y = 3}, new() {X = 4, Y = 5}, new() {X = 6, Y = 7}});
        const string expectedCorners = "0,1 2,3 4,5 6,7 ";
        var learningSpaceViewModel = Substitute.For<ILearningSpaceViewModel>();
        learningSpaceViewModel.LearningSpaceLayout.FloorPlanViewModel.Returns(floorPlanViewModel);

        // Act
        var systemUnderTest = GetRenderedLearningSpaceLayoutView(learningSpaceViewModel);

        // Assert
        var floorPlan = systemUnderTest.Find("div.mt-15");
        floorPlan.MarkupMatches(FloorPlanClass + FloorPlanStyleOpen + expectedCorners + FloorPlanStyleMid +
                                FloorPlanStyleClose + DivClose);
    }

    [Test]
    public void Render_Doors()
    {
        // Arrange
        var floorPlanViewModel = Substitute.For<IFloorPlanViewModel>();
        floorPlanViewModel.DoorPositions.Returns(new List<(Point, Point)>
            {(new Point {X = 0, Y = 1}, new Point {X = 2, Y = 3})});
        const string expectedDoors =
            "<line x1=&quot;0&quot; y1=&quot;1&quot; x2=&quot;2&quot; y2=&quot;3&quot; style=&quot;stroke:rgba(204,204,204);stroke-width:0.5&quot; />";
        var learningSpaceViewModel = Substitute.For<ILearningSpaceViewModel>();
        learningSpaceViewModel.LearningSpaceLayout.FloorPlanViewModel.Returns(floorPlanViewModel);

        // Act
        var systemUnderTest = GetRenderedLearningSpaceLayoutView(learningSpaceViewModel);

        // Assert
        var floorPlan = systemUnderTest.Find("div.mt-15");
        floorPlan.MarkupMatches(FloorPlanClass + FloorPlanStyleOpen + FloorPlanStyleMid + expectedDoors +
                                FloorPlanStyleClose + DivClose);
    }

    [Test]
    public void Render_LearningElementSlots()
    {
        // Arrange
        var jsRuntime = _ctx.JSInterop;
        jsRuntime.SetupVoid("mudDragAndDrop.initDropZone", _ => true);

        var floorPlanViewModel = Substitute.For<IFloorPlanViewModel>();
        floorPlanViewModel.ElementSlotPositions.Returns(new List<Point>
        {
            new() {X = 0, Y = 1}, new() {X = 2, Y = 3}, new() {X = 4, Y = 5}, new() {X = 6, Y = 7}
        });
        var learningSpaceViewModel = Substitute.For<ILearningSpaceViewModel>();
        learningSpaceViewModel.LearningSpaceLayout.FloorPlanViewModel.Returns(floorPlanViewModel);
        learningSpaceViewModel.LearningSpaceLayout.Capacity.Returns(4);
        var expectedId = Guid.NewGuid();
        learningSpaceViewModel.Id.Returns(expectedId);

        // Act
        var systemUnderTest = GetRenderedLearningSpaceLayoutView(learningSpaceViewModel);

        // Assert
        Assert.That(jsRuntime.Invocations["mudDragAndDrop.initDropZone"], Has.Count.EqualTo(4));
        var dropZones = systemUnderTest.FindAll("div.mud-drop-zone");
        Assert.That(dropZones, Has.Count.EqualTo(4));
        for (var i = 0; i < 4; i++)
        {
            Assert.Multiple(() =>
            {
                Assert.That(dropZones[i].Attributes["identifier"]?.Value, Is.EqualTo($"{expectedId.ToString()}_ele_{i}"));
                Assert.That(dropZones[i].Attributes["style"]?.Value,
                    Is.EqualTo(
                        $"position: absolute; " +
                        $"top: {GetSlotPositionPercentValue(floorPlanViewModel.ElementSlotPositions[i].Y)}%; " +
                        $"left: {GetSlotPositionPercentValue(floorPlanViewModel.ElementSlotPositions[i].X)}%; " +
                        $"transform: translate(-70%, -100%);"));
            });
        }
    }

    private static string GetSlotPositionPercentValue(int abs)
    {
        return ((int) ((abs + 1) / 35.0 * 100)).ToString();
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