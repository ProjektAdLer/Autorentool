using System;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.LearningSpace;
using TestContext = Bunit.TestContext;

namespace PresentationTest.Components;

[TestFixture]
public class ClickableUt
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
        var learningObject = Substitute.For<ILearningSpaceViewModel>();
        Action<ILearningSpaceViewModel> onClicked = _ => { };

        var systemUnderTest =
            CreateRenderedClickableComponent(childContent, learningObject, onClicked);

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.ChildContent, Is.EqualTo(childContent));
            Assert.That(systemUnderTest.Instance.OnClickedParam, Is.EqualTo(learningObject));
            Assert.That(
                systemUnderTest.Instance.OnClicked, Is.EqualTo(EventCallback.Factory.Create(
                    onClicked.Target ?? throw new InvalidOperationException("onClicked.Target is null"), onClicked)));
        });
    }

    [Test]
    public void ClickAndRelease_OnClickedTriggered()
    {
        ILearningSpaceViewModel? onClickedEventTriggered = null;
        var learningObject = Substitute.For<ILearningSpaceViewModel>();

        Action<ILearningSpaceViewModel> onClicked = e => { onClickedEventTriggered = e; };

        var systemUnderTest =
            CreateRenderedClickableComponent(null, learningObject, onClicked);

        systemUnderTest.WaitForElement("g", TimeSpan.FromSeconds(3)).MouseDown(new MouseEventArgs());
        _mouseService.OnUp += Raise.EventWith(new MouseEventArgs());

        Assert.That(onClickedEventTriggered, Is.EqualTo(learningObject));
    }

    [Test]
    public void ClickMoveAndRelease_OnClickedNotTriggered()
    {
        ILearningSpaceViewModel? onClickedEventTriggered = null;
        var learningObject = Substitute.For<ILearningSpaceViewModel>();

        Action<ILearningSpaceViewModel> onClicked = e => { onClickedEventTriggered = e; };

        var systemUnderTest =
            CreateRenderedClickableComponent(null, learningObject, onClicked);

        systemUnderTest.WaitForElement("g").MouseDown(new MouseEventArgs());
        _mouseService.OnMove += Raise.EventWith(new MouseEventArgs());
        _mouseService.OnUp += Raise.EventWith(new MouseEventArgs());

        Assert.That(onClickedEventTriggered, Is.EqualTo(null));
    }

    private IRenderedComponent<Clickable<ILearningSpaceViewModel>> CreateRenderedClickableComponent(
        RenderFragment? childContent = null,
        ILearningSpaceViewModel? learningObject = null, Action<ILearningSpaceViewModel>? onClicked = null)
    {
        onClicked ??= _ => { };
        return _testContext.RenderComponent<Clickable<ILearningSpaceViewModel>>(parameters => parameters
            .Add(p => p.ChildContent, childContent)
            .Add(p => p.OnClickedParam, learningObject)
            .Add(p => p.OnClicked, onClicked)
        );
    }
}