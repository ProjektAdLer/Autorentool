using System;
using System.Collections.Generic;
using System.Linq;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components;
using Presentation.Components.RightClickMenu;
using Presentation.PresentationLogic;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.Element;
using Presentation.PresentationLogic.Space;
using TestContext = Bunit.TestContext;

namespace PresentationTest.Components;

[TestFixture]
public class RightClickMenuUt
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
        var elementViewModel = Substitute.For<IElementViewModel>();
        var menuEntries = new List<RightClickMenuEntry>();
        var onClose = () => { };

        var systemUnderTest =
            CreateRenderedDraggableComponent(elementViewModel, menuEntries, onClose);

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.ClickableObject, Is.EqualTo(elementViewModel));
            Assert.That(
                systemUnderTest.Instance.MenuEntries, Is.EqualTo(menuEntries));
            Assert.That(
                systemUnderTest.Instance.OnClose, Is.EqualTo(EventCallback.Factory.Create(
                    onClose.Target ?? throw new InvalidOperationException("onClose.Target is null"), onClose)));
        });
    }
    
    [Test]
    public void OnParametersSet_WithISpaceViewModel_RightClickMenuInitialized(){
        var spaceViewModel = Substitute.For<ISpaceViewModel>();
        var menuEntries = new List<RightClickMenuEntry>();
        var onClose = () => { };

        var systemUnderTest = _testContext.RenderComponent<RightClickMenu<ISpaceViewModel>>(parameters => parameters
            .Add(p => p.ClickableObject, spaceViewModel)
            .Add(p => p.MenuEntries, menuEntries)
            .Add(p => p.OnClose, onClose)
        );

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.ClickableObject, Is.EqualTo(spaceViewModel));
            Assert.That(
                systemUnderTest.Instance.MenuEntries, Is.EqualTo(menuEntries));
            Assert.That(
                systemUnderTest.Instance.OnClose, Is.EqualTo(EventCallback.Factory.Create(
                    onClose.Target ?? throw new InvalidOperationException("onClose.Target is null"), onClose)));
        });
    }

    [Test]
    public void OnParametersSet_WithObjectEqualsNull_ThrowsException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            CreateRenderedDraggableComponent(null!, new List<RightClickMenuEntry>(), () => { }));
    }
    
    [Test]
    public void OnParametersSet_WithUnsupportedType_ThrowsException()
    {
        var displayableObject = Substitute.For<IDisplayableObject>();

        Assert.Throws<ArgumentException>(() =>
            _testContext.RenderComponent<RightClickMenu<IDisplayableObject>>(parameters => parameters
                .Add(p => p.ClickableObject, displayableObject)
                .Add(p => p.MenuEntries, new List<RightClickMenuEntry>())
                .Add(p => p.OnClose, () => { })
            ));
    }
    
    [Test]
    public void MenuEntryClicked_InvokesCallback()
    {
        var elementViewModel = Substitute.For<IElementViewModel>();
        const string onOpenText = "Open";
        var onOpenClicked = Substitute.For<Action>();
        var menuEntries = new List<RightClickMenuEntry>(){new RightClickMenuEntry(onOpenText, onOpenClicked)};

        var systemUnderTest =
            CreateRenderedDraggableComponent(elementViewModel, menuEntries);

        systemUnderTest.FindAll("g").Last(x => x.InnerHtml.Contains("Open")).MouseDown(new MouseEventArgs());
        systemUnderTest.InvokeAsync(() => _mouseService.OnUp += Raise.EventWith(new MouseEventArgs()));

        onOpenClicked.Received(1).Invoke();
    }

    [Test]
    public void OnClose_InvokesCallback()
    {
        var elementViewModel = Substitute.For<IElementViewModel>();
        var onClose = Substitute.For<Action>();

        var systemUnderTest =
            CreateRenderedDraggableComponent(elementViewModel, onClose: onClose);

        systemUnderTest.FindAll("g").Last(x => x.InnerHtml.Contains("Close")).MouseDown(new MouseEventArgs());
        systemUnderTest.InvokeAsync(() => _mouseService.OnUp += Raise.EventWith(new MouseEventArgs()));

        onClose.Received(1).Invoke();
    }


    private IRenderedComponent<RightClickMenu<IElementViewModel>> CreateRenderedDraggableComponent(
        IElementViewModel? elementViewModel = null, List<RightClickMenuEntry>? menuEntries = null, Action? onClose = null)
    {
        menuEntries ??= new List<RightClickMenuEntry>();
        onClose ??= () => { };

        return _testContext.RenderComponent<RightClickMenu<IElementViewModel>>(parameters => parameters
            .Add(p => p.ClickableObject, elementViewModel)
            .Add(p => p.MenuEntries, menuEntries)
            .Add(p => p.OnClose, onClose)
        );
    }
}