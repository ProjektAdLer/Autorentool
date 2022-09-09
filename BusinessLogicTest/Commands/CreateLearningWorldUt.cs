using BusinessLogic.Commands;
using BusinessLogic.Entities;
using NSubstitute;
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
        var mappingAction = Substitute.For<Action<AuthoringToolWorkspace>>();

        var command = new CreateLearningWorld(workspace, name, shortname, authors, language, description, goals,
            mappingAction);

        Assert.IsEmpty(workspace.LearningWorlds);

        command.Execute();

        Assert.That(workspace.LearningWorlds, Has.Count.EqualTo(1));
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
        var mappingAction = Substitute.For<Action<AuthoringToolWorkspace>>();

        var command = new CreateLearningWorld(workspace, name, shortname, authors, language, description, goals,
            mappingAction);

        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
    }

    [Test]
    public void Redo_LearningWorldIsNull_ThrowsException()
    {
        var workspace = new AuthoringToolWorkspace(null, new List<LearningWorld>());
        var name = "n";
        var shortname = "sn";
        var authors = "a";
        var language = "l";
        var description = "d";
        var goals = "g";
        var mappingAction = Substitute.For<Action<AuthoringToolWorkspace>>();

        var command = new CreateLearningWorld(workspace, name, shortname, authors, language, description, goals,
            mappingAction);

        var ex = Assert.Throws<InvalidOperationException>(() => command.Redo());
        Assert.That(ex!.Message, Is.EqualTo("_learningWorld is null"));
    }


    [Test]
    public void UndoRedo_UndoesAndRedoesCreateLearningSpace()
    {
        var workspace = new AuthoringToolWorkspace(null, new List<LearningWorld>());
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        workspace.LearningWorlds.Add(world);
        var name = "n";
        var shortname = "sn";
        var authors = "a";
        var language = "l";
        var description = "d";
        var goals = "g";
        var mappingAction = Substitute.For<Action<AuthoringToolWorkspace>>();

        var command = new CreateLearningWorld(workspace, name, shortname, authors, language, description, goals,
            mappingAction);

        Assert.That(workspace.LearningWorlds, Has.Count.EqualTo(1));

        command.Execute();

        Assert.That(workspace.LearningWorlds, Has.Count.EqualTo(2));

        command.Undo();

        Assert.That(workspace.LearningWorlds, Has.Count.EqualTo(1));

        command.Redo();

        Assert.That(workspace.LearningWorlds, Has.Count.EqualTo(2));
    }
}