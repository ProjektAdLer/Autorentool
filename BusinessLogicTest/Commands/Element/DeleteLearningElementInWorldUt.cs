using BusinessLogic.Commands.Element;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using TestHelpers;

namespace BusinessLogicTest.Commands.Element;

[TestFixture]
public class DeleteLearningElementInWorldUt
{
    [Test]
    // ANF-ID: [AWA0016, AWA0011]
    public void Execute_Undo_Redo_DeletesLearningElement()
    {
        var world = EntityProvider.GetLearningWorld(unsavedChanges: false);
        var element = EntityProvider.GetLearningElement(unsavedChanges: false);
        world.UnplacedLearningElements = new List<ILearningElement> { element };
        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteLearningElementInWorld(element, world, mappingAction,
            new NullLogger<DeleteLearningElementInWorld>());

        Assert.Multiple(() =>
        {
            Assert.That(world.UnplacedLearningElements, Does.Contain(element));
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(world.UnsavedChanges, Is.False);
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(world.UnplacedLearningElements, Is.Empty);

            Assert.That(actionWasInvoked, Is.True);
            Assert.That(world.UnsavedChanges, Is.True);
        });

        actionWasInvoked = false;

        command.Undo();

        Assert.Multiple(() =>
        {
            Assert.That(world.UnplacedLearningElements, Does.Contain(element));

            Assert.That(actionWasInvoked, Is.True);
            Assert.That(world.UnsavedChanges, Is.False);
        });

        actionWasInvoked = false;

        command.Redo();

        Assert.Multiple(() =>
        {
            Assert.That(world.UnplacedLearningElements, Is.Empty);

            Assert.That(actionWasInvoked, Is.True);
            Assert.That(world.UnsavedChanges, Is.True);
        });
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var world = EntityProvider.GetLearningWorld();
        var element = EntityProvider.GetLearningElement();
        world.UnplacedLearningElements = new List<ILearningElement> { element };
        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteLearningElementInWorld(element, world, mappingAction, null!);

        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());

        Assert.Multiple(() =>
        {
            Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
            Assert.That(actionWasInvoked, Is.False);
        });
    }
}