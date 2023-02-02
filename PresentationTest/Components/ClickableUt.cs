using System;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.Space;
using TestContext = Bunit.TestContext;

namespace PresentationTest.Components;

[TestFixture]
public class ClickableUt
{
#pragma warning disable CS8618
    private Bunit.TestContext _testContext;
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
        var clickableObject = Substitute.For<ISpaceViewModel>();
        Action<ISpaceViewModel> onClicked = _ => { };

        var systemUnderTest =
            CreateRenderedClickableComponent(childContent, clickableObject, onClicked);

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.ChildContent, Is.EqualTo(childContent));
            Assert.That(systemUnderTest.Instance.OnClickedParam, Is.EqualTo(clickableObject));
            Assert.That(
                systemUnderTest.Instance.OnClicked, Is.EqualTo(EventCallback.Factory.Create(
                    onClicked.Target ?? throw new InvalidOperationException("onClicked.Target is null"), onClicked)));
        });
    }
    
    [Test]
    public void ClickAndRelease_OnClickedTriggered()
    {
        ISpaceViewModel? onClickedEventTriggered = null;
        var clickableObject = Substitute.For<ISpaceViewModel>();

        Action<ISpaceViewModel> onClicked = e => { onClickedEventTriggered = e; };

        var systemUnderTest =
            CreateRenderedClickableComponent(null, clickableObject, onClicked);

        systemUnderTest.WaitForElement("g").MouseDown(new MouseEventArgs());
        _mouseService.OnUp += Raise.EventWith(new MouseEventArgs());

        Assert.That(onClickedEventTriggered, Is.EqualTo(clickableObject));
    }

    [Test]
    public void ClickMoveAndRelease_OnClickedNotTriggered()
    {
        ISpaceViewModel? onClickedEventTriggered = null;
        var clickableObject = Substitute.For<ISpaceViewModel>();

        Action<ISpaceViewModel> onClicked = e => { onClickedEventTriggered = e; };

        var systemUnderTest =
            CreateRenderedClickableComponent(null, clickableObject, onClicked);

        systemUnderTest.WaitForElement("g").MouseDown(new MouseEventArgs());
        _mouseService.OnMove += Raise.EventWith(new MouseEventArgs());
        _mouseService.OnUp += Raise.EventWith(new MouseEventArgs());

        Assert.That(onClickedEventTriggered, Is.EqualTo(null));
    }

    private IRenderedComponent<Clickable<ISpaceViewModel>> CreateRenderedClickableComponent(RenderFragment? childContent = null,
        ISpaceViewModel? clickableObject = null,  Action<ISpaceViewModel>? onClicked = null)
    {
        onClicked ??= _ => { };
        return _testContext.RenderComponent<Clickable<ISpaceViewModel>>(parameters => parameters
            .Add(p => p.ChildContent, childContent)
            .Add(p => p.OnClickedParam, clickableObject)
            .Add(p => p.OnClicked, onClicked)
        );
    }
}