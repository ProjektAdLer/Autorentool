using Bunit;
using NUnit.Framework;
using Presentation.View.LearningWorld;
using Shared;
using TestContext = Bunit.TestContext;

namespace PresentationTest.View.LearningWorld;

[TestFixture]
public class PathWayUt
{
#pragma warning disable CS8618 // set in setup - n.stich
    private TestContext _ctx;
#pragma warning restore CS8618
    
    [SetUp]
    public void Setup()
    {
        _ctx = new TestContext();
    }
    
    [Test]
    public void StandardConstructor_AllPropertiesInitialized()
    {
        double x1 = 10;
        double y1 = 20;
        double x2 = 30;
        double y2 = 50;
        
        var systemUnderTest = GetPathWayForTesting(x1, y1, x2, y2);
        
        Assert.That(systemUnderTest.Instance.Dir1, Is.EqualTo(Direction.Right));
        Assert.That(systemUnderTest.Instance.Dir2, Is.EqualTo(Direction.Left));
        Assert.That(systemUnderTest.Instance.X1, Is.EqualTo(x1));
        Assert.That(systemUnderTest.Instance.Y1, Is.EqualTo(y1));
        Assert.That(systemUnderTest.Instance.X2, Is.EqualTo(x2));
        Assert.That(systemUnderTest.Instance.Y2, Is.EqualTo(y2));
    }
    
    private IRenderedComponent<PathWay> GetPathWayForTesting( double x1 = 0, double y1 = 0, double x2 = 0, double y2 = 0)
    {
        return _ctx.RenderComponent<PathWay>(parameters => parameters
            .Add(p => p.X1, x1)
            .Add(p => p.Y1, y1)
            .Add(p => p.X2, x2)
            .Add(p => p.Y2, y2));
    }
}