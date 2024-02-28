using System.Globalization;
using BusinessLogic.Commands.LearningOutcomes;
using BusinessLogic.Entities.LearningOutcome;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using Shared.LearningOutcomes;
using TestHelpers;

namespace BusinessLogicTest.Commands.LearningOutcomes;

[TestFixture]
public class AddLearningOutcomeUt
{
    [Test]
    public void Execute_AddsLearningOutcome_WithoutIndex()
    {
        // Arrange
        var learningOutcomeCollection = EntityProvider.GetLearningOutcomeCollection();
        var taxonomyLevel = TaxonomyLevel.Level6;
        var what = "What";
        var verbOfVisibility = "VerbOfVisibility";
        var whereby = "Whereby";
        var whatFor = "WhatFor";
        var cultureInfo = CultureInfo.CurrentCulture;
        var actionWasInvoked = false;
        Action<LearningOutcomeCollection> mappingAction = _ => actionWasInvoked = true;
        var index = -1;

        var command = new AddLearningOutcome(learningOutcomeCollection, taxonomyLevel, what,
            verbOfVisibility, whereby, whatFor, cultureInfo, mappingAction, new NullLogger<AddLearningOutcome>(),
            index);

        // Act
        command.Execute();

        // Assert
        Assert.That(learningOutcomeCollection.LearningOutcomes, Has.Count.EqualTo(4));
        Assert.That(actionWasInvoked, Is.True);
        var learningOutcome = learningOutcomeCollection.LearningOutcomes.Last();
        Assert.Multiple(() =>
        {
            Assert.That(learningOutcome, Is.InstanceOf<StructuredLearningOutcome>());
            var learningOutcomeCasted = learningOutcome as StructuredLearningOutcome;
            Assert.Multiple(() =>
            {
                Assert.That(learningOutcomeCasted!.TaxonomyLevel, Is.EqualTo(taxonomyLevel));
                Assert.That(learningOutcomeCasted.What, Is.EqualTo(what));
                Assert.That(learningOutcomeCasted.VerbOfVisibility, Is.EqualTo(verbOfVisibility));
                Assert.That(learningOutcomeCasted.Whereby, Is.EqualTo(whereby));
                Assert.That(learningOutcomeCasted.WhatFor, Is.EqualTo(whatFor));
                Assert.That(learningOutcomeCasted.Language, Is.EqualTo(cultureInfo));
            });
        });
    }

    [Test]
    public void Execute_AddsLearningOutcome_WithIndex()
    {
        // Arrange
        var learningOutcomeCollection = EntityProvider.GetLearningOutcomeCollection();
        var taxonomyLevel = TaxonomyLevel.Level6;
        var what = "What";
        var verbOfVisibility = "VerbOfVisibility";
        var whereby = "Whereby";
        var whatFor = "WhatFor";
        var cultureInfo = CultureInfo.CurrentCulture;
        var actionWasInvoked = false;
        Action<LearningOutcomeCollection> mappingAction = _ => actionWasInvoked = true;
        var index = 1;

        var command = new AddLearningOutcome(learningOutcomeCollection, taxonomyLevel, what,
            verbOfVisibility, whereby, whatFor, cultureInfo, mappingAction, new NullLogger<AddLearningOutcome>(),
            index);

        // Act
        command.Execute();

        // Assert
        Assert.That(learningOutcomeCollection.LearningOutcomes, Has.Count.EqualTo(4));
        var learningOutcome = learningOutcomeCollection.LearningOutcomes.ElementAt(index);
        Assert.That(actionWasInvoked, Is.True);
        Assert.Multiple(() =>
        {
            Assert.That(learningOutcome, Is.InstanceOf<StructuredLearningOutcome>());
            var learningOutcomeCasted = learningOutcome as StructuredLearningOutcome;
            Assert.Multiple(() =>
            {
                Assert.That(learningOutcomeCasted!.TaxonomyLevel, Is.EqualTo(taxonomyLevel));
                Assert.That(learningOutcomeCasted.What, Is.EqualTo(what));
                Assert.That(learningOutcomeCasted.VerbOfVisibility, Is.EqualTo(verbOfVisibility));
                Assert.That(learningOutcomeCasted.Whereby, Is.EqualTo(whereby));
                Assert.That(learningOutcomeCasted.WhatFor, Is.EqualTo(whatFor));
                Assert.That(learningOutcomeCasted.Language, Is.EqualTo(cultureInfo));
            });
        });
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        // Arrange
        var learningOutcomeCollection = EntityProvider.GetLearningOutcomeCollection();
        var taxonomyLevel = TaxonomyLevel.Level6;
        var what = "What";
        var verbOfVisibility = "VerbOfVisibility";
        var whereby = "Whereby";
        var whatFor = "WhatFor";
        var cultureInfo = CultureInfo.CurrentCulture;
        var actionWasInvoked = false;
        Action<LearningOutcomeCollection> mappingAction = _ => actionWasInvoked = true;
        var index = 1;

        var command = new AddLearningOutcome(learningOutcomeCollection, taxonomyLevel, what,
            verbOfVisibility, whereby, whatFor, cultureInfo, mappingAction, new NullLogger<AddLearningOutcome>(),
            index);

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.Multiple((() =>
        {
            Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
            Assert.That(actionWasInvoked, Is.False);
        }));
    }

    [Test]
    public void UndoRedo_UndoesAndRedoesAddLearningOutcome()
    {
        var learningOutcomeCollection = EntityProvider.GetLearningOutcomeCollection();
        var taxonomyLevel = TaxonomyLevel.Level6;
        var what = "What";
        var verbOfVisibility = "VerbOfVisibility";
        var whereby = "Whereby";
        var whatFor = "WhatFor";
        var cultureInfo = CultureInfo.CurrentCulture;
        var actionWasInvoked = false;
        Action<LearningOutcomeCollection> mappingAction = _ => actionWasInvoked = true;
        var index = 1;

        var command = new AddLearningOutcome(learningOutcomeCollection, taxonomyLevel, what,
            verbOfVisibility, whereby, whatFor, cultureInfo, mappingAction, new NullLogger<AddLearningOutcome>(),
            index);

        Assert.That(learningOutcomeCollection.LearningOutcomes, Has.Count.EqualTo(3));
        Assert.That(actionWasInvoked, Is.False);
        command.Execute();

        Assert.That(learningOutcomeCollection.LearningOutcomes, Has.Count.EqualTo(4));
        Assert.That(actionWasInvoked, Is.True);
        actionWasInvoked = false;

        command.Undo();

        Assert.That(learningOutcomeCollection.LearningOutcomes, Has.Count.EqualTo(3));
        Assert.That(actionWasInvoked, Is.True);
        actionWasInvoked = false;

        command.Redo();

        Assert.That(learningOutcomeCollection.LearningOutcomes, Has.Count.EqualTo(4));
        Assert.That(actionWasInvoked, Is.True);
    }
}