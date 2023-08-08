using System;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.LearningElement;
using TestContext = Bunit.TestContext;

namespace PresentationTest.Components;

[TestFixture]
public class DraggableUt
{
    [SetUp]
    public void Setup()
    {
        _testContext = new TestContext();
        _mouseService = Substitute.For<IMouseService>();
        _testContext.Services.AddSingleton(_mouseService);
    }

    [TearDown]
    public void TearDown() => _testContext.Dispose();

    private TestContext _testContext;
    private IMouseService _mouseService;

    [Test]
    public void StandardConstructor_AllPropertiesInitialized()
    {
        RenderFragment childContent = builder => builder.AddContent(0, "<text/>");
        var learningObject = Substitute.For<ILearningElementViewModel>();
        double x = 10;
        double y = 20;
        Action<double> xChanged = _ => { };
        Action<double> yChanged = _ => { };
        Action<ILearningElementViewModel> onClicked = _ => { };

        var systemUnderTest =
            CreateRenderedDraggableComponent(childContent, learningObject, x, y, xChanged, yChanged, onClicked);

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.ChildContent, Is.EqualTo(childContent));
            Assert.That(systemUnderTest.Instance.LearningObject, Is.EqualTo(learningObject));
            Assert.That(systemUnderTest.Instance.X, Is.EqualTo(x));
            Assert.That(systemUnderTest.Instance.Y, Is.EqualTo(y));
            Assert.That(
                systemUnderTest.Instance.XChanged, Is.EqualTo(EventCallback.Factory.Create(
                    xChanged.Target ?? throw new InvalidOperationException("xChanged.Target is null"), xChanged)));
            Assert.That(
                systemUnderTest.Instance.YChanged, Is.EqualTo(EventCallback.Factory.Create(
                    yChanged.Target ?? throw new InvalidOperationException("yChanged.Target is null"), yChanged)));
            Assert.That(
                systemUnderTest.Instance.OnClicked, Is.EqualTo(EventCallback.Factory.Create(
                    onClicked.Target ?? throw new InvalidOperationException("onClicked.Target is null"), onClicked)));
        });
    }

    [Test]
    public void ClickAndRelease_OnClickedTriggered()
    {
        ILearningElementViewModel? onClickedEventTriggered = null;
        var learningObject = Substitute.For<ILearningElementViewModel>();

        Action<ILearningElementViewModel> onClicked = e => { onClickedEventTriggered = e; };

        var systemUnderTest =
            CreateRenderedDraggableComponent(null, learningObject, 0, 0, _ => { }, _ => { }, onClicked);

        systemUnderTest.WaitForElement("g").MouseDown(new MouseEventArgs());
        _mouseService.OnUp += Raise.EventWith(new MouseEventArgs());

        Assert.That(onClickedEventTriggered, Is.EqualTo(learningObject));
    }

    [Test]
    public void ClickMoveAndRelease_OnClickedNotTriggered()
    {
        ILearningElementViewModel? onClickedEventTriggered = null;
        var learningObject = Substitute.For<ILearningElementViewModel>();

        Action<ILearningElementViewModel> onClicked = e => { onClickedEventTriggered = e; };

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
        var learningObject = Substitute.For<ILearningElementViewModel>();

        double x = 10;
        double y = 20;
        Action<ILearningElementViewModel> onClicked = _ => { };

        var systemUnderTest =
            CreateRenderedDraggableComponent(null, learningObject, x, y, _ => { }, _ => { }, onClicked);

        systemUnderTest.WaitForElement("g").MouseDown(new MouseEventArgs());
        _mouseService.OnMove +=
            Raise.EventWith(new MouseEventArgs { ClientX = 13, ClientY = 24 });
        _mouseService.OnUp += Raise.EventWith(new MouseEventArgs());

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.X, Is.EqualTo(x + 13));
            Assert.That(systemUnderTest.Instance.Y, Is.EqualTo(y + 24));
        });
    }

    [Test]
    public void ClickMoveAndRelease_OnDraggedTriggered()
    {
        var learningObject = Substitute.For<ILearningElementViewModel>();
        var onDraggedEventTriggered = false;
        DraggedEventArgs<ILearningElementViewModel>? onDraggedEventArgs = null;

        double x = 10;
        double y = 20;
        Action<ILearningElementViewModel> onClicked = _ => { };
        DraggedEventArgs<ILearningElementViewModel>.DraggedEventHandler onDragged = (_, args) =>
        {
            onDraggedEventTriggered = true;
            onDraggedEventArgs = args;
        };

        var systemUnderTest =
            CreateRenderedDraggableComponent(null, learningObject, x, y, _ => { }, _ => { }, onClicked, onDragged);

        systemUnderTest.WaitForElement("g").MouseDown(new MouseEventArgs());
        _mouseService.OnMove +=
            Raise.EventWith(new MouseEventArgs { ClientX = 13, ClientY = 24 });
        _mouseService.OnUp += Raise.EventWith(new MouseEventArgs());

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.X, Is.EqualTo(x + 13));
            Assert.That(systemUnderTest.Instance.Y, Is.EqualTo(y + 24));
        });

        Assert.That(onDraggedEventTriggered, Is.True);
        Assert.That(onDraggedEventArgs, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(onDraggedEventArgs!.LearningObject, Is.EqualTo(learningObject));
            Assert.That(onDraggedEventArgs.OldPositionX, Is.EqualTo(10));
            Assert.That(onDraggedEventArgs.OldPositionY, Is.EqualTo(20));
        });
    }

    [Test]
    public void MoveAndReleaseWithoutPreviousClick_PositionNotChanged()
    {
        var learningObject = Substitute.For<ILearningElementViewModel>();

        double x = 10;
        double y = 20;
        Action<ILearningElementViewModel> onClicked = _ => { };

        var systemUnderTest =
            CreateRenderedDraggableComponent(null, learningObject, x, y, _ => { }, _ => { }, onClicked);

        _mouseService.OnMove +=
            Raise.EventWith(new MouseEventArgs { ClientX = 13, ClientY = 24 });
        _mouseService.OnUp += Raise.EventWith(new MouseEventArgs());

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.X, Is.EqualTo(x));
            Assert.That(systemUnderTest.Instance.Y, Is.EqualTo(y));
        });
    }

    [Test]
    public void DoubleClick_OnDoubleClickTriggered()
    {
        var learningObject = Substitute.For<ILearningElementViewModel>();
        ILearningElementViewModel? onDoubleClickEventTriggered = null;

        Action<ILearningElementViewModel> onDoubleClick = e => { onDoubleClickEventTriggered = e; };

        var systemUnderTest =
            CreateRenderedDraggableComponent(null, learningObject, onDoubleClicked: onDoubleClick);

        systemUnderTest.WaitForElement("g").MouseDown(new MouseEventArgs());
        _mouseService.OnUp += Raise.EventWith(new MouseEventArgs());
        systemUnderTest.WaitForElement("g").MouseDown(new MouseEventArgs());
        _mouseService.OnUp += Raise.EventWith(new MouseEventArgs());

        Assert.That(onDoubleClickEventTriggered, Is.EqualTo(learningObject));
    }

    [Test]
    public void RightClick_OnRightClickedTriggered()
    {
        var learningObject = Substitute.For<ILearningElementViewModel>();
        ILearningElementViewModel? onRightClickedEventTriggered = null;

        Action<ILearningElementViewModel> onRightClicked = e => { onRightClickedEventTriggered = e; };

        var systemUnderTest =
            CreateRenderedDraggableComponent(null, learningObject, onRightClicked: onRightClicked);

        systemUnderTest.WaitForElement("g").MouseDown(new MouseEventArgs { Button = 2 });
        _mouseService.OnUp += Raise.EventWith(new MouseEventArgs { Button = 2 });

        Assert.That(onRightClickedEventTriggered, Is.EqualTo(learningObject));
    }

    private IRenderedComponent<Draggable<ILearningElementViewModel>> CreateRenderedDraggableComponent(
        RenderFragment? childContent = null,
        ILearningElementViewModel? learningObject = null, double x = 0, double y = 0, Action<double>? xChanged = null,
        Action<double>? yChanged = null, Action<ILearningElementViewModel>? onClicked = null,
        DraggedEventArgs<ILearningElementViewModel>.DraggedEventHandler? onDragged = null,
        Action<ILearningElementViewModel>? onDoubleClicked = null,
        Action<ILearningElementViewModel>? onRightClicked = null)
    {
        xChanged ??= _ => { };
        yChanged ??= _ => { };
        onClicked ??= _ => { };
        onDragged ??= (_, _) => { };
        onDoubleClicked ??= _ => { };
        onRightClicked ??= _ => { };
        return _testContext.RenderComponent<Draggable<ILearningElementViewModel>>(parameters => parameters
            .Add(p => p.ChildContent, childContent)
            .Add(p => p.LearningObject, learningObject)
            .Add(p => p.X, x)
            .Add(p => p.Y, y)
            .Add(p => p.XChanged, xChanged)
            .Add(p => p.YChanged, yChanged)
            .Add(p => p.OnClicked, onClicked)
            .Add(p => p.OnDragged, onDragged)
            .Add(p => p.OnDoubleClicked, onDoubleClicked)
            .Add(p => p.OnRightClicked, onRightClicked)
        );
    }
}