using Bunit;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.LearningSpace;
using Shared;
using TestContext = Bunit.TestContext;

namespace PresentationTest.Components;

[TestFixture]

public class PullablePathUt
{
    #pragma warning disable CS8618
    private TestContext _testContext;
    private IMouseService _mouseService;
#pragma warning restore CS8618

    [SetUp]
    public void Setup()
    {
        _testContext = new TestContext();
        _mouseService = Substitute.For<IMouseService>();
        _testContext.Services.AddSingleton(_mouseService);
    }

    [TearDown]
    public void TearDown() => _testContext.Dispose();

    [Test]
    public void StandardConstructor_AllPropertiesInitialized()
    {
        var learningObject = Substitute.For<ILearningSpaceViewModel>();
        double x1 = 5;
        double y1 = 6;
        double x2 = 10;
        double y2 = 11;

        var systemUnderTest =
            CreateRenderedPullablePathComponent(learningObject, x1, y1, x2, y2);

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.LearningSpace, Is.EqualTo(learningObject));
            Assert.That(systemUnderTest.Instance.X1, Is.EqualTo(x1));
            Assert.That(systemUnderTest.Instance.Y1, Is.EqualTo(y1));
            Assert.That(systemUnderTest.Instance.X2, Is.EqualTo(x1));
            Assert.That(systemUnderTest.Instance.Y2, Is.EqualTo(y1));
        });
    }
    
    private IRenderedComponent<PullablePath> CreateRenderedPullablePathComponent(
        ILearningSpaceViewModel? learningObject = null, double x1 = 0, double y1 = 0, double x2 = 0, double y2 = 0,
        Direction dir1 = Direction.Right, Direction dir2 = Direction.Left)
    {
        return _testContext.RenderComponent<PullablePath>(parameters => parameters
            .Add(p => p.LearningSpace, learningObject)
            .Add(p => p.X1, x1)
            .Add(p => p.Y1, y1)
            .Add(p => p.X2, x2)
            .Add(p => p.Y2, y2)
            .Add(p => p.Dir1, dir1)
            .Add(p => p.Dir2, dir2)
        );
    }
}