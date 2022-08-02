using System;
using AuthoringTool.Components;
using AuthoringTool.PresentationLogic;
using AuthoringTool.PresentationLogic.AuthoringToolWorkspace;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;
using TestContext = Bunit.TestContext;

namespace AuthoringToolTest.Components;

[TestFixture]
public class DraggableUt
{
#pragma warning disable CS8618
    private TestContext _testContext;
    private IMouseService _mouseService;
#pragma warning restore CS8618

    [SetUp]
    public void Setup()
    {
        _testContext = new TestContext();
        _mouseService = Substitute.For<IMouseService>();
        _testContext.Services.AddSingleton(_mouseService);
    }

    [TearDown]
    public void TearDown() => _testContext.Dispose();

    [Test]
    public void StandardConstructor_AllPropertiesInitialized()
    {
        RenderFragment childContent = builder => builder.AddContent(0, "<text/>");
        var learningObject = Substitute.For<ILearningObjectViewModel>();
        double x = 10;
        double y = 20;
        Action<double> xChanged = _ => { };
        Action<double> yChanged = _ => { };
        Action<ILearningObjectViewModel> onClicked = _ => { };

        var systemUnderTest =
            CreateRenderedDraggableComponent(childContent, learningObject, x, y, xChanged, yChanged, onClicked);

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.ChildContent, Is.EqualTo(childContent));
            Assert.That(systemUnderTest.Instance.LearningObject, Is.EqualTo(learningObject));
            Assert.That(systemUnderTest.Instance.X, Is.EqualTo(x));
            Assert.That(systemUnderTest.Instance.Y, Is.EqualTo(y));
            Assert.That(
                systemUnderTest.Instance.OnClicked, Is.EqualTo(EventCallback.Factory.Create(
                    onClicked.Target ?? throw new InvalidOperationException("onClicked.Target is null"), onClicked)));
        });
    }

    [Test]
    public void ClickAndRelease_OnClickedTriggered()
    {
        ILearningObjectViewModel? onClickedEventTriggered = null;
        var learningObject = Substitute.For<ILearningObjectViewModel>();

        Action<ILearningObjectViewModel> onClicked = e => { onClickedEventTriggered = e; };

        var systemUnderTest =
            CreateRenderedDraggableComponent(null, learningObject, 0, 0, _ => { }, _ => { }, onClicked);

        systemUnderTest.WaitForElement("g").MouseDown(new MouseEventArgs());
        _mouseService.OnUp += Raise.EventWith(new MouseEventArgs());

        Assert.That(onClickedEventTriggered, Is.EqualTo(learningObject));
    }

    [Test]
    public void ClickMoveAndRelease_OnClickedNotTriggered()
    {
        ILearningObjectViewModel? onClickedEventTriggered = null;
        var learningObject = Substitute.For<ILearningObjectViewModel>();

        Action<ILearningObjectViewModel> onClicked = e => { onClickedEventTriggered = e; };

        var systemUnderTest =
            CreateRenderedDraggableComponent(null, learningObject, 0, 0, _ => { }, _ => { }, onClicked);

        systemUnderTest.WaitForElement("g").MouseDown(new MouseEventArgs());
        _mouseService.OnMove += Raise.EventWith(new MouseEventArgs());
        _mouseService.OnUp += Raise.EventWith(new MouseEventArgs());

        Assert.That(onClickedEventTriggered, Is.EqualTo(null));
    }

    [Test]
    public void ClickMoveAndRelease_PositionChanged()
    {
        var learningObject = Substitute.For<ILearningObjectViewModel>();

        double x = 10;
        double y = 20;
        Action<ILearningObjectViewModel> onClicked = _ => { };

        var systemUnderTest =
            CreateRenderedDraggableComponent(null, learningObject, x, y, _ => { }, _ => { }, onClicked);

        systemUnderTest.WaitForElement("g").MouseDown(new MouseEventArgs());
        _mouseService.OnMove +=
            Raise.EventWith(new MouseEventArgs {ClientX = 13, ClientY = 24});
        _mouseService.OnUp += Raise.EventWith(new MouseEventArgs());

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.X, Is.EqualTo(x + 13));
            Assert.That(systemUnderTest.Instance.Y, Is.EqualTo(y + 24));
        });
    }

    [Test]
    public void MoveAndReleaseWithoutPreviousClick_PositionNotChanged()
    {
        var learningObject = Substitute.For<ILearningObjectViewModel>();

        double x = 10;
        double y = 20;
        Action<ILearningObjectViewModel> onClicked = _ => { };

        var systemUnderTest =
            CreateRenderedDraggableComponent(null, learningObject, x, y, _ => { }, _ => { }, onClicked);

        _mouseService.OnMove +=
            Raise.EventWith(new MouseEventArgs {ClientX = 13, ClientY = 24});
        _mouseService.OnUp += Raise.EventWith(new MouseEventArgs());

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.X, Is.EqualTo(x));
            Assert.That(systemUnderTest.Instance.Y, Is.EqualTo(y));
        });
    }

    private IRenderedComponent<Draggable> CreateRenderedDraggableComponent(RenderFragment? childContent = null,
        ILearningObjectViewModel? learningObject = null, double x = 0, double y = 0, Action<double>? xChanged = null,
        Action<double>? yChanged = null, Action<ILearningObjectViewModel>? onClicked = null)
    {
        xChanged ??= _ => { };
        yChanged ??= _ => { };
        onClicked ??= _ => { };
        return _testContext.RenderComponent<Draggable>(parameters => parameters
            .Add(p => p.ChildContent, childContent)
            .Add(p => p.LearningObject, learningObject)
            .Add(p => p.X, x)
            .Add(p => p.Y, y)
            .Add(p => p.OnClicked, onClicked)
        );
    }
}