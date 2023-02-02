using BusinessLogic.Commands;
using BusinessLogic.Commands.Element;
using BusinessLogic.Entities;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Commands;

[TestFixture]

public class DeleteElementUt
{
    [Test]
    public void Execute_DeletesElement()
    {
        var space = new Space("a", "b", "c","d", "e", 5);
        var element = new Element("g", "h", null!, "url","i", "j", "k", ElementDifficultyEnum.Easy, space);
        space.SpaceLayout.Elements = new IElement?[] {element};
        space.SelectedElement = element;
        bool actionWasInvoked = false;
        Action<Space> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteElement(element, space, mappingAction);
        
        Assert.That(space.ContainedElements, Does.Contain(element));
        Assert.IsFalse(actionWasInvoked);
        
        command.Execute();
        
        Assert.That(space.ContainedElements, Is.Empty);
        Assert.IsTrue(actionWasInvoked);
        Assert.That(space.SelectedElement, Is.Null);
    }

    [Test]
    public void Execute_DeletesElementAndSetsAnotherElementSelectedElement()
    {
        var space = new Space("a", "b", "c","d", "e", 5);
        var element = new Element("g", "h", null!, "url","i", "j", "k", ElementDifficultyEnum.Easy, space);
        var element2 = new Element("l", "m", null!, "url","n", "o", "p", ElementDifficultyEnum.Easy, space);
        space.SpaceLayout.Elements = new IElement?[] {element, element2};
        space.SelectedElement = element;
        bool actionWasInvoked = false;
        Action<Space> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteElement(element, space, mappingAction);
        
        Assert.That(space.ContainedElements, Does.Contain(element));
        Assert.That(space.ContainedElements.Count(), Is.EqualTo(2));
        Assert.IsFalse(actionWasInvoked);
        
        command.Execute();
        
        Assert.That(space.ContainedElements.Count(), Is.EqualTo(1));
        Assert.That(space.ContainedElements, Does.Not.Contain(element));
        Assert.IsTrue(actionWasInvoked);
        Assert.That(space.SelectedElement, Is.EqualTo(element2));
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var space = new Space("a", "b", "c","d", "e", 5);
        var element = new Element("g", "h", null!, "url","i", "j", "k", ElementDifficultyEnum.Easy, space);
        bool actionWasInvoked = false;
        Action<Space> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteElement(element, space, mappingAction);
        
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
        
        Assert.IsFalse(actionWasInvoked);
    }

    [Test]
    public void UndoRedo_UndoesRedoesDeleteElement()
    {
        var space = new Space("a", "b", "c","d", "e", 5);
        var element = new Element("g", "h", null!, "url","i", "j", "k", ElementDifficultyEnum.Easy, space);
        var element2 = new Element("l", "m", null!, "url","n", "o", "p", ElementDifficultyEnum.Easy, space);
        space.SpaceLayout.Elements = new IElement?[] {element, element2};
        space.SelectedElement = element;
        bool actionWasInvoked = false;
        Action<Space> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteElement(element, space, mappingAction);
        
        Assert.That(space.ContainedElements.Count(), Is.EqualTo(2));
        Assert.That(space.ContainedElements, Does.Contain(element));
        Assert.That(space.SelectedElement, Is.EqualTo(element));
        Assert.IsFalse(actionWasInvoked);
        
        command.Execute();
        
        Assert.That(space.ContainedElements.Count(), Is.EqualTo(1));
        Assert.That(space.SelectedElement, Is.EqualTo(element2));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Undo();
        
        Assert.That(space.ContainedElements.Count(), Is.EqualTo(2));
        Assert.That(space.ContainedElements, Does.Contain(element));
        Assert.That(space.SelectedElement, Is.EqualTo(element));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Redo();
        
        Assert.That(space.ContainedElements.Count(), Is.EqualTo(1));
        Assert.That(space.SelectedElement, Is.EqualTo(element2));
        Assert.IsTrue(actionWasInvoked);
    }
}