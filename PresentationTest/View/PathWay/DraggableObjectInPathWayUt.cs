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
using Presentation.PresentationLogic.PathWay;
using Presentation.PresentationLogic.Space;
using Presentation.PresentationLogic.World;
using Presentation.View.PathWay;
using Shared;
using TestContext = Bunit.TestContext;

namespace PresentationTest.View.PathWay;

[TestFixture]
public class DraggableObjectInPathWay
{
#pragma warning disable CS8618 //set in setup - n.stich
    private TestContext _ctx;
    private IMouseService _mouseService;
#pragma warning restore CS8618

    [SetUp]
    public void Setup()
    {
        _ctx = new TestContext();
        _ctx.ComponentFactories.AddStub<Draggable<IObjectInPathWayViewModel>>();
        _mouseService = Substitute.For<IMouseService>();
        _ctx.Services.AddSingleton(_mouseService);
    }
    
    [Test]
    public void Constructor_SetsParametersCorrectly_Space()
    {
        var space = Substitute.For<ISpaceViewModel>();
        var onClicked = new Action<IObjectInPathWayViewModel>(_ => { });
        var onDragged = new DraggedEventArgs<IObjectInPathWayViewModel>.DraggedEventHandler((_,_) => { });
        var onDoubleClicked = new Action<IObjectInPathWayViewModel>(_ => { });
        var onRightClicked = new Action<IObjectInPathWayViewModel>(_ => { });
        var showingRightClickMenu = false;
        var onOpenSpace = new Action<ISpaceViewModel>(_ => { });
        var onEditSpace = new Action<ISpaceViewModel>(_ => { });
        var onDeleteSpace = new Action<ISpaceViewModel>(_ => { });
        var onCloseRightClickMenu = new Action(() => { });
        var positioningService = Substitute.For<IWorldPresenter>();
        var systemUnderTest =
            GetRenderedDraggableSpace(space, onClicked, onDragged, onDoubleClicked, onRightClicked,
                showingRightClickMenu, onOpenSpace, onEditSpace, onDeleteSpace,
                onCloseRightClickMenu, positioningService);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.ObjectInPathWay, Is.EqualTo(space));
            //overriding nullability warning because we know target isn't null as onClicked isn't a static method but instead a lambda -n.stich
            Assert.That(systemUnderTest.Instance.OnClickedDraggable, Is.EqualTo(EventCallback.Factory.Create(onClicked.Target!, onClicked)));
            Assert.That(systemUnderTest.Instance.OnDoubleClickedDraggable, Is.EqualTo(EventCallback.Factory.Create(onDoubleClicked.Target!, onDoubleClicked)));
            Assert.That(systemUnderTest.Instance.OnRightClickedDraggable, Is.EqualTo(EventCallback.Factory.Create(onRightClicked.Target!, onRightClicked)));
            Assert.That(systemUnderTest.Instance.ShowingRightClickMenu, Is.EqualTo(showingRightClickMenu));
            Assert.That(systemUnderTest.Instance.OnOpenSpace, Is.EqualTo(EventCallback.Factory.Create(onOpenSpace.Target!, onOpenSpace)));
            Assert.That(systemUnderTest.Instance.OnEditSpace, Is.EqualTo(EventCallback.Factory.Create(onEditSpace.Target!, onEditSpace)));
            Assert.That(systemUnderTest.Instance.OnDeleteSpace, Is.EqualTo(EventCallback.Factory.Create(onDeleteSpace.Target!, onDeleteSpace)));
            Assert.That(systemUnderTest.Instance.OnCloseRightClickMenu, Is.EqualTo(EventCallback.Factory.Create(onCloseRightClickMenu.Target!, onCloseRightClickMenu)));
            Assert.That(systemUnderTest.Instance.PositioningService, Is.EqualTo(positioningService));
        });
    }
    
    [Test]
    public void Constructor_SetsParametersCorrectly_PathWayCondition()
    {
        var pathWayCondition = new PathWayConditionViewModel(ConditionEnum.And, 2, 1);
        var onClicked = new Action<IObjectInPathWayViewModel>(_ => { });
        var onDragged = new DraggedEventArgs<IObjectInPathWayViewModel>.DraggedEventHandler((_,_) => { });
        var onDoubleClicked = new Action<IObjectInPathWayViewModel>(_ => { });
        var onRightClicked = new Action<IObjectInPathWayViewModel>(_ => { });
        var showingRightClickMenu = false;
        var onEditPathWayCondition = new Action<PathWayConditionViewModel>(_ => { });
        var onDeletePathWayCondition = new Action<PathWayConditionViewModel>(_ => { });
        var onCloseRightClickMenu = new Action(() => { });
        var positioningService = Substitute.For<IWorldPresenter>();
        var systemUnderTest =
            GetRenderedDraggablePathWayCondition(pathWayCondition, onClicked, onDragged, onDoubleClicked, onRightClicked,
                showingRightClickMenu, onEditPathWayCondition, onDeletePathWayCondition, onCloseRightClickMenu , positioningService);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.ObjectInPathWay, Is.EqualTo(pathWayCondition));
            //overriding nullability warning because we know target isn't null as onClicked isn't a static method but instead a lambda -n.stich
            Assert.That(systemUnderTest.Instance.OnClickedDraggable, Is.EqualTo(EventCallback.Factory.Create(onClicked.Target!, onClicked)));
            Assert.That(systemUnderTest.Instance.OnDoubleClickedDraggable, Is.EqualTo(EventCallback.Factory.Create(onDoubleClicked.Target!, onDoubleClicked)));
            Assert.That(systemUnderTest.Instance.OnRightClickedDraggable, Is.EqualTo(EventCallback.Factory.Create(onRightClicked.Target!, onRightClicked)));
            Assert.That(systemUnderTest.Instance.ShowingRightClickMenu, Is.EqualTo(showingRightClickMenu));
            Assert.That(systemUnderTest.Instance.OnEditPathWayCondition, Is.EqualTo(EventCallback.Factory.Create(onEditPathWayCondition.Target!, onEditPathWayCondition)));
            Assert.That(systemUnderTest.Instance.OnDeletePathWayCondition, Is.EqualTo(EventCallback.Factory.Create(onDeletePathWayCondition.Target!, onDeletePathWayCondition)));
            Assert.That(systemUnderTest.Instance.OnCloseRightClickMenu, Is.EqualTo(EventCallback.Factory.Create(onCloseRightClickMenu.Target!, onCloseRightClickMenu)));
            Assert.That(systemUnderTest.Instance.PositioningService, Is.EqualTo(positioningService));
        });
    }

    [Test]
    public void Constructor_PassesCorrectValuesToDraggable_Space()
    {
        var space = Substitute.For<ISpaceViewModel>();
        space.Name.Returns("foo bar super cool name");
        var onClicked = new Action<IObjectInPathWayViewModel>(_ => { });
        var onDragged = new DraggedEventArgs<IObjectInPathWayViewModel>.DraggedEventHandler((_,_) => { });
        var onDoubleClicked = new Action<IObjectInPathWayViewModel>(_ => { });
        var onRightClicked = new Action<IObjectInPathWayViewModel>(_ => { });
        const bool showingRightClickMenu = false;
        var onOpenSpace = new Action<ISpaceViewModel>(_ => { });
        var onEditSpace = new Action<ISpaceViewModel>(_ => { });
        var onDeleteSpace = new Action<ISpaceViewModel>(_ => { });
        var onCloseRightClickMenu = new Action(() => { });
        var positioningService = Substitute.For<IWorldPresenter>();
        var systemUnderTest =
            GetRenderedDraggableSpace(space, onClicked, onDragged, onDoubleClicked, onRightClicked,
                showingRightClickMenu, onOpenSpace, onEditSpace, onDeleteSpace,
                onCloseRightClickMenu, positioningService);

        Assert.That(systemUnderTest.HasComponent<Stub<Draggable<IObjectInPathWayViewModel>>>());
        var stub = systemUnderTest.FindComponent<Stub<Draggable<IObjectInPathWayViewModel>>>();
        Assert.Multiple(() =>
        {
            Assert.That(stub.Instance.Parameters[nameof(Draggable<ISpaceViewModel>.DraggableObject)], Is.EqualTo(space));
            //overriding nullability warning because we know target isn't null as onClicked isn't a static method but instead a lambda -n.stich
            Assert.That(stub.Instance.Parameters[nameof(Draggable<ISpaceViewModel>.OnClicked)], Is.EqualTo(EventCallback.Factory.Create(onClicked.Target!, onClicked)));
            Assert.That(stub.Instance.Parameters[nameof(Draggable<ISpaceViewModel>.ChildContent)], Is.Not.Null);
        });
        var childContent = _ctx.Render((RenderFragment)stub.Instance.Parameters[nameof(Draggable<ISpaceViewModel>.ChildContent)]);
        childContent.MarkupMatches(
            @"<rect height=""50"" width=""100"" style=""fill:lightgreen;stroke:black;stroke-width:1""></rect>" +
            @$"<text x=""3"" y=""15"">{space.Name}</text>" +
            @"<g  ></g>");
    }
    
    [Test]
    public void Constructor_PassesCorrectValuesToDraggable_PathWayCondition()
    {
        var pathWayCondition = new PathWayConditionViewModel(ConditionEnum.And, 2, 1);
        var onClicked = new Action<IObjectInPathWayViewModel>(_ => { });
        var onDragged = new DraggedEventArgs<IObjectInPathWayViewModel>.DraggedEventHandler((_,_) => { });
        var onDoubleClicked = new Action<IObjectInPathWayViewModel>(_ => { });
        var onRightClicked = new Action<IObjectInPathWayViewModel>(_ => { });
        const bool showingRightClickMenu = false;
        var onEditPathWayCondition = new Action<PathWayConditionViewModel>(_ => { });
        var onDeletePathWayCondition = new Action<PathWayConditionViewModel>(_ => { });
        var onCloseRightClickMenu = new Action(() => { });
        var positioningService = Substitute.For<IWorldPresenter>();
        var systemUnderTest =
            GetRenderedDraggablePathWayCondition(pathWayCondition, onClicked, onDragged, onDoubleClicked, onRightClicked,
                showingRightClickMenu, onEditPathWayCondition, onDeletePathWayCondition, onCloseRightClickMenu, positioningService);

        Assert.That(systemUnderTest.HasComponent<Stub<Draggable<IObjectInPathWayViewModel>>>());
        var stub = systemUnderTest.FindComponent<Stub<Draggable<IObjectInPathWayViewModel>>>();
        Assert.Multiple(() =>
        {
            Assert.That(stub.Instance.Parameters[nameof(Draggable<PathWayConditionViewModel>.DraggableObject)], Is.EqualTo(pathWayCondition));
            //overriding nullability warning because we know target isn't null as onClicked isn't a static method but instead a lambda -n.stich
            Assert.That(stub.Instance.Parameters[nameof(Draggable<PathWayConditionViewModel>.OnClicked)], Is.EqualTo(EventCallback.Factory.Create(onClicked.Target!, onClicked)));
            Assert.That(stub.Instance.Parameters[nameof(Draggable<PathWayConditionViewModel>.ChildContent)], Is.Not.Null);
        });
        var childContent = _ctx.Render((RenderFragment)stub.Instance.Parameters[nameof(Draggable<PathWayConditionViewModel>.ChildContent)]);
        childContent.MarkupMatches(
            @"<circle r=""20"" style=""fill:darkgray;stroke:black;stroke-width:1""></circle>" +
            @$"<text x=""3"" y=""15"" transform=""translate(-16,-10)"">{pathWayCondition.Condition.ToString()}</text>" +
            @"<g  ></g>");
    }

    [Test]
    public void Constructor_SpaceNull_ThrowsException()
    {
        //Override warning for this test as we are testing exactly what happens when we break the nullability contract - n.stich
        Assert.That(
            () => GetRenderedDraggableSpace(null!, _ => { }, (_, _) => { }, _ => { }, _ => { }, false, _ => { },
                _ => { }, _ => { }, () => { }, null!), Throws.ArgumentNullException);
    }
    
    [Test]
    public void Constructor_ConditionNull_ThrowsException()
    {
        //Override warning for this test as we are testing exactly what happens when we break the nullability contract - n.stich
        Assert.That(
            () => GetRenderedDraggablePathWayCondition(null!, _ => { }, (_, _) => { }, _ => { }, _ => { }, false, _ => { },_ => { },
                 () => { }, null!), Throws.ArgumentNullException);
    }

    private IRenderedComponent<DraggableSpace> GetRenderedDraggableSpace(
        IObjectInPathWayViewModel objectViewmodel, Action<IObjectInPathWayViewModel> onClicked, 
        DraggedEventArgs<IObjectInPathWayViewModel>.DraggedEventHandler onDragged, Action<IObjectInPathWayViewModel> onDoubleClicked,
        Action<IObjectInPathWayViewModel> onRightClicked, bool showingRightClickMenu, 
        Action<ISpaceViewModel> onOpenSpace, Action<ISpaceViewModel> onEditSpace, 
        Action<ISpaceViewModel> onDeleteSpace, Action onCloseRightClickMenu,
        IWorldPresenter positioningService)
    {
        return _ctx.RenderComponent<DraggableSpace>(parameters => parameters
            .Add(p => p.ObjectInPathWay, objectViewmodel)
            .Add(p => p.OnClickedDraggable, onClicked)
            .Add(p => p.OnDraggedDraggable, onDragged)
            .Add(p=>p.OnDoubleClickedDraggable, onDoubleClicked)
            .Add(p=>p.OnRightClickedDraggable, onRightClicked)
            .Add(p=>p.ShowingRightClickMenu, showingRightClickMenu)
            .Add(p=>p.OnOpenSpace, onOpenSpace)
            .Add(p=>p.OnEditSpace, onEditSpace)
            .Add(p=>p.OnDeleteSpace, onDeleteSpace)
            .Add(p=>p.OnCloseRightClickMenu, onCloseRightClickMenu)
            .Add(p=>p.PositioningService, positioningService)
        );
    }
    
    private IRenderedComponent<DraggablePathWayCondition> GetRenderedDraggablePathWayCondition(
        IObjectInPathWayViewModel objectViewmodel, Action<IObjectInPathWayViewModel> onClicked, 
        DraggedEventArgs<IObjectInPathWayViewModel>.DraggedEventHandler onDragged, Action<IObjectInPathWayViewModel> onDoubleClicked,
        Action<IObjectInPathWayViewModel> onRightClicked, bool showingRightClickMenu,Action<PathWayConditionViewModel> onEditPathWayCondition, 
        Action<PathWayConditionViewModel> onDeletePathWayCondition, Action onCloseRightClickMenu,
        IWorldPresenter positioningService)
    {
        return _ctx.RenderComponent<DraggablePathWayCondition>(parameters => parameters
            .Add(p => p.ObjectInPathWay, objectViewmodel)
            .Add(p => p.OnClickedDraggable, onClicked)
            .Add(p => p.OnDraggedDraggable, onDragged)
            .Add(p=>p.OnDoubleClickedDraggable, onDoubleClicked)
            .Add(p=>p.OnRightClickedDraggable, onRightClicked)
            .Add(p=>p.ShowingRightClickMenu, showingRightClickMenu)
            .Add(p=>p.OnEditPathWayCondition, onEditPathWayCondition)
            .Add(p=>p.OnDeletePathWayCondition, onDeletePathWayCondition)
            .Add(p=>p.OnCloseRightClickMenu, onCloseRightClickMenu)
            .Add(p=>p.PositioningService, positioningService)
        );
    }

}