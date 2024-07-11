using System.Threading.Tasks;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components.Web;
using NUnit.Framework;
using Presentation.Components.Forms;
using Presentation.Components.LearningOutcomes;
using Presentation.PresentationLogic.LearningWorld;
using TestHelpers;
using TestContext = Bunit.TestContext;

namespace PresentationTest.Components.LearningOutcomes;

[TestFixture]
public class LearningOutcomesWorldOverviewUt
{
    private TestContext _context;

    [SetUp]
    public void Setup()
    {
        _context = new TestContext();
        _context.AddLocalizerForTest<LearningOutcomesWorldOverview>();
        _context.ComponentFactories.AddStub<LearningOutcomeItem>();
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public void Render_NoOutcomesInWorld_RendersNoOutcomeP()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var space = ViewModelProvider.GetLearningSpace();
        space.LearningOutcomeCollection.LearningOutcomes.Clear();
        world.LearningSpaces.Add(space);
        
        var sut = GetRenderedComponent(world);

        var element = sut.FindOrFail("p.no-outcomes");
        element.MarkupMatches(@"<p class=""opacity-80 cursor-default pl-1 text-base font-bold text-adlertextgrey no-outcomes"">LearningOutcomesWorldOverview.NoOutcome</p>");
    }

    [Test]
    public async Task Render_OutcomesInWorld_RendersOutcomes()
    {
        var world = ViewModelProvider.GetLearningWorld();
        var space = ViewModelProvider.GetLearningSpace();
        space.LearningOutcomeCollection.LearningOutcomes.Clear();
        space.LearningOutcomeCollection.LearningOutcomes.Add(ViewModelProvider.GetLearningOutcome());
        world.LearningSpaces.Add(space);
        
        var sut = GetRenderedComponent(world);

        var collapsable = sut.FindComponent<Collapsable>();
        await collapsable.Find("div.toggler").ClickAsync(new MouseEventArgs());
        
        Assert.That(sut.FindComponents<Stub<LearningOutcomeItem>>(), Has.Count.EqualTo(1));
    }

    private IRenderedComponent<LearningOutcomesWorldOverview> GetRenderedComponent(ILearningWorldViewModel world)
    {
        return _context.RenderComponent<LearningOutcomesWorldOverview>(pBuilder =>
        {
            pBuilder.Add(p => p.LearningWorld, world);
        });
    }
}