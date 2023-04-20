using System;
using System.Collections.Generic;
using System.Linq;
using Bunit;
using Bunit.Rendering;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.LearningPathway;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.PresentationLogic.Topic;
using Presentation.View.LearningElement;
using Presentation.View.LearningPathWay;
using Presentation.View.LearningSpace;
using Presentation.View.LearningWorld;
using Shared;
using TestContext = Bunit.TestContext;

namespace PresentationTest.View.LearningWorld;

[TestFixture]
public class LearningWorldViewUt
{
#pragma warning disable CS8618 // set in setup - n.stich
    private TestContext _ctx;
    private IMouseService _mouseService;
    private ILearningWorldPresenter _worldPresenter;
#pragma warning restore CS8618
    
    [SetUp]
    public void Setup()
    {
        _ctx = new TestContext();
        _mouseService = Substitute.For<IMouseService>();
        _worldPresenter = Substitute.For<ILearningWorldPresenter>();
        _ctx.ComponentFactories.AddStub<LearningSpaceView>();
        _ctx.ComponentFactories.AddStub<DraggableObjectInPathWay>();
        _ctx.ComponentFactories.AddStub<PathWay>();
        _ctx.ComponentFactories.AddStub<MudIcon>();
        _ctx.Services.AddSingleton(_mouseService);
        _ctx.Services.AddSingleton(_worldPresenter);
    }

