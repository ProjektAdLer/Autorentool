using System;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components;
using Presentation.PresentationLogic;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.LearningPathway;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.PresentationLogic.SelectedViewModels;
using Presentation.View.LearningPathWay;
using Shared;
using Shared.Theme;
using TestContext = Bunit.TestContext;

namespace PresentationTest.View.LearningPathWay;

[TestFixture]
public class DraggableObjectInPathWay
{
    [SetUp]
    public void Setup()
    {
        _dialogService = Substitute.For<IDialogService>();
        _snackbar = Substitute.For<ISnackbar>();
        _draggableLearningSpaceStringLocalizer = Substitute.For<IStringLocalizer<DraggableLearningSpace>>();
        _draggableLearningSpaceStringLocalizer[Arg.Any<string>()]
            .Returns(cinfo => new LocalizedString(cinfo.Arg<string>(), cinfo.Arg<string>()));

        _draggablePathWayConditionStringLocalizer = Substitute.For<IStringLocalizer<DraggablePathWayCondition>>();
        _draggablePathWayConditionStringLocalizer[Arg.Any<string>()]
            .Returns(cinfo => new LocalizedString(cinfo.Arg<string>(), cinfo.Arg<string>()));
        
        
        _ctx = new TestContext();
        _ctx.Services.AddSingleton(_draggablePathWayConditionStringLocalizer);
        _ctx.Services.AddSingleton(_draggableLearningSpaceStringLocalizer);
        _ctx.Services.AddSingleton(_dialogService);
        _ctx.ComponentFactories.AddStub<Draggable<IObjectInPathWayViewModel>>();
        _mouseService = Substitute.For<IMouseService>();
        _selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        _ctx.Services.AddSingleton(_mouseService);
        _ctx.Services.AddSingleton(_snackbar);
        _ctx.Services.AddSingleton(_selectedViewModelsProvider);
    }

    [TearDown]
    public void TearDown()
    {
        _ctx.Dispose();
        _snackbar.Dispose();
    }

    private TestContext _ctx;
    private IStringLocalizer<DraggableLearningSpace> _draggableLearningSpaceStringLocalizer;
    private IStringLocalizer<DraggablePathWayCondition> _draggablePathWayConditionStringLocalizer;
    private IMouseService _mouseService;
    private ISelectedViewModelsProvider _selectedViewModelsProvider;
    private IDialogService _dialogService;
    private ISnackbar _snackbar;

