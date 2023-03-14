using BusinessLogic.Commands;
using BusinessLogic.Commands.Element;
using BusinessLogic.Entities;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Commands;

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
        var element = new LearningElement("a", "b", null!, "d", "e", 
            "f", LearningElementDifficultyEnum.Easy, null, 5, 5, newPositionX, newPositionY);
        bool actionWasInvoked = false;
        Action<LearningElement> mappingAction = _ => actionWasInvoked = true;

        var command = new DragLearningElement(element, oldPositionX, oldPositionY, newPositionX, newPositionY, mappingAction);
        
        Assert.Multiple(() =>
        {
            Assert.IsFalse(actionWasInvoked);
            Assert.That(element.PositionX, Is.EqualTo(newPositionX));
            Assert.That(element.PositionY, Is.EqualTo(newPositionY));
        });
        
        command.Execute();
        
        Assert.Multiple(() =>
        {
            Assert.IsTrue(actionWasInvoked);
            Assert.That(element.PositionX, Is.EqualTo(newPositionX));
            Assert.That(element.PositionY, Is.EqualTo(newPositionY));
        });
    }
    
    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        double oldPositionX = 1;
        double oldPositionY = 2;
        double newPositionX = 3;
        double newPositionY = 4;
        var element = new LearningElement("a", "b", null!, "d", "e", 
            "f", LearningElementDifficultyEnum.Easy, null, 5, 5, newPositionX, newPositionY);
        bool actionWasInvoked = false;
        Action<LearningElement> mappingAction = _ => actionWasInvoked = true;

        var command = new DragLearningElement(element, oldPositionX, oldPositionY, newPositionX, newPositionY, mappingAction);
        
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
        Assert.IsFalse(actionWasInvoked);
    }
    
    [Test]
    public void UndoRedo_UndoesAndRedoesDragLearningElement()
    {
        double oldPositionX = 1;
        double oldPositionY = 2;
        double newPositionX = 3;
        double newPositionY = 4;
        var element = new LearningElement("a", "b", null!, "d", "e", 
            "f", LearningElementDifficultyEnum.Easy, null, 5, 5, newPositionX, newPositionY);        
        bool actionWasInvoked = false;
        Action<LearningElement> mappingAction = _ => actionWasInvoked = true;

        var command = new DragLearningElement(element, oldPositionX, oldPositionY, newPositionX, newPositionY, mappingAction);
        
        Assert.Multiple(() =>
        {
            Assert.IsFalse(actionWasInvoked);
            Assert.That(element.PositionX, Is.EqualTo(newPositionX));
            Assert.That(element.PositionY, Is.EqualTo(newPositionY));
        });
        
        command.Execute();
        
        Assert.Multiple(() =>
        {
            Assert.IsTrue(actionWasInvoked);
            Assert.That(element.PositionX, Is.EqualTo(newPositionX));
            Assert.That(element.PositionY, Is.EqualTo(newPositionY));
        });
        actionWasInvoked = false;
        
        command.Undo();
        
        Assert.Multiple(() =>
        {
            Assert.IsTrue(actionWasInvoked);
            Assert.That(element.PositionX, Is.EqualTo(oldPositionX));
            Assert.That(element.PositionY, Is.EqualTo(oldPositionY));
        });
        actionWasInvoked = false;
        
        command.Redo();
        
        Assert.Multiple(() =>
        {
            Assert.IsTrue(actionWasInvoked);
            Assert.That(element.PositionX, Is.EqualTo(newPositionX));
            Assert.That(element.PositionY, Is.EqualTo(newPositionY));
        });
    }
}