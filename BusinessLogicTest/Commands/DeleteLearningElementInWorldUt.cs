using BusinessLogic.Commands.Element;
using BusinessLogic.Entities;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Commands;

[TestFixture]

public class DeleteLearningElementInWorldUt
{
    [Test]
    public void Execute_Undo_Redo_DeletesLearningElement()
    {
        var world = new LearningWorld("a", "b", "c","d", "e","f");
        var element = new LearningElement("g", null!, "j", "k", LearningElementDifficultyEnum.Easy);
        world.UnplacedLearningElements = new List<ILearningElement>() { element } ;
        world.SelectedLearningElement = element;
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteLearningElementInWorld(element, world, mappingAction);
        
        Assert.That(world.UnplacedLearningElements, Does.Contain(element));
        Assert.IsFalse(actionWasInvoked);
        
        command.Execute();
        
        Assert.That(world.UnplacedLearningElements, Is.Empty);
        Assert.That(world.SelectedLearningElement, Is.Null);
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Undo();
        
        Assert.That(world.UnplacedLearningElements, Does.Contain(element));
        Assert.That(world.SelectedLearningElement, Is.EqualTo(element));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Redo();
        Assert.That(world.UnplacedLearningElements, Is.Empty);
        Assert.That(world.SelectedLearningElement, Is.Null);
        Assert.IsTrue(actionWasInvoked);
    }
    
    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var world = new LearningWorld("a", "b", "c","d", "e","f");
        var element = new LearningElement("g", null!, "j", "k", LearningElementDifficultyEnum.Easy);
        world.UnplacedLearningElements = new List<ILearningElement>() { element } ;
        world.SelectedLearningElement = element;
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteLearningElementInWorld(element, world, mappingAction);
        
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
        
        Assert.IsFalse(actionWasInvoked);
    }
}