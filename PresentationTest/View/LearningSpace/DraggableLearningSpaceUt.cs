using System;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.View.LearningSpace;
using Shared;
using TestContext = Bunit.TestContext;

namespace PresentationTest.View.LearningSpace;

[TestFixture]
public class DraggableLearningSpaceUt
{
#pragma warning disable CS8618 //set in setup - n.stich
    private TestContext _ctx;
#pragma warning restore CS8618

    [SetUp]
    public void Setup()
    {
        _ctx = new TestContext();
        _ctx.ComponentFactories.AddStub<Draggable<ILearningSpaceViewModel>>();
    }
    
    [Test]
    public void Constructor_SetsParametersCorrectly()
    {
        var learningSpace = Substitute.For<ILearningSpaceViewModel>();
        var onClicked = new Action<ILearningSpaceViewModel>(_ => { });
        var onDragged = new DraggedEventArgs<ILearningSpaceViewModel>.DraggedEventHandler((_,_) => { });
        var onDoubleClicked = new Action<ILearningSpaceViewModel>(_ => { });
        var onRightClicked = new Action<ILearningSpaceViewModel>(_ => { });
        var showingRightClickMenu = false;
        var onOpenLearningSpace = new Action<ILearningSpaceViewModel>(_ => { });
        var onEditLearningSpace = new Action<ILearningSpaceViewModel>(_ => { });
        var onDeleteLearningSpace = new Action<ILearningSpaceViewModel>(_ => { });
        var onCloseRightClickMenu = new Action<ILearningSpaceViewModel>(_ => { });
        var systemUnderTest =
            GetRenderedDraggableLearningSpace(learningSpace, onClicked, onDragged, onDoubleClicked, onRightClicked,
                showingRightClickMenu, onOpenLearningSpace, onEditLearningSpace, onDeleteLearningSpace,
                onCloseRightClickMenu);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.LearningSpace, Is.EqualTo(learningSpace));
            //overriding nullability warning because we know target isn't null as onClicked isn't a static method but instead a lambda -n.stich
            Assert.That(systemUnderTest.Instance.OnClickedDraggable, Is.EqualTo(EventCallback.Factory.Create(onClicked.Target!, onClicked)));
            Assert.That(systemUnderTest.Instance.OnDoubleClickedDraggable, Is.EqualTo(EventCallback.Factory.Create(onDoubleClicked.Target!, onDoubleClicked)));
            Assert.That(systemUnderTest.Instance.OnRightClickedDraggable, Is.EqualTo(EventCallback.Factory.Create(onRightClicked.Target!, onRightClicked)));
            Assert.That(systemUnderTest.Instance.ShowingRightClickMenu, Is.EqualTo(showingRightClickMenu));
            Assert.That(systemUnderTest.Instance.OnOpenLearningSpace, Is.EqualTo(EventCallback.Factory.Create(onOpenLearningSpace.Target!, onOpenLearningSpace)));
            Assert.That(systemUnderTest.Instance.OnEditLearningSpace, Is.EqualTo(EventCallback.Factory.Create(onEditLearningSpace.Target!, onEditLearningSpace)));
            Assert.That(systemUnderTest.Instance.OnDeleteLearningSpace, Is.EqualTo(EventCallback.Factory.Create(onDeleteLearningSpace.Target!, onDeleteLearningSpace)));
            Assert.That(systemUnderTest.Instance.OnCloseRightClickMenu, Is.EqualTo(EventCallback.Factory.Create(onCloseRightClickMenu.Target!, onCloseRightClickMenu)));
        });
    }

    [Test]
    public void Constructor_ElementNull_ThrowsException()
    {
        //Override warning for this test as we are testing exactly what happens when we break the nullability contract - n.stich
        Assert.That(
            () => GetRenderedDraggableLearningSpace(null!, _ => { }, (_, _) => { }, _ => { }, _ => { }, false, _ => { },
                _ => { }, _ => { }, _ => { }), Throws.ArgumentNullException);
    }

    private IRenderedComponent<DraggableLearningSpace> GetRenderedDraggableLearningSpace(
        ILearningSpaceViewModel objectViewmodel, Action<ILearningSpaceViewModel> onClicked, 
        DraggedEventArgs<ILearningSpaceViewModel>.DraggedEventHandler onDragged, Action<ILearningSpaceViewModel> onDoubleClicked,
        Action<ILearningSpaceViewModel> onRightClicked, bool showingRightClickMenu, 
        Action<ILearningSpaceViewModel> onOpenLearningSpace, Action<ILearningSpaceViewModel> onEditLearningSpace, 
        Action<ILearningSpaceViewModel> onDeleteLearningSpace, Action<ILearningSpaceViewModel> onCloseRightClickMenu)
    {
        return _ctx.RenderComponent<DraggableLearningSpace>(parameters => parameters
            .Add(p => p.LearningSpace, objectViewmodel)
            .Add(p => p.OnClickedDraggable, onClicked)
            .Add(p => p.OnDraggedDraggable, onDragged)
            .Add(p=>p.OnDoubleClickedDraggable, onDoubleClicked)
            .Add(p=>p.OnRightClickedDraggable, onRightClicked)
            .Add(p=>p.ShowingRightClickMenu, showingRightClickMenu)
            .Add(p=>p.OnOpenLearningSpace, onOpenLearningSpace)
            .Add(p=>p.OnEditLearningSpace, onEditLearningSpace)
            .Add(p=>p.OnDeleteLearningSpace, onDeleteLearningSpace)
            .Add(p=>p.OnCloseRightClickMenu, onCloseRightClickMenu)
        );
    }

}