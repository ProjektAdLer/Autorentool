using BusinessLogic.Commands.World;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace BusinessLogicTest.Commands.World;

[TestFixture]
public class DeleteLearningWorldUt
{
    [Test]
    public void Execute_DeletesLearningWorld()
    {
        var workspace = new AuthoringToolWorkspace(new List<ILearningWorld>());
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        workspace.LearningWorlds.Add(world);
        var actionWasInvoked = false;
        Action<AuthoringToolWorkspace> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteLearningWorld(workspace, world, mappingAction, new NullLogger<DeleteLearningWorld>());

        Assert.Multiple(() =>
        {
            Assert.That(workspace.LearningWorlds, Does.Contain(world));
            Assert.That(actionWasInvoked, Is.False);
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(workspace.LearningWorlds, Is.Empty);
            Assert.That(actionWasInvoked, Is.True);
        });
    }

    [Test]
    public void Execute_DeletesLearningWorldAndSetsAnotherLearningWorldAsSelected()
    {
        var workspace = new AuthoringToolWorkspace(new List<ILearningWorld>());
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var world2 = new LearningWorld("g", "h", "i", "j", "k", "l");
        workspace.LearningWorlds.Add(world);
        workspace.LearningWorlds.Add(world2);
        var actionWasInvoked = false;
        Action<AuthoringToolWorkspace> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteLearningWorld(workspace, world, mappingAction, new NullLogger<DeleteLearningWorld>());

        Assert.That(workspace.LearningWorlds, Has.Count.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(workspace.LearningWorlds, Does.Contain(world));
            Assert.That(actionWasInvoked, Is.False);
        });

        command.Execute();


        Assert.That(workspace.LearningWorlds, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(workspace.LearningWorlds, Does.Not.Contain(world));
            Assert.That(actionWasInvoked, Is.True);
        });
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var workspace = new AuthoringToolWorkspace(new List<ILearningWorld>());
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        workspace.LearningWorlds.Add(world);
        var actionWasInvoked = false;
        Action<AuthoringToolWorkspace> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteLearningWorld(workspace, world, mappingAction, new NullLogger<DeleteLearningWorld>());

        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.Multiple(() =>
        {
            Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
            Assert.That(actionWasInvoked, Is.False);
        });
    }

    [Test]
    public void UndoRedo_UndoesAndRedoesCreateLearningWorld()
    {
        var workspace = new AuthoringToolWorkspace(new List<ILearningWorld>());
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var world2 = new LearningWorld("g", "h", "i", "j", "k", "l");
        workspace.LearningWorlds.Add(world);
        workspace.LearningWorlds.Add(world2);
        var actionWasInvoked = false;
        Action<AuthoringToolWorkspace> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteLearningWorld(workspace, world, mappingAction, new NullLogger<DeleteLearningWorld>());

        Assert.Multiple(() =>
        {
            Assert.That(workspace.LearningWorlds, Has.Count.EqualTo(2));
            Assert.That(actionWasInvoked, Is.False);
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(workspace.LearningWorlds, Has.Count.EqualTo(1));
            Assert.That(actionWasInvoked, Is.True);
        });
        actionWasInvoked = false;

        command.Undo();

        Assert.Multiple(() =>
        {
            Assert.That(workspace.LearningWorlds, Has.Count.EqualTo(2));
            Assert.That(actionWasInvoked, Is.True);
        });
        actionWasInvoked = false;

        command.Redo();

        Assert.Multiple(() =>
        {
            Assert.That(workspace.LearningWorlds, Has.Count.EqualTo(1));
            Assert.That(actionWasInvoked, Is.True);
        });
    }
}