using BusinessLogic.Commands.Topic;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using Shared.Theme;

namespace BusinessLogicTest.Commands.Topic;

[TestFixture]
public class DeleteTopicUt
{
    [Test]
    public void Execute_Undo_Redo_DeletesTopic()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f", WorldTheme.CampusAschaffenburg)
        {
            UnsavedChanges = false
        };
        var topic = new BusinessLogic.Entities.Topic("Topic1")
        {
            UnsavedChanges = false
        };
        world.Topics.Add(topic);
        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;
        var command = new DeleteTopic(world, topic, mappingAction, new NullLogger<DeleteTopic>());

        Assert.Multiple(() =>
        {
            Assert.That(world.Topics, Does.Contain(topic));
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(world.UnsavedChanges, Is.False);
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(world.Topics, Is.Empty);
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(world.UnsavedChanges, Is.True);
        });
        actionWasInvoked = false;

        command.Undo();

        Assert.Multiple(() =>
        {
            Assert.That(world.Topics, Does.Contain(topic));
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(world.UnsavedChanges, Is.False);
        });
        actionWasInvoked = false;

        command.Redo();
        Assert.Multiple(() =>
        {
            Assert.That(world.Topics, Is.Empty);
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(world.UnsavedChanges, Is.True);
        });
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f", WorldTheme.CampusAschaffenburg);
        var topic = new BusinessLogic.Entities.Topic("Topic1");
        world.Topics.Add(topic);
        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteTopic(world, topic, mappingAction, new NullLogger<DeleteTopic>());

        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.Multiple(() =>
        {
            Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
            Assert.That(actionWasInvoked, Is.False);
        });
    }
}