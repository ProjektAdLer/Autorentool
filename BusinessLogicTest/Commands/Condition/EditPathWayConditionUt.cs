using BusinessLogic.Commands.Condition;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Commands.Condition;

[TestFixture]
public class EditPathWayConditionUt
{
    [Test]
    // ANF-ID: [AHO62]
    public void Execute_EditsLearningSpace()
    {
        var pathWayCondition = new PathWayCondition(ConditionEnum.And, 3, 5);
        var condition = ConditionEnum.Or;
        var actionWasInvoked = false;
        Action<PathWayCondition> mappingAction = _ => actionWasInvoked = true;

        var command = new EditPathWayCondition(pathWayCondition, condition, mappingAction,
            new NullLogger<EditPathWayCondition>());

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(pathWayCondition.Condition, Is.EqualTo(ConditionEnum.And));
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(pathWayCondition.Condition, Is.EqualTo(condition));
        });
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var pathWayCondition = new PathWayCondition(ConditionEnum.And, 4, 5);
        var condition = ConditionEnum.Or;
        var actionWasInvoked = false;
        Action<PathWayCondition> mappingAction = _ => actionWasInvoked = true;

        var command = new EditPathWayCondition(pathWayCondition, condition, mappingAction, null!);

        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
        Assert.That(actionWasInvoked, Is.False);
    }

    [Test]
    public void UndoRedo_UndoesAndRedoesEditLearningSpace()
    {
        var pathWayCondition = new PathWayCondition(ConditionEnum.And, 2, 5);
        var condition = ConditionEnum.Or;
        var actionWasInvoked = false;
        Action<PathWayCondition> mappingAction = _ => actionWasInvoked = true;

        var command = new EditPathWayCondition(pathWayCondition, condition, mappingAction,
            new NullLogger<EditPathWayCondition>());

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(pathWayCondition.Condition, Is.EqualTo(ConditionEnum.And));
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(pathWayCondition.Condition, Is.EqualTo(condition));
        });
        actionWasInvoked = false;

        command.Undo();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(pathWayCondition.Condition, Is.EqualTo(ConditionEnum.And));
        });
        actionWasInvoked = false;

        command.Redo();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(pathWayCondition.Condition, Is.EqualTo(condition));
        });
    }
}