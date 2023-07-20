using BusinessLogic.Commands.Condition;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Commands.Condition;

[TestFixture]
public class DeletePathWayConditionUt
{
    [Test]
    public void Execute_DeletesLearningSpace()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var pathWayCondition = new PathWayCondition(ConditionEnum.And, 3, 4);
        var pathWayCondition1 = new PathWayCondition(ConditionEnum.Or, 6, 5);
        var pathWayCondition2 = new PathWayCondition(ConditionEnum.And, 9, 5);
        world.PathWayConditions.Add(pathWayCondition);
        world.PathWayConditions.Add(pathWayCondition1);
        world.PathWayConditions.Add(pathWayCondition2);
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;
        
        world.LearningPathways.Add(new LearningPathway(pathWayCondition, pathWayCondition1));
        pathWayCondition.OutBoundObjects.Add(pathWayCondition1);
        pathWayCondition1.InBoundObjects.Add(pathWayCondition);
        world.LearningPathways.Add(new LearningPathway(pathWayCondition1, pathWayCondition2));
        pathWayCondition1.OutBoundObjects.Add(pathWayCondition2);
        pathWayCondition2.InBoundObjects.Add(pathWayCondition1);
        var logger = Substitute.For<ILogger<ConditionCommandFactory>>();

        var command = new DeletePathWayCondition(world, pathWayCondition1, mappingAction, logger);
        
        Assert.That(world.PathWayConditions, Does.Contain(pathWayCondition1));
        Assert.IsFalse(actionWasInvoked);
        
        command.Execute();
        
        Assert.That(world.PathWayConditions, Has.Count.EqualTo(2));
        Assert.IsTrue(actionWasInvoked);
        Assert.That(world.LearningPathways, Has.Count.EqualTo(0));
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var pathWayCondition = new PathWayCondition(ConditionEnum.And, 3, 5);
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new DeletePathWayCondition(world,pathWayCondition, mappingAction, null!);
        
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
        
        Assert.IsFalse(actionWasInvoked);
    }
    
    [Test]
    public void UndoRedo_UndoesAndRedoesDeleteLearningSpace()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var pathWayCondition = new PathWayCondition(ConditionEnum.And,3, 5);
        var pathWayCondition1 = new PathWayCondition(ConditionEnum.Or, 2 ,7);
        world.PathWayConditions.Add(pathWayCondition);
        world.PathWayConditions.Add(pathWayCondition1);
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;
        var logger = Substitute.For<ILogger<ConditionCommandFactory>>();
        
        var command = new DeletePathWayCondition(world, pathWayCondition, mappingAction, logger);
        
        Assert.That(world.PathWayConditions, Has.Count.EqualTo(2));
        Assert.IsFalse(actionWasInvoked);
        
        command.Execute();
        
        Assert.That(world.PathWayConditions, Has.Count.EqualTo(1));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Undo();
        
        Assert.That(world.PathWayConditions, Has.Count.EqualTo(2));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Redo();
        
        Assert.That(world.PathWayConditions, Has.Count.EqualTo(1));
        Assert.IsTrue(actionWasInvoked);
    }
}