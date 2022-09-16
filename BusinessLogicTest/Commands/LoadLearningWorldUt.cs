using BusinessLogic.API;
using BusinessLogic.Commands;
using BusinessLogic.Entities;
using NSubstitute;
using NUnit.Framework;

namespace BusinessLogicTest.Commands;

[TestFixture]

public class LoadLearningWorldUt
{
    [Test]
    public void Execute_LoadsLearningWorld()
    {
        var authoringToolWorkspace = new AuthoringToolWorkspace(null, new List<LearningWorld>());
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var world = new LearningWorld("a", "b", "b", "b", "b", "b");
        const string filepath = "c:\\temp\\test";
        mockBusinessLogic.LoadLearningWorld(filepath).Returns(world);
        bool actionWasInvoked = false;
        Action<AuthoringToolWorkspace> mappingAction = _ => actionWasInvoked = true;

        var command = new LoadLearningWorld(authoringToolWorkspace, filepath, mockBusinessLogic, mappingAction);
        Assert.Multiple(() =>
        {
            Assert.That(authoringToolWorkspace.LearningWorlds, Is.Empty);
            Assert.That(actionWasInvoked, Is.False);
        });
        command.Execute();
        
        mockBusinessLogic.Received().LoadLearningWorld(filepath);
        Assert.That(authoringToolWorkspace.LearningWorlds, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(authoringToolWorkspace.LearningWorlds[0], Is.EqualTo(world));
            Assert.That(actionWasInvoked, Is.True);
        });
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var authoringToolWorkspace = new AuthoringToolWorkspace(null, new List<LearningWorld>());
        bool actionWasInvoked = false;
        Action<AuthoringToolWorkspace> mappingAction = _ => actionWasInvoked = true;

        var command = new LoadLearningWorld(authoringToolWorkspace,"space", mockBusinessLogic, mappingAction);
        
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.Multiple(() =>
        {
            Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
            Assert.That(actionWasInvoked, Is.False);
        });
    }

    [Test]
    public void UndoRedo_UndoesAndRedoesLoadLearningWorld()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var authoringToolWorkspace = new AuthoringToolWorkspace(null, new List<LearningWorld>());
        bool actionWasInvoked = false;
        Action<AuthoringToolWorkspace> mappingAction = _ => actionWasInvoked = true;
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");        
        mockBusinessLogic.LoadLearningWorld(Arg.Any<string>()).Returns(world);
        var command = new LoadLearningWorld(authoringToolWorkspace, "space", mockBusinessLogic, mappingAction);
        
        Assert.That(world.LearningSpaces, Is.Empty);
        
        command.Execute();
        
        Assert.That(authoringToolWorkspace.LearningWorlds, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(authoringToolWorkspace.LearningWorlds[0], Is.EqualTo(world));
            Assert.That(actionWasInvoked, Is.True);
        });
        
        actionWasInvoked = false;
        
        command.Undo();
        
        Assert.Multiple(() =>
        {
            Assert.That(authoringToolWorkspace.LearningWorlds, Is.Empty);
            Assert.That(actionWasInvoked, Is.True);
        });
        
        actionWasInvoked = false;
        
        command.Redo();
        
        Assert.That(authoringToolWorkspace.LearningWorlds, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(authoringToolWorkspace.LearningWorlds[0], Is.EqualTo(world));
            Assert.That(actionWasInvoked, Is.True);
        });
        
        command.Undo();
        
        Assert.Multiple(() =>
        {
            Assert.That(authoringToolWorkspace.LearningWorlds, Is.Empty);
            Assert.That(actionWasInvoked, Is.True);
        });
        
        actionWasInvoked = false;
        
        command.Redo();
        
        Assert.That(authoringToolWorkspace.LearningWorlds, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(authoringToolWorkspace.LearningWorlds[0], Is.EqualTo(world));
            Assert.That(actionWasInvoked, Is.True);
        });
    }
}