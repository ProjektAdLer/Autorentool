using System;
using AuthoringTool.BusinessLogic;
using AuthoringTool.Components;
using AuthoringTool.PresentationLogic;
using AuthoringTool.PresentationLogic.LearningSpace;
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
    private TestContext? _testContext;

    [SetUp]
    public void Setup()
    {
        _testContext = new TestContext();
        var service = Substitute.For<IMouseService>();
        _testContext.Services.AddSingleton<IMouseService>(service);
    }

    [TearDown]
    public void TearDown() => _testContext?.Dispose();

    [Test]
    public void Draggable_StandardConstructor_AllPropertiesInitialized()
    {
        RenderFragment childContent = new RenderFragment(builder => builder.AddContent(0, "<text/>"));
        ILearningObjectViewModel learningObject =
            new LearningSpaceViewModel("name", "shortname", "authors", "description", "goals");
        double x = 10;
        double y = 20;
        Action<double> xChanged = _ => { };
        Action<double> yChanged = _ => { };
        Action<ILearningObjectViewModel> onClicked = _ => { };

        var systemUnderTest =
            CreateRenderedDraggableComponent(childContent, learningObject, x, y, xChanged, yChanged, onClicked);

        Assert.AreEqual(childContent, systemUnderTest.Instance.ChildContent);
        Assert.AreEqual(learningObject, systemUnderTest.Instance.LearningObject);
        Assert.AreEqual(x, systemUnderTest.Instance.X);
        Assert.AreEqual(y, systemUnderTest.Instance.Y);
        Assert.AreEqual(
            EventCallback.Factory.Create(
                xChanged.Target ?? throw new InvalidOperationException("xChanged.Target is null"), xChanged),
            systemUnderTest.Instance.XChanged);
        Assert.AreEqual(
            EventCallback.Factory.Create(
                yChanged.Target ?? throw new InvalidOperationException("yChanged.Target is null"), yChanged),
            systemUnderTest.Instance.YChanged);
        Assert.AreEqual(
            EventCallback.Factory.Create(
                onClicked.Target ?? throw new InvalidOperationException("onClicked.Target is null"), onClicked),
            systemUnderTest.Instance.OnClicked);
    }

    [Test]
    public void Draggable_ClickAndRelease_OnClickedTriggered()
    {
        ILearningObjectViewModel onClickedEventTriggered = null;
        ILearningObjectViewModel learningObject =
            new LearningSpaceViewModel("name", "shortname", "authors", "description", "goals");

        Action<ILearningObjectViewModel> onClicked = e => { onClickedEventTriggered = e; };

        var systemUnderTest =
            CreateRenderedDraggableComponent(null, learningObject, 0, 0, _ => { }, _ => { }, onClicked);

        systemUnderTest.WaitForElement("g").MouseDown(new MouseEventArgs());
        _testContext.Services.GetService<IMouseService>().OnUp += Raise.EventWith(new MouseEventArgs());

        Assert.AreEqual(learningObject, onClickedEventTriggered);
    }

    [Test]
    public void Draggable_ClickMoveAndRelease_OnClickedNotTriggered()
    {
        ILearningObjectViewModel onClickedEventTriggered = null;
        ILearningObjectViewModel learningObject =
            new LearningSpaceViewModel("name", "shortname", "authors", "description", "goals");

        Action<ILearningObjectViewModel> onClicked = e => { onClickedEventTriggered = e; };

        var systemUnderTest =
            CreateRenderedDraggableComponent(null, learningObject, 0, 0, _ => { }, _ => { }, onClicked);

        systemUnderTest.WaitForElement("g").MouseDown(new MouseEventArgs());
        _testContext.Services.GetService<IMouseService>().OnMove += Raise.EventWith(new MouseEventArgs());
        _testContext.Services.GetService<IMouseService>().OnUp += Raise.EventWith(new MouseEventArgs());

        Assert.AreEqual(null, onClickedEventTriggered);
    }

    [Test]
    public void Draggable_ClickMoveAndRelease_PositionChanged()
    {
        ILearningObjectViewModel onClickedEventTriggered = null;
        ILearningObjectViewModel learningObject =
            new LearningSpaceViewModel("name", "shortname", "authors", "description", "goals");

        double x = 10;
        double y = 20;
        Action<ILearningObjectViewModel> onClicked = e => { onClickedEventTriggered = e; };

        var systemUnderTest =
            CreateRenderedDraggableComponent(null, learningObject, x, y, _ => { }, _ => { }, onClicked);

        systemUnderTest.WaitForElement("g").MouseDown(new MouseEventArgs());
        _testContext.Services.GetService<IMouseService>().OnMove +=
            Raise.EventWith(new MouseEventArgs {ClientX = 13, ClientY = 24});
        _testContext.Services.GetService<IMouseService>().OnUp += Raise.EventWith(new MouseEventArgs());


        Assert.AreEqual(x + 13, systemUnderTest.Instance.X);
        Assert.AreEqual(y + 24, systemUnderTest.Instance.Y);
    }

    [Test]
    public void Draggable_MoveAndRelease_PositionNotChanged()
    {
        ILearningObjectViewModel onClickedEventTriggered = null;
        ILearningObjectViewModel learningObject =
            new LearningSpaceViewModel("name", "shortname", "authors", "description", "goals");

        double x = 10;
        double y = 20;
        Action<ILearningObjectViewModel> onClicked = e => { onClickedEventTriggered = e; };

        var systemUnderTest =
            CreateRenderedDraggableComponent(null, learningObject, x, y, _ => { }, _ => { }, onClicked);

        _testContext.Services.GetService<IMouseService>().OnMove +=
            Raise.EventWith(new MouseEventArgs {ClientX = 13, ClientY = 24});
        _testContext.Services.GetService<IMouseService>().OnUp += Raise.EventWith(new MouseEventArgs());


        Assert.AreEqual(x, systemUnderTest.Instance.X);
        Assert.AreEqual(y, systemUnderTest.Instance.Y);
    }


    private IRenderedComponent<Draggable> CreateRenderedDraggableComponent(RenderFragment? childContent = null,
        ILearningObjectViewModel? learningObject = null, double x = 0, double y = 0, Action<double>? xChanged = null,
        Action<double>? yChanged = null, Action<ILearningObjectViewModel>? onClicked = null)
    {
        return _testContext.RenderComponent<Draggable>(parameters => parameters
            .Add(p => p.ChildContent, childContent)
            .Add(p => p.LearningObject, learningObject)
            .Add(p => p.X, x)
            .Add(p => p.Y, y)
            .Add(p => p.XChanged, xChanged)
            .Add(p => p.YChanged, yChanged)
            .Add(p => p.OnClicked, onClicked)
        );
    }
}