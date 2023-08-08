using BusinessLogic.Commands.Topic;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace BusinessLogicTest.Commands.Topic;

[TestFixture]
public class DeleteTopicUt
{
    [Test]
    public void Execute_Undo_Redo_DeletesTopic()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f")
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

        Assert.That(world.Topics, Does.Contain(topic));
        Assert.IsFalse(actionWasInvoked);
        Assert.That(world.UnsavedChanges, Is.False);

        command.Execute();

        Assert.That(world.Topics, Is.Empty);
        Assert.IsTrue(actionWasInvoked);
        actionWasInvoked = false;
        Assert.That(world.UnsavedChanges, Is.True);

        command.Undo();

        Assert.That(world.Topics, Does.Contain(topic));
        Assert.IsTrue(actionWasInvoked);
        actionWasInvoked = false;
        Assert.That(world.UnsavedChanges, Is.False);

        command.Redo();
        Assert.That(world.Topics, Is.Empty);
        Assert.IsTrue(actionWasInvoked);
        Assert.That(world.UnsavedChanges, Is.True);
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var topic = new BusinessLogic.Entities.Topic("Topic1");
        world.Topics.Add(topic);
        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteTopic(world, topic, mappingAction, new NullLogger<DeleteTopic>());

        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));

        Assert.IsFalse(actionWasInvoked);
    }
}