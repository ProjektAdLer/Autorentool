using BusinessLogic.Commands;
using BusinessLogic.Commands.Space;
using BusinessLogic.Entities;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Commands;

[TestFixture]

public class DeleteSpaceUt
{
    [Test]
    public void Execute_DeletesSpace()
    {
        var world = new World("a", "b", "c", "d", "e", "f");
        var space0 = new Space("a", "b", "c", "d", "e", 4);
        var space = new Space("g", "h", "i", "j", "k", 5);
        var space1 = new Space("g", "h", "i", "j", "k", 5);
        world.Spaces.Add(space0);
        world.Spaces.Add(space);
        world.Spaces.Add(space1);
        world.SelectedObject = space;
        bool actionWasInvoked = false;
        Action<World> mappingAction = _ => actionWasInvoked = true;
        
        world.Pathways.Add(new Pathway(space0, space));
        space0.OutBoundObjects.Add(space);
        space.InBoundObjects.Add(space0);
        world.Pathways.Add(new Pathway(space, space1));
        space.OutBoundObjects.Add(space1);
        space1.InBoundObjects.Add(space);

        var command = new DeleteSpace(world, space, mappingAction);
        
        Assert.That(world.Spaces, Does.Contain(space));
        Assert.IsFalse(actionWasInvoked);
        Assert.That(world.SelectedObject, Is.EqualTo(space));
        
        command.Execute();
        
        Assert.That(world.Spaces, Has.Count.EqualTo(2));
        Assert.IsTrue(actionWasInvoked);
        Assert.That(world.SelectedObject, Is.EqualTo(space1));
        Assert.That(world.Pathways, Has.Count.EqualTo(0));
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var world = new World("a", "b", "c", "d", "e", "f");
        var space = new Space("g", "h", "i", "j", "k", 5);
        bool actionWasInvoked = false;
        Action<World> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteSpace(world,space, mappingAction);
        
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
        
        Assert.IsFalse(actionWasInvoked);
    }
    
    [Test]
    public void UndoRedo_UndoesAndRedoesDeleteSpace()
    {
        var world = new World("a", "b", "c", "d", "e", "f");
        var space = new Space("g", "h", "i", "j", "k", 5);
        var space2 = new Space("l", "m", "n", "o", "p", 7);
        world.Spaces.Add(space);
        world.Spaces.Add(space2);
        world.SelectedObject = space;
        bool actionWasInvoked = false;
        Action<World> mappingAction = _ => actionWasInvoked = true;
        
        var command = new DeleteSpace(world, space, mappingAction);
        
        Assert.That(world.Spaces, Has.Count.EqualTo(2));
        Assert.That(world.SelectedObject, Is.EqualTo(space));
        Assert.IsFalse(actionWasInvoked);
        
        command.Execute();
        
        Assert.That(world.Spaces, Has.Count.EqualTo(1));
        Assert.That(world.SelectedObject, Is.EqualTo(space2));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Undo();
        
        Assert.That(world.Spaces, Has.Count.EqualTo(2));
        Assert.That(world.SelectedObject, Is.EqualTo(space));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Redo();
        
        Assert.That(world.Spaces, Has.Count.EqualTo(1));
        Assert.That(world.SelectedObject, Is.EqualTo(space2));
        Assert.IsTrue(actionWasInvoked);
    }
}