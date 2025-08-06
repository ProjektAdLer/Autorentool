using BusinessLogic.Commands.Condition;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using Shared;
using TestHelpers;

namespace BusinessLogicTest.Commands.Condition;

[TestFixture]
public class DeletePathWayConditionUt
{
    [Test]
    // ANF-ID: [AHO63]
    public void Execute_DeletesLearningSpace()
    {
        var world = EntityProvider.GetLearningWorld();
        var pathWayCondition = new PathWayCondition(ConditionEnum.And, 3, 4);
        var pathWayCondition1 = new PathWayCondition(ConditionEnum.Or, 6, 5);
        var pathWayCondition2 = new PathWayCondition(ConditionEnum.And, 9, 5);
        world.PathWayConditions.Add(pathWayCondition);
        world.PathWayConditions.Add(pathWayCondition1);
        world.PathWayConditions.Add(pathWayCondition2);
        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        world.LearningPathways.Add(new LearningPathway(pathWayCondition, pathWayCondition1));
        pathWayCondition.OutBoundObjects.Add(pathWayCondition1);
        pathWayCondition1.InBoundObjects.Add(pathWayCondition);
        world.LearningPathways.Add(new LearningPathway(pathWayCondition1, pathWayCondition2));
        pathWayCondition1.OutBoundObjects.Add(pathWayCondition2);
        pathWayCondition2.InBoundObjects.Add(pathWayCondition1);

        var command = new DeletePathWayCondition(world, pathWayCondition1, mappingAction,
            new NullLogger<DeletePathWayCondition>());

        Assert.Multiple(() =>
        {
            Assert.That(world.PathWayConditions, Does.Contain(pathWayCondition1));
            Assert.That(actionWasInvoked, Is.False);
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(world.PathWayConditions, Has.Count.EqualTo(2));
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(world.LearningPathways, Has.Count.EqualTo(0));
        });
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var world = EntityProvider.GetLearningWorld();
        var pathWayCondition = new PathWayCondition(ConditionEnum.And, 3, 5);
        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new DeletePathWayCondition(world, pathWayCondition, mappingAction, null!);

        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.Multiple(() =>
        {
            Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
            Assert.That(actionWasInvoked, Is.False);
        });
    }

    [Test]
    public void UndoRedo_UndoesAndRedoesDeleteLearningSpace()
    {
        var world = EntityProvider.GetLearningWorld();
        var pathWayCondition = new PathWayCondition(ConditionEnum.And, 3, 5);
        var pathWayCondition1 = new PathWayCondition(ConditionEnum.Or, 2, 7);
        world.PathWayConditions.Add(pathWayCondition);
        world.PathWayConditions.Add(pathWayCondition1);
        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new DeletePathWayCondition(world, pathWayCondition, mappingAction,
            new NullLogger<DeletePathWayCondition>());

        Assert.Multiple(() =>
        {
            Assert.That(world.PathWayConditions, Has.Count.EqualTo(2));
            Assert.That(actionWasInvoked, Is.False);
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(world.PathWayConditions, Has.Count.EqualTo(1));
            Assert.That(actionWasInvoked, Is.True);
        });
        actionWasInvoked = false;

        command.Undo();

        Assert.Multiple(() =>
        {
            Assert.That(world.PathWayConditions, Has.Count.EqualTo(2));
            Assert.That(actionWasInvoked, Is.True);
        });
        actionWasInvoked = false;

        command.Redo();

        Assert.Multiple(() =>
        {
            Assert.That(world.PathWayConditions, Has.Count.EqualTo(1));
            Assert.That(actionWasInvoked, Is.True);
        });
    }
}