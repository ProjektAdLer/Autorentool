using BusinessLogic.Commands;
using BusinessLogic.Commands.World;
using BusinessLogic.Entities;
using NUnit.Framework;

namespace BusinessLogicTest.Commands;

[TestFixture]
public class CreateWorldUt
{
    [Test]
    public void Execute_CreatesWorld()
    {
        var workspace = new AuthoringToolWorkspace(null, new List<World>());
        var name = "n";
        var shortname = "sn";
        var authors = "a";
        var language = "l";
        var description = "d";
        var goals = "g";
        bool actionWasInvoked = false;
        Action<AuthoringToolWorkspace> mappingAction = _ => actionWasInvoked = true;

        var command = new CreateWorld(workspace, name, shortname, authors, language, description, goals,
            mappingAction);

        Assert.IsEmpty(workspace.Worlds);
        Assert.IsFalse(actionWasInvoked);
        Assert.That(workspace.SelectedWorld, Is.Null);

        command.Execute();

        Assert.That(workspace.Worlds, Has.Count.EqualTo(1));
        Assert.IsTrue(actionWasInvoked);
        var world = workspace.Worlds.First();
        Assert.Multiple(() =>
        {
            Assert.That(world.Name, Is.EqualTo("n"));
            Assert.That(world.Shortname, Is.EqualTo("sn"));
            Assert.That(world.Authors, Is.EqualTo("a"));
            Assert.That(world.Language, Is.EqualTo("l"));
            Assert.That(world.Description, Is.EqualTo("d"));
            Assert.That(world.Goals, Is.EqualTo("g"));
        });
        Assert.That(workspace.SelectedWorld, Is.SameAs(world));
    }

    [Test]
    public void Execute_WithDuplicateName_CreatesWorldWithNewUniqueName()
    {
        var world1 = new World("Foo", "", "", "", "", "");
        var world2 = new World("Foo(1)", "", "", "", "", "");
        var workspace = new AuthoringToolWorkspace(null, new List<World> { world1, world2 });

        var systemUnderTest = new CreateWorld(workspace, "Foo", "", "", "", "", "", _ => { });
        
        systemUnderTest.Execute();
        Assert.That(workspace.Worlds.Last().Name, Is.EqualTo("Foo(2)"));
    }
    
    [Test]
    public void Execute_AddsWorld()
    {
        var workspace = new AuthoringToolWorkspace(null, new List<World>());
        var world = new World("n", "sn", "a", "l", "d", "g");
        bool actionWasInvoked = false;
        Action<AuthoringToolWorkspace> mappingAction = _ => actionWasInvoked = true;

        var command = new CreateWorld(workspace, world, mappingAction);

        Assert.IsEmpty(workspace.Worlds);
        Assert.IsFalse(actionWasInvoked);

        command.Execute();

        Assert.That(workspace.Worlds, Has.Count.EqualTo(1));
        Assert.IsTrue(actionWasInvoked);
        Assert.That(workspace.Worlds.First(), Is.EqualTo(world));
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var workspace = new AuthoringToolWorkspace(null, new List<World>());
        var name = "n";
        var shortname = "sn";
        var authors = "a";
        var language = "l";
        var description = "d";
        var goals = "g";
        bool actionWasInvoked = false;
        Action<AuthoringToolWorkspace> mappingAction = _ => actionWasInvoked = true;

        var command = new CreateWorld(workspace, name, shortname, authors, language, description, goals,
            mappingAction);

        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
        
        Assert.IsFalse(actionWasInvoked);
    }

    [Test]
    public void UndoRedo_UndoesAndRedoesCreateSpace()
    {
        var workspace = new AuthoringToolWorkspace(null, new List<World>());
        var world = new World("a", "b", "c", "d", "e", "f");
        workspace.Worlds.Add(world);
        workspace.SelectedWorld = world;
        var name = "n";
        var shortname = "sn";
        var authors = "a";
        var language = "l";
        var description = "d";
        var goals = "g";
        bool actionWasInvoked = false;
        Action<AuthoringToolWorkspace> mappingAction = _ => actionWasInvoked = true;

        var command = new CreateWorld(workspace, name, shortname, authors, language, description, goals,
            mappingAction);

        Assert.That(workspace.Worlds, Has.Count.EqualTo(1));
        Assert.That(workspace.SelectedWorld, Is.EqualTo(world));
        Assert.IsFalse(actionWasInvoked);

        command.Execute();

        Assert.That(workspace.Worlds, Has.Count.EqualTo(2));
        Assert.That(workspace.SelectedWorld, Is.EqualTo(command.World));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;

        command.Undo();

        Assert.That(workspace.Worlds, Has.Count.EqualTo(1));
        Assert.That(workspace.SelectedWorld, Is.EqualTo(world));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;

        command.Redo();

        Assert.That(workspace.Worlds, Has.Count.EqualTo(2));
        Assert.That(workspace.SelectedWorld, Is.EqualTo(command.World));
        Assert.IsTrue(actionWasInvoked); 
    }
}