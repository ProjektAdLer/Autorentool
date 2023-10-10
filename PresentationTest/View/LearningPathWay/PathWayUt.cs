using System;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.LearningPathway;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.PresentationLogic.SelectedViewModels;
using Presentation.View.LearningPathWay;
using Shared;
using TestContext = Bunit.TestContext;

namespace PresentationTest.View.LearningPathWay;

[TestFixture]
public class PathWayUt
{
    [SetUp]
    public void Setup()
    {
        _ctx = new TestContext();
        _mouseService = Substitute.For<IMouseService>();
        _positioningService = Substitute.For<ILearningWorldPresenter>();
        _selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        _ctx.Services.AddSingleton(_mouseService);
        _ctx.Services.AddSingleton(_selectedViewModelsProvider);
    }

    [TearDown]
    public void TearDown()
    {
        _ctx.Dispose();
    }

    private TestContext _ctx = null!;
    private IMouseService _mouseService = null!;
    private ILearningWorldPresenter _positioningService = null!;
    private ISelectedViewModelsProvider _selectedViewModelsProvider = null!;

    [Test]
    public void StandardConstructor_AllPropertiesInitialized()
    {
        double x1 = 10;
        double y1 = 20;
        double x2 = 30;
        double y2 = 50;

        var systemUnderTest = GetPathWayForTesting(x1, y1, x2, y2);

        Assert.That(systemUnderTest.Instance.Direction1, Is.EqualTo(Direction.Right));
        Assert.That(systemUnderTest.Instance.Direction2, Is.EqualTo(Direction.Left));
        Assert.That(systemUnderTest.Instance.X1, Is.EqualTo(x1));
        Assert.That(systemUnderTest.Instance.Y1, Is.EqualTo(y1));
        Assert.That(systemUnderTest.Instance.X2, Is.EqualTo(x2));
        Assert.That(systemUnderTest.Instance.Y2, Is.EqualTo(y2));
    }

    private IRenderedComponent<PathWay> GetPathWayForTesting(double x1 = 0, double y1 = 0, double x2 = 0,
        double y2 = 0, ILearningPathWayViewModel? pathWay = null, Action<ILearningPathWayViewModel>? onClicked = null)
    {
        onClicked ??= _ => { };
        return _ctx.RenderComponent<PathWay>(parameters => parameters
            .Add(p => p.LearningPathWay, pathWay)
            .Add(p => p.X1, x1)
            .Add(p => p.Y1, y1)
            .Add(p => p.X2, x2)
            .Add(p => p.Y2, y2)
            .Add(p => p.PositioningService, _positioningService)
            .Add(p => p.OnClickedClickable, onClicked)
        );
    }
}