    [Test]
    public void Constructor_InjectsDependencies()
    {
        var systemUnderTest = GetLearningWorldViewForTesting();
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.MouseService, Is.EqualTo(_mouseService));
            Assert.That(systemUnderTest.Instance.LearningWorldP, Is.EqualTo(_worldPresenter));
        });
    }

    [Test]
    public void Render_ChildContentSet_RendersChildContent()
    {
        RenderFragment childContent = builder =>
        {
            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "class", "barbaz");
            builder.AddContent(2, "foobar");
            builder.CloseElement();
        };
        
        var systemUnderTest = GetLearningWorldViewForTesting(childContent);

        Assert.That(systemUnderTest.FindOrFail("div.barbaz").TextContent, Is.EqualTo("foobar"));
    }

    [Test]
    public void Render_LearningWorldSet_RendersNameWorkloadAndPoints()
    {
        var learningWorld = Substitute.For<ILearningWorldViewModel>();
        learningWorld.Name.Returns("my insanely sophisticated name");
        learningWorld.Workload.Returns(42);
        learningWorld.Points.Returns(9);
        _worldPresenter.LearningWorldVm.Returns(learningWorld);
        
        var systemUnderTest = GetLearningWorldViewForTesting();

        var h3 = systemUnderTest.FindAllOrFail("h3").ToList();
        h3[0].MarkupMatches(@"<h3 class=""font-bold text-lg"">World: my insanely sophisticated name</h3>");
        h3[1].MarkupMatches(@"<h3 class=""font-bold text-lg"">Workload: 42 m</h3>");
        h3[2].MarkupMatches(@"<h3 class=""font-bold text-lg"">Points: 9</h3>");
    }
    
    [Test]
    public void Render_LearningSpaceInWorld_CorrectDraggableForEachSpace()
    {
        var space1 = Substitute.For<ILearningSpaceViewModel>();
        var space2 = Substitute.For<ILearningSpaceViewModel>();
        var pathWayCondition = new PathWayConditionViewModel(ConditionEnum.And, 2, 1);
        var pathWayConditions = new List<PathWayConditionViewModel> { pathWayCondition };
        var learningSpaces = new List<ILearningSpaceViewModel> { space1, space2 };
        var learningPathWay = new LearningPathwayViewModel(space1, space2);
        var learningPathWays = new List<ILearningPathWayViewModel> { learningPathWay };

        var learningWorld = Substitute.For<ILearningWorldViewModel>();
        learningWorld.LearningSpaces.Returns(learningSpaces);
        learningWorld.ObjectsInPathWays.Returns(learningSpaces);
        learningWorld.LearningPathWays.Returns(learningPathWays);
        learningWorld.PathWayConditions.Returns(pathWayConditions);
        _worldPresenter.LearningWorldVm.Returns(learningWorld);
        
        var systemUnderTest = GetLearningWorldViewForTesting();

        var draggableLearningSpaces = systemUnderTest.FindComponentsOrFail<Stub<DraggableLearningSpace>>().ToList();
        var draggablePathWayConditions = systemUnderTest.FindComponentsOrFail<Stub<DraggablePathWayCondition>>().ToList();
        var pathWays = systemUnderTest.FindComponentsOrFail<Stub<PathWay>>().ToList();

        Assert.Multiple(() =>
        {
            Assert.That(draggableLearningSpaces, Has.Count.EqualTo(learningSpaces.Count));
            Assert.That(draggablePathWayConditions, Has.Count.EqualTo(pathWayConditions.Count));
            Assert.That(learningSpaces.All(le =>
                draggableLearningSpaces.Any(dle =>
                    dle.Instance.Parameters[nameof(DraggableObjectInPathWay.ObjectInPathWay)] == le)));
            Assert.That(pathWays, Has.Count.EqualTo(learningPathWays.Count));
        });
    }

    [Test]
    public void Svg_MouseMove_CallsMouseService()
    {
        var mouseEventArgs = new MouseEventArgs();
        var systemUnderTest = GetLearningWorldViewForTesting();

        var svg = systemUnderTest.FindOrFail("svg");
        svg.MouseMove(mouseEventArgs);
        
        _mouseService.Received().FireMove(systemUnderTest.Instance, mouseEventArgs);
    }

    [Test]
    public void Svg_MouseUp_CallsMouseService()
    {
        var mouseEventArgs = new MouseEventArgs();
        var systemUnderTest = GetLearningWorldViewForTesting();

        var svg = systemUnderTest.FindOrFail("svg");
        svg.MouseUp(mouseEventArgs);
        
        _mouseService.Received().FireUp(systemUnderTest.Instance, mouseEventArgs);
    }

    [Test]
    public void Svg_MouseLeave_CallsMouseService()
    {
        var mouseEventArgs = new MouseEventArgs();
        var systemUnderTest = GetLearningWorldViewForTesting();

        var svg = systemUnderTest.FindOrFail("svg");
        svg.MouseLeave(mouseEventArgs);
        
        _mouseService.Received().FireOut(systemUnderTest.Instance, mouseEventArgs);
    }

    [Test]
    public void AddSpaceButton_Clicked_CallsAddNewLearningSpace()
    {
        var systemUnderTest = GetLearningWorldViewForTesting();

        var addSpaceButton = systemUnderTest.FindComponentWithMarkup<MudButton>("New Space");
        addSpaceButton.Find("*").Click();
        Assert.Fail("NYI");
    }
    
    [Test]
    public void AddConditionButton_Clicked_CallsAddNewPathWayCondition()
    {
        var systemUnderTest = GetLearningWorldViewForTesting();

        var addConditionButton = systemUnderTest.FindComponentWithMarkup<MudButton>("New Condition");
        //we search for * because we don't necessarily know what is inside the MudButton component as it is third party
        addConditionButton.Find("*").Click();
        _worldPresenter.Received().CreatePathWayCondition(ConditionEnum.Or);
    }
    
    [Test]
    public void LoadSpaceButton_Clicked_CallsLoadLearningSpaceAsync()
    {
        var systemUnderTest = GetLearningWorldViewForTesting();
        
        var loadSpaceButton = systemUnderTest.FindComponentWithMarkup<MudButton>("Load Space");
        loadSpaceButton.Find("*").Click();
        _worldPresenter.Received().LoadLearningSpaceAsync();
    }

    [Test]
    public void LoadSpaceButton_Clicked_OperationCancelledExceptionCaught()
    {
        _worldPresenter.LoadLearningSpaceAsync().Throws<OperationCanceledException>();
        
        var systemUnderTest = GetLearningWorldViewForTesting();
        
        var loadSpaceButton = systemUnderTest.FindComponentWithMarkup<MudButton>("Load Space");
        Assert.That(() => loadSpaceButton.Find("*").Click(), Throws.Nothing);
        _worldPresenter.Received().LoadLearningSpaceAsync();
    }

    [Test]
    public void EditSpaceButton_Clicked_CallsOpenEditSelectedLearningSpaceDialog()
    {
        var space = Substitute.For<ILearningSpaceViewModel>();
        var worldVm = Substitute.For<ILearningWorldViewModel>();
        worldVm.LearningSpaces.Returns(new List<ILearningSpaceViewModel> { space });
        worldVm.SelectedLearningObjectInPathWay.Returns(space);
        _worldPresenter.LearningWorldVm.Returns(worldVm);

        var systemUnderTest = GetLearningWorldViewForTesting();
        
        //TODO: fix? - n.stich
        Assert.Fail("NYI");
        var editSpaceButton = systemUnderTest.FindOrFail("button.btn.btn-primary.edit-learning-object");
        editSpaceButton.Click();
    }
    
    [Test]
    public void DeleteSpaceButton_Clicked_CallsDeleteSelectedLearningSpace()
    {
        var space = Substitute.For<ILearningSpaceViewModel>();
        var worldVm = Substitute.For<ILearningWorldViewModel>();
        worldVm.LearningSpaces.Returns(new List<ILearningSpaceViewModel> { space });
        worldVm.SelectedLearningObjectInPathWay.Returns(space);
        _worldPresenter.LearningWorldVm.Returns(worldVm);

        var systemUnderTest = GetLearningWorldViewForTesting();
        
        //TODO: fix? - n.stich
        Assert.Fail("NYI");
        var editSpaceButton = systemUnderTest.FindOrFail("button.btn.btn-primary.delete-learning-object");
        editSpaceButton.Click();
        _worldPresenter.Received().DeleteSelectedLearningObject();
    }
    
    private IRenderedComponent<LearningWorldView> GetLearningWorldViewForTesting(RenderFragment? childContent = null)
    {
        return _ctx.RenderComponent<LearningWorldView>(parameters => parameters
            .Add(p => p.ChildContent, childContent));
    }
}