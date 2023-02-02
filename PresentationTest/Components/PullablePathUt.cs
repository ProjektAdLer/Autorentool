using Bunit;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.Space;
using Presentation.PresentationLogic.World;
using Shared;
using TestContext = Bunit.TestContext;

namespace PresentationTest.Components;

[TestFixture]

public class PullablePathUt
{
    #pragma warning disable CS8618
    private TestContext _testContext;
    private IMouseService _mouseService;
    private IPositioningService _positioningService;
#pragma warning restore CS8618

    [SetUp]
    public void Setup()
    {
        _testContext = new TestContext();
        _mouseService = Substitute.For<IMouseService>();
        _positioningService = Substitute.For<IPositioningService>();
        _testContext.Services.AddSingleton(_mouseService);
    }

    [TearDown]
    public void TearDown() => _testContext.Dispose();

    [Test]
    public void StandardConstructor_AllPropertiesInitialized()
    {
        var draggableObject = Substitute.For<ISpaceViewModel>();
        double x1 = 5;
        double y1 = 6;

        var systemUnderTest = CreateRenderedPullablePathComponent(draggableObject, x1, y1);

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.DraggableObject, Is.EqualTo(draggableObject));
            Assert.That(systemUnderTest.Instance.X1, Is.EqualTo(x1));
            Assert.That(systemUnderTest.Instance.Y1, Is.EqualTo(y1));
            Assert.That(systemUnderTest.Instance.X2, Is.EqualTo(x1));
            Assert.That(systemUnderTest.Instance.Y2, Is.EqualTo(y1));
        });
    }
    
    [Test]
    public void ClickAndMove_CallsWorldPresenter()
    {
        var space = Substitute.For<ISpaceViewModel>();

        double x1 = 5;
        double y1 = 6;

       var systemUnderTest = CreateRenderedPullablePathComponent(space, x1, y1);
        
        systemUnderTest.WaitForElement("g").MouseDown(new MouseEventArgs());
        _mouseService.OnMove +=
            Raise.EventWith(new MouseEventArgs {ClientX = 13, ClientY = 24});
        _mouseService.OnUp += Raise.EventWith(new MouseEventArgs());
        
        Assert.That(systemUnderTest.Instance.X1, Is.EqualTo(x1));
        Assert.That(systemUnderTest.Instance.Y1, Is.EqualTo(y1));
        Assert.That(systemUnderTest.Instance.X2, Is.EqualTo(x1 + 13));
        Assert.That(systemUnderTest.Instance.Y2, Is.EqualTo(y1 + 24));
        
        _positioningService.Received().SetOnHoveredObjectInPathWay(space, x1+13, y1+24);
        _positioningService.Received().CreatePathWay(space, x1+13, y1+24);
    }

    [Test]
    public void ClickAndNotMoved_X2AndY2Set()
    {
        var space = Substitute.For<ISpaceViewModel>();

        double x1 = 5;
        double y1 = 6;

        var systemUnderTest = CreateRenderedPullablePathComponent(space, x1, y1);
        
        systemUnderTest.WaitForElement("g").MouseDown(new MouseEventArgs());
        _mouseService.OnUp += Raise.EventWith(new MouseEventArgs());
        
        Assert.That(systemUnderTest.Instance.X1, Is.EqualTo(x1));
        Assert.That(systemUnderTest.Instance.Y1, Is.EqualTo(y1));
        Assert.That(systemUnderTest.Instance.X2, Is.EqualTo(x1));
        Assert.That(systemUnderTest.Instance.Y2, Is.EqualTo(y1));
    }
    
    private IRenderedComponent<PullablePath> CreateRenderedPullablePathComponent(
        ISpaceViewModel? space = null, double x1 = 0, double y1 = 0,
        Direction dir1 = Direction.Right, Direction dir2 = Direction.Left)
    {
        return _testContext.RenderComponent<PullablePath>(parameters => parameters
            .Add(p => p.DraggableObject, space)
            .Add(p => p.X1, x1)
            .Add(p => p.Y1, y1)
            .Add(p => p.Direction1, dir1)
            .Add(p => p.Direction2, dir2)
            .Add(p => p.PositioningSrv, _positioningService)
        );
    }
}