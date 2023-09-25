using BusinessLogic.Commands.Adaptivity.Task;
using BusinessLogic.Entities.LearningContent.Adaptivity;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace BusinessLogicTest.Commands.Adaptivity;

[TestFixture]
public class CreateAdaptivityTaskUt
{
    [Test]
    public void Execute_CreatesAdaptivityTask()
    {
        var adaptivityContent = new AdaptivityContent(new List<IAdaptivityTask>());
        var name = "Task1";
        var actionWasInvoked = false;
        Action<AdaptivityContent> mappingAction = _ => actionWasInvoked = true;

        var command = new CreateAdaptivityTask(adaptivityContent, name, mappingAction,
            new NullLogger<CreateAdaptivityTask>());

        Assert.Multiple(() =>
        {
            Assert.That(adaptivityContent.Tasks, Is.Empty);
            Assert.That(actionWasInvoked, Is.False);
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(adaptivityContent.Tasks, Has.Count.EqualTo(1));
            Assert.That(actionWasInvoked, Is.True);
        });

        var createdTask = adaptivityContent.Tasks.First();
        Assert.That(createdTask.Name, Is.EqualTo(name));
    }

    [Test]
    public void Undo_UndoesCreateAdaptivityTask()
    {
        var adaptivityContent = new AdaptivityContent(new List<IAdaptivityTask>());
        var name = "Task1";
        var actionWasInvoked = false;
        Action<AdaptivityContent> mappingAction = _ => actionWasInvoked = true;

        var command = new CreateAdaptivityTask(adaptivityContent, name, mappingAction,
            new NullLogger<CreateAdaptivityTask>());

        Assert.That(adaptivityContent.Tasks, Is.Empty);
        Assert.IsFalse(actionWasInvoked);

        command.Execute();

        Assert.That(adaptivityContent.Tasks, Has.Count.EqualTo(1));
        Assert.IsTrue(actionWasInvoked);
        actionWasInvoked = false;

        command.Undo();

        Assert.That(adaptivityContent.Tasks, Is.Empty);
        Assert.IsTrue(actionWasInvoked);
    }

    [Test]
    public void Redo_RedoCreatesAdaptivityTask()
    {
        var adaptivityContent = new AdaptivityContent(new List<IAdaptivityTask>());
        var name = "Task1";
        var actionWasInvoked = false;
        Action<AdaptivityContent> mappingAction = _ => actionWasInvoked = true;

        var command = new CreateAdaptivityTask(adaptivityContent, name, mappingAction,
            new NullLogger<CreateAdaptivityTask>());

        Assert.That(adaptivityContent.Tasks, Is.Empty);
        Assert.IsFalse(actionWasInvoked);

        command.Execute();

        Assert.That(adaptivityContent.Tasks, Has.Count.EqualTo(1));
        Assert.IsTrue(actionWasInvoked);
        actionWasInvoked = false;

        command.Undo();

        Assert.That(adaptivityContent.Tasks, Is.Empty);
        Assert.IsTrue(actionWasInvoked);
        actionWasInvoked = false;

        command.Redo();

        Assert.That(adaptivityContent.Tasks, Has.Count.EqualTo(1));
        Assert.IsTrue(actionWasInvoked);

        var createdTask = adaptivityContent.Tasks.First();
        Assert.That(createdTask.Name, Is.EqualTo(name));
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var adaptivityContent = new AdaptivityContent(new List<IAdaptivityTask>());
        var name = "Task1";
        var actionWasInvoked = false;
        Action<AdaptivityContent> mappingAction = _ => actionWasInvoked = true;

        var command = new CreateAdaptivityTask(adaptivityContent, name, mappingAction, null!);

        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.Multiple(() =>
        {
            Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
            Assert.That(actionWasInvoked, Is.False);
        });
    }
}