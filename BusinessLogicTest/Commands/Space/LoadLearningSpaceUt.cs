using BusinessLogic.API;
using BusinessLogic.Commands.Space;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Commands.Space;

[TestFixture]
public class LoadLearningSpaceUt
{
    [Test]
    public void Execute_LoadsLearningSpace()
    {
        var world = new LearningWorld("a", "b", "b", "b", "b", "b");
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var space = new LearningSpace("a", "d", 5, Theme.CampusAschaffenburg);
        const string filepath = "c:\\temp\\test";
        mockBusinessLogic.LoadLearningSpace(filepath).Returns(space);
        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new LoadLearningSpace(world, filepath, mockBusinessLogic, mappingAction,
            new NullLogger<LoadLearningSpace>());

        Assert.Multiple(() =>
        {
            Assert.That(world.LearningSpaces, Is.Empty);
            Assert.That(actionWasInvoked, Is.False);
        });

        command.Execute();

        mockBusinessLogic.Received().LoadLearningSpace(filepath);
        Assert.That(world.LearningSpaces, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(world.LearningSpaces[0], Is.EqualTo(space));
            Assert.That(actionWasInvoked, Is.True);
        });
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new LoadLearningSpace(world, "space", mockBusinessLogic, mappingAction,
            new NullLogger<LoadLearningSpace>());

        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.Multiple(() =>
        {
            Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
            Assert.That(actionWasInvoked, Is.False);
        });
    }

    [Test]
    public void UndoRedo_UndoesAndRedoesLoadLearningSpace()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;
        var space = new LearningSpace("a", "d", 5, Theme.CampusAschaffenburg);
        var space2 = new LearningSpace("f", "i", 6, Theme.CampusAschaffenburg);
        world.LearningSpaces.Add(space2);
        mockBusinessLogic.LoadLearningSpace(Arg.Any<string>()).Returns(space);
        var command = new LoadLearningSpace(world, "space", mockBusinessLogic, mappingAction,
            new NullLogger<LoadLearningSpace>());

        Assert.That(world.LearningSpaces, Has.Count.EqualTo(1));

        command.Execute();

        Assert.That(world.LearningSpaces, Has.Count.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(world.LearningSpaces[1], Is.EqualTo(space));
            Assert.That(actionWasInvoked, Is.True);
        });
        actionWasInvoked = false;

        command.Undo();

        Assert.Multiple(() =>
        {
            Assert.That(world.LearningSpaces, Has.Count.EqualTo(1));
            Assert.That(actionWasInvoked, Is.True);
        });
        actionWasInvoked = false;

        command.Redo();

        Assert.That(world.LearningSpaces, Has.Count.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(world.LearningSpaces[1], Is.EqualTo(space));
            Assert.That(actionWasInvoked, Is.True);
        });
        actionWasInvoked = false;

        command.Undo();

        Assert.Multiple(() =>
        {
            Assert.That(world.LearningSpaces, Has.Count.EqualTo(1));
            Assert.That(actionWasInvoked, Is.True);
        });
        actionWasInvoked = false;

        command.Redo();

        Assert.That(world.LearningSpaces, Has.Count.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(world.LearningSpaces[1], Is.EqualTo(space));
            Assert.That(actionWasInvoked, Is.True);
        });
        actionWasInvoked = false;
    }
}