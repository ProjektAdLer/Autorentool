using BusinessLogic.API;
using BusinessLogic.Commands;
using BusinessLogic.Commands.Space;
using BusinessLogic.Entities;
using NSubstitute;
using NUnit.Framework;

namespace BusinessLogicTest.Commands;

[TestFixture]

public class LoadLearningSpaceUt
{
    [Test]
    public void Execute_LoadsLearningSpace()
    {
        var world = new LearningWorld("a", "b", "b", "b", "b", "b");
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var space = new LearningSpace("a", "b", "c", "d", "e", 5);
        const string filepath = "c:\\temp\\test";
        mockBusinessLogic.LoadLearningSpace(filepath).Returns(space);
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new LoadLearningSpace(world, filepath, mockBusinessLogic, mappingAction);
        
        Assert.That(world.LearningSpaces, Is.Empty);
        Assert.That(world.SelectedLearningObjectInPathWay, Is.Null);
        Assert.IsFalse(actionWasInvoked);
        
        command.Execute();
        
        mockBusinessLogic.Received().LoadLearningSpace(filepath);
        Assert.That(world.LearningSpaces, Has.Count.EqualTo(1));
        Assert.That(world.LearningSpaces[0], Is.EqualTo(space));
        Assert.That(world.SelectedLearningObjectInPathWay, Is.EqualTo(space));
        Assert.IsTrue(actionWasInvoked);
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new LoadLearningSpace(world,"space", mockBusinessLogic, mappingAction);
        
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
        
        Assert.IsFalse(actionWasInvoked);
    }
    
    [Test]
    public void UndoRedo_UndoesAndRedoesLoadLearningSpace()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;
        var space = new LearningSpace("a", "b", "c", "d", "e", 5);
        var space2 = new LearningSpace("f", "g", "h", "i", "j", 6);
        world.LearningSpaces.Add(space2);
        world.SelectedLearningObjectInPathWay = space2;
        mockBusinessLogic.LoadLearningSpace(Arg.Any<string>()).Returns(space);
        var command = new LoadLearningSpace(world, "space", mockBusinessLogic, mappingAction);
        
        Assert.That(world.LearningSpaces, Has.Count.EqualTo(1));
        Assert.That(world.SelectedLearningObjectInPathWay, Is.EqualTo(space2));
        
        command.Execute();
        
        Assert.That(world.LearningSpaces, Has.Count.EqualTo(2));
        Assert.That(world.LearningSpaces[1], Is.EqualTo(space));
        Assert.That(world.SelectedLearningObjectInPathWay, Is.EqualTo(space));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Undo();
        
        Assert.That(world.LearningSpaces, Has.Count.EqualTo(1));
        Assert.That(world.SelectedLearningObjectInPathWay, Is.EqualTo(space2));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Redo();
        
        Assert.That(world.LearningSpaces, Has.Count.EqualTo(2));
        Assert.That(world.LearningSpaces[1], Is.EqualTo(space));
        Assert.That(world.SelectedLearningObjectInPathWay, Is.EqualTo(space));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Undo();
        
        Assert.That(world.LearningSpaces, Has.Count.EqualTo(1));
        Assert.That(world.SelectedLearningObjectInPathWay, Is.EqualTo(space2));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Redo();
        
        Assert.That(world.LearningSpaces, Has.Count.EqualTo(2));
        Assert.That(world.LearningSpaces[1], Is.EqualTo(space));
        Assert.That(world.SelectedLearningObjectInPathWay, Is.EqualTo(space));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
    }
}