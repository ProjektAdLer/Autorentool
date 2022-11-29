using System;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.View.LearningElement;
using Shared;
using TestContext = Bunit.TestContext;

namespace PresentationTest.View.LearningElement;

[TestFixture]
public class DraggableLearningElementUt
{
#pragma warning disable CS8618 //set in setup - n.stich
    private TestContext _ctx;
#pragma warning restore CS8618

    [SetUp]
    public void Setup()
    {
        _ctx = new TestContext();
        _ctx.ComponentFactories.AddStub<Draggable<ILearningElementViewModel>>();
    }
    
    [Test]
    public void Constructor_SetsParametersCorrectly()
    {
        var learningElement = Substitute.For<ILearningElementViewModel>();
        var onClicked = new Action<ILearningElementViewModel>(_ => { });
        var onDragged = new DraggedEventArgs<ILearningElementViewModel>.DraggedEventHandler((_,_) => { });
        var onDoubleClicked = new Action<ILearningElementViewModel>(_ => { });
        var onRightClicked = new Action<ILearningElementViewModel>(_ => { });
        var showingRightClickMenu = false;
        var onEditLearningElement = new Action<ILearningElementViewModel>(_ => { });
        var onDeleteLearningElement = new Action<ILearningElementViewModel>(_ => { });
        var onShowLearningElementContent = new Action<ILearningElementViewModel>(_ => { });
        var onCloseRightClickMenu = new Action<ILearningElementViewModel>(_ => { });
        var systemUnderTest =
            GetRenderedDraggableLearningElement(learningElement, onClicked, onDragged, onDoubleClicked, onRightClicked,
                showingRightClickMenu, onEditLearningElement, onDeleteLearningElement, onShowLearningElementContent,
                onCloseRightClickMenu);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.LearningElement, Is.EqualTo(learningElement));
            //overriding nullability warning because we know target isn't null as onClicked isn't a static method but instead a lambda -n.stich
            Assert.That(systemUnderTest.Instance.OnClicked, Is.EqualTo(EventCallback.Factory.Create(onClicked.Target!, onClicked)));
            Assert.That(systemUnderTest.Instance.OnDoubleClicked, Is.EqualTo(EventCallback.Factory.Create(onDoubleClicked.Target!, onDoubleClicked)));
            Assert.That(systemUnderTest.Instance.OnRightClicked, Is.EqualTo(EventCallback.Factory.Create(onRightClicked.Target!, onRightClicked)));
            Assert.That(systemUnderTest.Instance.ShowingRightClickMenu, Is.EqualTo(showingRightClickMenu));
            Assert.That(systemUnderTest.Instance.OnEditLearningElement, Is.EqualTo(EventCallback.Factory.Create(onEditLearningElement.Target!, onEditLearningElement)));
            Assert.That(systemUnderTest.Instance.OnDeleteLearningElement, Is.EqualTo(EventCallback.Factory.Create(onDeleteLearningElement.Target!, onDeleteLearningElement)));
            Assert.That(systemUnderTest.Instance.OnShowLearningElementContent, Is.EqualTo(EventCallback.Factory.Create(onShowLearningElementContent.Target!, onShowLearningElementContent)));
            Assert.That(systemUnderTest.Instance.OnCloseRightClickMenu, Is.EqualTo(EventCallback.Factory.Create(onCloseRightClickMenu.Target!, onCloseRightClickMenu)));
        });
    }

    [Test]
    public void Constructor_PassesCorrectValuesToDraggable()
    {
        var learningElement = Substitute.For<ILearningElementViewModel>();
        learningElement.Difficulty.Returns(LearningElementDifficultyEnum.Medium);
        learningElement.Name.Returns("foo bar super cool name");
        var onClicked = new Action<ILearningElementViewModel>(_ => { });
        var onDragged = new DraggedEventArgs<ILearningElementViewModel>.DraggedEventHandler((_,_) => { });
        var onDoubleClicked = new Action<ILearningElementViewModel>(_ => { });
        var onRightClicked = new Action<ILearningElementViewModel>(_ => { });
        var showingRightClickMenu = false;
        var onEditLearningElement = new Action<ILearningElementViewModel>(_ => { });
        var onDeleteLearningElement = new Action<ILearningElementViewModel>(_ => { });
        var onShowLearningElementContent = new Action<ILearningElementViewModel>(_ => { });
        var onCloseRightClickMenu = new Action<ILearningElementViewModel>(_ => { });
        var systemUnderTest =
            GetRenderedDraggableLearningElement(learningElement, onClicked, onDragged, onDoubleClicked, onRightClicked,
                showingRightClickMenu, onEditLearningElement, onDeleteLearningElement, onShowLearningElementContent,
                onCloseRightClickMenu);

        Assert.That(systemUnderTest.HasComponent<Stub<Draggable<ILearningElementViewModel>>>());
        var stub = systemUnderTest.FindComponent<Stub<Draggable<ILearningElementViewModel>>>();
        Assert.Multiple(() =>
        {
            Assert.That(stub.Instance.Parameters[nameof(Draggable<ILearningElementViewModel>.LearningObject)], Is.EqualTo(learningElement));
            //overriding nullability warning because we know target isn't null as onClicked isn't a static method but instead a lambda -n.stich
            Assert.That(stub.Instance.Parameters[nameof(Draggable<ILearningElementViewModel>.OnClicked)], Is.EqualTo(EventCallback.Factory.Create(onClicked.Target!, onClicked)));
            Assert.That(stub.Instance.Parameters[nameof(Draggable<ILearningElementViewModel>.ChildContent)], Is.Not.Null);
        });
        var childContent = _ctx.Render((RenderFragment)stub.Instance.Parameters[nameof(Draggable<ILearningElementViewModel>.ChildContent)]);
        childContent.MarkupMatches(
            @"<rect height=""50"" width=""100"" style=""fill:lightblue;stroke:black""></rect>" +
            @"<polygon transform=""translate(75,1)"" fill=""yellow"" points=""13 1 5 25 24 10 2 10 21 25""></polygon>" +
            @$"<text x=""3"" y=""15"">{learningElement.Name}</text>");
    }

    [Test]
    public void Constructor_ElementNull_ThrowsException()
    {
        //Override warning for this test as we are testing exactly what happens when we break the nullability contract - n.stich
        Assert.That(
            () => GetRenderedDraggableLearningElement(null!, _ => { }, (_, _) => { }, _ => { }, _ => { }, false,
                _ => { }, _ => { }, _ => { }, _ => { }), Throws.ArgumentNullException);
    }
    
    [Test]
    public void GetDifficultyPolygon_InputOutOfRange_ThrowsException()
    {
        Assert.That(() => DraggableLearningElement.GetDifficultyPolygon((LearningElementDifficultyEnum)123), Throws.TypeOf<ArgumentOutOfRangeException>());
    }
    
    [Test]
    [TestCase(LearningElementDifficultyEnum.Easy, "13 1 10 10 2 13 10 16 13 25 16 16 24 13 16 10", "green")]
    [TestCase(LearningElementDifficultyEnum.Medium, "13 1 5 25 24 10 2 10 21 25", "yellow")]
    [TestCase(LearningElementDifficultyEnum.Hard, "13 1 10 8 2 7 8 13 2 19 10 18 13 25 16 18 24 19 19 13 24 7 16 8 13 1", "red")]
    [TestCase(LearningElementDifficultyEnum.None, "0", "lightblue")]
    public void GetDifficultyPolygon_ValidInput_ReturnsCorrectPolygon(LearningElementDifficultyEnum difficulty, string expectedPoints, string expectedColor)
    {
        var (actualPoints, actualColor) = DraggableLearningElement.GetDifficultyPolygon(difficulty);
        Assert.Multiple(() =>
        {
            Assert.That(actualPoints, Is.EqualTo(expectedPoints));
            Assert.That(actualColor, Is.EqualTo(expectedColor));
        });
    }

    private IRenderedComponent<DraggableLearningElement> GetRenderedDraggableLearningElement(
        ILearningElementViewModel objectViewmodel, Action<ILearningElementViewModel> onClicked, 
        DraggedEventArgs<ILearningElementViewModel>.DraggedEventHandler onDragged, Action<ILearningElementViewModel> onDoubleClicked,
        Action<ILearningElementViewModel> onRightClicked, bool showingRightClickMenu, 
        Action<ILearningElementViewModel> onEditLearningElement, Action<ILearningElementViewModel> onDeleteLearningElement, 
        Action<ILearningElementViewModel> onShowLearningElementContent, Action<ILearningElementViewModel> onCloseRightClickMenu)
    {
        return _ctx.RenderComponent<DraggableLearningElement>(parameters => parameters
            .Add(p => p.LearningElement, objectViewmodel)
            .Add(p => p.OnClicked, onClicked)
            .Add(p => p.OnDragged, onDragged)
            .Add(p=>p.OnDoubleClicked, onDoubleClicked)
            .Add(p=>p.OnRightClicked, onRightClicked)
            .Add(p=>p.ShowingRightClickMenu, showingRightClickMenu)
            .Add(p=>p.OnEditLearningElement, onEditLearningElement)
            .Add(p=>p.OnDeleteLearningElement, onDeleteLearningElement)
            .Add(p=>p.OnShowLearningElementContent, onShowLearningElementContent)
            .Add(p=>p.OnCloseRightClickMenu, onCloseRightClickMenu)
        );
    }

}