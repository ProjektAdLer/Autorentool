using BusinessLogic.Commands.Element;
using BusinessLogic.Entities;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Commands.Element;

[TestFixture]

public class DeleteLearningElementInWorldUt
{
    [Test]
    public void Execute_Undo_Redo_DeletesLearningElement()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f")
        {
            UnsavedChanges = false
        };
        var element = new LearningElement("g", null!, "j", "k", LearningElementDifficultyEnum.Easy)
        {
            UnsavedChanges = false
        };
        world.UnplacedLearningElements = new List<ILearningElement>() { element } ;
        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteLearningElementInWorld(element, world, mappingAction);
        
        Assert.Multiple(() =>
        {
            Assert.That(world.UnplacedLearningElements, Does.Contain(element));
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(world.UnsavedChanges, Is.False);
        });
        
        command.Execute();
        
        Assert.Multiple(() =>
        {
            Assert.That(world.UnplacedLearningElements, Is.Empty);
            
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(world.UnsavedChanges, Is.True);
        });
        
        actionWasInvoked = false;
        
        command.Undo();
        
        Assert.Multiple(() =>
        {
            Assert.That(world.UnplacedLearningElements, Does.Contain(element));
            
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(world.UnsavedChanges, Is.False);
        });
        
        actionWasInvoked = false;
        
        command.Redo();
        
        Assert.Multiple(() =>
        {
            Assert.That(world.UnplacedLearningElements, Is.Empty);
            
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(world.UnsavedChanges, Is.True);
        });
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var world = new LearningWorld("a", "b", "c","d", "e","f");
        var element = new LearningElement("g", null!, "j", "k", LearningElementDifficultyEnum.Easy);
        world.UnplacedLearningElements = new List<ILearningElement>() { element } ;
        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteLearningElementInWorld(element, world, mappingAction);
        
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        
        Assert.Multiple(() =>
        {
            Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
            Assert.That(actionWasInvoked, Is.False);
        });
    }
}