using BusinessLogic.Commands;
using BusinessLogic.Commands.Condition;
using BusinessLogic.Commands.Space;
using BusinessLogic.Commands.Topic;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Commands;

[TestFixture]
public class BatchCommandUt
{
    [Test]
    public void Execute_Undo_Redo_BatchCommand()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var name = "topic1";
        var actionWasInvoked1 = false;
        var actionWasInvoked2 = false;
        var actionWasInvoked3 = false;
        Action<LearningWorld> mappingAction1 = _ => actionWasInvoked1 = true;
        Action<LearningWorld> mappingAction2 = _ => actionWasInvoked2 = true;
        Action<LearningWorld> mappingAction3 = _ => actionWasInvoked3 = true;

        var command1 = new CreateLearningSpace(world, "a", "b", "c", 2, Theme.Campus,
            0, 0, null, mappingAction: mappingAction1, new NullLogger<CreateLearningSpace>());
        var command2 = new CreateTopic(world, name, mappingAction2, new NullLogger<CreateTopic>());
        var command3 = new CreatePathWayCondition(world, ConditionEnum.And, 3, 2, mappingAction3,
            new NullLogger<CreatePathWayCondition>());

        var batchCommand = new BatchCommand(new List<IUndoCommand> { command1, command2, command3 });

        Assert.Multiple(() =>
        {
            Assert.That(world.LearningSpaces, Is.Empty);
            Assert.That(world.Topics, Is.Empty);
            Assert.That(world.PathWayConditions, Is.Empty);
            Assert.That(actionWasInvoked1, Is.False);
            Assert.That(actionWasInvoked2, Is.False);
            Assert.That(actionWasInvoked3, Is.False);
        });

        batchCommand.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(world.LearningSpaces, Has.Count.EqualTo(1));
            Assert.That(world.Topics, Has.Count.EqualTo(1));
            Assert.That(world.PathWayConditions, Has.Count.EqualTo(1));
            Assert.That(actionWasInvoked1, Is.True);
            actionWasInvoked1 = false;
            Assert.That(actionWasInvoked2, Is.True);
            actionWasInvoked2 = false;
            Assert.That(actionWasInvoked3, Is.True);
            actionWasInvoked3 = false;
        });

        batchCommand.Undo();

        Assert.Multiple(() =>
        {
            Assert.That(world.LearningSpaces, Is.Empty);
            Assert.That(world.Topics, Is.Empty);
            Assert.That(world.PathWayConditions, Is.Empty);
            Assert.That(actionWasInvoked1, Is.True);
            actionWasInvoked1 = false;
            Assert.That(actionWasInvoked2, Is.True);
            actionWasInvoked2 = false;
            Assert.That(actionWasInvoked3, Is.True);
            actionWasInvoked3 = false;
        });

        batchCommand.Redo();

        Assert.Multiple(() =>
        {
            Assert.That(world.LearningSpaces, Has.Count.EqualTo(1));
            Assert.That(world.Topics, Has.Count.EqualTo(1));
            Assert.That(world.PathWayConditions, Has.Count.EqualTo(1));
            Assert.That(actionWasInvoked1, Is.True);
            Assert.That(actionWasInvoked2, Is.True);
            Assert.That(actionWasInvoked3, Is.True);
        });
    }
}