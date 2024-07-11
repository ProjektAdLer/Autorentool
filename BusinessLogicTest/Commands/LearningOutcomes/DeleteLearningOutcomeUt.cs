using BusinessLogic.Commands.LearningOutcomes;
using BusinessLogic.Entities.LearningOutcome;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using TestHelpers;

namespace BusinessLogicTest.Commands.LearningOutcomes;

[TestFixture]
public class DeleteLearningOutcomeUt
{
    [Test]
    // ANF-ID: [AHO05, AHO03, AHO04]
    public void Execute_DeletesLearningOutcome()
    {
        // Arrange
        var learningOutcomeCollection = EntityProvider.GetLearningOutcomeCollection();
        var learningOutcome = EntityProvider.GetLearningOutcomes().First();
        learningOutcomeCollection.LearningOutcomes.Add(learningOutcome);
        var actionWasInvoked = false;
        var mappingAction = new Action<LearningOutcomeCollection>(_ => actionWasInvoked = true);

        var command = new DeleteLearningOutcome(learningOutcomeCollection, learningOutcome, mappingAction,
            new NullLogger<DeleteLearningOutcome>());

        Assert.That(learningOutcomeCollection.LearningOutcomes, Has.Count.EqualTo(4));

        command.Execute();

        Assert.That(learningOutcomeCollection.LearningOutcomes, Has.Count.EqualTo(3));
        Assert.That(actionWasInvoked, Is.True);
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        // Arrange
        var learningOutcomeCollection = EntityProvider.GetLearningOutcomeCollection();
        var learningOutcome = EntityProvider.GetLearningOutcomes().First();
        learningOutcomeCollection.LearningOutcomes.Add(learningOutcome);
        var actionWasInvoked = false;
        var mappingAction = new Action<LearningOutcomeCollection>(_ => actionWasInvoked = true);

        var command = new DeleteLearningOutcome(learningOutcomeCollection, learningOutcome, mappingAction,
            new NullLogger<DeleteLearningOutcome>());

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.Multiple((() =>
        {
            Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
            Assert.That(actionWasInvoked, Is.False);
        }));
    }

    [Test]
    public void UndoRedo_UndoesAndRedoesDeleteLearningOutcome()
    {
        // Arrange
        var learningOutcomeCollection = EntityProvider.GetLearningOutcomeCollection();
        var learningOutcome = EntityProvider.GetLearningOutcomes().First();
        learningOutcomeCollection.LearningOutcomes.Add(learningOutcome);
        var actionWasInvoked = false;
        var mappingAction = new Action<LearningOutcomeCollection>(_ => actionWasInvoked = true);

        var command = new DeleteLearningOutcome(learningOutcomeCollection, learningOutcome, mappingAction,
            new NullLogger<DeleteLearningOutcome>());

        Assert.That(learningOutcomeCollection.LearningOutcomes, Has.Count.EqualTo(4));
        Assert.That(actionWasInvoked, Is.False);
        command.Execute();

        Assert.That(learningOutcomeCollection.LearningOutcomes, Has.Count.EqualTo(3));
        Assert.That(actionWasInvoked, Is.True);
        actionWasInvoked = false;

        command.Undo();

        Assert.That(learningOutcomeCollection.LearningOutcomes, Has.Count.EqualTo(4));
        Assert.That(actionWasInvoked, Is.True);
        actionWasInvoked = false;

        command.Redo();

        Assert.That(learningOutcomeCollection.LearningOutcomes, Has.Count.EqualTo(3));
        Assert.That(actionWasInvoked, Is.True);
    }
}