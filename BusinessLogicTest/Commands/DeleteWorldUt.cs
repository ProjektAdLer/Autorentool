using BusinessLogic.Commands;
using BusinessLogic.Commands.World;
using BusinessLogic.Entities;
using NUnit.Framework;

namespace BusinessLogicTest.Commands;

[TestFixture]
public class DeleteWorldUt
{
    [Test]
    public void Execute_DeletesWorld()
    {
        var workspace = new AuthoringToolWorkspace(null, new List<World>());
        var world = new World("a", "b", "c", "d", "e", "f");
        workspace.Worlds.Add(world);
        workspace.SelectedWorld = world;
        bool actionWasInvoked = false;
        Action<AuthoringToolWorkspace> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteWorld(workspace, world, mappingAction);

        Assert.That(workspace.Worlds, Does.Contain(world));
        Assert.IsFalse(actionWasInvoked);
        Assert.That(workspace.SelectedWorld, Is.EqualTo(world));

        command.Execute();

        Assert.That(workspace.Worlds, Is.Empty);
        Assert.IsTrue(actionWasInvoked);
        Assert.That(workspace.SelectedWorld, Is.Null);
    }
    
    [Test]
    public void Execute_DeletesWorldAndSetsAnotherWorldAsSelected()
    {
        var workspace = new AuthoringToolWorkspace(null, new List<World>());
        var world = new World("a", "b", "c", "d", "e", "f");
        var world2 = new World("g", "h", "i", "j", "k", "l");
        workspace.Worlds.Add(world);
        workspace.Worlds.Add(world2);
        workspace.SelectedWorld = world;
        bool actionWasInvoked = false;
        Action<AuthoringToolWorkspace> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteWorld(workspace, world, mappingAction);

        Assert.That(workspace.Worlds.Count, Is.EqualTo(2));
        Assert.That(workspace.Worlds, Does.Contain(world));
        Assert.IsFalse(actionWasInvoked);
        Assert.That(workspace.SelectedWorld, Is.EqualTo(world));

        command.Execute();

        
        Assert.That(workspace.Worlds.Count, Is.EqualTo(1));
        Assert.That(workspace.Worlds, Does.Not.Contain(world));
        Assert.IsTrue(actionWasInvoked);
        Assert.That(workspace.SelectedWorld, Is.EqualTo(world2));
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var workspace = new AuthoringToolWorkspace(null, new List<World>());
        var world = new World("a", "b", "c", "d", "e", "f");
        workspace.Worlds.Add(world);
        bool actionWasInvoked = false;
        Action<AuthoringToolWorkspace> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteWorld(workspace, world, mappingAction);

        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
        
        Assert.IsFalse(actionWasInvoked);
    }

    [Test]
    public void UndoRedo_UndoesAndRedoesCreateWorld()
    {
        var workspace = new AuthoringToolWorkspace(null, new List<World>());
        var world = new World("a", "b", "c", "d", "e", "f");
        var world2 = new World("g", "h", "i", "j", "k", "l");
        workspace.Worlds.Add(world);
        workspace.Worlds.Add(world2);
        workspace.SelectedWorld = world;
        bool actionWasInvoked = false;
        Action<AuthoringToolWorkspace> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteWorld(workspace, world, mappingAction);

        Assert.That(workspace.Worlds, Has.Count.EqualTo(2));
        Assert.That(workspace.SelectedWorld, Is.EqualTo(world));
        Assert.IsFalse(actionWasInvoked);

        command.Execute();

        Assert.That(workspace.Worlds, Has.Count.EqualTo(1));
        Assert.That(workspace.SelectedWorld, Is.EqualTo(world2));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;

        command.Undo();

        Assert.That(workspace.Worlds, Has.Count.EqualTo(2));
        Assert.That(workspace.SelectedWorld, Is.EqualTo(world));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;

        command.Redo();

        Assert.That(workspace.Worlds, Has.Count.EqualTo(1));
        Assert.That(workspace.SelectedWorld, Is.EqualTo(world2));
        Assert.IsTrue(actionWasInvoked);
    }
}