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
        var element = new LearningElement("a", null!, "e",
            "f", LearningElementDifficultyEnum.Easy, null, workload: 5, points: 5, positionX: newPositionX,
            positionY: newPositionY)
        {
            UnsavedChanges = false
        };
        var actionWasInvoked = false;
        Action<LearningElement> mappingAction = _ => actionWasInvoked = true;

        var command = new DragLearningElement(element, oldPositionX, oldPositionY, newPositionX, newPositionY, mappingAction);
        
        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(element.PositionX, Is.EqualTo(newPositionX));
            Assert.That(element.PositionY, Is.EqualTo(newPositionY));
            Assert.That(element.UnsavedChanges, Is.False);
        });
        
        command.Execute();
        
        Assert.Multiple(() =>
        {
            Assert.IsTrue(actionWasInvoked);
            Assert.That(element.PositionX, Is.EqualTo(newPositionX));
            Assert.That(element.PositionY, Is.EqualTo(newPositionY));
            Assert.That(element.UnsavedChanges, Is.True);
        });
    }
    
    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        double oldPositionX = 1;
        double oldPositionY = 2;
        double newPositionX = 3;
        double newPositionY = 4;
        var element = new LearningElement("a", null!, "e", 
            "f", LearningElementDifficultyEnum.Easy, null, workload: 5, points: 5, positionX: newPositionX, positionY: newPositionY);
        var actionWasInvoked = false;
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
        var element = new LearningElement("a", null!, "e",
            "f", LearningElementDifficultyEnum.Easy, null, workload: 5, points: 5, positionX: newPositionX,
            positionY: newPositionY)
        {
            UnsavedChanges = false
        };
        var actionWasInvoked = false;
        Action<LearningElement> mappingAction = _ => actionWasInvoked = true;

        var command = new DragLearningElement(element, oldPositionX, oldPositionY, newPositionX, newPositionY, mappingAction);
        
        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(element.PositionX, Is.EqualTo(newPositionX));
            Assert.That(element.PositionY, Is.EqualTo(newPositionY));
            Assert.That(element.UnsavedChanges, Is.False);
        });
        
        command.Execute();
        
        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(element.PositionX, Is.EqualTo(newPositionX));
            Assert.That(element.PositionY, Is.EqualTo(newPositionY));
            Assert.That(element.UnsavedChanges, Is.True);
        });
        actionWasInvoked = false;
        
        command.Undo();
        
        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(element.PositionX, Is.EqualTo(oldPositionX));
            Assert.That(element.PositionY, Is.EqualTo(oldPositionY));
            Assert.That(element.UnsavedChanges, Is.False);
        });
        actionWasInvoked = false;
        
        command.Redo();
        
        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(element.PositionX, Is.EqualTo(newPositionX));
            Assert.That(element.PositionY, Is.EqualTo(newPositionY));
            Assert.That(element.UnsavedChanges, Is.True);
        });
    }
}