using BusinessLogic.API;
using BusinessLogic.Commands;
using BusinessLogic.Commands.World;
using BusinessLogic.Entities;
using NSubstitute;
using NUnit.Framework;

namespace BusinessLogicTest.Commands;

[TestFixture]

public class LoadWorldUt
{
    [Test]
    public void Execute_LoadsWorld()
    {
        var authoringToolWorkspace = new AuthoringToolWorkspace(null, new List<World>());
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var world = new World("a", "b", "b", "b", "b", "b");
        const string filepath = "c:\\temp\\test";
        mockBusinessLogic.LoadWorld(filepath).Returns(world);
        bool actionWasInvoked = false;
        Action<AuthoringToolWorkspace> mappingAction = _ => actionWasInvoked = true;

        var command = new LoadWorld(authoringToolWorkspace, filepath, mockBusinessLogic, mappingAction);
        Assert.Multiple(() =>
        {
            Assert.That(authoringToolWorkspace.Worlds, Is.Empty);
            Assert.That(authoringToolWorkspace.SelectedWorld, Is.Null);
            Assert.That(actionWasInvoked, Is.False);
        });
        
        command.Execute();
        
        mockBusinessLogic.Received().LoadWorld(filepath);
        Assert.That(authoringToolWorkspace.Worlds, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(authoringToolWorkspace.Worlds[0], Is.EqualTo(world));
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(authoringToolWorkspace.SelectedWorld, Is.EqualTo(world));
        });
    }
    
    [Test]
    public void Execute_LoadsWorld_WithStream()
    {
        var authoringToolWorkspace = new AuthoringToolWorkspace(null, new List<World>());
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var world = new World("a", "b", "b", "b", "b", "b");
        var stream = Substitute.For<Stream>();
        mockBusinessLogic.LoadWorld(stream).Returns(world);
        bool actionWasInvoked = false;
        Action<AuthoringToolWorkspace> mappingAction = _ => actionWasInvoked = true;

        var command = new LoadWorld(authoringToolWorkspace, stream, mockBusinessLogic, mappingAction);
        Assert.Multiple(() =>
        {
            Assert.That(authoringToolWorkspace.Worlds, Is.Empty);
            Assert.That(authoringToolWorkspace.SelectedWorld, Is.Null);
            Assert.That(actionWasInvoked, Is.False);
        });
        command.Execute();
        
        mockBusinessLogic.Received().LoadWorld(stream);
        Assert.That(authoringToolWorkspace.Worlds, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(authoringToolWorkspace.Worlds[0], Is.EqualTo(world));
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(authoringToolWorkspace.SelectedWorld, Is.EqualTo(world));
        });
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var authoringToolWorkspace = new AuthoringToolWorkspace(null, new List<World>());
        bool actionWasInvoked = false;
        Action<AuthoringToolWorkspace> mappingAction = _ => actionWasInvoked = true;

        var command = new LoadWorld(authoringToolWorkspace,"space", mockBusinessLogic, mappingAction);
        
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.Multiple(() =>
        {
            Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
            Assert.That(actionWasInvoked, Is.False);
        });
    }

    [Test]
    public void UndoRedo_UndoesAndRedoesLoadWorld()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var authoringToolWorkspace = new AuthoringToolWorkspace(null, new List<World>());
        bool actionWasInvoked = false;
        Action<AuthoringToolWorkspace> mappingAction = _ => actionWasInvoked = true;
        var world = new World("a", "b", "c", "d", "e", "f");        
        mockBusinessLogic.LoadWorld(Arg.Any<string>()).Returns(world);
        var world2 = new World("g", "h", "i", "j", "k", "l");
        authoringToolWorkspace.Worlds.Add(world2);
        authoringToolWorkspace.SelectedWorld = world2;
        var command = new LoadWorld(authoringToolWorkspace, "space", mockBusinessLogic, mappingAction);
        
        Assert.That(authoringToolWorkspace.Worlds, Has.Count.EqualTo(1));
        Assert.That(authoringToolWorkspace.SelectedWorld, Is.EqualTo(world2));
        
        command.Execute();
        
        Assert.That(authoringToolWorkspace.Worlds, Has.Count.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(authoringToolWorkspace.Worlds[1], Is.EqualTo(world));
            Assert.That(authoringToolWorkspace.SelectedWorld, Is.EqualTo(world));
            Assert.That(actionWasInvoked, Is.True);
        });
        
        actionWasInvoked = false;
        
        command.Undo();
        
        Assert.Multiple(() =>
        {
            Assert.That(authoringToolWorkspace.Worlds, Has.Count.EqualTo(1));
            Assert.That(authoringToolWorkspace.SelectedWorld, Is.EqualTo(world2));
            Assert.That(actionWasInvoked, Is.True);
        });
        
        actionWasInvoked = false;
        
        command.Redo();
        
        Assert.That(authoringToolWorkspace.Worlds, Has.Count.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(authoringToolWorkspace.Worlds[1], Is.EqualTo(world));
            Assert.That(authoringToolWorkspace.SelectedWorld, Is.EqualTo(world));
            Assert.That(actionWasInvoked, Is.True);
        });
        
        command.Undo();
        
        Assert.Multiple(() =>
        {
            Assert.That(authoringToolWorkspace.Worlds, Has.Count.EqualTo(1));
            Assert.That(authoringToolWorkspace.SelectedWorld, Is.EqualTo(world2));
            Assert.That(actionWasInvoked, Is.True);
        });
        
        actionWasInvoked = false;
        
        command.Redo();
        
        Assert.That(authoringToolWorkspace.Worlds, Has.Count.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(authoringToolWorkspace.Worlds[1], Is.EqualTo(world));
            Assert.That(authoringToolWorkspace.SelectedWorld, Is.EqualTo(world));
            Assert.That(actionWasInvoked, Is.True);
        });
    }
}