using BusinessLogic.Commands.Topic;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using TestHelpers;

namespace BusinessLogicTest.Commands.Topic;

[TestFixture]
public class CreateTopicUt
{
    [Test]
    public void Execute_CreatesTopic()
    {
        var world = EntityProvider.GetLearningWorld(unsavedChanges: false);
        var name = "topic1";
        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new CreateTopic(world, name, mappingAction, new NullLogger<CreateTopic>());

        Assert.Multiple(() =>
        {
            Assert.That(world.Topics, Is.Empty);
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(world.UnsavedChanges, Is.False);
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(world.Topics, Has.Count.EqualTo(1));
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(world.UnsavedChanges, Is.True);
        });
        var topic = world.Topics.First();
        Assert.That(topic.Name, Is.EqualTo("topic1"));
    }

    [Test]
    public void Execute_NameOfTopicAlreadyExists_CreatesTopicWithEditedName()
    {
        var world = EntityProvider.GetLearningWorld();
        var name = "topic1";
        var topic = new BusinessLogic.Entities.Topic(name);
        world.Topics.Add(topic);
        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new CreateTopic(world, name, mappingAction, new NullLogger<CreateTopic>());

        Assert.Multiple(() =>
        {
            Assert.That(world.Topics, Has.Count.EqualTo(1));
            Assert.That(actionWasInvoked, Is.False);
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(world.Topics, Has.Count.EqualTo(2));
            Assert.That(actionWasInvoked, Is.True);
        });
        actionWasInvoked = false;
        var topic1 = world.Topics.Last();
        Assert.That(topic1.Name, Is.EqualTo("topic1(1)"));

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(world.Topics, Has.Count.EqualTo(3));
            Assert.That(actionWasInvoked, Is.True);
        });
        var topic2 = world.Topics.Last();
        Assert.That(topic2.Name, Is.EqualTo("topic1(2)"));
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var world = EntityProvider.GetLearningWorld();
        var name = "topic1";
        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new CreateTopic(world, name, mappingAction, new NullLogger<CreateTopic>());

        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.Multiple(() =>
        {
            Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
            Assert.That(actionWasInvoked, Is.False);
        });
    }

    [Test]
    public void UndoRedo_UndoesAndRedoesCreateTopic()
    {
        var world = EntityProvider.GetLearningWorld();
        var name = "topic1";
        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new CreateTopic(world, name, mappingAction, new NullLogger<CreateTopic>());

        Assert.Multiple(() =>
        {
            Assert.That(world.Topics, Has.Count.EqualTo(0));
            Assert.That(actionWasInvoked, Is.False);
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(world.Topics, Has.Count.EqualTo(1));
            Assert.That(actionWasInvoked, Is.True);
        });
        actionWasInvoked = false;

        command.Undo();

        Assert.Multiple(() =>
        {
            Assert.That(world.Topics, Has.Count.EqualTo(0));
            Assert.That(actionWasInvoked, Is.True);
        });
        actionWasInvoked = false;

        command.Redo();

        Assert.Multiple(() =>
        {
            Assert.That(world.Topics, Has.Count.EqualTo(1));
            Assert.That(actionWasInvoked, Is.True);
        });
    }
}