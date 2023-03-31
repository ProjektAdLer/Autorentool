using BusinessLogic.Commands;
using BusinessLogic.Commands.Space;
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
        var space0 = new LearningSpace("a", "d", "e", 4);
        var space = new LearningSpace("g", "j", "k", 5);
        var space1 = new LearningSpace("g", "j", "k", 5);
        world.LearningSpaces.Add(space0);
        world.LearningSpaces.Add(space);
        world.LearningSpaces.Add(space1);
        world.SelectedLearningObjectInPathWay = space;
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;
        
        world.LearningPathways.Add(new LearningPathway(space0, space));
        space0.OutBoundObjects.Add(space);
        space.InBoundObjects.Add(space0);
        world.LearningPathways.Add(new LearningPathway(space, space1));
        space.OutBoundObjects.Add(space1);
        space1.InBoundObjects.Add(space);

        var command = new DeleteLearningSpace(world, space, mappingAction);
        
        Assert.That(world.LearningSpaces, Does.Contain(space));
        Assert.IsFalse(actionWasInvoked);
        Assert.That(world.SelectedLearningObjectInPathWay, Is.EqualTo(space));
        
        command.Execute();
        
        Assert.That(world.LearningSpaces, Has.Count.EqualTo(2));
        Assert.IsTrue(actionWasInvoked);
        Assert.That(world.SelectedLearningObjectInPathWay, Is.EqualTo(space1));
        Assert.That(world.LearningPathways, Has.Count.EqualTo(0));
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var space = new LearningSpace("g", "j", "k", 5);
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteLearningSpace(world,space, mappingAction);
        
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
        
        Assert.IsFalse(actionWasInvoked);
    }
    
    [Test]
    public void UndoRedo_UndoesAndRedoesDeleteLearningSpace()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var space = new LearningSpace("g", "j", "k", 5);
        var space2 = new LearningSpace("l", "o", "p", 7);
        world.LearningSpaces.Add(space);
        world.LearningSpaces.Add(space2);
        world.SelectedLearningObjectInPathWay = space;
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;
        
        var command = new DeleteLearningSpace(world, space, mappingAction);
        
        Assert.That(world.LearningSpaces, Has.Count.EqualTo(2));
        Assert.That(world.SelectedLearningObjectInPathWay, Is.EqualTo(space));
        Assert.IsFalse(actionWasInvoked);
        
        command.Execute();
        
        Assert.That(world.LearningSpaces, Has.Count.EqualTo(1));
        Assert.That(world.SelectedLearningObjectInPathWay, Is.EqualTo(space2));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Undo();
        
        Assert.That(world.LearningSpaces, Has.Count.EqualTo(2));
        Assert.That(world.SelectedLearningObjectInPathWay, Is.EqualTo(space));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Redo();
        
        Assert.That(world.LearningSpaces, Has.Count.EqualTo(1));
        Assert.That(world.SelectedLearningObjectInPathWay, Is.EqualTo(space2));
        Assert.IsTrue(actionWasInvoked);
    }
}