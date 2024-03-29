using BusinessLogic.Commands.Element;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using TestHelpers;

namespace BusinessLogicTest.Commands.Element;

[TestFixture]
public class DragLearningElementUt
{
    [Test]
    public void Execute_DragsLearningElement()
    {
        double oldPositionX = 1;
        double oldPositionY = 2;
        double newPositionX = 3;
        double newPositionY = 4;
        var element =
            EntityProvider.GetLearningElement(unsavedChanges: false, positionX: newPositionX, positionY: newPositionY);
        var actionWasInvoked = false;
        Action<LearningElement> mappingAction = _ => actionWasInvoked = true;

        var command = new DragLearningElement(element, oldPositionX, oldPositionY, newPositionX, newPositionY,
            mappingAction, new NullLogger<DragLearningElement>());

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(element.PositionX, Is.EqualTo(newPositionX));
            Assert.That(element.PositionY, Is.EqualTo(newPositionY));
            Assert.That(element.UnsavedChanges, Is.False);
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(element.PositionX, Is.EqualTo(newPositionX));
            Assert.That(element.PositionY, Is.EqualTo(newPositionY));
            Assert.That(element.UnsavedChanges, Is.True);
        });
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        double oldPositionX = 1;
        double oldPositionY = 2;
        double newPositionX = 3;
        double newPositionY = 4;
        var element = EntityProvider.GetLearningElement(positionX: newPositionX, positionY: newPositionY);
        var actionWasInvoked = false;
        Action<LearningElement> mappingAction = _ => actionWasInvoked = true;

        var command = new DragLearningElement(element, oldPositionX, oldPositionY, newPositionX, newPositionY,
            mappingAction, null!);

        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.Multiple(() =>
        {
            Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
            Assert.That(actionWasInvoked, Is.False);
        });
    }

    [Test]
    public void UndoRedo_UndoesAndRedoesDragLearningElement()
    {
        double oldPositionX = 1;
        double oldPositionY = 2;
        double newPositionX = 3;
        double newPositionY = 4;
        var element =
            EntityProvider.GetLearningElement(unsavedChanges: false, positionX: newPositionX, positionY: newPositionY);
        var actionWasInvoked = false;
        Action<LearningElement> mappingAction = _ => actionWasInvoked = true;

        var command = new DragLearningElement(element, oldPositionX, oldPositionY, newPositionX, newPositionY,
            mappingAction, new NullLogger<DragLearningElement>());

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(element.PositionX, Is.EqualTo(newPositionX));
            Assert.That(element.PositionY, Is.EqualTo(newPositionY));
            Assert.That(element.UnsavedChanges, Is.False);
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(element.PositionX, Is.EqualTo(newPositionX));
            Assert.That(element.PositionY, Is.EqualTo(newPositionY));
            Assert.That(element.UnsavedChanges, Is.True);
        });
        actionWasInvoked = false;

        command.Undo();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(element.PositionX, Is.EqualTo(oldPositionX));
            Assert.That(element.PositionY, Is.EqualTo(oldPositionY));
            Assert.That(element.UnsavedChanges, Is.False);
        });
        actionWasInvoked = false;

        command.Redo();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(element.PositionX, Is.EqualTo(newPositionX));
            Assert.That(element.PositionY, Is.EqualTo(newPositionY));
            Assert.That(element.UnsavedChanges, Is.True);
        });
    }
}