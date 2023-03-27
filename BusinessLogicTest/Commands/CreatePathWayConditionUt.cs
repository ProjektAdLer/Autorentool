using BusinessLogic.Commands;
using BusinessLogic.Commands.Condition;
using BusinessLogic.Entities;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Commands;

[TestFixture]

public class CreatePathWayConditionUt
{
    [Test]
    public void Execute_CreatesPathWayCondition()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        ConditionEnum condition = ConditionEnum.And;
        var positionX = 1;
        var positionY = 2;
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;
        
        var command = new CreatePathWayCondition(world, condition, positionX, positionY, mappingAction);
        
        Assert.IsEmpty(world.PathWayConditions);
        Assert.IsFalse(actionWasInvoked);

        command.Execute();
        
        Assert.That(world.PathWayConditions, Has.Count.EqualTo(1));
        Assert.IsTrue(actionWasInvoked);
        var pathWayCondition = world.PathWayConditions.First();
        Assert.Multiple(() =>
        {
            Assert.That(pathWayCondition.Condition, Is.EqualTo(ConditionEnum.And));
            Assert.That(pathWayCondition.PositionX, Is.EqualTo(1));
            Assert.That(pathWayCondition.PositionY, Is.EqualTo(2));
            Assert.That(world.SelectedLearningObjectInPathWay, Is.EqualTo(pathWayCondition));
        });
    }
    
    [Test]
    public void Execute_CreatesPathWayConditionBetweenGivenObjects()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var previousCondition = new PathWayCondition(ConditionEnum.And, 2, 1);
        var previousSpace = new LearningSpace("a", "b", "c", "d", "e", 5, null!,200,200);
        var previousPathWay = new LearningPathway(previousCondition, previousSpace);
        previousCondition.OutBoundObjects.Add(previousSpace);
        previousSpace.InBoundObjects.Add(previousCondition);
        world.PathWayConditions.Add(previousCondition);
        world.LearningSpaces.Add(previousSpace);
        world.LearningPathways.Add(previousPathWay);
        ConditionEnum condition = ConditionEnum.And;
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;
        
        var command = new CreatePathWayCondition(world, condition, previousCondition, previousSpace, mappingAction);
        
        Assert.That(world.PathWayConditions.Count, Is.EqualTo(1));
        Assert.That(world.LearningSpaces.Count, Is.EqualTo(1));
        Assert.That(world.LearningPathways.Count, Is.EqualTo(1));
        Assert.IsFalse(actionWasInvoked);

        command.Execute();
        
        Assert.That(world.PathWayConditions, Has.Count.EqualTo(2));
        Assert.That(world.LearningSpaces.Count, Is.EqualTo(1));
        Assert.That(world.LearningPathways.Count, Is.EqualTo(3));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        var pathWayCondition = world.PathWayConditions.Last();
        Assert.Multiple(() =>
        {
            Assert.That(pathWayCondition.Condition, Is.EqualTo(ConditionEnum.And));
            Assert.That(pathWayCondition.PositionX, Is.EqualTo(242));
            Assert.That(pathWayCondition.PositionY, Is.EqualTo(140));
            Assert.That(world.SelectedLearningObjectInPathWay, Is.EqualTo(pathWayCondition));
        });
        
        command.Undo();
        
        Assert.That(world.PathWayConditions.Count, Is.EqualTo(1));
        Assert.That(world.LearningSpaces.Count, Is.EqualTo(1));
        Assert.That(world.LearningPathways.Count, Is.EqualTo(1));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Redo();
        
        Assert.That(world.PathWayConditions, Has.Count.EqualTo(2));
        Assert.That(world.LearningSpaces.Count, Is.EqualTo(1));
        Assert.That(world.LearningPathways.Count, Is.EqualTo(3));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        var pathWayConditionRedo = world.PathWayConditions.Last();
        Assert.Multiple(() =>
        {
            Assert.That(pathWayConditionRedo.Condition, Is.EqualTo(ConditionEnum.And));
            Assert.That(pathWayConditionRedo.PositionX, Is.EqualTo(242));
            Assert.That(pathWayConditionRedo.PositionY, Is.EqualTo(140));
            Assert.That(world.SelectedLearningObjectInPathWay, Is.EqualTo(pathWayConditionRedo));
        });
    }

    [Test]
    public void Execute_PreviousPathWayIsNull_ThrowsException()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var previousCondition = new PathWayCondition(ConditionEnum.And, 2, 1);
        var previousSpace = new LearningSpace("a", "b", "c", "d", "e", 5, null!, 200, 200);
        previousCondition.OutBoundObjects.Add(previousSpace);
        previousSpace.InBoundObjects.Add(previousCondition);
        world.PathWayConditions.Add(previousCondition);
        world.LearningSpaces.Add(previousSpace);
        ConditionEnum condition = ConditionEnum.And;
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new CreatePathWayCondition(world, condition, previousCondition, previousSpace, mappingAction);

        var ex = Assert.Throws<ApplicationException>(() => command.Execute());
        Assert.That(ex!.Message, Is.EqualTo("Previous pathway is null"));
        
        Assert.IsFalse(actionWasInvoked);
    }

    [Test]
    public void Execute_PreviousInBoundObjectIsNull_ThrowsException()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var previousCondition = new PathWayCondition(ConditionEnum.And, 2, 1);
        var previousSpace = new LearningSpace("a", "b", "c", "d", "e", 5, null!, 200, 200);
        var previousPathWay = new LearningPathway(previousCondition, previousSpace);
        world.PathWayConditions.Add(previousCondition);
        world.LearningSpaces.Add(previousSpace);
        world.LearningPathways.Add(previousPathWay);
        ConditionEnum condition = ConditionEnum.And;
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new CreatePathWayCondition(world, condition, previousCondition, previousSpace, mappingAction);

        var ex = Assert.Throws<ApplicationException>(() => command.Execute());
        Assert.That(ex!.Message, Is.EqualTo("Previous in bound object is null"));
        
        Assert.IsFalse(actionWasInvoked);
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        ConditionEnum condition = ConditionEnum.And;
        var positionX = 1;
        var positionY = 2;
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new CreatePathWayCondition(world, condition, positionX, positionY, mappingAction);
        
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
        
        Assert.IsFalse(actionWasInvoked);
    }
    

    [Test]
    public void UndoRedo_UndoesAndRedoesCreateLearningSpace()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var pathWayCondition = new PathWayCondition(ConditionEnum.And, 1, 2);
        world.PathWayConditions.Add(pathWayCondition);
        world.SelectedLearningObjectInPathWay = pathWayCondition;
        ConditionEnum condition = ConditionEnum.Or;
        var positionX = 4;
        var positionY = 5;
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;
        
        var command = new CreatePathWayCondition(world, condition, positionX, positionY, mappingAction);
        
        Assert.That(world.PathWayConditions, Has.Count.EqualTo(1));
        Assert.That(world.SelectedLearningObjectInPathWay, Is.EqualTo(pathWayCondition));
        Assert.IsFalse(actionWasInvoked);
        
        command.Execute();
        
        Assert.That(world.PathWayConditions, Has.Count.EqualTo(2));
        Assert.That(world.SelectedLearningObjectInPathWay, Is.EqualTo(command.PathWayCondition));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Undo();
        
        Assert.That(world.PathWayConditions, Has.Count.EqualTo(1));
        Assert.That(world.SelectedLearningObjectInPathWay, Is.EqualTo(pathWayCondition));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Redo();
        
        Assert.That(world.PathWayConditions, Has.Count.EqualTo(2));
        Assert.That(world.SelectedLearningObjectInPathWay, Is.EqualTo(command.PathWayCondition));
        Assert.IsTrue(actionWasInvoked);
    }
}