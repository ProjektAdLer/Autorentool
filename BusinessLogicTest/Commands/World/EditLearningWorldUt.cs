using BusinessLogic.Commands.World;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using Shared.Theme;

namespace BusinessLogicTest.Commands.World;

[TestFixture]
public class EditLearningWorldUt
{
    [Test]
    // ANF-ID: [ASE3]
    public void Execute_EditsLearningSpace()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f", WorldTheme.CampusAschaffenburg, "g", "h", "i", "j",
            "k", "l")
        {
            UnsavedChanges = false
        };
        var name = "n";
        var shortname = "sn";
        var authors = "a";
        var language = "l";
        var description = "d";
        var goals = "g";
        var theme = WorldTheme.CampusKempten;
        var evaluationLink = "el";
        var evaluationLinkName = "eln";
        var evaluationLinkText = "elt";
        var enrolmentKey = "ek";
        var storyStart = "ss";
        var storyEnd = "se";
        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command =
            new EditLearningWorld(world, name, shortname, authors, language, description, goals, theme,
                evaluationLink, evaluationLinkName, evaluationLinkText,
                enrolmentKey, storyStart, storyEnd,
                mappingAction,
                new NullLogger<EditLearningWorld>());

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(world.Name, Is.EqualTo("a"));
            Assert.That(world.Shortname, Is.EqualTo("b"));
            Assert.That(world.Authors, Is.EqualTo("c"));
            Assert.That(world.Language, Is.EqualTo("d"));
            Assert.That(world.Description, Is.EqualTo("e"));
            Assert.That(world.Goals, Is.EqualTo("f"));
            Assert.That(world.WorldTheme, Is.EqualTo(WorldTheme.CampusAschaffenburg));
            Assert.That(world.EvaluationLink, Is.EqualTo("g"));
            Assert.That(world.EvaluationLinkName, Is.EqualTo("h"));
            Assert.That(world.EvaluationLinkText, Is.EqualTo("i"));
            Assert.That(world.EnrolmentKey, Is.EqualTo("j"));
            Assert.That(world.StoryStart, Is.EqualTo("k"));
            Assert.That(world.StoryEnd, Is.EqualTo("l"));
            Assert.That(world.UnsavedChanges, Is.False);
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(world.Name, Is.EqualTo("n"));
            Assert.That(world.Shortname, Is.EqualTo("sn"));
            Assert.That(world.Authors, Is.EqualTo("a"));
            Assert.That(world.Language, Is.EqualTo("l"));
            Assert.That(world.Description, Is.EqualTo("d"));
            Assert.That(world.Goals, Is.EqualTo("g"));
            Assert.That(world.WorldTheme, Is.EqualTo(WorldTheme.CampusKempten));
            Assert.That(world.EvaluationLink, Is.EqualTo("el"));
            Assert.That(world.EvaluationLinkName, Is.EqualTo("eln"));
            Assert.That(world.EvaluationLinkText, Is.EqualTo("elt"));
            Assert.That(world.EnrolmentKey, Is.EqualTo("ek"));
            Assert.That(world.StoryStart, Is.EqualTo("ss"));
            Assert.That(world.StoryEnd, Is.EqualTo("se"));
            Assert.That(world.UnsavedChanges, Is.True);
        });
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f", WorldTheme.CampusAschaffenburg, "g", "h", "i", "j",
            "k", "l");
        var name = "n";
        var shortname = "sn";
        var authors = "a";
        var language = "l";
        var description = "d";
        var goals = "g";
        var theme = WorldTheme.CampusKempten;
        var evaluationLink = "el";
        var evaluationLinkName = "eln";
        var evaluationLinkText = "elt";
        var enrolmentKey = "ek";
        var storyStart = "ss";
        var storyEnd = "se";
        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command =
            new EditLearningWorld(world, name, shortname, authors, language, description, goals, theme, evaluationLink,
                evaluationLinkName, evaluationLinkText, enrolmentKey, storyStart, storyEnd,
                mappingAction,
                new NullLogger<EditLearningWorld>());

        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.Multiple(() =>
        {
            Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
            Assert.That(actionWasInvoked, Is.False);
        });
    }

    [Test]
    public void UndoRedo_UndoesAndRedoesEditLearningWorld()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f", WorldTheme.CampusAschaffenburg, "g", "h", "i", "j",
            "k", "l")
        {
            UnsavedChanges = false
        };
        var name = "n";
        var shortname = "sn";
        var authors = "a";
        var language = "l";
        var description = "d";
        var goals = "g";
        var theme = WorldTheme.CampusKempten;
        var evaluationLink = "el";
        var evaluationLinkName = "eln";
        var evaluationLinkText = "elt";
        var enrolmentKey = "ek";
        var storyStart = "ss";
        var storyEnd = "se";
        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command =
            new EditLearningWorld(world, name, shortname, authors, language, description, goals, theme, evaluationLink,
                evaluationLinkName, evaluationLinkText, enrolmentKey, storyStart, storyEnd,
                mappingAction,
                new NullLogger<EditLearningWorld>());

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(world.Name, Is.EqualTo("a"));
            Assert.That(world.Shortname, Is.EqualTo("b"));
            Assert.That(world.Authors, Is.EqualTo("c"));
            Assert.That(world.Language, Is.EqualTo("d"));
            Assert.That(world.Description, Is.EqualTo("e"));
            Assert.That(world.Goals, Is.EqualTo("f"));
            Assert.That(world.WorldTheme, Is.EqualTo(WorldTheme.CampusAschaffenburg));
            Assert.That(world.EvaluationLink, Is.EqualTo("g"));
            Assert.That(world.EvaluationLinkName, Is.EqualTo("h"));
            Assert.That(world.EvaluationLinkText, Is.EqualTo("i"));
            Assert.That(world.EnrolmentKey, Is.EqualTo("j"));
            Assert.That(world.StoryStart, Is.EqualTo("k"));
            Assert.That(world.StoryEnd, Is.EqualTo("l"));
            Assert.That(world.UnsavedChanges, Is.False);
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(world.Name, Is.EqualTo("n"));
            Assert.That(world.Shortname, Is.EqualTo("sn"));
            Assert.That(world.Authors, Is.EqualTo("a"));
            Assert.That(world.Language, Is.EqualTo("l"));
            Assert.That(world.Description, Is.EqualTo("d"));
            Assert.That(world.Goals, Is.EqualTo("g"));
            Assert.That(world.WorldTheme, Is.EqualTo(WorldTheme.CampusKempten));
            Assert.That(world.EvaluationLink, Is.EqualTo("el"));
            Assert.That(world.EvaluationLinkName, Is.EqualTo("eln"));
            Assert.That(world.EvaluationLinkText, Is.EqualTo("elt"));
            Assert.That(world.EnrolmentKey, Is.EqualTo("ek"));
            Assert.That(world.StoryStart, Is.EqualTo("ss"));
            Assert.That(world.StoryEnd, Is.EqualTo("se"));
            Assert.That(world.UnsavedChanges, Is.True);
        });
        actionWasInvoked = false;

        command.Undo();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(world.Name, Is.EqualTo("a"));
            Assert.That(world.Shortname, Is.EqualTo("b"));
            Assert.That(world.Authors, Is.EqualTo("c"));
            Assert.That(world.Language, Is.EqualTo("d"));
            Assert.That(world.Description, Is.EqualTo("e"));
            Assert.That(world.Goals, Is.EqualTo("f"));
            Assert.That(world.WorldTheme, Is.EqualTo(WorldTheme.CampusAschaffenburg));
            Assert.That(world.EvaluationLink, Is.EqualTo("g"));
            Assert.That(world.EvaluationLinkName, Is.EqualTo("h"));
            Assert.That(world.EvaluationLinkText, Is.EqualTo("i"));
            Assert.That(world.EnrolmentKey, Is.EqualTo("j"));
            Assert.That(world.StoryStart, Is.EqualTo("k"));
            Assert.That(world.StoryEnd, Is.EqualTo("l"));
            Assert.That(world.UnsavedChanges, Is.False);
        });
        actionWasInvoked = false;

        command.Redo();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(world.Name, Is.EqualTo("n"));
            Assert.That(world.Shortname, Is.EqualTo("sn"));
            Assert.That(world.Authors, Is.EqualTo("a"));
            Assert.That(world.Language, Is.EqualTo("l"));
            Assert.That(world.Description, Is.EqualTo("d"));
            Assert.That(world.Goals, Is.EqualTo("g"));
            Assert.That(world.WorldTheme, Is.EqualTo(WorldTheme.CampusKempten));
            Assert.That(world.EvaluationLink, Is.EqualTo("el"));
            Assert.That(world.EvaluationLinkName, Is.EqualTo("eln"));
            Assert.That(world.EvaluationLinkText, Is.EqualTo("elt"));
            Assert.That(world.EnrolmentKey, Is.EqualTo("ek"));
            Assert.That(world.StoryStart, Is.EqualTo("ss"));
            Assert.That(world.StoryEnd, Is.EqualTo("se"));
            Assert.That(world.UnsavedChanges, Is.True);
        });
    }
}