using BusinessLogic.Commands.Pathway;
using BusinessLogic.Entities;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Commands.Pathway;

[TestFixture]

public class DragObjectInPathWayUt
{
    [Test]
    public void Execute_DragsLearningSpace()
    {
        double oldPositionX = 1;
        double oldPositionY = 2;
        double newPositionX = 3;
        double newPositionY = 4;
        var space = new LearningSpace("a", "d", "e", 5, Theme.Campus, null, positionX: newPositionX, positionY: newPositionY)
        {
            UnsavedChanges = false
        };
        bool actionWasInvoked = false;
        Action<IObjectInPathWay> mappingAction = _ => actionWasInvoked = true;

        var command = new DragObjectInPathWay(space, oldPositionX, oldPositionY, newPositionX, newPositionY, mappingAction);
        
        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(space.PositionX, Is.EqualTo(newPositionX));
            Assert.That(space.PositionY, Is.EqualTo(newPositionY));
            Assert.That(space.UnsavedChanges, Is.False);
        });
        
        command.Execute();
        
        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(space.PositionX, Is.EqualTo(newPositionX));
            Assert.That(space.PositionY, Is.EqualTo(newPositionY));
            Assert.That(space.UnsavedChanges, Is.True);
        });
    }
    
    [Test]
    public void Execute_DragsPathWayCondition()
    {
        double oldPositionX = 1;
        double oldPositionY = 2;
        double newPositionX = 3;
        double newPositionY = 4;
        var pathWayCondition = new PathWayCondition(ConditionEnum.And, newPositionX, newPositionY)
        {
            UnsavedChanges = false
        };
        var actionWasInvoked = false;
        Action<IObjectInPathWay> mappingAction = _ => actionWasInvoked = true;

        var command = new DragObjectInPathWay(pathWayCondition, oldPositionX, oldPositionY, newPositionX, newPositionY, mappingAction);
        
        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(pathWayCondition.PositionX, Is.EqualTo(newPositionX));
            Assert.That(pathWayCondition.PositionY, Is.EqualTo(newPositionY));
            Assert.That(pathWayCondition.UnsavedChanges, Is.False);
        });
        
        command.Execute();
        
        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(pathWayCondition.PositionX, Is.EqualTo(newPositionX));
            Assert.That(pathWayCondition.PositionY, Is.EqualTo(newPositionY));
            Assert.That(pathWayCondition.UnsavedChanges, Is.True);
        });
    }
    
    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        double oldPositionX = 1;
        double oldPositionY = 2;
        double newPositionX = 3;
        double newPositionY = 4;
        var space = new LearningSpace("a", "d", "e", 5, Theme.Campus, null, positionX: newPositionX, positionY: newPositionY);
        bool actionWasInvoked = false;
        Action<IObjectInPathWay> mappingAction = _ => actionWasInvoked = true;

        var command = new DragObjectInPathWay(space, oldPositionX, oldPositionY, newPositionX, newPositionY, mappingAction);
        
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
        Assert.IsFalse(actionWasInvoked);
    }
    
    [Test]
    public void UndoRedo_UndoesAndRedoesDragLearningSpace()
    {
        double oldPositionX = 1;
        double oldPositionY = 2;
        double newPositionX = 3;
        double newPositionY = 4;
        var space = new LearningSpace("a", "d", "e", 5, Theme.Campus, null, positionX: newPositionX, positionY: newPositionY)
        {
            UnsavedChanges = false
        };
        var actionWasInvoked = false;
        Action<IObjectInPathWay> mappingAction = _ => actionWasInvoked = true;

        var command = new DragObjectInPathWay(space, oldPositionX, oldPositionY, newPositionX, newPositionY, mappingAction);
        
        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(space.PositionX, Is.EqualTo(newPositionX));
            Assert.That(space.PositionY, Is.EqualTo(newPositionY));
            Assert.That(space.UnsavedChanges, Is.False);
        });
        
        command.Execute();
        
        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(space.PositionX, Is.EqualTo(newPositionX));
            Assert.That(space.PositionY, Is.EqualTo(newPositionY));
            Assert.That(space.UnsavedChanges, Is.True);
        });
        actionWasInvoked = false;
        
        command.Undo();
        
        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(space.PositionX, Is.EqualTo(oldPositionX));
            Assert.That(space.PositionY, Is.EqualTo(oldPositionY));
            Assert.That(space.UnsavedChanges, Is.False);
        });
        actionWasInvoked = false;
        
        command.Redo();
        
        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(space.PositionX, Is.EqualTo(newPositionX));
            Assert.That(space.PositionY, Is.EqualTo(newPositionY));
            Assert.That(space.UnsavedChanges, Is.True);
        });
    }
}