using BusinessLogic.Commands;
using BusinessLogic.Entities;
using NUnit.Framework;

namespace BusinessLogicTest.Commands;

[TestFixture]

public class DeleteLearningPathWayUt
{
    [Test]
    public void Execute_DeletesLearningPathWay()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var space1 = new LearningSpace("z", "y", "x", "w", "v", 5);
        var space2 = new LearningSpace("l", "m", "n", "o", "p", 3);
        var space3 = new LearningSpace("l", "m", "n", "o", "p", 3);
        var space4 = new LearningSpace("l", "m", "n", "o", "p", 3);
        world.LearningSpaces.Add(space1);
        world.LearningSpaces.Add(space2);
        world.LearningSpaces.Add(space3);
        world.LearningSpaces.Add(space4);
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;
        
        var pathway1 = new LearningPathway(space1, space2);
        space1.OutBoundObjects.Add(space2);
        space2.InBoundObjects.Add(space1);
        var pathway2 = new LearningPathway(space3, space4);
        space3.OutBoundObjects.Add(space4);
        space4.InBoundObjects.Add(space3);
        
        world.LearningPathways.Add(pathway1);
        world.LearningPathways.Add(pathway2);

        var command = new DeleteLearningPathWay(world, pathway2, mappingAction);
        
        Assert.That(world.LearningPathways, Has.Count.EqualTo(2));
        Assert.IsFalse(actionWasInvoked);
        
        command.Execute();
        
        Assert.That(world.LearningPathways, Has.Count.EqualTo(1));
        Assert.That(world.LearningPathways, Does.Not.Contain(pathway2));
        Assert.That(actionWasInvoked, Is.True);
    }
    
    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var space1 = new LearningSpace("z", "y", "x", "w", "v", 5);
        var space2 = new LearningSpace("l", "m", "n", "o", "p", 3);
        var pathWay = new LearningPathway(space1, space2);
        world.LearningSpaces.Add(space1);
        world.LearningSpaces.Add(space2);
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;
        
        var command = new DeleteLearningPathWay(world, pathWay, mappingAction);
    
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
        
        Assert.IsFalse(actionWasInvoked);
    }
    
    [Test]
    public void UndoRedo_UndoesAndRedoesCreateLearningPathWay()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var space1 = new LearningSpace("z", "y", "x", "w", "v", 5);
        var space2 = new LearningSpace("l", "m", "n", "o", "p", 3);
        world.LearningSpaces.Add(space1);
        world.LearningSpaces.Add(space2);
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;
        
        var pathway = new LearningPathway(space1, space2);
        
        world.LearningPathways.Add(pathway);
        
        var command = new DeleteLearningPathWay(world, pathway, mappingAction);
        
        Assert.That(world.LearningPathways, Has.Count.EqualTo(1));
        Assert.IsFalse(actionWasInvoked);

        command.Execute();
        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True); actionWasInvoked = false;
            Assert.That(world.LearningPathways, Has.Count.EqualTo(0));
        });
        
        command.Undo();
        
        Assert.That(world.LearningPathways, Has.Count.EqualTo(1));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Redo();
        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True); actionWasInvoked = false;
            Assert.That(world.LearningPathways, Has.Count.EqualTo(0));
        });
    }
}