using BusinessLogic.Commands;
using BusinessLogic.Commands.Condition;
using BusinessLogic.Commands.Space;
using BusinessLogic.Commands.Topic;
using BusinessLogic.Entities;
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
        bool actionWasInvoked1 = false;
        bool actionWasInvoked2 = false;
        bool actionWasInvoked3 = false;
        Action<LearningWorld> mappingAction1 = _ => actionWasInvoked1 = true;
        Action<LearningWorld> mappingAction2 = _ => actionWasInvoked2 = true;
        Action<LearningWorld> mappingAction3 = _ => actionWasInvoked3 = true;

        var command1 = new CreateLearningSpace(world, "a", "b", "c", 2, Theme.Campus,
            0, 0, null, mappingAction: mappingAction1);
        var command2 = new CreateTopic(world, name, mappingAction2);
        var command3 = new CreatePathWayCondition(world, ConditionEnum.And, 3, 2, mappingAction3);
        
        var batchCommand = new BatchCommand(new List<IUndoCommand>(){command1, command2, command3});

        Assert.Multiple(() =>
        {
            Assert.IsEmpty(world.LearningSpaces);
            Assert.IsEmpty(world.Topics);
            Assert.IsEmpty(world.PathWayConditions);
            Assert.IsFalse(actionWasInvoked1);
            Assert.IsFalse(actionWasInvoked2);
            Assert.IsFalse(actionWasInvoked3);
        });
        
        batchCommand.Execute();
        
        Assert.Multiple(() =>
        {
            Assert.That(world.LearningSpaces, Has.Count.EqualTo(1));
            Assert.That(world.Topics, Has.Count.EqualTo(1));
            Assert.That(world.PathWayConditions, Has.Count.EqualTo(1));
            Assert.IsTrue(actionWasInvoked1); actionWasInvoked1 = false;
            Assert.IsTrue(actionWasInvoked2); actionWasInvoked2 = false;
            Assert.IsTrue(actionWasInvoked3); actionWasInvoked3 = false;
        });
        
        batchCommand.Undo();
        
        Assert.Multiple(() =>
        {
            Assert.IsEmpty(world.LearningSpaces);
            Assert.IsEmpty(world.Topics);
            Assert.IsEmpty(world.PathWayConditions);
            Assert.IsTrue(actionWasInvoked1); actionWasInvoked1 = false;
            Assert.IsTrue(actionWasInvoked2); actionWasInvoked2 = false;
            Assert.IsTrue(actionWasInvoked3); actionWasInvoked3 = false;
        });
        
        batchCommand.Redo();
        
        Assert.Multiple(() =>
        {
            Assert.That(world.LearningSpaces, Has.Count.EqualTo(1));
            Assert.That(world.Topics, Has.Count.EqualTo(1));
            Assert.That(world.PathWayConditions, Has.Count.EqualTo(1));
            Assert.IsTrue(actionWasInvoked1);
            Assert.IsTrue(actionWasInvoked2);
            Assert.IsTrue(actionWasInvoked3);
        });
    }
}