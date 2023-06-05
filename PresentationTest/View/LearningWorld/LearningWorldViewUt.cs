using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bunit;
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
using Presentation.PresentationLogic.SelectedViewModels;
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
    private ISelectedViewModelsProvider _selectedViewModelsProvider;
#pragma warning restore CS8618
    
    [SetUp]
    public void Setup()
    {
        _ctx = new TestContext();
        _mouseService = Substitute.For<IMouseService>();
        _worldPresenter = Substitute.For<ILearningWorldPresenter>();
        _selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        _ctx.ComponentFactories.AddStub<LearningSpaceView>();
        _ctx.ComponentFactories.AddStub<DraggableObjectInPathWay>();
        _ctx.ComponentFactories.AddStub<PathWay>();
        _ctx.ComponentFactories.AddStub<MudIcon>();
        _ctx.ComponentFactories.AddStub<MudButton>();
        _ctx.Services.AddSingleton(_mouseService);
        _ctx.Services.AddSingleton(_worldPresenter);
        _ctx.Services.AddSingleton(_selectedViewModelsProvider);
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
        h3[0].MarkupMatches(
            @"<h3 class=""text-base text-adlerblue-600""><span class=""text-adlergrey-600"">Workload: </span> 42<span class=""text-adlergrey-600""> min.</span></h3>");
        h3[1].MarkupMatches(@"<h3 class=""text-base text-adlerblue-600""><span class=""text-adlergrey-600"">Points: </span> 9</h3>");
    }
    
    [Test]
    public void Render_LearningSpaceInWorld_CorrectDraggableForEachSpace()
    {
        var space1 = Substitute.For<ILearningSpaceViewModel>();
        var space2 = Substitute.For<ILearningSpaceViewModel>();
        var pathWayCondition = new PathWayConditionViewModel(ConditionEnum.And, false, 2, 1);
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
            //TODO: This Test fails on the CI (macos), but not locally. I have no idea why.
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
    public async Task AddSpaceButton_Clicked_CallsAddNewLearningSpace()
    {
        var systemUnderTest = GetLearningWorldViewForTesting();

        var addSpaceButton = systemUnderTest.FindComponents<Stub<MudButton>>().First(btn =>
            ((string)btn.Instance.Parameters["Class"]).Contains("add-learning-space"));
        await addSpaceButton.InvokeAsync(async () =>
            await ((EventCallback<MouseEventArgs>)addSpaceButton.Instance.Parameters["onclick"]).InvokeAsync(null));
        _worldPresenter.Received().AddNewLearningSpace();
    }
    
    [Test]
    public async Task AddConditionButton_Clicked_CallsAddNewPathWayCondition()
    {
        var systemUnderTest = GetLearningWorldViewForTesting();

        var addConditionButton = systemUnderTest.FindComponents<Stub<MudButton>>().First(btn =>
            ((string)btn.Instance.Parameters["Class"]).Contains("add-condition"));
        await addConditionButton.InvokeAsync(async () =>
            await ((EventCallback<MouseEventArgs>)addConditionButton.Instance.Parameters["onclick"]).InvokeAsync(null));
        _worldPresenter.Received().CreatePathWayCondition(ConditionEnum.Or);
    }
    
    [Test]
    public async Task LoadSpaceButton_Clicked_CallsLoadLearningSpaceAsync()
    {
        var systemUnderTest = GetLearningWorldViewForTesting();
        
        var loadSpaceButton = systemUnderTest.FindComponents<Stub<MudButton>>().First(btn =>
            ((string)btn.Instance.Parameters["Class"]).Contains("load-learning-space"));
        await loadSpaceButton.InvokeAsync(async () =>
            await ((EventCallback<MouseEventArgs>)loadSpaceButton.Instance.Parameters["onclick"]).InvokeAsync(null));
        await _worldPresenter.Received().LoadLearningSpaceAsync();
    }

    [Test]
    public async Task LoadSpaceButton_Clicked_OperationCancelledExceptionCaught()
    {
        _worldPresenter.LoadLearningSpaceAsync().Throws<OperationCanceledException>();
        
        var systemUnderTest = GetLearningWorldViewForTesting();
        
        var loadSpaceButton = systemUnderTest.FindComponents<Stub<MudButton>>().First(btn =>
            ((string)btn.Instance.Parameters["Class"]).Contains("load-learning-space"));
        await loadSpaceButton.InvokeAsync(async () =>
            await ((EventCallback<MouseEventArgs>)loadSpaceButton.Instance.Parameters["onclick"]).InvokeAsync(null));
        await _worldPresenter.Received().LoadLearningSpaceAsync();
    }

    private IRenderedComponent<LearningWorldView> GetLearningWorldViewForTesting(RenderFragment? childContent = null)
    {
        return _ctx.RenderComponent<LearningWorldView>(parameters => parameters
            .Add(p => p.ChildContent, childContent));
    }
}