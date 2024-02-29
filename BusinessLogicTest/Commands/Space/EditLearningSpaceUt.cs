using BusinessLogic.Commands.Space;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using Shared;
using TestHelpers;

namespace BusinessLogicTest.Commands.Space;

[TestFixture]
public class EditLearningSpaceUt
{
    [Test]
    public void Execute_EditsLearningSpace()
    {
        var space = new LearningSpace("a", "d", 5, Theme.Arcade, EntityProvider.GetLearningOutcomeCollection())
        {
            UnsavedChanges = false
        };
        var name = "space1";
        var description = "space for learning";
        var topic = new BusinessLogic.Entities.Topic("abc");
        var requiredPoints = 10;
        var theme = Theme.CampusAschaffenburg;
        var actionWasInvoked = false;
        Action<ILearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new EditLearningSpace(space, name, description, requiredPoints, theme, topic,
            mappingAction, new NullLogger<EditLearningSpace>());

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(space.Name, Is.EqualTo("a"));
            Assert.That(space.Description, Is.EqualTo("d"));
            Assert.That(space.RequiredPoints, Is.EqualTo(5));
            Assert.That(space.Theme, Is.EqualTo(Theme.Arcade));
            Assert.That(space.AssignedTopic, Is.EqualTo(null));
            Assert.That(space.UnsavedChanges, Is.False);
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(space.Name, Is.EqualTo("space1"));
            Assert.That(space.Description, Is.EqualTo("space for learning"));
            Assert.That(space.RequiredPoints, Is.EqualTo(10));
            Assert.That(space.Theme, Is.EqualTo(theme));
            Assert.That(space.AssignedTopic, Is.EqualTo(topic));
            Assert.That(space.UnsavedChanges, Is.True);
        });
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var space = new LearningSpace("a", "d", 5, Theme.Arcade, EntityProvider.GetLearningOutcomeCollection());
        var name = "space1";
        var description = "space for learning";
        var requiredPoints = 10;
        var theme = Theme.CampusAschaffenburg;
        var topic = new BusinessLogic.Entities.Topic("abc");
        var actionWasInvoked = false;
        Action<ILearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new EditLearningSpace(space, name, description, requiredPoints, theme, topic,
            mappingAction, new NullLogger<EditLearningSpace>());

        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.Multiple(() =>
        {
            Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
            Assert.That(actionWasInvoked, Is.False);
        });
    }

    [Test]
    public void UndoRedo_UndoesAndRedoesEditLearningSpace()
    {
        var space = new LearningSpace("g", "j", 5, Theme.Arcade, EntityProvider.GetLearningOutcomeCollection())
        {
            UnsavedChanges = false
        };
        var name = "space1";
        var description = "space for learning";
        var requiredPoints = 10;
        var theme = Theme.CampusAschaffenburg;
        var topic = new BusinessLogic.Entities.Topic("abc");
        var actionWasInvoked = false;
        Action<ILearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new EditLearningSpace(space, name, description, requiredPoints, theme, topic,
            mappingAction, new NullLogger<EditLearningSpace>());

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(space.Name, Is.EqualTo("g"));
            Assert.That(space.Description, Is.EqualTo("j"));
            Assert.That(space.RequiredPoints, Is.EqualTo(5));
            Assert.That(space.Theme, Is.EqualTo(Theme.Arcade));
            Assert.That(space.AssignedTopic, Is.EqualTo(null));
            Assert.That(space.UnsavedChanges, Is.False);
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(space.Name, Is.EqualTo("space1"));
            Assert.That(space.Description, Is.EqualTo("space for learning"));
            Assert.That(space.RequiredPoints, Is.EqualTo(10));
            Assert.That(space.Theme, Is.EqualTo(theme));
            Assert.That(space.AssignedTopic, Is.EqualTo(topic));
            Assert.That(space.UnsavedChanges, Is.True);
        });
        actionWasInvoked = false;

        command.Undo();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(space.Name, Is.EqualTo("g"));
            Assert.That(space.Description, Is.EqualTo("j"));
            Assert.That(space.RequiredPoints, Is.EqualTo(5));
            Assert.That(space.Theme, Is.EqualTo(Theme.Arcade));
            Assert.That(space.AssignedTopic, Is.EqualTo(null));
            Assert.That(space.UnsavedChanges, Is.False);
        });
        actionWasInvoked = false;

        command.Redo();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(space.Name, Is.EqualTo("space1"));
            Assert.That(space.Description, Is.EqualTo("space for learning"));
            Assert.That(space.RequiredPoints, Is.EqualTo(10));
            Assert.That(space.Theme, Is.EqualTo(theme));
            Assert.That(space.AssignedTopic, Is.EqualTo(topic));
            Assert.That(space.UnsavedChanges, Is.True);
        });
    }
}