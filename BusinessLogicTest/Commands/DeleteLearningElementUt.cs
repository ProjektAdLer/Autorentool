using BusinessLogic.Commands;
using BusinessLogic.Entities;
using NSubstitute;
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
        var mappingAction = Substitute.For<Action<ILearningElementParent>>();

        var command = new DeleteLearningElement(element, world, mappingAction);
        
        Assert.That(world.LearningElements, Does.Contain(element));
        
        command.Execute();
        
        Assert.That(world.LearningElements, Is.Empty);
    }

    [Test]
    public void Execute_DeletesLearningElement_SpaceParent()
    {
        var space = new LearningSpace("a", "b", "c","d", "e");
        var element = new LearningElement("g", "h", null!, "i", "j", "k", LearningElementDifficultyEnum.Easy, space);
        space.LearningElements.Add(element);
        var mappingAction = Substitute.For<Action<ILearningElementParent>>();

        var command = new DeleteLearningElement(element, space, mappingAction);
        
        Assert.That(space.LearningElements, Does.Contain(element));
        
        command.Execute();
        
        Assert.That(space.LearningElements, Is.Empty);
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var element = new LearningElement("g", "h", null!, "i", "j", "k", LearningElementDifficultyEnum.Easy, world);
        var mappingAction = Substitute.For<Action<ILearningElementParent>>();

        var command = new DeleteLearningElement(element, world, mappingAction);
        
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
    }
    
    [Test]
    public void UndoRedo_UndoesRedoesDeleteLearningElement_WorldParent()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var element = new LearningElement("g", "h", null!, "i", "j", "k", LearningElementDifficultyEnum.Easy, world);
        world.LearningElements.Add(element);
        var mappingAction = Substitute.For<Action<ILearningElementParent>>();

        var command = new DeleteLearningElement(element, world, mappingAction);
        
        Assert.That(world.LearningElements, Does.Contain(element));
        
        command.Execute();
        
        Assert.That(world.LearningElements, Is.Empty);
        
        command.Undo();
        
        Assert.That(world.LearningElements, Does.Contain(element));
        
        command.Redo();
        
        Assert.That(world.LearningElements, Is.Empty);
    }

    [Test]
    public void UndoRedo_UndoesRedoesDeleteLearningElement_SpaceParent()
    {
        var space = new LearningSpace("a", "b", "c","d", "e");
        var element = new LearningElement("g", "h", null!, "i", "j", "k", LearningElementDifficultyEnum.Easy, space);
        space.LearningElements.Add(element);
        var mappingAction = Substitute.For<Action<ILearningElementParent>>();

        var command = new DeleteLearningElement(element, space, mappingAction);
        
        Assert.That(space.LearningElements, Does.Contain(element));
        
        command.Execute();
        
        Assert.That(space.LearningElements, Is.Empty);
        
        command.Undo();
        
        Assert.That(space.LearningElements, Does.Contain(element));
        
        command.Redo();
        
        Assert.That(space.LearningElements, Is.Empty);
    }
}