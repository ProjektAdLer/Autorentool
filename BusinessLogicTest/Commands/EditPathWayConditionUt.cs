using BusinessLogic.Commands;
using BusinessLogic.Commands.Condition;
using BusinessLogic.Entities;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Commands;

[TestFixture]
public class EditPathWayConditionUt
{
    [Test]
    public void Execute_EditsLearningSpace()
    {
        var pathWayCondition = new PathWayCondition(ConditionEnum.And,3, 5);
        var condition = ConditionEnum.Or;
        bool actionWasInvoked = false;
        Action<PathWayCondition> mappingAction = _ => actionWasInvoked = true;

        var command = new EditPathWayCondition(pathWayCondition, condition, mappingAction);
        
        Assert.Multiple(() =>
        {
            Assert.IsFalse(actionWasInvoked);
            Assert.That(pathWayCondition.Condition, Is.EqualTo(ConditionEnum.And));
        });
        
        command.Execute();
        
        Assert.Multiple(() =>
        {
            Assert.IsTrue(actionWasInvoked);
            Assert.That(pathWayCondition.Condition, Is.EqualTo(condition));
        });
    }
    
    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var pathWayCondition = new PathWayCondition(ConditionEnum.And,4, 5);
        var condition = ConditionEnum.Or;
        bool actionWasInvoked = false;
        Action<PathWayCondition> mappingAction = _ => actionWasInvoked = true;

        var command = new EditPathWayCondition(pathWayCondition, condition, mappingAction);
        
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
        Assert.IsFalse(actionWasInvoked);
    }
    
    [Test]
    public void UndoRedo_UndoesAndRedoesEditLearningSpace()
    {
        var pathWayCondition = new PathWayCondition(ConditionEnum.And, 2, 5);
        var condition = ConditionEnum.Or;
        bool actionWasInvoked = false;
        Action<PathWayCondition> mappingAction = _ => actionWasInvoked = true;
        
        var command = new EditPathWayCondition(pathWayCondition, condition, mappingAction);
        
        Assert.Multiple(() =>
        {
            Assert.IsFalse(actionWasInvoked);
            Assert.That(pathWayCondition.Condition, Is.EqualTo(ConditionEnum.And));
        });
        
        command.Execute();
        
        Assert.Multiple(() =>
        {
            Assert.IsTrue(actionWasInvoked);
            Assert.That(pathWayCondition.Condition, Is.EqualTo(condition));
        });
        actionWasInvoked = false;
        
        command.Undo();
        
        Assert.Multiple(() =>
        {
            Assert.IsTrue(actionWasInvoked);
            Assert.That(pathWayCondition.Condition, Is.EqualTo(ConditionEnum.And));
        });
        actionWasInvoked = false;
        
        command.Redo();
        
        Assert.Multiple(() =>
        {
            Assert.IsTrue(actionWasInvoked);
            Assert.That(pathWayCondition.Condition, Is.EqualTo(condition));
        });
    }
}