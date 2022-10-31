using BusinessLogic.Commands;
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
            Assert.That(world.SelectedLearningObject, Is.EqualTo(pathWayCondition));
        });
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
        world.SelectedLearningObject = pathWayCondition;
        ConditionEnum condition = ConditionEnum.Or;
        var positionX = 4;
        var positionY = 5;
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;
        
        var command = new CreatePathWayCondition(world, condition, positionX, positionY, mappingAction);
        
        Assert.That(world.PathWayConditions, Has.Count.EqualTo(1));
        Assert.That(world.SelectedLearningObject, Is.EqualTo(pathWayCondition));
        Assert.IsFalse(actionWasInvoked);
        
        command.Execute();
        
        Assert.That(world.PathWayConditions, Has.Count.EqualTo(2));
        Assert.That(world.SelectedLearningObject, Is.EqualTo(command.PathWayCondition));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Undo();
        
        Assert.That(world.PathWayConditions, Has.Count.EqualTo(1));
        Assert.That(world.SelectedLearningObject, Is.EqualTo(pathWayCondition));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Redo();
        
        Assert.That(world.PathWayConditions, Has.Count.EqualTo(2));
        Assert.That(world.SelectedLearningObject, Is.EqualTo(command.PathWayCondition));
        Assert.IsTrue(actionWasInvoked);
    }
}