using System.Globalization;
using BusinessLogic.Commands.LearningOutcomes;
using BusinessLogic.Entities.LearningOutcome;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using Shared.LearningOutcomes;
using TestHelpers;

namespace BusinessLogicTest.Commands.LearningOutcomes;

[TestFixture]
public class LearningOutcomeCommandFactoryUt
{
    [SetUp]
    public void Setup()
    {
        _factory = new LearningOutcomeCommandFactory(new NullLoggerFactory());
    }

    private LearningOutcomeCommandFactory _factory = null!;

    [Test]
    // ANF-ID: [AHO01, AHO03]
    public void GetAddLearningOutcomeCommand_StructuredLearningOutcome_ReturnsAddLearningOutcome()
    {
        var learningOutcomeCollection = EntityProvider.GetLearningOutcomeCollection();
        var taxonomyLevel = TaxonomyLevel.Level6;
        var what = "What";
        var verbOfVisibility = "VerbOfVisibility";
        var whereby = "Whereby";
        var whatFor = "WhatFor";
        var cultureInfo = CultureInfo.CurrentCulture;
        Action<LearningOutcomeCollection> mappingAction = _ => { };
        var index = 1;

        var result = _factory.GetAddLearningOutcomeCommand(learningOutcomeCollection, taxonomyLevel, what,
            verbOfVisibility, whereby, whatFor, cultureInfo, mappingAction, index);

        Assert.That(result, Is.InstanceOf<AddLearningOutcome>());
        var resultCasted = result as AddLearningOutcome;
        Assert.Multiple(() =>
        {
            Assert.That(resultCasted!.LearningOutcomeCollection, Is.EqualTo(learningOutcomeCollection));
            Assert.That(resultCasted.MappingAction, Is.EqualTo(mappingAction));
            Assert.That(resultCasted.LearningOutcome, Is.InstanceOf<StructuredLearningOutcome>());
            var learningOutcomeCasted = resultCasted.LearningOutcome as StructuredLearningOutcome;
            Assert.Multiple(() =>
            {
                Assert.That(learningOutcomeCasted!.TaxonomyLevel, Is.EqualTo(taxonomyLevel));
                Assert.That(learningOutcomeCasted.What, Is.EqualTo(what));
                Assert.That(learningOutcomeCasted.VerbOfVisibility, Is.EqualTo(verbOfVisibility));
                Assert.That(learningOutcomeCasted.Whereby, Is.EqualTo(whereby));
                Assert.That(learningOutcomeCasted.WhatFor, Is.EqualTo(whatFor));
                Assert.That(learningOutcomeCasted.Language, Is.EqualTo(cultureInfo));
            });
            Assert.That(resultCasted.Index, Is.EqualTo(index));
        });
    }

    [Test]
    // ANF-ID: [AHO02, AHO04]
    public void GetAddLearningOutcomeCommand_ManualLearningOutcome_ReturnsAddLearningOutcome()
    {
        var learningOutcomeCollection = EntityProvider.GetLearningOutcomeCollection();
        var manualLearningOutcomeText = "ManualLearningOutcomeText";
        Action<LearningOutcomeCollection> mappingAction = _ => { };
        var index = 1;

        var result = _factory.GetAddLearningOutcomeCommand(learningOutcomeCollection, manualLearningOutcomeText,
            mappingAction, index);

        Assert.That(result, Is.InstanceOf<AddLearningOutcome>());
        var resultCasted = result as AddLearningOutcome;
        Assert.Multiple(() =>
        {
            Assert.That(resultCasted!.LearningOutcomeCollection, Is.EqualTo(learningOutcomeCollection));
            Assert.That(resultCasted.MappingAction, Is.EqualTo(mappingAction));
            Assert.That(resultCasted.LearningOutcome, Is.InstanceOf<ManualLearningOutcome>());
            var learningOutcomeCasted = resultCasted.LearningOutcome as ManualLearningOutcome;
            Assert.That(learningOutcomeCasted!.Outcome, Is.EqualTo(manualLearningOutcomeText));
            Assert.That(resultCasted.Index, Is.EqualTo(index));
        });
    }

    [Test]
    // ANF-ID: [AHO03, AHO03, AHO05]
    public void GetDeleteLearningOutcomeCommand_ReturnsDeleteLearningOutcome()
    {
        var learningOutcomeCollection = EntityProvider.GetLearningOutcomeCollection();
        var learningOutcome = EntityProvider.GetLearningOutcomes().First();
        learningOutcomeCollection.LearningOutcomes.Add(learningOutcome);
        Action<LearningOutcomeCollection> mappingAction = _ => { };

        var result =
            _factory.GetDeleteLearningOutcomeCommand(learningOutcomeCollection, learningOutcome, mappingAction);

        Assert.That(result, Is.InstanceOf<DeleteLearningOutcome>());
        var resultCasted = result as DeleteLearningOutcome;
        Assert.Multiple(() =>
        {
            Assert.That(resultCasted!.LearningOutcomeCollection, Is.EqualTo(learningOutcomeCollection));
            Assert.That(resultCasted.MappingAction, Is.EqualTo(mappingAction));
            Assert.That(resultCasted.LearningOutcome, Is.EqualTo(learningOutcome));
        });
    }
}