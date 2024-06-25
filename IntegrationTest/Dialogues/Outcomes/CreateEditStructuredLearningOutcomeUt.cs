using Bunit;
using NUnit.Framework;
using Presentation.Components.LearningOutcomes;
using Presentation.PresentationLogic.LearningSpace.LearningOutcomeViewModel;
using TestHelpers;
using TestContext = Bunit.TestContext;

namespace IntegrationTest.Dialogues.Outcomes;

[TestFixture]
public class CreateEditStructuredLearningOutcomeUt
{
    private TestContext _context;

    [SetUp]
    public void Setup()
    {
        _context = new TestContext();
        _context.AddLocalizerForTest<CreateEditStructuredLearningOutcome>();
    }
    
    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }
 
    //TODO: Add tests for this dialog component

    private IRenderedComponent<CreateEditStructuredLearningOutcome> GetRenderedComponent(
        StructuredLearningOutcomeViewModel? outcome = null)
    {
        return _context.RenderComponent<CreateEditStructuredLearningOutcome>(pBuilder =>
        {
            pBuilder.Add(p => p.CurrentLearningOutcome, outcome);
        });
    }
}