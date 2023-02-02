using BusinessLogic.Commands;
using BusinessLogic.Commands.Pathway;
using BusinessLogic.Entities;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Commands;

[TestFixture]

public class CreatePathWayUt
{
    [Test]
    public void Execute_CreatePathWay()
    {
        var world = new World("a", "b", "c", "d", "e", "f");
        var space1 = new Space("z", "y", "x", "w", "v", 5);
        var pathWayCondition = new PathWayCondition(ConditionEnum.And,3, 3);
        var space3 = new Space("l", "m", "n", "o", "p", 3);
        var space4 = new Space("l", "m", "n", "o", "p", 3);
        world.Spaces.Add(space1);
        world.PathWayConditions.Add(pathWayCondition);
        world.Spaces.Add(space3);
        world.Spaces.Add(space4);
        bool actionWasInvoked = false;
        Action<World> mappingAction = _ => actionWasInvoked = true;
        
        var pathway1 = new Pathway(space1, pathWayCondition);
        space1.OutBoundObjects.Add(pathWayCondition);
        pathWayCondition.InBoundObjects.Add(space1);
        var pathway2 = new Pathway(space3, space4);
        space3.OutBoundObjects.Add(space4);
        space4.InBoundObjects.Add(space3);
        
        world.Pathways.Add(pathway1);
        world.Pathways.Add(pathway2);

        var command = new CreatePathWay(world, pathWayCondition, space3, mappingAction);

        Assert.That(world.Pathways, Has.Count.EqualTo(2));
        Assert.IsFalse(actionWasInvoked);

        command.Execute();
        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(command.HasError, Is.False);
            Assert.That(world.Pathways, Has.Count.EqualTo(3));
            Assert.That(world.Pathways.Last().SourceObject, Is.EqualTo(pathWayCondition));
            Assert.That(world.Pathways.Last().TargetObject, Is.EqualTo(space3));
        });
    }

    [Test]
    public void Execute_PathWayAlreadyExists_HasErrorIsTrue()
    {
        var world = new World("a", "b", "c", "d", "e", "f");
        var space1 = new Space("z", "y", "x", "w", "v", 5);
        var space2 = new Space("l", "m", "n", "o", "p", 3);
        world.Spaces.Add(space1);
        world.Spaces.Add(space2);
        bool actionWasInvoked = false;
        Action<World> mappingAction = _ => actionWasInvoked = true;

        var pathway = new Pathway(space1, space2);
        world.Pathways.Add(pathway);
        
        var command = new CreatePathWay(world, space1, space2, mappingAction);
        
        command.Execute();
        
        Assert.That(actionWasInvoked, Is.False);
        Assert.That(command.HasError, Is.True);
        Assert.That(world.Pathways, Has.Count.EqualTo(1));
    }
    
    [Test]
    public void Execute_SourceSpaceIsTargetSpace_HasErrorIsTrue()
    {
        var world = new World("a", "b", "c", "d", "e", "f");
        var space = new Space("z", "y", "x", "w", "v", 5);
        world.Spaces.Add(space);
        bool actionWasInvoked = false;
        Action<World> mappingAction = _ => actionWasInvoked = true;

        var command = new CreatePathWay(world, space, space, mappingAction);
        
        command.Execute();
        
        Assert.That(actionWasInvoked, Is.False);
        Assert.That(command.HasError, Is.True);
        Assert.That(world.Pathways, Has.Count.EqualTo(0));
    }
    
    [Test]
    public void Execute_TargetSpaceAlreadyHasInboundSpace_HasErrorIsTrue()
    {
        var world = new World("a", "b", "c", "d", "e", "f");
        var space0 = new Space("z", "y", "x", "w", "v", 5);
        var space1 = new Space("z", "y", "x", "w", "v", 5);
        var space2 = new Space("z", "y", "x", "w", "v", 5);
        world.Spaces.Add(space0);
        world.Spaces.Add(space1);
        world.Spaces.Add(space2);
        var pathway = new Pathway(space0, space1);
        world.Pathways.Add(pathway);
        bool actionWasInvoked = false;
        Action<World> mappingAction = _ => actionWasInvoked = true;

        var command = new CreatePathWay(world, space2, space1, mappingAction);
        
        command.Execute();
        
        Assert.That(actionWasInvoked, Is.False);
        Assert.That(command.HasError, Is.True);
        Assert.That(world.Pathways, Has.Count.EqualTo(1));
    }
    
    [Test]
    public void Execute_PathWayIsCircular_HasErrorIsTrue()
    {
        var world = new World("a", "b", "c", "d", "e", "f");
        var space1 = new Space("z", "y", "x", "w", "v", 5);
        var space2 = new Space("l", "m", "n", "o", "p", 3);
        var space3 = new Space("n", "q", "w", "e", "r", 6);
        var space4 = new Space("t", "i", "o", "l", "p", 6);
        world.Spaces.Add(space1);
        world.Spaces.Add(space2);
        world.Spaces.Add(space3);
        world.Spaces.Add(space4);
        bool actionWasInvoked = false;
        Action<World> mappingAction = _ => actionWasInvoked = true;

        var pathway1 = new Pathway(space1, space2);
        space1.OutBoundObjects.Add(space2);
        space2.InBoundObjects.Add(space1);
        var pathway2 = new Pathway(space2, space3);
        space2.OutBoundObjects.Add(space3);
        space3.InBoundObjects.Add(space2);
        var pathway3 = new Pathway(space3, space4);
        space3.OutBoundObjects.Add(space4);
        space4.InBoundObjects.Add(space3);
        
        world.Pathways.Add(pathway1);
        world.Pathways.Add(pathway2);
        world.Pathways.Add(pathway3);
        
        var command = new CreatePathWay(world, space4, space1, mappingAction);
        
        command.Execute();
        
        Assert.That(actionWasInvoked, Is.False);
        Assert.That(command.HasError, Is.True);
        Assert.That(world.Pathways, Has.Count.EqualTo(3));
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var world = new World("a", "b", "c", "d", "e", "f");
        var space1 = new Space("z", "y", "x", "w", "v", 5);
        var space2 = new Space("l", "m", "n", "o", "p", 3);
        world.Spaces.Add(space1);
        world.Spaces.Add(space2);
        bool actionWasInvoked = false;
        Action<World> mappingAction = _ => actionWasInvoked = true;
        
        var command = new CreatePathWay(world, space1, space2, mappingAction);
    
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
        
        Assert.IsFalse(actionWasInvoked);
    }

    [Test]
    public void UndoRedo_UndoesAndRedoesCreatePathWay()
    {
        var world = new World("a", "b", "c", "d", "e", "f");
        var space1 = new Space("z", "y", "x", "w", "v", 5);
        var space2 = new Space("l", "m", "n", "o", "p", 3);
        world.Spaces.Add(space1);
        world.Spaces.Add(space2);
        bool actionWasInvoked = false;
        Action<World> mappingAction = _ => actionWasInvoked = true;
        
        var command = new CreatePathWay(world, space1, space2, mappingAction);
        
        Assert.IsEmpty(world.Pathways);
        Assert.IsFalse(actionWasInvoked);

        command.Execute();
        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True); actionWasInvoked = false;
            Assert.That(world.Pathways, Has.Count.EqualTo(1));
            Assert.That(world.Pathways.First().SourceObject, Is.EqualTo(space1));
            Assert.That(world.Pathways.First().TargetObject, Is.EqualTo(space2));
        });
        
        command.Undo();
        
        Assert.IsEmpty(world.Pathways);
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Redo();
        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True); actionWasInvoked = false;
            Assert.That(world.Pathways, Has.Count.EqualTo(1));
            Assert.That(world.Pathways.First().SourceObject, Is.EqualTo(space1));
            Assert.That(world.Pathways.First().TargetObject, Is.EqualTo(space2));
        });
    }
}