    [Test]
    public void Constructor_SetsParametersCorrectly_LearningSpace()
    {
        var learningSpace = new LearningSpaceViewModel("a", "b", SpaceTheme.LearningArea, 1);
        var onClicked = new Action<IObjectInPathWayViewModel>(_ => { });
        var onDragged = new DraggedEventArgs<IObjectInPathWayViewModel>.DraggedEventHandler((_, _) => { });
        var onDoubleClicked = new Action<IObjectInPathWayViewModel>(_ => { });
        var onDeleteLearningSpace = new Action<ILearningSpaceViewModel>(_ => { });
        var onRemoveLearningSpaceFromTopic = new Action<ILearningSpaceViewModel>(_ => { });
        var positioningService = Substitute.For<ILearningWorldPresenter>();
        var systemUnderTest =
            GetRenderedDraggableLearningSpace(learningSpace, onClicked, onDragged, onDoubleClicked,
                onDeleteLearningSpace, onRemoveLearningSpaceFromTopic, positioningService);

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.ObjectInPathWay, Is.EqualTo(learningSpace));
            //overriding nullability warning because we know target isn't null as onClicked isn't a static method but instead a lambda -n.stich
            Assert.That(systemUnderTest.Instance.OnClickedDraggable,
                Is.EqualTo(EventCallback.Factory.Create(onClicked.Target!, onClicked)));
            Assert.That(systemUnderTest.Instance.OnDoubleClickedDraggable,
                Is.EqualTo(EventCallback.Factory.Create(onDoubleClicked.Target!, onDoubleClicked)));
            Assert.That(systemUnderTest.Instance.OnDeleteLearningSpace,
                Is.EqualTo(EventCallback.Factory.Create(onDeleteLearningSpace.Target!, onDeleteLearningSpace)));
            Assert.That(systemUnderTest.Instance.OnRemoveLearningSpaceFromTopic,
                Is.EqualTo(EventCallback.Factory.Create(onRemoveLearningSpaceFromTopic.Target!,
                    onRemoveLearningSpaceFromTopic)));
            Assert.That(systemUnderTest.Instance.PositioningService, Is.EqualTo(positioningService));
        });
    }

    [Test]
    public void Constructor_SetsParametersCorrectly_PathWayCondition()
    {
        var pathWayCondition = new PathWayConditionViewModel(ConditionEnum.And, false, 2, 1);
        var onClicked = new Action<IObjectInPathWayViewModel>(_ => { });
        var onDragged = new DraggedEventArgs<IObjectInPathWayViewModel>.DraggedEventHandler((_, _) => { });
        var onDoubleClicked = new Action<IObjectInPathWayViewModel>(_ => { });
        var onDeletePathWayCondition = new Action<PathWayConditionViewModel>(_ => { });
        var positioningService = Substitute.For<ILearningWorldPresenter>();
        var systemUnderTest =
            GetRenderedDraggablePathWayCondition(pathWayCondition, onClicked, onDragged, onDoubleClicked,
                onDeletePathWayCondition, positioningService);

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.ObjectInPathWay, Is.EqualTo(pathWayCondition));
            //overriding nullability warning because we know target isn't null as onClicked isn't a static method but instead a lambda -n.stich
            Assert.That(systemUnderTest.Instance.OnClickedDraggable,
                Is.EqualTo(EventCallback.Factory.Create(onClicked.Target!, onClicked)));
            Assert.That(systemUnderTest.Instance.OnDoubleClickedDraggable,
                Is.EqualTo(EventCallback.Factory.Create(onDoubleClicked.Target!, onDoubleClicked)));
            Assert.That(systemUnderTest.Instance.OnDeletePathWayCondition,
                Is.EqualTo(EventCallback.Factory.Create(onDeletePathWayCondition.Target!, onDeletePathWayCondition)));
            Assert.That(systemUnderTest.Instance.PositioningService, Is.EqualTo(positioningService));
        });
    }

    [Test]
    public void Constructor_PassesCorrectValuesToDraggable_LearningSpace()
    {
        var learningSpace = Substitute.For<ILearningSpaceViewModel>();
        var spaceName = "foo bar super cool name";
        learningSpace.Name.Returns(spaceName);
        var onClicked = new Action<IObjectInPathWayViewModel>(_ => { });
        var onDragged = new DraggedEventArgs<IObjectInPathWayViewModel>.DraggedEventHandler((_, _) => { });
        var onDoubleClicked = new Action<IObjectInPathWayViewModel>(_ => { });
        var onDeleteLearningSpace = new Action<ILearningSpaceViewModel>(_ => { });
        var onRemoveLearningSpaceFromTopic = new Action<ILearningSpaceViewModel>(_ => { });
        var positioningService = Substitute.For<ILearningWorldPresenter>();
        var systemUnderTest =
            GetRenderedDraggableLearningSpace(learningSpace, onClicked, onDragged, onDoubleClicked,
                onDeleteLearningSpace,
                onRemoveLearningSpaceFromTopic,
                positioningService);

        Assert.That(systemUnderTest.HasComponent<Stub<Draggable<IObjectInPathWayViewModel>>>());
        var stub = systemUnderTest.FindComponent<Stub<Draggable<IObjectInPathWayViewModel>>>();
        Assert.Multiple(() =>
        {
            Assert.That(stub.Instance.Parameters[nameof(Draggable<ILearningSpaceViewModel>.LearningObject)],
                Is.EqualTo(learningSpace));
            //overriding nullability warning because we know target isn't null as onClicked isn't a static method but instead a lambda -n.stich
            Assert.That(stub.Instance.Parameters[nameof(Draggable<ILearningSpaceViewModel>.OnClicked)],
                Is.EqualTo(EventCallback.Factory.Create(onClicked.Target!, onClicked)));
            Assert.That(stub.Instance.Parameters[nameof(Draggable<ILearningSpaceViewModel>.ChildContent)], Is.Not.Null);
        });
        var childContent =
            _ctx.Render(
                (RenderFragment)stub.Instance.Parameters[nameof(Draggable<ILearningSpaceViewModel>.ChildContent)]);
        childContent.MarkupMatches(
            $@"
<svg id=""uuid-ed2d2eeb-03d9-453b-94f0-6f55eb9b4a74"" data-name=""R-Lernraum"" xmlns=""http://www.w3.org/2000/svg"" width=""5rem"" height=""5rem"" viewBox=""-100 -100 2200 2200"">
  <polygon points=""152 1409.6 956.37 945.2 1760.73 1409.6 956.37 1874 152 1409.6"" style=""fill: #e9f2fa; fill-rule: evenodd; stroke: #172d4d; stroke-miterlimit: 10; stroke-width: 60px; opacity: .3;""></polygon>
  <path d=""m977.38,899.86c-4.98-2.88-11.13-2.88-16.12,0L64.03,1417.89c-4.99,2.88-8.07,8.21-8.07,13.97,0,5.76,3.08,11.09,8.08,13.98l897.23,518.02c4.98,2.88,11.13,2.88,16.12,0l897.23-518.02c4.99-2.88,8.07-8.21,8.08-13.97,0-5.77-3.08-11.09-8.06-13.97l-897.24-518.02Zm-8.06,32.6l864.97,499.39-864.97,499.39L104.35,1431.85l864.97-499.39h0Z"" style=""fill: #08234d; fill-rule: evenodd; stroke: #172d4d; stroke-miterlimit: 10; stroke-width: 60px; opacity: .3;""></path>
  <g>
    <polygon points=""999.57 138 1834 594.38 1834 1336 999.57 879.62 999.57 138"" style=""fill: #e9f2fa; fill-rule: evenodd; stroke: #172d4d; stroke-miterlimit: 10; stroke-width: 60px; opacity: .3;""></polygon>
    <path d=""m1901,576.36c0-5.75-3.08-11.07-8.07-13.95L995.11,45.16c-4.99-2.88-11.15-2.87-16.14,0-4.99,2.87-8.07,8.19-8.07,13.95v840.54c0,5.76,3.08,11.07,8.07,13.95l897.81,517.25c4.99,2.88,11.15,2.88,16.14,0,4.99-2.87,8.07-8.19,8.07-13.95v-840.54Zm-32.29,9.3v803.33s-777.55-447.97-865.53-498.66V87.01l865.53,498.66h0Z"" style=""fill: #172d4d; fill-rule: evenodd; stroke: #172d4d; stroke-miterlimit: 10; stroke-width: 60px; opacity: .3;""></path>
  </g>
  <g>
    <polygon points=""140 594.38 937 138 937 879.62 140 1336 140 594.38"" style=""fill: #e9f2fa; fill-rule: evenodd; stroke: #172d4d; stroke-miterlimit: 10; stroke-width: 60px; opacity: .3;""></polygon>
    <path d=""m991.23,59.27c0-5.82-3.1-11.19-8.13-14.09-5.03-2.9-11.24-2.91-16.27,0L61.9,567.64c-5.03,2.91-8.13,8.28-8.13,14.09v849c0,5.82,3.1,11.19,8.13,14.09,5.03,2.91,11.24,2.91,16.27,0l904.93-522.46c5.03-2.91,8.13-8.27,8.13-14.09V59.27Zm-32.54,28.18v811.42L86.3,1402.54v-811.42L958.69,87.45h0Z"" style=""fill: #172d4d; fill-rule: evenodd; stroke: #172d4d; stroke-miterlimit: 10; stroke-width: 60px; opacity: .3;""></path>
  </g>
  <rect transform=""translate(-50,-50)"" height=""2100"" width=""2100"" rx=""100"" style=""fill:#e9e9e9;opacity:80%;stroke:rgb(204,204,204);stroke-width:50""></rect>
  <foreignObject x=""80"" y=""750"" width=""1800"" height=""600"">
    <p style=""font-family: Roboto, sans-serif; font-size: 350px; font-weight:bold; user-select:none; color:#111111; text-align:center;"">foo bar super cool name</p>
  </foreignObject>
</svg>
<g  ></g>
<g  >
  <g transform=""translate(60,2) scale(0.8,0.8)"">
    <path d=""M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z"" fill=""gray"" style=""user-select:none; cursor: pointer"">
    </path>
  </g>
  <title>DraggableLearningSpace.Delete</title>
</g>
<title>foo bar super cool name</title>
            ");
    }

    [Test]
    public void Constructor_PassesCorrectValuesToDraggable_PathWayCondition()
    {
        var pathWayCondition = new PathWayConditionViewModel(ConditionEnum.And, false, 2, 1);
        var onClicked = new Action<IObjectInPathWayViewModel>(_ => { });
        var onDragged = new DraggedEventArgs<IObjectInPathWayViewModel>.DraggedEventHandler((_, _) => { });
        var onDoubleClicked = new Action<IObjectInPathWayViewModel>(_ => { });
        var onDeletePathWayCondition = new Action<PathWayConditionViewModel>(_ => { });
        var positioningService = Substitute.For<ILearningWorldPresenter>();
        var systemUnderTest =
            GetRenderedDraggablePathWayCondition(pathWayCondition, onClicked, onDragged, onDoubleClicked,
                onDeletePathWayCondition,
                positioningService);

        Assert.That(systemUnderTest.HasComponent<Stub<Draggable<IObjectInPathWayViewModel>>>());
        var stub = systemUnderTest.FindComponent<Stub<Draggable<IObjectInPathWayViewModel>>>();
        Assert.Multiple(() =>
        {
            Assert.That(stub.Instance.Parameters[nameof(Draggable<PathWayConditionViewModel>.LearningObject)],
                Is.EqualTo(pathWayCondition));
            //overriding nullability warning because we know target isn't null as onClicked isn't a static method but instead a lambda -n.stich
            Assert.That(stub.Instance.Parameters[nameof(Draggable<PathWayConditionViewModel>.OnClicked)],
                Is.EqualTo(EventCallback.Factory.Create(onClicked.Target!, onClicked)));
            Assert.That(stub.Instance.Parameters[nameof(Draggable<PathWayConditionViewModel>.ChildContent)],
                Is.Not.Null);
        });
        var childContent =
            _ctx.Render(
                (RenderFragment)stub.Instance.Parameters[nameof(Draggable<PathWayConditionViewModel>.ChildContent)]);
        childContent.MarkupMatches(
            @"<rect x=""0"" y=""0"" width=""75"" height=""41.5"" rx=""2"" style=""fill:#e9e9e9;opacity:80%;stroke:rgb(204,204,204);stroke-width:1""></rect>
            <g  ></g>
            <g  >
            <g transform=""translate(59,-1) scale(0.8,0.8)"">
            <path d=""M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z"" fill=""gray"" style=""user-select:none; cursor: pointer"">
            </path>
            </g>
            <title>DraggablePathWayCondition.Delete</title>
            </g>
            <title></title>");
    }

    [Test]
    public void Constructor_SpaceNull_ThrowsException()
    {
        //Override warning for this test as we are testing exactly what happens when we break the nullability contract - n.stich
        Assert.That(
            () => GetRenderedDraggableLearningSpace(null!, _ => { }, (_, _) => { }, _ => { }, _ => { },
                _ => { }, null!), Throws.ArgumentNullException);
    }

    [Test]
    public void Constructor_ConditionNull_ThrowsException()
    {
        //Override warning for this test as we are testing exactly what happens when we break the nullability contract - n.stich
        Assert.That(
            () => GetRenderedDraggablePathWayCondition(null!, _ => { }, (_, _) => { }, _ => { },
                _ => { }, null!), Throws.ArgumentNullException);
    }

    private IRenderedComponent<DraggableLearningSpace> GetRenderedDraggableLearningSpace(
        IObjectInPathWayViewModel objectViewmodel, Action<IObjectInPathWayViewModel> onClicked,
        DraggedEventArgs<IObjectInPathWayViewModel>.DraggedEventHandler onDragged,
        Action<IObjectInPathWayViewModel> onDoubleClicked, 
        Action<ILearningSpaceViewModel> onDeleteLearningSpace,
        Action<ILearningSpaceViewModel> onRemoveLearningSpaceFromTopic,
        ILearningWorldPresenter positioningService)
    {
        return _ctx.RenderComponent<DraggableLearningSpace>(parameters => parameters
            .Add(p => p.ObjectInPathWay, objectViewmodel)
            .Add(p => p.OnClickedDraggable, onClicked)
            .Add(p => p.OnDraggedDraggable, onDragged)
            .Add(p => p.OnDoubleClickedDraggable, onDoubleClicked)
            .Add(p => p.OnDeleteLearningSpace, onDeleteLearningSpace)
            .Add(p => p.OnRemoveLearningSpaceFromTopic, onRemoveLearningSpaceFromTopic)
            .Add(p => p.PositioningService, positioningService)
        );
    }

    private IRenderedComponent<DraggablePathWayCondition> GetRenderedDraggablePathWayCondition(
        IObjectInPathWayViewModel objectViewmodel, Action<IObjectInPathWayViewModel> onClicked,
        DraggedEventArgs<IObjectInPathWayViewModel>.DraggedEventHandler onDragged,
        Action<IObjectInPathWayViewModel> onDoubleClicked,
        Action<PathWayConditionViewModel> onDeletePathWayCondition,
        ILearningWorldPresenter positioningService)
    {
        return _ctx.RenderComponent<DraggablePathWayCondition>(parameters => parameters
            .Add(p => p.ObjectInPathWay, objectViewmodel)
            .Add(p => p.OnClickedDraggable, onClicked)
            .Add(p => p.OnDraggedDraggable, onDragged)
            .Add(p => p.OnDoubleClickedDraggable, onDoubleClicked)
            .Add(p => p.OnDeletePathWayCondition, onDeletePathWayCondition)
            .Add(p => p.PositioningService, positioningService)
        );
    }
}