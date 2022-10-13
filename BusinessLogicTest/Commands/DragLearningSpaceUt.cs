using BusinessLogic.Commands;
using BusinessLogic.Entities;
using NUnit.Framework;

namespace BusinessLogicTest.Commands;

[TestFixture]

public class DragLearningSpaceUt
{
    [Test]
    public void Execute_DragsLearningSpace()
    {
        double oldPositionX = 1;
        double oldPositionY = 2;
        double newPositionX = 3;
        double newPositionY = 4;
        var space = new LearningSpace("a", "b", "c", "d", "e", 5, null, newPositionX, newPositionY);
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new DragLearningSpace(space, oldPositionX, oldPositionY, newPositionX, newPositionY, mappingAction);
        
        Assert.Multiple(() =>
        {
            Assert.IsFalse(actionWasInvoked);
            Assert.That(space.PositionX, Is.EqualTo(newPositionX));
            Assert.That(space.PositionY, Is.EqualTo(newPositionY));
        });
        
        command.Execute();
        
        Assert.Multiple(() =>
        {
            Assert.IsTrue(actionWasInvoked);
            Assert.That(space.PositionX, Is.EqualTo(newPositionX));
            Assert.That(space.PositionY, Is.EqualTo(newPositionY));
        });
    }
    
    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        double oldPositionX = 1;
        double oldPositionY = 2;
        double newPositionX = 3;
        double newPositionY = 4;
        var space = new LearningSpace("a", "b", "c", "d", "e", 5, null, newPositionX, newPositionY);
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new DragLearningSpace(space, oldPositionX, oldPositionY, newPositionX, newPositionY, mappingAction);
        
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
        var space = new LearningSpace("a", "b", "c", "d", "e", 5, null, newPositionX, newPositionY);
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new DragLearningSpace(space, oldPositionX, oldPositionY, newPositionX, newPositionY, mappingAction);
        
        Assert.Multiple(() =>
        {
            Assert.IsFalse(actionWasInvoked);
            Assert.That(space.PositionX, Is.EqualTo(newPositionX));
            Assert.That(space.PositionY, Is.EqualTo(newPositionY));
        });
        
        command.Execute();
        
        Assert.Multiple(() =>
        {
            Assert.IsTrue(actionWasInvoked);
            Assert.That(space.PositionX, Is.EqualTo(newPositionX));
            Assert.That(space.PositionY, Is.EqualTo(newPositionY));
        });
        actionWasInvoked = false;
        
        command.Undo();
        
        Assert.Multiple(() =>
        {
            Assert.IsTrue(actionWasInvoked);
            Assert.That(space.PositionX, Is.EqualTo(oldPositionX));
            Assert.That(space.PositionY, Is.EqualTo(oldPositionY));
        });
        actionWasInvoked = false;
        
        command.Redo();
        
        Assert.Multiple(() =>
        {
            Assert.IsTrue(actionWasInvoked);
            Assert.That(space.PositionX, Is.EqualTo(newPositionX));
            Assert.That(space.PositionY, Is.EqualTo(newPositionY));
        });
    }
}