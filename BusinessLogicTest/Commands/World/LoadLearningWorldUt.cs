using BusinessLogic.API;
using BusinessLogic.Commands.World;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NUnit.Framework;

namespace BusinessLogicTest.Commands.World;

[TestFixture]
public class LoadLearningWorldUt
{
    [Test]
    // ANF-ID: [ASE2]
    public void Execute_LoadsLearningWorld()
    {
        var authoringToolWorkspace = new AuthoringToolWorkspace(new List<ILearningWorld>());
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var world = new LearningWorld("a", "b", "b", "b", "b", "b");
        const string filepath = "c:\\temp\\test";
        mockBusinessLogic.LoadLearningWorld(filepath).Returns(world);
        var actionWasInvoked = false;
        Action<AuthoringToolWorkspace> mappingAction = _ => actionWasInvoked = true;

        var command = new LoadLearningWorld(authoringToolWorkspace, filepath, mockBusinessLogic, mappingAction,
            new NullLogger<LoadLearningWorld>());
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
    // ANF-ID: [ASE2]
    public void Execute_LoadsLearningWorld_WithStream()
    {
        var authoringToolWorkspace = new AuthoringToolWorkspace(new List<ILearningWorld>());
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var world = new LearningWorld("a", "b", "b", "b", "b", "b");
        var stream = Substitute.For<Stream>();
        mockBusinessLogic.LoadLearningWorld(stream).Returns(world);
        var actionWasInvoked = false;
        Action<AuthoringToolWorkspace> mappingAction = _ => actionWasInvoked = true;

        var command = new LoadLearningWorld(authoringToolWorkspace, stream, mockBusinessLogic, mappingAction,
            new NullLogger<LoadLearningWorld>());
        Assert.Multiple(() =>
        {
            Assert.That(authoringToolWorkspace.LearningWorlds, Is.Empty);
            Assert.That(actionWasInvoked, Is.False);
        });
        command.Execute();

        mockBusinessLogic.Received().LoadLearningWorld(stream);
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
        var authoringToolWorkspace = new AuthoringToolWorkspace(new List<ILearningWorld>());
        var actionWasInvoked = false;
        Action<AuthoringToolWorkspace> mappingAction = _ => actionWasInvoked = true;

        var command = new LoadLearningWorld(authoringToolWorkspace, "space", mockBusinessLogic, mappingAction,
            new NullLogger<LoadLearningWorld>());

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
        var authoringToolWorkspace = new AuthoringToolWorkspace(new List<ILearningWorld>());
        var actionWasInvoked = false;
        Action<AuthoringToolWorkspace> mappingAction = _ => actionWasInvoked = true;
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        mockBusinessLogic.LoadLearningWorld(Arg.Any<string>()).Returns(world);
        var world2 = new LearningWorld("g", "h", "i", "j", "k", "l");
        authoringToolWorkspace.LearningWorlds.Add(world2);
        var command = new LoadLearningWorld(authoringToolWorkspace, "space", mockBusinessLogic, mappingAction,
            new NullLogger<LoadLearningWorld>());

        Assert.That(authoringToolWorkspace.LearningWorlds, Has.Count.EqualTo(1));

        command.Execute();

        Assert.That(authoringToolWorkspace.LearningWorlds, Has.Count.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(authoringToolWorkspace.LearningWorlds[1], Is.EqualTo(world));
            Assert.That(actionWasInvoked, Is.True);
        });

        actionWasInvoked = false;

        command.Undo();

        Assert.Multiple(() =>
        {
            Assert.That(authoringToolWorkspace.LearningWorlds, Has.Count.EqualTo(1));
            Assert.That(actionWasInvoked, Is.True);
        });

        actionWasInvoked = false;

        command.Redo();

        Assert.That(authoringToolWorkspace.LearningWorlds, Has.Count.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(authoringToolWorkspace.LearningWorlds[1], Is.EqualTo(world));
            Assert.That(actionWasInvoked, Is.True);
        });

        command.Undo();

        Assert.Multiple(() =>
        {
            Assert.That(authoringToolWorkspace.LearningWorlds, Has.Count.EqualTo(1));
            Assert.That(actionWasInvoked, Is.True);
        });

        actionWasInvoked = false;

        command.Redo();

        Assert.That(authoringToolWorkspace.LearningWorlds, Has.Count.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(authoringToolWorkspace.LearningWorlds[1], Is.EqualTo(world));
            Assert.That(actionWasInvoked, Is.True);
        });
    }
}