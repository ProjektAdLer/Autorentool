using BusinessLogic.Commands.Element;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace BusinessLogicTest.Commands.Element;

[TestFixture]
public class CreateUnplacedLearningElementUt
{
    [Test]
    public void Execute_CreatesLearningElement()
    {
        var testParameter = new TestParameter();
        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;
        var command = new CreateUnplacedLearningElement(testParameter.WorldParent, testParameter.Name,
            testParameter.Content, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
            testParameter.ElementModel, testParameter.Workload, testParameter.Points, testParameter.PositionX,
            testParameter.PositionY, mappingAction, new NullLogger<CreateUnplacedLearningElement>());

        Assert.Multiple(() =>
        {
            Assert.That(testParameter.WorldParent.UnplacedLearningElements, Is.Empty);
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(testParameter.WorldParent.UnsavedChanges, Is.False);
        });

        command.Execute();

        var element = testParameter.WorldParent.UnplacedLearningElements.First();
        Assert.Multiple(() =>
        {
            Assert.That(testParameter.WorldParent.UnplacedLearningElements.Count, Is.EqualTo(1));
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(element.Name, Is.EqualTo(testParameter.Name));
            Assert.That(element.LearningContent, Is.EqualTo(testParameter.Content));
            Assert.That(element.Description, Is.EqualTo(testParameter.Description));
            Assert.That(element.Goals, Is.EqualTo(testParameter.Goals));
            Assert.That(element.Workload, Is.EqualTo(testParameter.Workload));
            Assert.That(element.Points, Is.EqualTo(testParameter.Points));
            Assert.That(element.Difficulty, Is.EqualTo(testParameter.Difficulty));
            Assert.That(testParameter.WorldParent.UnsavedChanges, Is.True);
        });
    }

    [Test]
    public void UndoRedo_UndoesRedoesCreateLearningElement()
    {
        var testParameter = new TestParameter();
        var worldParent = testParameter.WorldParent;
        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;
        var command = new CreateUnplacedLearningElement(testParameter.WorldParent, testParameter.Name,
            testParameter.Content, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
            testParameter.ElementModel, testParameter.Workload, testParameter.Points, testParameter.PositionX,
            testParameter.PositionY, mappingAction, new NullLogger<CreateUnplacedLearningElement>());

        Assert.Multiple(() =>
        {
            Assert.That(worldParent.UnplacedLearningElements, Is.Empty);
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(worldParent.UnsavedChanges, Is.False);
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(worldParent.UnplacedLearningElements, Has.Count.EqualTo(1));
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(worldParent.UnsavedChanges, Is.True);
        });

        actionWasInvoked = false;

        command.Undo();

        Assert.Multiple(() =>
        {
            Assert.That(worldParent.UnplacedLearningElements, Is.Empty);
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(worldParent.UnsavedChanges, Is.False);
        });

        actionWasInvoked = false;

        command.Redo();

        Assert.Multiple(() =>
        {
            Assert.That(worldParent.UnplacedLearningElements, Has.Count.EqualTo(1));
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(worldParent.UnsavedChanges, Is.True);
        });
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var testParameter = new TestParameter();
        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;
        var command = new CreateUnplacedLearningElement(testParameter.WorldParent, testParameter.Name,
            testParameter.Content, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
            testParameter.ElementModel, testParameter.Workload, testParameter.Points, testParameter.PositionX,
            testParameter.PositionY, mappingAction, null!);

        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
        Assert.IsFalse(actionWasInvoked);
    }
}