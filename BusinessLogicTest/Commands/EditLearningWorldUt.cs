using BusinessLogic.Commands;
using BusinessLogic.Entities;
using NSubstitute;
using NUnit.Framework;

namespace BusinessLogicTest.Commands;

[TestFixture]
public class EditLearningWorldUt
{
    [Test]
    public void Execute_EditsLearningSpace()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var name = "n";
        var shortname = "sn";
        var authors = "a";
        var language = "l";
        var description = "d";
        var goals = "g";
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command =
            new EditLearningWorld(world, name, shortname, authors, language, description, goals, mappingAction);

        Assert.Multiple(() =>
        {
            Assert.IsFalse(actionWasInvoked);
            Assert.That(world.Name, Is.EqualTo("a"));
            Assert.That(world.Shortname, Is.EqualTo("b"));
            Assert.That(world.Authors, Is.EqualTo("c"));
            Assert.That(world.Language, Is.EqualTo("d"));
            Assert.That(world.Description, Is.EqualTo("e"));
            Assert.That(world.Goals, Is.EqualTo("f"));
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.IsTrue(actionWasInvoked);
            Assert.That(world.Name, Is.EqualTo("n"));
            Assert.That(world.Shortname, Is.EqualTo("sn"));
            Assert.That(world.Authors, Is.EqualTo("a"));
            Assert.That(world.Language, Is.EqualTo("l"));
            Assert.That(world.Description, Is.EqualTo("d"));
            Assert.That(world.Goals, Is.EqualTo("g"));
        });
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var name = "n";
        var shortname = "sn";
        var authors = "a";
        var language = "l";
        var description = "d";
        var goals = "g";
        var mappingAction = Substitute.For<Action<LearningWorld>>();

        var command =
            new EditLearningWorld(world, name, shortname, authors, language, description, goals, mappingAction);

        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
    }

    [Test]
    public void UndoRedo_UndoesAndRedoesEditLearningSpace()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var name = "n";
        var shortname = "sn";
        var authors = "a";
        var language = "l";
        var description = "d";
        var goals = "g";
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command =
            new EditLearningWorld(world, name, shortname, authors, language, description, goals, mappingAction);

        Assert.Multiple(() =>
        {
            Assert.IsFalse(actionWasInvoked);
            Assert.That(world.Name, Is.EqualTo("a"));
            Assert.That(world.Shortname, Is.EqualTo("b"));
            Assert.That(world.Authors, Is.EqualTo("c"));
            Assert.That(world.Language, Is.EqualTo("d"));
            Assert.That(world.Description, Is.EqualTo("e"));
            Assert.That(world.Goals, Is.EqualTo("f"));
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.IsTrue(actionWasInvoked);
            Assert.That(world.Name, Is.EqualTo("n"));
            Assert.That(world.Shortname, Is.EqualTo("sn"));
            Assert.That(world.Authors, Is.EqualTo("a"));
            Assert.That(world.Language, Is.EqualTo("l"));
            Assert.That(world.Description, Is.EqualTo("d"));
            Assert.That(world.Goals, Is.EqualTo("g"));
        });
        actionWasInvoked = false;

        command.Undo();

        Assert.Multiple(() =>
        {
            Assert.IsTrue(actionWasInvoked);
            Assert.That(world.Name, Is.EqualTo("a"));
            Assert.That(world.Shortname, Is.EqualTo("b"));
            Assert.That(world.Authors, Is.EqualTo("c"));
            Assert.That(world.Language, Is.EqualTo("d"));
            Assert.That(world.Description, Is.EqualTo("e"));
            Assert.That(world.Goals, Is.EqualTo("f"));
        });
        actionWasInvoked = false;

        command.Redo();

        Assert.Multiple(() =>
        {
            Assert.IsTrue(actionWasInvoked);
            Assert.That(world.Name, Is.EqualTo("n"));
            Assert.That(world.Shortname, Is.EqualTo("sn"));
            Assert.That(world.Authors, Is.EqualTo("a"));
            Assert.That(world.Language, Is.EqualTo("l"));
            Assert.That(world.Description, Is.EqualTo("d"));
            Assert.That(world.Goals, Is.EqualTo("g"));
        });
    }
}