using BusinessLogic.Commands;
using BusinessLogic.Entities;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Commands;

[TestFixture]

public class DeleteLearningElementUt
{
    [Test]
    public void Execute_DeletesLearningElement()
    {
        var space = new LearningSpace("a", "b", "c","d", "e", 5);
        var element = new LearningElement("g", "h", null!, "url","i", "j", "k", LearningElementDifficultyEnum.Easy, space);
        space.LearningSpaceLayout.LearningElements = new ILearningElement?[] {element};
        space.SelectedLearningElement = element;
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteLearningElement(element, space, mappingAction);
        
        Assert.That(space.ContainedLearningElements, Does.Contain(element));
        Assert.IsFalse(actionWasInvoked);
        
        command.Execute();
        
        Assert.That(space.ContainedLearningElements, Is.Empty);
        Assert.IsTrue(actionWasInvoked);
        Assert.That(space.SelectedLearningElement, Is.Null);
    }

    [Test]
    public void Execute_DeletesLearningElementAndSetsAnotherElementSelectedLearningElement()
    {
        var space = new LearningSpace("a", "b", "c","d", "e", 5);
        var element = new LearningElement("g", "h", null!, "url","i", "j", "k", LearningElementDifficultyEnum.Easy, space);
        var element2 = new LearningElement("l", "m", null!, "url","n", "o", "p", LearningElementDifficultyEnum.Easy, space);
        space.LearningSpaceLayout.LearningElements = new ILearningElement?[] {element, element2};
        space.SelectedLearningElement = element;
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteLearningElement(element, space, mappingAction);
        
        Assert.That(space.ContainedLearningElements, Does.Contain(element));
        Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(2));
        Assert.IsFalse(actionWasInvoked);
        
        command.Execute();
        
        Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(1));
        Assert.That(space.ContainedLearningElements, Does.Not.Contain(element));
        Assert.IsTrue(actionWasInvoked);
        Assert.That(space.SelectedLearningElement, Is.EqualTo(element2));
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var space = new LearningSpace("a", "b", "c","d", "e", 5);
        var element = new LearningElement("g", "h", null!, "url","i", "j", "k", LearningElementDifficultyEnum.Easy, space);
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteLearningElement(element, space, mappingAction);
        
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
        
        Assert.IsFalse(actionWasInvoked);
    }

    [Test]
    public void UndoRedo_UndoesRedoesDeleteLearningElement()
    {
        var space = new LearningSpace("a", "b", "c","d", "e", 5);
        var element = new LearningElement("g", "h", null!, "url","i", "j", "k", LearningElementDifficultyEnum.Easy, space);
        var element2 = new LearningElement("l", "m", null!, "url","n", "o", "p", LearningElementDifficultyEnum.Easy, space);
        space.LearningSpaceLayout.LearningElements = new ILearningElement?[] {element, element2};
        space.SelectedLearningElement = element;
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteLearningElement(element, space, mappingAction);
        
        Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(2));
        Assert.That(space.ContainedLearningElements, Does.Contain(element));
        Assert.That(space.SelectedLearningElement, Is.EqualTo(element));
        Assert.IsFalse(actionWasInvoked);
        
        command.Execute();
        
        Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(1));
        Assert.That(space.SelectedLearningElement, Is.EqualTo(element2));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Undo();
        
        Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(2));
        Assert.That(space.ContainedLearningElements, Does.Contain(element));
        Assert.That(space.SelectedLearningElement, Is.EqualTo(element));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Redo();
        
        Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(1));
        Assert.That(space.SelectedLearningElement, Is.EqualTo(element2));
        Assert.IsTrue(actionWasInvoked);
    }
}