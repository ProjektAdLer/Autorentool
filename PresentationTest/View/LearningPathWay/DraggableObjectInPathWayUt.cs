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
using Presentation.PresentationLogic.LearningPathway;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.View.LearningPathWay;
using Shared;
using TestContext = Bunit.TestContext;

namespace PresentationTest.View.LearningPathWay;

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
    public void Constructor_SetsParametersCorrectly_LearningSpace()
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
        var positioningService = Substitute.For<ILearningWorldPresenter>();
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
    public void Constructor_PassesCorrectValuesToDraggable_LearningSpace()
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

        Assert.That(systemUnderTest.HasComponent<Stub<Draggable<IObjectInPathWayViewModel>>>());
        var stub = systemUnderTest.FindComponent<Stub<Draggable<IObjectInPathWayViewModel>>>();
        Assert.Multiple(() =>
        {
            Assert.That(stub.Instance.Parameters[nameof(Draggable<ILearningSpaceViewModel>.LearningObject)], Is.EqualTo(learningSpace));
            //overriding nullability warning because we know target isn't null as onClicked isn't a static method but instead a lambda -n.stich
            Assert.That(stub.Instance.Parameters[nameof(Draggable<ILearningSpaceViewModel>.OnClicked)], Is.EqualTo(EventCallback.Factory.Create(onClicked.Target!, onClicked)));
            Assert.That(stub.Instance.Parameters[nameof(Draggable<ILearningSpaceViewModel>.ChildContent)], Is.Not.Null);
        });
        var childContent = _ctx.Render((RenderFragment)stub.Instance.Parameters[nameof(Draggable<ILearningSpaceViewModel>.ChildContent)]);
        childContent.MarkupMatches(
            @"<rect height=""50"" width=""100"" style=""fill:lightgreen;stroke:black;stroke-width:1""></rect>" +
            @$"<text x=""3"" y=""15"">{learningSpace.Name}</text>" +
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
        var positioningService = Substitute.For<ILearningWorldPresenter>();
        var systemUnderTest =
            GetRenderedDraggablePathWayCondition(pathWayCondition, onClicked, onDragged, onDoubleClicked, onRightClicked,
                showingRightClickMenu, onEditPathWayCondition, onDeletePathWayCondition, onCloseRightClickMenu, positioningService);

        Assert.That(systemUnderTest.HasComponent<Stub<Draggable<IObjectInPathWayViewModel>>>());
        var stub = systemUnderTest.FindComponent<Stub<Draggable<IObjectInPathWayViewModel>>>();
        Assert.Multiple(() =>
        {
            Assert.That(stub.Instance.Parameters[nameof(Draggable<PathWayConditionViewModel>.LearningObject)], Is.EqualTo(pathWayCondition));
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
            () => GetRenderedDraggableLearningSpace(null!, _ => { }, (_, _) => { }, _ => { }, _ => { }, false, _ => { },
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
    
    private IRenderedComponent<DraggablePathWayCondition> GetRenderedDraggablePathWayCondition(
        IObjectInPathWayViewModel objectViewmodel, Action<IObjectInPathWayViewModel> onClicked, 
        DraggedEventArgs<IObjectInPathWayViewModel>.DraggedEventHandler onDragged, Action<IObjectInPathWayViewModel> onDoubleClicked,
        Action<IObjectInPathWayViewModel> onRightClicked, bool showingRightClickMenu,Action<PathWayConditionViewModel> onEditPathWayCondition, 
        Action<PathWayConditionViewModel> onDeletePathWayCondition, Action onCloseRightClickMenu,
        ILearningWorldPresenter positioningService)
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