using BusinessLogic.Commands;
using BusinessLogic.Entities;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Commands;

[TestFixture]

public class DeleteLearningElementUt
{
    [Test]
    public void Execute_DeletesLearningElement_WorldParent()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var element = new LearningElement("g", "h", null!, "i", "j", "k", LearningElementDifficultyEnum.Easy, world);
        world.LearningElements.Add(element);
        bool actionWasInvoked = false;
        Action<ILearningElementParent> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteLearningElement(element, world, mappingAction);
        
        Assert.That(world.LearningElements, Does.Contain(element));
        Assert.IsFalse(actionWasInvoked);
        
        command.Execute();
        
        Assert.That(world.LearningElements, Is.Empty);
        Assert.IsTrue(actionWasInvoked);
    }

    [Test]
    public void Execute_DeletesLearningElement_SpaceParent()
    {
        var space = new LearningSpace("a", "b", "c","d", "e");
        var element = new LearningElement("g", "h", null!, "i", "j", "k", LearningElementDifficultyEnum.Easy, space);
        space.LearningElements.Add(element);
        bool actionWasInvoked = false;
        Action<ILearningElementParent> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteLearningElement(element, space, mappingAction);
        
        Assert.That(space.LearningElements, Does.Contain(element));
        Assert.IsFalse(actionWasInvoked);
        
        command.Execute();
        
        Assert.That(space.LearningElements, Is.Empty);
        Assert.IsTrue(actionWasInvoked);
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var element = new LearningElement("g", "h", null!, "i", "j", "k", LearningElementDifficultyEnum.Easy, world);
        bool actionWasInvoked = false;
        Action<ILearningElementParent> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteLearningElement(element, world, mappingAction);
        
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
        
        Assert.IsFalse(actionWasInvoked);
    }
    
    [Test]
    public void UndoRedo_UndoesRedoesDeleteLearningElement_WorldParent()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var element = new LearningElement("g", "h", null!, "i", "j", "k", LearningElementDifficultyEnum.Easy, world);
        world.LearningElements.Add(element);
        bool actionWasInvoked = false;
        Action<ILearningElementParent> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteLearningElement(element, world, mappingAction);
        
        Assert.That(world.LearningElements, Does.Contain(element));
        Assert.IsFalse(actionWasInvoked);
        
        command.Execute();
        
        Assert.That(world.LearningElements, Is.Empty);
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Undo();
        
        Assert.That(world.LearningElements, Does.Contain(element));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Redo();
        
        Assert.That(world.LearningElements, Is.Empty);
        Assert.IsTrue(actionWasInvoked);
    }

    [Test]
    public void UndoRedo_UndoesRedoesDeleteLearningElement_SpaceParent()
    {
        var space = new LearningSpace("a", "b", "c","d", "e");
        var element = new LearningElement("g", "h", null!, "i", "j", "k", LearningElementDifficultyEnum.Easy, space);
        space.LearningElements.Add(element);
        bool actionWasInvoked = false;
        Action<ILearningElementParent> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteLearningElement(element, space, mappingAction);
        
        Assert.That(space.LearningElements, Does.Contain(element));
        Assert.IsFalse(actionWasInvoked);
        
        command.Execute();
        
        Assert.That(space.LearningElements, Is.Empty);
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Undo();
        
        Assert.That(space.LearningElements, Does.Contain(element));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Redo();
        
        Assert.That(space.LearningElements, Is.Empty);
        Assert.IsTrue(actionWasInvoked);
    }
}