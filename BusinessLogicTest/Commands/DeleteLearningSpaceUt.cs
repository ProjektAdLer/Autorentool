using BusinessLogic.Commands;
using BusinessLogic.Entities;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Commands;

[TestFixture]

public class DeleteLearningSpaceUt
{
    [Test]
    public void Execute_DeletesLearningSpace()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var space = new LearningSpace("g", "h", "i", "j", "k", 5);
        world.LearningSpaces.Add(space);
        world.SelectedLearningSpace = space;
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteLearningSpace(world, space, mappingAction);
        
        Assert.That(world.LearningSpaces, Does.Contain(space));
        Assert.IsFalse(actionWasInvoked);
        Assert.That(world.SelectedLearningSpace, Is.EqualTo(space));
        
        command.Execute();
        
        Assert.That(world.LearningSpaces, Is.Empty);
        Assert.IsTrue(actionWasInvoked);
        Assert.That(world.SelectedLearningSpace, Is.Null);
    }

    [Test]
    public void Execute_DeletesLearningSpaceAndSetsAnotherLearningSpaceAsSelectedLearningSpace()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var space1 = new LearningSpace("g", "h", "i", "j", "k", 5);
        var space2 = new LearningSpace("g2", "h2", "i2", "j2", "k2", 5);
        world.LearningSpaces.Add(space1);
        world.LearningSpaces.Add(space2);
        world.SelectedLearningSpace = space1;
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteLearningSpace(world, space1, mappingAction);
        
        Assert.That(world.LearningSpaces, Does.Contain(space1));
        Assert.That(world.LearningSpaces, Does.Contain(space2));
        Assert.IsFalse(actionWasInvoked);
        Assert.That(world.SelectedLearningSpace, Is.EqualTo(space1));
        
        command.Execute();
        
        Assert.That(world.LearningSpaces, Has.Count.EqualTo(1));
        Assert.IsTrue(actionWasInvoked);
        Assert.That(world.SelectedLearningSpace, Is.EqualTo(space2));
    }
    
    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var space = new LearningSpace("g", "h", "i", "j", "k", 5);
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteLearningSpace(world,space, mappingAction);
        
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
        
        Assert.IsFalse(actionWasInvoked);
    }
    
    [Test]
    public void UndoRedo_UndoesAndRedoesCreateLearningSpace()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var space = new LearningSpace("g", "h", "i", "j", "k", 5);
        var space2 = new LearningSpace("l", "m", "n", "o", "p", 7);
        world.LearningSpaces.Add(space);
        world.LearningSpaces.Add(space2);
        world.SelectedLearningSpace = space;
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;
        
        var command = new DeleteLearningSpace(world, space, mappingAction);
        
        Assert.That(world.LearningSpaces, Has.Count.EqualTo(2));
        Assert.That(world.SelectedLearningSpace, Is.EqualTo(space));
        Assert.IsFalse(actionWasInvoked);
        
        command.Execute();
        
        Assert.That(world.LearningSpaces, Has.Count.EqualTo(1));
        Assert.That(world.SelectedLearningSpace, Is.EqualTo(space2));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Undo();
        
        Assert.That(world.LearningSpaces, Has.Count.EqualTo(2));
        Assert.That(world.SelectedLearningSpace, Is.EqualTo(space));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Redo();
        
        Assert.That(world.LearningSpaces, Has.Count.EqualTo(1));
        Assert.That(world.SelectedLearningSpace, Is.EqualTo(space2));
        Assert.IsTrue(actionWasInvoked);
    }
}