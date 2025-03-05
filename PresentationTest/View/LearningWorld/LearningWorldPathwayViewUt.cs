using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using MudBlazor;
using NSubstitute;
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
using Shared.Command;
using TestContext = Bunit.TestContext;

namespace PresentationTest.View.LearningWorld;

[TestFixture]
public class LearningWorldPathwayViewUt
{
    [SetUp]
    public void Setup()
    {
        _ctx = new TestContext();
        _mouseService = Substitute.For<IMouseService>();
        _worldPresenter = Substitute.For<ILearningWorldPresenter>();
        _selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        _localizer = Substitute.For<IStringLocalizer<LearningWorldPathwayView>>();
        _localizer[Arg.Any<string>()].Returns(ci => new LocalizedString(ci.Arg<string>(), ci.Arg<string>()));
        _ctx.ComponentFactories.AddStub<LearningSpaceView>();
        _ctx.ComponentFactories.AddStub<DraggableObjectInPathWay>();
        _ctx.ComponentFactories.AddStub<PathWay>();
        _ctx.ComponentFactories.AddStub<MudIcon>();
        _ctx.ComponentFactories.AddStub<MudButton>();
        _ctx.Services.AddSingleton(_mouseService);
        _ctx.Services.AddSingleton(_worldPresenter);
        _ctx.Services.AddSingleton(_selectedViewModelsProvider);
        _ctx.Services.AddSingleton(_localizer);
    }

    [TearDown]
    public void TearDown()
    {
        _ctx.Dispose();
    }

    private TestContext _ctx = null!;
    private IMouseService _mouseService = null!;
    private ILearningWorldPresenter _worldPresenter = null!;
    private ISelectedViewModelsProvider _selectedViewModelsProvider = null!;
    private IStringLocalizer<LearningWorldPathwayView> _localizer = null!;

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
    public void OnParametersSet_EventHandlersRegisteredOnPresenter()
    {
        _ = GetLearningWorldViewForTesting();

        _worldPresenter.Received().PropertyChanging += Arg.Any<PropertyChangingEventHandler>();
        _worldPresenter.Received().PropertyChanged += Arg.Any<PropertyChangedEventHandler>();
        _worldPresenter.Received().OnCommandUndoRedoOrExecute += Arg.Any<EventHandler<CommandUndoRedoOrExecuteArgs>>();
        _selectedViewModelsProvider.Received().PropertyChanged += Arg.Any<PropertyChangedEventHandler>();
    }

    [Test]
    public void LearningWorldPresenter_PropertyChangingThenChanged_EventHandlersDeregisteredAndRegistered()
    {
        _ = GetLearningWorldViewForTesting();

        var learningWorldVm = Substitute.For<ILearningWorldViewModel>();
        _worldPresenter.LearningWorldVm.Returns(learningWorldVm);

        _worldPresenter.PropertyChanging += Raise.Event<PropertyChangingEventHandler>(new object(),
            new PropertyChangingEventArgs(nameof(ILearningWorldPresenter.LearningWorldVm)));

        learningWorldVm.Received().PropertyChanged -= Arg.Any<PropertyChangedEventHandler>();

        _worldPresenter.PropertyChanged += Raise.Event<PropertyChangedEventHandler>(new object(),
            new PropertyChangedEventArgs(nameof(ILearningWorldPresenter.LearningWorldVm)));

        learningWorldVm.Received().PropertyChanged += Arg.Any<PropertyChangedEventHandler>();
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

        var p = systemUnderTest.FindAllOrFail("p").ToList();
        p[1].MarkupMatches(
            @"<p class=""text-xs 2xl:text-base text-adlerblue-600""><span class=""text-adlergrey-600"">LearningWorldView.Workload.Text</span> 42<span class=""text-adlergrey-600"">LearningWorldView.Workload.TimeScale</span></p>");
        p[2].MarkupMatches(
            @"<p class=""text-xs 2xl:text-base text-adlerblue-600""><span class=""text-adlergrey-600"">LearningWorldView.Points.Text</span> 9<span class=""text-adlergrey-600"">LearningWorldPathwayView.Points.Summary</span></p>");
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
        var draggablePathWayConditions =
            systemUnderTest.FindComponentsOrFail<Stub<DraggablePathWayCondition>>().ToList();
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
    public async Task AddSpaceButton_Clicked_CallsAddNewLearningSpace()
    {
        var systemUnderTest = GetLearningWorldViewForTesting();

        var addSpaceButton = systemUnderTest.Find(".create-space-button");
        
        await addSpaceButton.ClickAsync(new MouseEventArgs());
        
        _worldPresenter.Received().AddNewLearningSpace();
    }

    [Test]
    public async Task AddConditionButton_Clicked_CallsAddNewPathWayCondition()
    {
        var systemUnderTest = GetLearningWorldViewForTesting();

        var addConditionButton = systemUnderTest.Find(".create-condition-button");

        await addConditionButton.ClickAsync(new MouseEventArgs());
        
        _worldPresenter.Received().CreatePathWayCondition();
    }

    private IRenderedComponent<LearningWorldPathwayView> GetLearningWorldViewForTesting(
        RenderFragment? childContent = null)
    {
        return _ctx.RenderComponent<LearningWorldPathwayView>(parameters => parameters
            .Add(p => p.ChildContent, childContent));
    }
}