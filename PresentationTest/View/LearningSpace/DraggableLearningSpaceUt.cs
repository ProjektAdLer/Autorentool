using System;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components;
using Presentation.PresentationLogic;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.View.LearningPathWay;
using Presentation.View.LearningSpace;
using TestContext = Bunit.TestContext;

namespace PresentationTest.View.LearningSpace;

[TestFixture]
public class DraggableLearningSpaceUt
{
#pragma warning disable CS8618 //set in setup - n.stich
    private TestContext _ctx;
    private IMouseService _mouseService;
#pragma warning restore CS8618

    [SetUp]
    public void Setup()
    {
        _ctx = new TestContext();
        _ctx.ComponentFactories.AddStub<Draggable<ILearningSpaceViewModel>>();
        _mouseService = Substitute.For<IMouseService>();
        _ctx.Services.AddSingleton(_mouseService);
    }
    
    [Test]
    public void Constructor_SetsParametersCorrectly()
    {
        var learningSpace = Substitute.For<ILearningSpaceViewModel>();
        var onClicked = new Action<IObjectInPathWayViewModel>(_ => { });
        var onDragged = new DraggedEventArgs<IObjectInPathWayViewModel>.DraggedEventHandler((_,_) => { });
        var onDoubleClicked = new Action<IObjectInPathWayViewModel>(_ => { });
        var onRightClicked = new Action<IObjectInPathWayViewModel>(_ => { });
        var showingRightClickMenu = false;
        var onOpenLearningSpace = new Action<ILearningSpaceViewModel>(_ => { });
        var onEditLearningSpace = new Action<ILearningSpaceViewModel>(_ => { });
        var onDeleteLearningSpace = new Action<ILearningSpaceViewModel>(_ => { });
        var onCloseRightClickMenu = new Action(() => { });
        var positioningService = Substitute.For<ILearningWorldPresenter>();
        var systemUnderTest =
            GetRenderedDraggableLearningSpace(learningSpace, onClicked, onDragged, onDoubleClicked, onRightClicked,
                showingRightClickMenu, onOpenLearningSpace, onEditLearningSpace, onDeleteLearningSpace,
                onCloseRightClickMenu, positioningService);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.ObjectInPathWay, Is.EqualTo(learningSpace));
            //overriding nullability warning because we know target isn't null as onClicked isn't a static method but instead a lambda -n.stich
            Assert.That(systemUnderTest.Instance.OnClickedDraggable, Is.EqualTo(EventCallback.Factory.Create(onClicked.Target!, onClicked)));
            Assert.That(systemUnderTest.Instance.OnDoubleClickedDraggable, Is.EqualTo(EventCallback.Factory.Create(onDoubleClicked.Target!, onDoubleClicked)));
            Assert.That(systemUnderTest.Instance.OnRightClickedDraggable, Is.EqualTo(EventCallback.Factory.Create(onRightClicked.Target!, onRightClicked)));
            Assert.That(systemUnderTest.Instance.ShowingRightClickMenu, Is.EqualTo(showingRightClickMenu));
            Assert.That(systemUnderTest.Instance.OnOpenLearningSpace, Is.EqualTo(EventCallback.Factory.Create(onOpenLearningSpace.Target!, onOpenLearningSpace)));
            Assert.That(systemUnderTest.Instance.OnEditLearningSpace, Is.EqualTo(EventCallback.Factory.Create(onEditLearningSpace.Target!, onEditLearningSpace)));
            Assert.That(systemUnderTest.Instance.OnDeleteLearningSpace, Is.EqualTo(EventCallback.Factory.Create(onDeleteLearningSpace.Target!, onDeleteLearningSpace)));
            Assert.That(systemUnderTest.Instance.OnCloseRightClickMenu, Is.EqualTo(EventCallback.Factory.Create(onCloseRightClickMenu.Target!, onCloseRightClickMenu)));
            Assert.That(systemUnderTest.Instance.PositioningService, Is.EqualTo(positioningService));
        });
    }

    [Test]
    public void Constructor_PassesCorrectValuesToDraggable()
    {
        var learningSpace = Substitute.For<ILearningSpaceViewModel>();
        learningSpace.Name.Returns("foo bar super cool name");
        var onClicked = new Action<IObjectInPathWayViewModel>(_ => { });
        var onDragged = new DraggedEventArgs<IObjectInPathWayViewModel>.DraggedEventHandler((_,_) => { });
        var onDoubleClicked = new Action<IObjectInPathWayViewModel>(_ => { });
        var onRightClicked = new Action<IObjectInPathWayViewModel>(_ => { });
        const bool showingRightClickMenu = false;
        var onOpenLearningSpace = new Action<ILearningSpaceViewModel>(_ => { });
        var onEditLearningSpace = new Action<ILearningSpaceViewModel>(_ => { });
        var onDeleteLearningSpace = new Action<ILearningSpaceViewModel>(_ => { });
        var onCloseRightClickMenu = new Action(() => { });
        var positioningService = Substitute.For<ILearningWorldPresenter>();
        var systemUnderTest =
            GetRenderedDraggableLearningSpace(learningSpace, onClicked, onDragged, onDoubleClicked, onRightClicked,
                showingRightClickMenu, onOpenLearningSpace, onEditLearningSpace, onDeleteLearningSpace,
                onCloseRightClickMenu, positioningService);

        Assert.That(systemUnderTest.HasComponent<Stub<Draggable<ILearningSpaceViewModel>>>());
        var stub = systemUnderTest.FindComponent<Stub<Draggable<ILearningSpaceViewModel>>>();
        Assert.Multiple(() =>
        {
            Assert.That(stub.Instance.Parameters[nameof(Draggable<ILearningSpaceViewModel>.LearningObject)], Is.EqualTo(learningSpace));
            //overriding nullability warning because we know target isn't null as onClicked isn't a static method but instead a lambda -n.stich
            Assert.That(stub.Instance.Parameters[nameof(Draggable<ILearningSpaceViewModel>.OnClicked)], Is.EqualTo(EventCallback.Factory.Create(onClicked.Target!, onClicked)));
            Assert.That(stub.Instance.Parameters[nameof(Draggable<ILearningSpaceViewModel>.ChildContent)], Is.Not.Null);
        });
        var childContent = _ctx.Render((RenderFragment)stub.Instance.Parameters[nameof(Draggable<ILearningSpaceViewModel>.ChildContent)]);
        childContent.MarkupMatches(
            @"<rect height=""50"" width=""100"" style=""fill:lightgreen;stroke:black""></rect>" +
            @$"<text x=""3"" y=""15"">{learningSpace.Name}</text>" +
            @"<g  ></g>");
    }

    [Test]
    public void Constructor_ElementNull_ThrowsException()
    {
        //Override warning for this test as we are testing exactly what happens when we break the nullability contract - n.stich
        Assert.That(
            () => GetRenderedDraggableLearningSpace(null!, _ => { }, (_, _) => { }, _ => { }, _ => { }, false, _ => { },
                _ => { }, _ => { }, () => { }, null!), Throws.ArgumentNullException);
    }

    private IRenderedComponent<DraggableLearningSpace> GetRenderedDraggableLearningSpace(
        IObjectInPathWayViewModel objectViewmodel, Action<IObjectInPathWayViewModel> onClicked, 
        DraggedEventArgs<IObjectInPathWayViewModel>.DraggedEventHandler onDragged, Action<IObjectInPathWayViewModel> onDoubleClicked,
        Action<IObjectInPathWayViewModel> onRightClicked, bool showingRightClickMenu, 
        Action<ILearningSpaceViewModel> onOpenLearningSpace, Action<ILearningSpaceViewModel> onEditLearningSpace, 
        Action<ILearningSpaceViewModel> onDeleteLearningSpace, Action onCloseRightClickMenu,
        ILearningWorldPresenter positioningService)
    {
        return _ctx.RenderComponent<DraggableLearningSpace>(parameters => parameters
            .Add(p => p.ObjectInPathWay, objectViewmodel)
            .Add(p => p.OnClickedDraggable, onClicked)
            .Add(p => p.OnDraggedDraggable, onDragged)
            .Add(p=>p.OnDoubleClickedDraggable, onDoubleClicked)
            .Add(p=>p.OnRightClickedDraggable, onRightClicked)
            .Add(p=>p.ShowingRightClickMenu, showingRightClickMenu)
            .Add(p=>p.OnOpenLearningSpace, onOpenLearningSpace)
            .Add(p=>p.OnEditLearningSpace, onEditLearningSpace)
            .Add(p=>p.OnDeleteLearningSpace, onDeleteLearningSpace)
            .Add(p=>p.OnCloseRightClickMenu, onCloseRightClickMenu)
            .Add(p=>p.PositioningService, positioningService)
        );
    }

}