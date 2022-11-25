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
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningSpace;
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
        var learningObject = Substitute.For<ILearningElementViewModel>();
        var menuEntries = new List<RightClickMenuEntry>();
        var onClose = () => { };

        var systemUnderTest =
            CreateRenderedDraggableComponent(learningObject, menuEntries, onClose);

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.LearningObject, Is.EqualTo(learningObject));
            Assert.That(
                systemUnderTest.Instance.MenuEntries, Is.EqualTo(menuEntries));
            Assert.That(
                systemUnderTest.Instance.OnClose, Is.EqualTo(EventCallback.Factory.Create(
                    onClose.Target ?? throw new InvalidOperationException("onClose.Target is null"), onClose)));
        });
    }
    
    [Test]
    public void OnParametersSet_WithILearningSpaceViewModel_RightClickMenuInitialized(){
        var learningObject = Substitute.For<ILearningSpaceViewModel>();
        var menuEntries = new List<RightClickMenuEntry>();
        var onClose = () => { };

        var systemUnderTest = _testContext.RenderComponent<RightClickMenu<ILearningSpaceViewModel>>(parameters => parameters
            .Add(p => p.LearningObject, learningObject)
            .Add(p => p.MenuEntries, menuEntries)
            .Add(p => p.OnClose, onClose)
        );

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.LearningObject, Is.EqualTo(learningObject));
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
        var learningObject = Substitute.For<IDisplayableLearningObject>();

        Assert.Throws<ArgumentException>(() =>
            _testContext.RenderComponent<RightClickMenu<IDisplayableLearningObject>>(parameters => parameters
                .Add(p => p.LearningObject, learningObject)
                .Add(p => p.MenuEntries, new List<RightClickMenuEntry>())
                .Add(p => p.OnClose, () => { })
            ));
    }
    
    [Test]
    public void MenuEntryClicked_InvokesCallback()
    {
        var learningObject = Substitute.For<ILearningElementViewModel>();
        const string onOpenText = "Open";
        var onOpenClicked = Substitute.For<Action>();
        var menuEntries = new List<RightClickMenuEntry>(){new RightClickMenuEntry(onOpenText, onOpenClicked)};

        var systemUnderTest =
            CreateRenderedDraggableComponent(learningObject, menuEntries);

        systemUnderTest.FindAll("g").Last(x => x.InnerHtml.Contains("Open")).MouseDown(new MouseEventArgs());
        systemUnderTest.InvokeAsync(() => _mouseService.OnUp += Raise.EventWith(new MouseEventArgs()));

        onOpenClicked.Received(1).Invoke();
    }

    [Test]
    public void OnClose_InvokesCallback()
    {
        var learningObject = Substitute.For<ILearningElementViewModel>();
        var onClose = Substitute.For<Action>();

        var systemUnderTest =
            CreateRenderedDraggableComponent(learningObject, onClose: onClose);

        systemUnderTest.FindAll("g").Last(x => x.InnerHtml.Contains("Close")).MouseDown(new MouseEventArgs());
        systemUnderTest.InvokeAsync(() => _mouseService.OnUp += Raise.EventWith(new MouseEventArgs()));

        onClose.Received(1).Invoke();
    }


    private IRenderedComponent<RightClickMenu<ILearningElementViewModel>> CreateRenderedDraggableComponent(
        ILearningElementViewModel? learningObject = null, List<RightClickMenuEntry>? menuEntries = null, Action? onClose = null)
    {
        menuEntries ??= new List<RightClickMenuEntry>();
        onClose ??= () => { };

        return _testContext.RenderComponent<RightClickMenu<ILearningElementViewModel>>(parameters => parameters
            .Add(p => p.LearningObject, learningObject)
            .Add(p => p.MenuEntries, menuEntries)
            .Add(p => p.OnClose, onClose)
        );
    }
}