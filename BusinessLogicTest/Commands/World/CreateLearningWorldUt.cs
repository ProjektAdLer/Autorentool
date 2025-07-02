using BusinessLogic.Commands.World;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using Shared.Theme;

namespace BusinessLogicTest.Commands.World;

[TestFixture]
public class CreateLearningWorldUt
{
    [Test]
    // ANF-ID: [ASE1]
    public void Execute_CreatesLearningWorld()
    {
        var workspace = new AuthoringToolWorkspace(new List<ILearningWorld>());
        var name = "n";
        var shortname = "sn";
        var authors = "a";
        var language = "l";
        var description = "d";
        var goals = "g";
        var theme = WorldTheme.CampusAschaffenburg;
        var evaluationLink = "el";
        var evaluationLinkName = "eln";
        var evaluationLinkText = "elt";
        var enrolmentKey = "ek";
        var storyStart = "ss";
        var storyEnd = "se";
        var actionWasInvoked = false;
        Action<AuthoringToolWorkspace> mappingAction = _ => actionWasInvoked = true;

        var command = new CreateLearningWorld(workspace, name, shortname, authors, language, description, goals, theme,
            evaluationLink, evaluationLinkName, evaluationLinkText, enrolmentKey, storyStart, storyEnd,
            mappingAction, new NullLogger<CreateLearningWorld>());

        Assert.Multiple(() =>
        {
            Assert.That(workspace.LearningWorlds, Is.Empty);
            Assert.That(actionWasInvoked, Is.False);
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(workspace.LearningWorlds, Has.Count.EqualTo(1));
            Assert.That(actionWasInvoked, Is.True);
        });
        var world = workspace.LearningWorlds.First();
        Assert.Multiple(() =>
        {
            Assert.That(world.Name, Is.EqualTo("n"));
            Assert.That(world.Shortname, Is.EqualTo("sn"));
            Assert.That(world.Authors, Is.EqualTo("a"));
            Assert.That(world.Language, Is.EqualTo("l"));
            Assert.That(world.Description, Is.EqualTo("d"));
            Assert.That(world.Goals, Is.EqualTo("g"));
            Assert.That(world.WorldTheme, Is.EqualTo(WorldTheme.CampusAschaffenburg));
            Assert.That(world.EvaluationLink, Is.EqualTo("el"));
            Assert.That(world.EvaluationLinkName, Is.EqualTo("eln"));
            Assert.That(world.EvaluationLinkText, Is.EqualTo("elt"));
            Assert.That(world.EnrolmentKey, Is.EqualTo("ek"));
            Assert.That(world.StoryStart, Is.EqualTo("ss"));
            Assert.That(world.StoryEnd, Is.EqualTo("se"));
        });
    }

    [Test]
    public void Execute_WithDuplicateName_CreatesWorldWithNewUniqueName()
    {
        var world1 = new LearningWorld("Foo", "", "", "", "", "", WorldTheme.CampusAschaffenburg);
        var world2 = new LearningWorld("Foo(1)", "", "", "", "", "", WorldTheme.CampusAschaffenburg);
        var workspace = new AuthoringToolWorkspace(new List<ILearningWorld> { world1, world2 });

        var systemUnderTest = new CreateLearningWorld(workspace, "Foo", "", "", "", "", "", default, "", "", "", "", "",
            "", _ => { },
            new NullLogger<CreateLearningWorld>());

        systemUnderTest.Execute();
        Assert.That(workspace.LearningWorlds.Last().Name, Is.EqualTo("Foo(2)"));
    }

    [Test]
    // ANF-ID: [ASE1]
    public void Execute_AddsLearningWorld()
    {
        var workspace = new AuthoringToolWorkspace(new List<ILearningWorld>());
        var world = new LearningWorld("n", "sn", "a", "l", "d", "g", WorldTheme.CampusAschaffenburg, "el");
        var actionWasInvoked = false;
        Action<AuthoringToolWorkspace> mappingAction = _ => actionWasInvoked = true;

        var command = new CreateLearningWorld(workspace, world, mappingAction, new NullLogger<CreateLearningWorld>());

        Assert.Multiple(() =>
        {
            Assert.That(workspace.LearningWorlds, Is.Empty);
            Assert.That(actionWasInvoked, Is.False);
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(workspace.LearningWorlds, Has.Count.EqualTo(1));
            Assert.That(actionWasInvoked, Is.True);
        });
        Assert.That(workspace.LearningWorlds.First(), Is.EqualTo(world));
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var workspace = new AuthoringToolWorkspace(new List<ILearningWorld>());
        var name = "n";
        var shortname = "sn";
        var authors = "a";
        var language = "l";
        var description = "d";
        var goals = "g";
        var theme = WorldTheme.CampusAschaffenburg;
        var evaluationLink = "el";
        var evaluationLinkName = "eln";
        var evaluationLinkText = "elt";
        var enrolmentKey = "ek";
        var storyStart = "ss";
        var storyEnd = "se";
        var actionWasInvoked = false;
        Action<AuthoringToolWorkspace> mappingAction = _ => actionWasInvoked = true;

        var command = new CreateLearningWorld(workspace, name, shortname, authors, language, description, goals, theme,
            evaluationLink, evaluationLinkName, evaluationLinkText, enrolmentKey, storyStart, storyEnd,
            mappingAction, new NullLogger<CreateLearningWorld>());

        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.Multiple(() =>
        {
            Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
            Assert.That(actionWasInvoked, Is.False);
        });
    }

    [Test]
    public void UndoRedo_UndoesAndRedoesCreateLearningSpace()
    {
        var workspace = new AuthoringToolWorkspace(new List<ILearningWorld>());
        var world = new LearningWorld("a", "b", "c", "d", "e", "f", default, "g", "h", "i", "j", "k", "l");
        workspace.LearningWorlds.Add(world);
        var name = "n";
        var shortname = "sn";
        var authors = "a";
        var language = "l";
        var description = "d";
        var goals = "g";
        var theme = WorldTheme.CampusAschaffenburg;
        var evaluationLink = "el";
        var evaluationLinkName = "eln";
        var evaluationLinkText = "elt";
        var enrolmentKey = "ek";
        var storyStart = "ss";
        var storyEnd = "se";
        var actionWasInvoked = false;
        Action<AuthoringToolWorkspace> mappingAction = _ => actionWasInvoked = true;

        var command = new CreateLearningWorld(workspace, name, shortname, authors, language, description, goals, theme,
            evaluationLink, evaluationLinkName, evaluationLinkText, enrolmentKey, storyStart, storyEnd,
            mappingAction, new NullLogger<CreateLearningWorld>());

        Assert.Multiple(() =>
        {
            Assert.That(workspace.LearningWorlds, Has.Count.EqualTo(1));
            Assert.That(actionWasInvoked, Is.False);
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(workspace.LearningWorlds, Has.Count.EqualTo(2));
            Assert.That(actionWasInvoked, Is.True);
        });
        actionWasInvoked = false;

        command.Undo();

        Assert.Multiple(() =>
        {
            Assert.That(workspace.LearningWorlds, Has.Count.EqualTo(1));
            Assert.That(actionWasInvoked, Is.True);
        });
        actionWasInvoked = false;

        command.Redo();

        Assert.Multiple(() =>
        {
            Assert.That(workspace.LearningWorlds, Has.Count.EqualTo(2));
            Assert.That(actionWasInvoked, Is.True);
        });
    }
}