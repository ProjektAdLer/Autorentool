using BusinessLogic.Commands;
using BusinessLogic.Commands.Condition;
using BusinessLogic.Entities;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Commands;

[TestFixture]
public class DeletePathWayConditionUt
{
    [Test]
    public void Execute_DeletesSpace()
    {
        var world = new World("a", "b", "c", "d", "e", "f");
        var pathWayCondition = new PathWayCondition(ConditionEnum.And, 3, 4);
        var pathWayCondition1 = new PathWayCondition(ConditionEnum.Or, 6, 5);
        var pathWayCondition2 = new PathWayCondition(ConditionEnum.And, 9, 5);
        world.PathWayConditions.Add(pathWayCondition);
        world.PathWayConditions.Add(pathWayCondition1);
        world.PathWayConditions.Add(pathWayCondition2);
        world.SelectedObject = pathWayCondition1;
        bool actionWasInvoked = false;
        Action<World> mappingAction = _ => actionWasInvoked = true;
        
        world.Pathways.Add(new Pathway(pathWayCondition, pathWayCondition1));
        pathWayCondition.OutBoundObjects.Add(pathWayCondition1);
        pathWayCondition1.InBoundObjects.Add(pathWayCondition);
        world.Pathways.Add(new Pathway(pathWayCondition1, pathWayCondition2));
        pathWayCondition1.OutBoundObjects.Add(pathWayCondition2);
        pathWayCondition2.InBoundObjects.Add(pathWayCondition1);

        var command = new DeletePathWayCondition(world, pathWayCondition1, mappingAction);
        
        Assert.That(world.PathWayConditions, Does.Contain(pathWayCondition1));
        Assert.IsFalse(actionWasInvoked);
        Assert.That(world.SelectedObject, Is.EqualTo(pathWayCondition1));
        
        command.Execute();
        
        Assert.That(world.PathWayConditions, Has.Count.EqualTo(2));
        Assert.IsTrue(actionWasInvoked);
        Assert.That(world.SelectedObject, Is.EqualTo(pathWayCondition2));
        Assert.That(world.Pathways, Has.Count.EqualTo(0));
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var world = new World("a", "b", "c", "d", "e", "f");
        var pathWayCondition = new PathWayCondition(ConditionEnum.And, 3, 5);
        bool actionWasInvoked = false;
        Action<World> mappingAction = _ => actionWasInvoked = true;

        var command = new DeletePathWayCondition(world,pathWayCondition, mappingAction);
        
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
        
        Assert.IsFalse(actionWasInvoked);
    }
    
    [Test]
    public void UndoRedo_UndoesAndRedoesDeleteSpace()
    {
        var world = new World("a", "b", "c", "d", "e", "f");
        var pathWayCondition = new PathWayCondition(ConditionEnum.And,3, 5);
        var pathWayCondition1 = new PathWayCondition(ConditionEnum.Or, 2 ,7);
        world.PathWayConditions.Add(pathWayCondition);
        world.PathWayConditions.Add(pathWayCondition1);
        world.SelectedObject = pathWayCondition;
        bool actionWasInvoked = false;
        Action<World> mappingAction = _ => actionWasInvoked = true;
        
        var command = new DeletePathWayCondition(world, pathWayCondition, mappingAction);
        
        Assert.That(world.PathWayConditions, Has.Count.EqualTo(2));
        Assert.That(world.SelectedObject, Is.EqualTo(pathWayCondition));
        Assert.IsFalse(actionWasInvoked);
        
        command.Execute();
        
        Assert.That(world.PathWayConditions, Has.Count.EqualTo(1));
        Assert.That(world.SelectedObject, Is.EqualTo(pathWayCondition1));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Undo();
        
        Assert.That(world.PathWayConditions, Has.Count.EqualTo(2));
        Assert.That(world.SelectedObject, Is.EqualTo(pathWayCondition));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Redo();
        
        Assert.That(world.PathWayConditions, Has.Count.EqualTo(1));
        Assert.That(world.SelectedObject, Is.EqualTo(pathWayCondition1));
        Assert.IsTrue(actionWasInvoked);
    }
}