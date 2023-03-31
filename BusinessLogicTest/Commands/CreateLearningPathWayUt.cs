using BusinessLogic.Commands;
using BusinessLogic.Commands.Pathway;
using BusinessLogic.Entities;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Commands;

[TestFixture]

public class CreateLearningPathWayUt
{
    [Test]
    public void Execute_CreateLearningPathWay()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var space1 = new LearningSpace("z", "w", "v", 5);
        var pathWayCondition = new PathWayCondition(ConditionEnum.And,3, 3);
        var space3 = new LearningSpace("l", "o", "p", 3);
        var space4 = new LearningSpace("l", "o", "p", 3);
        world.LearningSpaces.Add(space1);
        world.PathWayConditions.Add(pathWayCondition);
        world.LearningSpaces.Add(space3);
        world.LearningSpaces.Add(space4);
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;
        
        var pathway1 = new LearningPathway(space1, pathWayCondition);
        space1.OutBoundObjects.Add(pathWayCondition);
        pathWayCondition.InBoundObjects.Add(space1);
        var pathway2 = new LearningPathway(space3, space4);
        space3.OutBoundObjects.Add(space4);
        space4.InBoundObjects.Add(space3);
        
        world.LearningPathways.Add(pathway1);
        world.LearningPathways.Add(pathway2);

        var command = new CreateLearningPathWay(world, pathWayCondition, space3, mappingAction);

        Assert.That(world.LearningPathways, Has.Count.EqualTo(2));
        Assert.IsFalse(actionWasInvoked);

        command.Execute();
        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(command.HasError, Is.False);
            Assert.That(world.LearningPathways, Has.Count.EqualTo(3));
            Assert.That(world.LearningPathways.Last().SourceObject, Is.EqualTo(pathWayCondition));
            Assert.That(world.LearningPathways.Last().TargetObject, Is.EqualTo(space3));
        });
    }

    [Test]
    public void Execute_LearningPathWayAlreadyExists_HasErrorIsTrue()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var space1 = new LearningSpace("z", "w", "v", 5);
        var space2 = new LearningSpace("l", "o", "p", 3);
        world.LearningSpaces.Add(space1);
        world.LearningSpaces.Add(space2);
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var pathway = new LearningPathway(space1, space2);
        world.LearningPathways.Add(pathway);
        
        var command = new CreateLearningPathWay(world, space1, space2, mappingAction);
        
        command.Execute();
        
        Assert.That(actionWasInvoked, Is.False);
        Assert.That(command.HasError, Is.True);
        Assert.That(world.LearningPathways, Has.Count.EqualTo(1));
    }
    
    [Test]
    public void Execute_SourceSpaceIsTargetSpace_HasErrorIsTrue()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var space = new LearningSpace("z", "w", "v", 5);
        world.LearningSpaces.Add(space);
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new CreateLearningPathWay(world, space, space, mappingAction);
        
        command.Execute();
        
        Assert.That(actionWasInvoked, Is.False);
        Assert.That(command.HasError, Is.True);
        Assert.That(world.LearningPathways, Has.Count.EqualTo(0));
    }
    
    [Test]
    public void Execute_TargetSpaceAlreadyHasInboundSpace_HasErrorIsTrue()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var space0 = new LearningSpace("z", "w", "v", 5);
        var space1 = new LearningSpace("z", "w", "v", 5);
        var space2 = new LearningSpace("z", "w", "v", 5);
        world.LearningSpaces.Add(space0);
        world.LearningSpaces.Add(space1);
        world.LearningSpaces.Add(space2);
        var pathway = new LearningPathway(space0, space1);
        world.LearningPathways.Add(pathway);
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new CreateLearningPathWay(world, space2, space1, mappingAction);
        
        command.Execute();
        
        Assert.That(actionWasInvoked, Is.False);
        Assert.That(command.HasError, Is.True);
        Assert.That(world.LearningPathways, Has.Count.EqualTo(1));
    }
    
    [Test]
    public void Execute_PathWayIsCircular_HasErrorIsTrue()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var space1 = new LearningSpace("z", "w", "v", 5);
        var space2 = new LearningSpace("l", "o", "p", 3);
        var space3 = new LearningSpace("n", "e", "r", 6);
        var space4 = new LearningSpace("t", "l", "p", 6);
        world.LearningSpaces.Add(space1);
        world.LearningSpaces.Add(space2);
        world.LearningSpaces.Add(space3);
        world.LearningSpaces.Add(space4);
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var pathway1 = new LearningPathway(space1, space2);
        space1.OutBoundObjects.Add(space2);
        space2.InBoundObjects.Add(space1);
        var pathway2 = new LearningPathway(space2, space3);
        space2.OutBoundObjects.Add(space3);
        space3.InBoundObjects.Add(space2);
        var pathway3 = new LearningPathway(space3, space4);
        space3.OutBoundObjects.Add(space4);
        space4.InBoundObjects.Add(space3);
        
        world.LearningPathways.Add(pathway1);
        world.LearningPathways.Add(pathway2);
        world.LearningPathways.Add(pathway3);
        
        var command = new CreateLearningPathWay(world, space4, space1, mappingAction);
        
        command.Execute();
        
        Assert.That(actionWasInvoked, Is.False);
        Assert.That(command.HasError, Is.True);
        Assert.That(world.LearningPathways, Has.Count.EqualTo(3));
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var space1 = new LearningSpace("z", "w", "v", 5);
        var space2 = new LearningSpace("l", "o", "p", 3);
        world.LearningSpaces.Add(space1);
        world.LearningSpaces.Add(space2);
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;
        
        var command = new CreateLearningPathWay(world, space1, space2, mappingAction);
    
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
        
        Assert.IsFalse(actionWasInvoked);
    }

    [Test]
    public void UndoRedo_UndoesAndRedoesCreateLearningPathWay()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var space1 = new LearningSpace("z", "w", "v", 5);
        var space2 = new LearningSpace("l", "o", "p", 3);
        world.LearningSpaces.Add(space1);
        world.LearningSpaces.Add(space2);
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;
        
        var command = new CreateLearningPathWay(world, space1, space2, mappingAction);
        
        Assert.IsEmpty(world.LearningPathways);
        Assert.IsFalse(actionWasInvoked);

        command.Execute();
        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True); actionWasInvoked = false;
            Assert.That(world.LearningPathways, Has.Count.EqualTo(1));
            Assert.That(world.LearningPathways.First().SourceObject, Is.EqualTo(space1));
            Assert.That(world.LearningPathways.First().TargetObject, Is.EqualTo(space2));
        });
        
        command.Undo();
        
        Assert.IsEmpty(world.LearningPathways);
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Redo();
        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True); actionWasInvoked = false;
            Assert.That(world.LearningPathways, Has.Count.EqualTo(1));
            Assert.That(world.LearningPathways.First().SourceObject, Is.EqualTo(space1));
            Assert.That(world.LearningPathways.First().TargetObject, Is.EqualTo(space2));
        });
    }
}