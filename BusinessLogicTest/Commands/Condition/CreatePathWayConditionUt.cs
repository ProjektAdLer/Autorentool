using BusinessLogic.Commands.Condition;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Commands.Condition;

[TestFixture]
public class CreatePathWayConditionUt
{
    [Test]
    // ANF-ID: [AHO61]
    public void Execute_CreatesPathWayCondition()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var condition = ConditionEnum.And;
        var positionX = 1;
        var positionY = 2;
        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new CreatePathWayCondition(world, condition, positionX, positionY, mappingAction,
            new NullLogger<CreatePathWayCondition>());

        Assert.Multiple(() =>
        {
            Assert.That(world.PathWayConditions, Is.Empty);
            Assert.That(actionWasInvoked, Is.False);
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(world.PathWayConditions, Has.Count.EqualTo(1));
            Assert.That(actionWasInvoked, Is.True);
        });
        var pathWayCondition = world.PathWayConditions.First();
        Assert.Multiple(() =>
        {
            Assert.That(pathWayCondition.Condition, Is.EqualTo(ConditionEnum.And));
            Assert.That(pathWayCondition.PositionX, Is.EqualTo(1));
            Assert.That(pathWayCondition.PositionY, Is.EqualTo(2));
        });
    }

    [Test]
    // ANF-ID: [AHO61]
    public void Execute_CreatesPathWayConditionBetweenGivenObjects()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var previousCondition = new PathWayCondition(ConditionEnum.And, 2, 1);
        var previousSpace =
            new LearningSpace("a", "d", 5, Theme.CampusAschaffenburg, positionX: 200, positionY: 200);
        var previousPathWay = new LearningPathway(previousCondition, previousSpace);
        previousCondition.OutBoundObjects.Add(previousSpace);
        previousSpace.InBoundObjects.Add(previousCondition);
        world.PathWayConditions.Add(previousCondition);
        world.LearningSpaces.Add(previousSpace);
        world.LearningPathways.Add(previousPathWay);
        var condition = ConditionEnum.And;
        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new CreatePathWayCondition(world, condition, previousCondition, previousSpace, mappingAction,
            new NullLogger<CreatePathWayCondition>());

        Assert.Multiple(() =>
        {
            Assert.That(world.PathWayConditions, Has.Count.EqualTo(1));
            Assert.That(world.LearningSpaces, Has.Count.EqualTo(1));
            Assert.That(world.LearningPathways, Has.Count.EqualTo(1));
            Assert.That(actionWasInvoked, Is.False);
        });
        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(world.PathWayConditions, Has.Count.EqualTo(2));
            Assert.That(world.LearningSpaces, Has.Count.EqualTo(1));
            Assert.That(world.LearningPathways, Has.Count.EqualTo(3));
            Assert.That(actionWasInvoked, Is.True);
        });
        actionWasInvoked = false;
        var pathWayCondition = world.PathWayConditions.Last();
        Assert.Multiple(() =>
        {
            Assert.That(pathWayCondition.Condition, Is.EqualTo(ConditionEnum.And));
            Assert.That(pathWayCondition.PositionX, Is.EqualTo(242));
            Assert.That(pathWayCondition.PositionY, Is.EqualTo(140));
        });

        command.Undo();

        Assert.Multiple(() =>
        {
            Assert.That(world.PathWayConditions.Count, Is.EqualTo(1));
            Assert.That(world.LearningSpaces.Count, Is.EqualTo(1));
            Assert.That(world.LearningPathways.Count, Is.EqualTo(1));
            Assert.That(actionWasInvoked, Is.True);
        });
        actionWasInvoked = false;

        command.Redo();

        Assert.Multiple(() =>
        {
            Assert.That(world.PathWayConditions, Has.Count.EqualTo(2));
            Assert.That(world.LearningSpaces.Count, Is.EqualTo(1));
            Assert.That(world.LearningPathways.Count, Is.EqualTo(3));
            Assert.That(actionWasInvoked, Is.True);
        });
        actionWasInvoked = false;
        var pathWayConditionRedo = world.PathWayConditions.Last();
        Assert.Multiple(() =>
        {
            Assert.That(pathWayConditionRedo.Condition, Is.EqualTo(ConditionEnum.And));
            Assert.That(pathWayConditionRedo.PositionX, Is.EqualTo(242));
            Assert.That(pathWayConditionRedo.PositionY, Is.EqualTo(140));
        });
    }

    [Test]
    // ANF-ID: [AHO61]
    public void Execute_PreviousPathWayIsNull_ThrowsException()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var previousCondition = new PathWayCondition(ConditionEnum.And, 2, 1);
        var previousSpace =
            new LearningSpace("a", "d", 5, Theme.CampusAschaffenburg, positionX: 200, positionY: 200);
        previousCondition.OutBoundObjects.Add(previousSpace);
        previousSpace.InBoundObjects.Add(previousCondition);
        world.PathWayConditions.Add(previousCondition);
        world.LearningSpaces.Add(previousSpace);
        var condition = ConditionEnum.And;
        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new CreatePathWayCondition(world, condition, previousCondition, previousSpace, mappingAction,
            new NullLogger<CreatePathWayCondition>());

        var ex = Assert.Throws<ApplicationException>(() => command.Execute());
        Assert.Multiple(() =>
        {
            Assert.That(ex!.Message, Is.EqualTo("Previous pathway is null"));
            Assert.That(actionWasInvoked, Is.False);
        });
    }

    [Test]
    // ANF-ID: [AHO61]
    public void Execute_PreviousInBoundObjectIsNull_ThrowsException()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var previousCondition = new PathWayCondition(ConditionEnum.And, 2, 1);
        var previousSpace =
            new LearningSpace("a", "d", 5, Theme.CampusAschaffenburg, positionX: 200, positionY: 200);
        var previousPathWay = new LearningPathway(previousCondition, previousSpace);
        world.PathWayConditions.Add(previousCondition);
        world.LearningSpaces.Add(previousSpace);
        world.LearningPathways.Add(previousPathWay);
        var condition = ConditionEnum.And;
        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new CreatePathWayCondition(world, condition, previousCondition, previousSpace, mappingAction,
            new NullLogger<CreatePathWayCondition>());

        var ex = Assert.Throws<ApplicationException>(() => command.Execute());
        Assert.Multiple(() =>
        {
            Assert.That(ex!.Message, Is.EqualTo("Previous in bound object is null"));
            Assert.That(actionWasInvoked, Is.False);
        });
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var condition = ConditionEnum.And;
        var positionX = 1;
        var positionY = 2;
        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new CreatePathWayCondition(world, condition, positionX, positionY, mappingAction, null!);

        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.Multiple(() =>
        {
            Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
            Assert.That(actionWasInvoked, Is.False);
        });
    }

    [Test]
    public void UndoRedo_UndoesAndRedoesCreateLearningSpace()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var pathWayCondition = new PathWayCondition(ConditionEnum.And, 1, 2);
        world.PathWayConditions.Add(pathWayCondition);
        var condition = ConditionEnum.Or;
        var positionX = 4;
        var positionY = 5;
        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new CreatePathWayCondition(world, condition, positionX, positionY, mappingAction,
            new NullLogger<CreatePathWayCondition>());

        Assert.Multiple(() =>
        {
            Assert.That(world.PathWayConditions, Has.Count.EqualTo(1));
            Assert.That(actionWasInvoked, Is.False);
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(world.PathWayConditions, Has.Count.EqualTo(2));
            Assert.That(actionWasInvoked, Is.True);
        });
        actionWasInvoked = false;

        command.Undo();

        Assert.Multiple(() =>
        {
            Assert.That(world.PathWayConditions, Has.Count.EqualTo(1));
            Assert.That(actionWasInvoked, Is.True);
        });
        actionWasInvoked = false;

        command.Redo();

        Assert.Multiple(() =>
        {
            Assert.That(world.PathWayConditions, Has.Count.EqualTo(2));
            Assert.That(actionWasInvoked, Is.True);
        });
    }
}