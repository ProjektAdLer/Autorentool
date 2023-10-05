using BusinessLogic.Commands.Adaptivity.Task;
using BusinessLogic.Entities.LearningContent.Adaptivity;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using TestHelpers;

namespace BusinessLogicTest.Commands.Adaptivity.Task;

[TestFixture]
public class DeleteAdaptivityTaskUt
{
    [Test]
    public void Execute_DeletesAdaptivityTask()
    {
        var adaptivityContent = EntityProvider.GetAdaptivityContent();
        var taskToDelete = EntityProvider.GetAdaptivityTask();
        adaptivityContent.Tasks.Add(taskToDelete);
        var tasksCount = adaptivityContent.Tasks.Count;

        var actionWasInvoked = false;
        Action<AdaptivityContent> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteAdaptivityTask(adaptivityContent, taskToDelete, mappingAction,
            new NullLogger<DeleteAdaptivityTask>());

        Assert.Multiple(() =>
        {
            Assert.That(adaptivityContent.Tasks, Has.Count.EqualTo(tasksCount));
            Assert.That(adaptivityContent.Tasks, Contains.Item(taskToDelete));
            Assert.That(actionWasInvoked, Is.False);
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(adaptivityContent.Tasks, Has.Count.EqualTo(tasksCount - 1));
            Assert.That(adaptivityContent.Tasks, Does.Not.Contain(taskToDelete));
            Assert.That(actionWasInvoked, Is.True);
        });
    }

    [Test]
    public void Undo_UndoesDeleteAdaptivityTask()
    {
        var adaptivityContent = EntityProvider.GetAdaptivityContent();
        var taskToDelete = EntityProvider.GetAdaptivityTask();
        adaptivityContent.Tasks.Add(taskToDelete);
        var tasksCount = adaptivityContent.Tasks.Count;

        var actionWasInvoked = false;
        Action<AdaptivityContent> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteAdaptivityTask(adaptivityContent, taskToDelete, mappingAction,
            new NullLogger<DeleteAdaptivityTask>());

        Assert.Multiple(() =>
        {
            Assert.That(adaptivityContent.Tasks, Has.Count.EqualTo(tasksCount));
            Assert.That(adaptivityContent.Tasks, Contains.Item(taskToDelete));
            Assert.That(actionWasInvoked, Is.False);
        });

        command.Execute();
        Assert.Multiple(() =>
        {
            Assert.That(adaptivityContent.Tasks, Has.Count.EqualTo(tasksCount - 1));
            Assert.That(adaptivityContent.Tasks, Does.Not.Contain(taskToDelete));
            Assert.That(actionWasInvoked, Is.True);
        });
        actionWasInvoked = false;

        command.Undo();

        Assert.Multiple(() =>
        {
            Assert.That(adaptivityContent.Tasks, Has.Count.EqualTo(tasksCount));
            Assert.That(adaptivityContent.Tasks, Contains.Item(taskToDelete));
            Assert.That(actionWasInvoked, Is.True);
        });
    }

    [Test]
    public void Redo_RedoDeletesAdaptivityTask()
    {
        var adaptivityContent = EntityProvider.GetAdaptivityContent();
        var taskToDelete = EntityProvider.GetAdaptivityTask();
        adaptivityContent.Tasks.Add(taskToDelete);
        var tasksCount = adaptivityContent.Tasks.Count;

        var actionWasInvoked = false;
        Action<AdaptivityContent> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteAdaptivityTask(adaptivityContent, taskToDelete, mappingAction,
            new NullLogger<DeleteAdaptivityTask>());

        Assert.Multiple(() =>
        {
            Assert.That(adaptivityContent.Tasks, Has.Count.EqualTo(tasksCount));
            Assert.That(adaptivityContent.Tasks, Contains.Item(taskToDelete));
            Assert.That(actionWasInvoked, Is.False);
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(adaptivityContent.Tasks, Has.Count.EqualTo(tasksCount - 1));
            Assert.That(adaptivityContent.Tasks, Does.Not.Contain(taskToDelete));
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
            Assert.That(adaptivityContent.Tasks, Has.Count.EqualTo(tasksCount - 1));
            Assert.That(actionWasInvoked, Is.True);
        });
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var adaptivityContent = EntityProvider.GetAdaptivityContent();
        var taskToDelete = EntityProvider.GetAdaptivityTask();
        adaptivityContent.Tasks.Add(taskToDelete);

        var actionWasInvoked = false;
        Action<AdaptivityContent> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteAdaptivityTask(adaptivityContent, taskToDelete, mappingAction, null!);

        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.Multiple(() =>
        {
            Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
            Assert.That(actionWasInvoked, Is.False);
        });
    }
}