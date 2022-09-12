using BusinessLogic.Commands;
using BusinessLogic.Entities;
using NSubstitute;
using NUnit.Framework;

namespace BusinessLogicTest.Commands;

[TestFixture]

public class DeleteLearningSpaceUt
{
    [Test]
    public void Execute_DeletesLearningSpace()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var space = new LearningSpace("g", "h", "i", "j", "k");
        world.LearningSpaces.Add(space);
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteLearningSpace(world, space, mappingAction);
        
        Assert.That(world.LearningSpaces, Does.Contain(space));
        Assert.IsFalse(actionWasInvoked);
        
        command.Execute();
        
        Assert.That(world.LearningSpaces, Is.Empty);
        Assert.IsTrue(actionWasInvoked);
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var space = new LearningSpace("g", "h", "i", "j", "k");
        var mappingAction = Substitute.For<Action<LearningWorld>>();

        var command = new DeleteLearningSpace(world,space, mappingAction);
        
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
    }
    
    [Test]
    public void UndoRedo_UndoesAndRedoesCreateLearningSpace()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var space = new LearningSpace("g", "h", "i", "j", "k");
        world.LearningSpaces.Add(space);
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;
        
        var command = new DeleteLearningSpace(world, space, mappingAction);
        
        Assert.That(world.LearningSpaces, Has.Count.EqualTo(1));
        Assert.IsFalse(actionWasInvoked);
        
        command.Execute();
        
        Assert.That(world.LearningSpaces, Has.Count.EqualTo(0));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Undo();
        
        Assert.That(world.LearningSpaces, Has.Count.EqualTo(1));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Redo();
        
        Assert.That(world.LearningSpaces, Has.Count.EqualTo(0));
        Assert.IsTrue(actionWasInvoked);
    }
}