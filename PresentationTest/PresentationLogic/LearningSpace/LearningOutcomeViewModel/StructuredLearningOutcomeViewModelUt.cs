using System;
using System.Globalization;
using NUnit.Framework;
using Presentation.PresentationLogic.LearningSpace.LearningOutcomeViewModel;
using Shared.LearningOutcomes;

namespace PresentationTest.PresentationLogic.LearningSpace.LearningOutcomeViewModel;

[TestFixture]
public class StructuredLearningOutcomeViewModelUt
{
    [Test]
    public void GetOutcome_ReturnsOutcome_De()
    {
        var structuredLearningOutcomeViewModel = new StructuredLearningOutcomeViewModel(TaxonomyLevel.Level1,
            "Programmieren", "programmieren", "programmieren zu können", "lernen",
            new CultureInfo("de-DE"));

        var result = structuredLearningOutcomeViewModel.GetOutcome();

        Assert.That("Sie können Programmieren lernen, \n indem Sie programmieren,\n um programmieren zu können.",
            Is.EqualTo(result));
    }

    [Test]
    public void GetOutcome_ReturnsOutcome_En()
    {
        var structuredLearningOutcomeViewModel = new StructuredLearningOutcomeViewModel(TaxonomyLevel.Level1,
            "Programing", "programing", "be able to program", "learn",
            new CultureInfo("en-DE"));

        var result = structuredLearningOutcomeViewModel.GetOutcome();

        Assert.That("You will be able to learn Programing \n by programing \n to be able to program.",
            Is.EqualTo(result));
    }

    [Test]
    public void GetOutcome_LanguageNotSupported_ThrowsNotSupportedException()
    {
        var structuredLearningOutcomeViewModel = new StructuredLearningOutcomeViewModel(TaxonomyLevel.Level1,
            "Programing", "programing", "be able to program", "learn",
            new CultureInfo("en-US"));

        Assert.Throws<NotSupportedException>(() => structuredLearningOutcomeViewModel.GetOutcome());
    }
}