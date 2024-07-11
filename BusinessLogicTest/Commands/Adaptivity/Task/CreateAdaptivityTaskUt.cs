using BusinessLogic.Commands.Adaptivity.Task;
using BusinessLogic.Entities.LearningContent.Adaptivity;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using TestHelpers;

namespace BusinessLogicTest.Commands.Adaptivity.Task;

[TestFixture]
public class CreateAdaptivityTaskUt
{
    [Test]
    // ANF-ID: [AWA0005]
    public void Execute_CreatesAdaptivityTask()
    {
        var adaptivityContent = EntityProvider.GetAdaptivityContent();
        var tasksCount = adaptivityContent.Tasks.Count;
        var name = "Task1";
        var actionWasInvoked = false;
        Action<AdaptivityContent> mappingAction = _ => actionWasInvoked = true;

        var command = new CreateAdaptivityTask(adaptivityContent, name, mappingAction,
            new NullLogger<CreateAdaptivityTask>());

        Assert.Multiple(() =>
        {
            Assert.That(adaptivityContent.Tasks, Has.Count.EqualTo(tasksCount));
            Assert.That(actionWasInvoked, Is.False);
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(adaptivityContent.Tasks, Has.Count.EqualTo(tasksCount + 1));
            Assert.That(adaptivityContent.Tasks.Last().Name, Is.EqualTo(name));
            Assert.That(actionWasInvoked, Is.True);
        });
    }

    [Test]
    public void Undo_UndoesCreateAdaptivityTask()
    {
        var adaptivityContent = EntityProvider.GetAdaptivityContent();
        var tasksCount = adaptivityContent.Tasks.Count;
        var name = "Task1";
        var actionWasInvoked = false;
        Action<AdaptivityContent> mappingAction = _ => actionWasInvoked = true;

        var command = new CreateAdaptivityTask(adaptivityContent, name, mappingAction,
            new NullLogger<CreateAdaptivityTask>());

        Assert.Multiple(() =>
        {
            Assert.That(adaptivityContent.Tasks, Has.Count.EqualTo(tasksCount));
            Assert.That(actionWasInvoked, Is.False);
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(adaptivityContent.Tasks, Has.Count.EqualTo(tasksCount + 1));
            Assert.That(adaptivityContent.Tasks.Last().Name, Is.EqualTo(name));
            Assert.That(actionWasInvoked, Is.True);
        });
        actionWasInvoked = false;

        command.Undo();

        Assert.Multiple(() =>
        {
            Assert.That(adaptivityContent.Tasks, Has.Count.EqualTo(tasksCount));
            Assert.That(actionWasInvoked, Is.True);
        });
    }

    [Test]
    public void Redo_RedoCreatesAdaptivityTask()
    {
        var adaptivityContent = EntityProvider.GetAdaptivityContent();
        var tasksCount = adaptivityContent.Tasks.Count;
        var name = "Task1";
        var actionWasInvoked = false;
        Action<AdaptivityContent> mappingAction = _ => actionWasInvoked = true;

        var command = new CreateAdaptivityTask(adaptivityContent, name, mappingAction,
            new NullLogger<CreateAdaptivityTask>());

        Assert.Multiple(() =>
        {
            Assert.That(adaptivityContent.Tasks, Has.Count.EqualTo(tasksCount));
            Assert.That(actionWasInvoked, Is.False);
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(adaptivityContent.Tasks, Has.Count.EqualTo(tasksCount + 1));
            Assert.That(adaptivityContent.Tasks.Last().Name, Is.EqualTo(name));
            Assert.That(actionWasInvoked, Is.True);
        });
        actionWasInvoked = false;

        command.Undo();

        Assert.Multiple(() =>
        {
            Assert.That(adaptivityContent.Tasks, Has.Count.EqualTo(tasksCount));
            Assert.That(actionWasInvoked, Is.True);
        });
        actionWasInvoked = false;

        command.Redo();

        Assert.Multiple(() =>
        {
            Assert.That(adaptivityContent.Tasks, Has.Count.EqualTo(tasksCount + 1));
            Assert.That(adaptivityContent.Tasks.Last().Name, Is.EqualTo(name));
            Assert.That(actionWasInvoked, Is.True);
        });
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var adaptivityContent = EntityProvider.GetAdaptivityContent();
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