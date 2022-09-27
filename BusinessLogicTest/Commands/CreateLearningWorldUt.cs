using BusinessLogic.Commands;
using BusinessLogic.Entities;
using NUnit.Framework;

namespace BusinessLogicTest.Commands;

[TestFixture]
public class CreateLearningWorldUt
{
    [Test]
    public void Execute_CreatesLearningWorld()
    {
        var workspace = new AuthoringToolWorkspace(null, new List<LearningWorld>());
        var name = "n";
        var shortname = "sn";
        var authors = "a";
        var language = "l";
        var description = "d";
        var goals = "g";
        bool actionWasInvoked = false;
        Action<AuthoringToolWorkspace> mappingAction = _ => actionWasInvoked = true;

        var command = new CreateLearningWorld(workspace, name, shortname, authors, language, description, goals,
            mappingAction);

        Assert.IsEmpty(workspace.LearningWorlds);
        Assert.IsFalse(actionWasInvoked);
        Assert.That(workspace.SelectedLearningWorld, Is.Null);

        command.Execute();

        Assert.That(workspace.LearningWorlds, Has.Count.EqualTo(1));
        Assert.IsTrue(actionWasInvoked);
        var world = workspace.LearningWorlds.First();
        Assert.Multiple(() =>
        {
            Assert.That(world.Name, Is.EqualTo("n"));
            Assert.That(world.Shortname, Is.EqualTo("sn"));
            Assert.That(world.Authors, Is.EqualTo("a"));
            Assert.That(world.Language, Is.EqualTo("l"));
            Assert.That(world.Description, Is.EqualTo("d"));
            Assert.That(world.Goals, Is.EqualTo("g"));
        });
        Assert.That(workspace.SelectedLearningWorld, Is.SameAs(world));
    }

    [Test]
    public void Execute_WithDuplicateName_CreatesWorldWithNewUniqueName()
    {
        var world1 = new LearningWorld("Foo", "", "", "", "", "");
        var world2 = new LearningWorld("Foo(1)", "", "", "", "", "");
        var workspace = new AuthoringToolWorkspace(null, new List<LearningWorld> { world1, world2 });

        var systemUnderTest = new CreateLearningWorld(workspace, "Foo", "", "", "", "", "", _ => { });
        
        systemUnderTest.Execute();
        Assert.That(workspace.LearningWorlds.Last().Name, Is.EqualTo("Foo(2)"));
    }
    
    [Test]
    public void Execute_AddsLearningWorld()
    {
        var workspace = new AuthoringToolWorkspace(null, new List<LearningWorld>());
        var world = new LearningWorld("n", "sn", "a", "l", "d", "g");
        bool actionWasInvoked = false;
        Action<AuthoringToolWorkspace> mappingAction = _ => actionWasInvoked = true;

        var command = new CreateLearningWorld(workspace, world, mappingAction);

        Assert.IsEmpty(workspace.LearningWorlds);
        Assert.IsFalse(actionWasInvoked);

        command.Execute();

        Assert.That(workspace.LearningWorlds, Has.Count.EqualTo(1));
        Assert.IsTrue(actionWasInvoked);
        Assert.That(workspace.LearningWorlds.First(), Is.EqualTo(world));
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var workspace = new AuthoringToolWorkspace(null, new List<LearningWorld>());
        var name = "n";
        var shortname = "sn";
        var authors = "a";
        var language = "l";
        var description = "d";
        var goals = "g";
        bool actionWasInvoked = false;
        Action<AuthoringToolWorkspace> mappingAction = _ => actionWasInvoked = true;

        var command = new CreateLearningWorld(workspace, name, shortname, authors, language, description, goals,
            mappingAction);

        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
        
        Assert.IsFalse(actionWasInvoked);
    }

    [Test]
    public void UndoRedo_UndoesAndRedoesCreateLearningSpace()
    {
        var workspace = new AuthoringToolWorkspace(null, new List<LearningWorld>());
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        workspace.LearningWorlds.Add(world);
        workspace.SelectedLearningWorld = world;
        var name = "n";
        var shortname = "sn";
        var authors = "a";
        var language = "l";
        var description = "d";
        var goals = "g";
        bool actionWasInvoked = false;
        Action<AuthoringToolWorkspace> mappingAction = _ => actionWasInvoked = true;

        var command = new CreateLearningWorld(workspace, name, shortname, authors, language, description, goals,
            mappingAction);

        Assert.That(workspace.LearningWorlds, Has.Count.EqualTo(1));
        Assert.That(workspace.SelectedLearningWorld, Is.EqualTo(world));
        Assert.IsFalse(actionWasInvoked);

        command.Execute();

        Assert.That(workspace.LearningWorlds, Has.Count.EqualTo(2));
        Assert.That(workspace.SelectedLearningWorld, Is.EqualTo(command.LearningWorld));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;

        command.Undo();

        Assert.That(workspace.LearningWorlds, Has.Count.EqualTo(1));
        Assert.That(workspace.SelectedLearningWorld, Is.EqualTo(world));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;

        command.Redo();

        Assert.That(workspace.LearningWorlds, Has.Count.EqualTo(2));
        Assert.That(workspace.SelectedLearningWorld, Is.EqualTo(command.LearningWorld));
        Assert.IsTrue(actionWasInvoked); 
    }
}