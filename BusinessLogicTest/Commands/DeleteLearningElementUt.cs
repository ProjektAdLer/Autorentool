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
        world.SelectedLearningObject = element;
        bool actionWasInvoked = false;
        Action<ILearningElementParent> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteLearningElement(element, world, mappingAction);
        
        Assert.That(world.LearningElements, Does.Contain(element));
        Assert.IsFalse(actionWasInvoked);
        
        command.Execute();
        
        Assert.That(world.LearningElements, Is.Empty);
        Assert.IsTrue(actionWasInvoked);
        Assert.That(world.SelectedLearningObject, Is.Null);
    }

    [Test]
    public void Execute_DeletesLearningElement_SpaceParent()
    {
        var space = new LearningSpace("a", "b", "c","d", "e", 5);
        var element = new LearningElement("g", "h", null!, "i", "j", "k", LearningElementDifficultyEnum.Easy, space);
        space.LearningElements.Add(element);
        space.SelectedLearningObject = element;
        bool actionWasInvoked = false;
        Action<ILearningElementParent> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteLearningElement(element, space, mappingAction);
        
        Assert.That(space.LearningElements, Does.Contain(element));
        Assert.IsFalse(actionWasInvoked);
        
        command.Execute();
        
        Assert.That(space.LearningElements, Is.Empty);
        Assert.IsTrue(actionWasInvoked);
        Assert.That(space.SelectedLearningObject, Is.Null);
    }
    
    [Test]
    public void Execute_DeletesLearningElementAndSetsAnotherElementSelectedLearningObject_WorldParent()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var element = new LearningElement("g", "h", null!, "i", "j", "k", LearningElementDifficultyEnum.Easy, world);
        var element2 = new LearningElement("l", "m", null!, "n", "o", "p", LearningElementDifficultyEnum.Easy, world);
        world.LearningElements.Add(element);
        world.LearningElements.Add(element2);
        world.SelectedLearningObject = element;
        bool actionWasInvoked = false;
        Action<ILearningElementParent> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteLearningElement(element, world, mappingAction);
        
        Assert.That(world.LearningElements.Count, Is.EqualTo(2));
        Assert.That(world.LearningElements, Does.Contain(element));
        Assert.IsFalse(actionWasInvoked);
        
        command.Execute();
        
        Assert.That(world.LearningElements.Count, Is.EqualTo(1));
        Assert.That(world.LearningElements, Does.Not.Contain(element));
        Assert.IsTrue(actionWasInvoked);
        Assert.That(world.SelectedLearningObject, Is.EqualTo(element2));
    }

    [Test]
    public void Execute_DeletesLearningElementAndSetsAnotherElementSelectedLearningObject_SpaceParent()
    {
        var space = new LearningSpace("a", "b", "c","d", "e", 5);
        var element = new LearningElement("g", "h", null!, "i", "j", "k", LearningElementDifficultyEnum.Easy, space);
        var element2 = new LearningElement("l", "m", null!, "n", "o", "p", LearningElementDifficultyEnum.Easy, space);
        space.LearningElements.Add(element);
        space.LearningElements.Add(element2);
        space.SelectedLearningObject = element;
        bool actionWasInvoked = false;
        Action<ILearningElementParent> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteLearningElement(element, space, mappingAction);
        
        Assert.That(space.LearningElements, Does.Contain(element));
        Assert.That(space.LearningElements.Count, Is.EqualTo(2));
        Assert.IsFalse(actionWasInvoked);
        
        command.Execute();
        
        Assert.That(space.LearningElements.Count, Is.EqualTo(1));
        Assert.That(space.LearningElements, Does.Not.Contain(element));
        Assert.IsTrue(actionWasInvoked);
        Assert.That(space.SelectedLearningObject, Is.EqualTo(element2));
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
        var element2 = new LearningElement("l", "m", null!, "n", "o", "p", LearningElementDifficultyEnum.Easy, world);
        world.LearningElements.Add(element);
        world.LearningElements.Add(element2);
        world.SelectedLearningObject = element;
        bool actionWasInvoked = false;
        Action<ILearningElementParent> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteLearningElement(element, world, mappingAction);
        
        Assert.That(world.LearningElements, Has.Count.EqualTo(2));
        Assert.That(world.LearningElements, Does.Contain(element));
        Assert.That(world.SelectedLearningObject, Is.EqualTo(element));
        Assert.IsFalse(actionWasInvoked);
        
        command.Execute();
        
        Assert.That(world.LearningElements, Has.Count.EqualTo(1));
        Assert.That(world.SelectedLearningObject, Is.EqualTo(element2));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Undo();
        
        Assert.That(world.LearningElements, Has.Count.EqualTo(2));
        Assert.That(world.LearningElements, Does.Contain(element));
        Assert.That(world.SelectedLearningObject, Is.EqualTo(element));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Redo();
        
        Assert.That(world.LearningElements, Has.Count.EqualTo(1));
        Assert.That(world.SelectedLearningObject, Is.EqualTo(element2));
        Assert.IsTrue(actionWasInvoked);
    }

    [Test]
    public void UndoRedo_UndoesRedoesDeleteLearningElement_SpaceParent()
    {
        var space = new LearningSpace("a", "b", "c","d", "e", 5);
        var element = new LearningElement("g", "h", null!, "i", "j", "k", LearningElementDifficultyEnum.Easy, space);
        var element2 = new LearningElement("l", "m", null!, "n", "o", "p", LearningElementDifficultyEnum.Easy, space);
        space.LearningElements.Add(element);
        space.LearningElements.Add(element2);
        space.SelectedLearningObject = element;
        bool actionWasInvoked = false;
        Action<ILearningElementParent> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteLearningElement(element, space, mappingAction);
        
        Assert.That(space.LearningElements, Has.Count.EqualTo(2));
        Assert.That(space.LearningElements, Does.Contain(element));
        Assert.That(space.SelectedLearningObject, Is.EqualTo(element));
        Assert.IsFalse(actionWasInvoked);
        
        command.Execute();
        
        Assert.That(space.LearningElements, Has.Count.EqualTo(1));
        Assert.That(space.SelectedLearningObject, Is.EqualTo(element2));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Undo();
        
        Assert.That(space.LearningElements, Has.Count.EqualTo(2));
        Assert.That(space.LearningElements, Does.Contain(element));
        Assert.That(space.SelectedLearningObject, Is.EqualTo(element));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Redo();
        
        Assert.That(space.LearningElements, Has.Count.EqualTo(1));
        Assert.That(space.SelectedLearningObject, Is.EqualTo(element2));
        Assert.IsTrue(actionWasInvoked);
    }
}