using BusinessLogic.Commands;
using BusinessLogic.Commands.Pathway;
using BusinessLogic.Entities;
using NUnit.Framework;

namespace BusinessLogicTest.Commands;

[TestFixture]

public class DeletePathWayUt
{
    [Test]
    public void Execute_DeletesPathWay()
    {
        var world = new World("a", "b", "c", "d", "e", "f");
        var space1 = new Space("z", "y", "x", "w", "v", 5);
        var space2 = new Space("l", "m", "n", "o", "p", 3);
        var space3 = new Space("l", "m", "n", "o", "p", 3);
        var space4 = new Space("l", "m", "n", "o", "p", 3);
        world.Spaces.Add(space1);
        world.Spaces.Add(space2);
        world.Spaces.Add(space3);
        world.Spaces.Add(space4);
        bool actionWasInvoked = false;
        Action<World> mappingAction = _ => actionWasInvoked = true;
        
        var pathway1 = new Pathway(space1, space2);
        space1.OutBoundObjects.Add(space2);
        space2.InBoundObjects.Add(space1);
        var pathway2 = new Pathway(space3, space4);
        space3.OutBoundObjects.Add(space4);
        space4.InBoundObjects.Add(space3);
        
        world.Pathways.Add(pathway1);
        world.Pathways.Add(pathway2);

        var command = new DeletePathWay(world, pathway2, mappingAction);
        
        Assert.That(world.Pathways, Has.Count.EqualTo(2));
        Assert.IsFalse(actionWasInvoked);
        
        command.Execute();
        
        Assert.That(world.Pathways, Has.Count.EqualTo(1));
        Assert.That(world.Pathways, Does.Not.Contain(pathway2));
        Assert.That(actionWasInvoked, Is.True);
    }
    
    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var world = new World("a", "b", "c", "d", "e", "f");
        var space1 = new Space("z", "y", "x", "w", "v", 5);
        var space2 = new Space("l", "m", "n", "o", "p", 3);
        var pathWay = new Pathway(space1, space2);
        world.Spaces.Add(space1);
        world.Spaces.Add(space2);
        world.Pathways.Add(pathWay);
        bool actionWasInvoked = false;
        Action<World> mappingAction = _ => actionWasInvoked = true;
        
        var command = new DeletePathWay(world, pathWay, mappingAction);
    
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
        
        var pathway = new Pathway(space1, space2);
        
        world.Pathways.Add(pathway);
        
        var command = new DeletePathWay(world, pathway, mappingAction);
        
        Assert.That(world.Pathways, Has.Count.EqualTo(1));
        Assert.IsFalse(actionWasInvoked);

        command.Execute();
        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True); actionWasInvoked = false;
            Assert.That(world.Pathways, Has.Count.EqualTo(0));
        });
        
        command.Undo();
        
        Assert.That(world.Pathways, Has.Count.EqualTo(1));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Redo();
        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True); actionWasInvoked = false;
            Assert.That(world.Pathways, Has.Count.EqualTo(0));
        });
    }
}