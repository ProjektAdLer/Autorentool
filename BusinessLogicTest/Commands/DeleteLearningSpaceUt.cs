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
        var mappingAction = Substitute.For<Action<LearningWorld>>();

        var command = new DeleteLearningSpace(world, space, mappingAction);
        
        Assert.That(world.LearningSpaces, Does.Contain(space));
        
        command.Execute();
        
        Assert.That(world.LearningSpaces, Is.Empty);
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
        var mappingAction = Substitute.For<Action<LearningWorld>>();
        
        var command = new DeleteLearningSpace(world, space, mappingAction);
        
        Assert.That(world.LearningSpaces, Has.Count.EqualTo(1));
        
        command.Execute();
        
        Assert.That(world.LearningSpaces, Has.Count.EqualTo(0));
        
        command.Undo();
        
        Assert.That(world.LearningSpaces, Has.Count.EqualTo(1));
        
        command.Redo();
        
        Assert.That(world.LearningSpaces, Has.Count.EqualTo(0));
    }
}