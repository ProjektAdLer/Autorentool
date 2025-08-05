using NUnit.Framework;
using Presentation.PresentationLogic.LearningOutcome;

namespace PresentationTest.PresentationLogic.LearningSpace.LearningOutcomeViewModel;

[TestFixture]
public class ManualLearningOutcomeViewModelUt
{
    [Test]
    public void GetOutcome_ReturnsOutcome()
    {
        var outcome = "Outcome";
        var manualLearningOutcomeViewModel = new ManualLearningOutcomeViewModel(outcome);

        var result = manualLearningOutcomeViewModel.GetOutcome();

        Assert.That(outcome, Is.EqualTo(result));
    }
}