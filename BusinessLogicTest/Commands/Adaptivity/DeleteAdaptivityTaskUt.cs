using BusinessLogic.Commands.Adaptivity.Task;
using BusinessLogic.Entities.LearningContent.Adaptivity;
using BusinessLogic.Entities.LearningContent.Adaptivity.Question;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace BusinessLogicTest.Commands.Adaptivity;

[TestFixture]
public class DeleteAdaptivityTaskUt
{
    [Test]
    public void Execute_DeletesAdaptivityTask()
    {
        var adaptivityContent = new AdaptivityContent(new List<IAdaptivityTask>());
        var taskToDelete = new AdaptivityTask(new List<IAdaptivityQuestion>(), null, "Task1");
        adaptivityContent.Tasks.Add(taskToDelete);

        var actionWasInvoked = false;
        Action<AdaptivityContent> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteAdaptivityTask(adaptivityContent, taskToDelete, mappingAction,
            new NullLogger<DeleteAdaptivityTask>());

        Assert.Multiple(() =>
        {
            Assert.That(adaptivityContent.Tasks, Has.Count.EqualTo(1));
            Assert.That(actionWasInvoked, Is.False);
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(adaptivityContent.Tasks, Is.Empty);
            Assert.That(actionWasInvoked, Is.True);
        });
    }

    [Test]
    public void Undo_UndoesDeleteAdaptivityTask()
    {
        var adaptivityContent = new AdaptivityContent(new List<IAdaptivityTask>());
        var taskToDelete = new AdaptivityTask(new List<IAdaptivityQuestion>(), null, "Task1");
        adaptivityContent.Tasks.Add(taskToDelete);

        var actionWasInvoked = false;
        Action<AdaptivityContent> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteAdaptivityTask(adaptivityContent, taskToDelete, mappingAction,
            new NullLogger<DeleteAdaptivityTask>());

        Assert.That(adaptivityContent.Tasks, Has.Count.EqualTo(1));
        Assert.IsFalse(actionWasInvoked);

        command.Execute();

        Assert.That(adaptivityContent.Tasks, Is.Empty);
        Assert.IsTrue(actionWasInvoked);
        actionWasInvoked = false;

        command.Undo();

        Assert.That(adaptivityContent.Tasks, Has.Count.EqualTo(1));
        Assert.IsTrue(actionWasInvoked);
    }

    [Test]
    public void Redo_RedoDeletesAdaptivityTask()
    {
        var adaptivityContent = new AdaptivityContent(new List<IAdaptivityTask>());
        var taskToDelete = new AdaptivityTask(new List<IAdaptivityQuestion>(), null, "Task1");
        adaptivityContent.Tasks.Add(taskToDelete);

        var actionWasInvoked = false;
        Action<AdaptivityContent> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteAdaptivityTask(adaptivityContent, taskToDelete, mappingAction,
            new NullLogger<DeleteAdaptivityTask>());

        Assert.That(adaptivityContent.Tasks, Has.Count.EqualTo(1));
        Assert.IsFalse(actionWasInvoked);

        command.Execute();

        Assert.That(adaptivityContent.Tasks, Is.Empty);
        Assert.IsTrue(actionWasInvoked);
        actionWasInvoked = false;

        command.Undo();

        Assert.That(adaptivityContent.Tasks, Has.Count.EqualTo(1));
        Assert.IsTrue(actionWasInvoked);
        actionWasInvoked = false;

        command.Redo();

        Assert.That(adaptivityContent.Tasks, Is.Empty);
        Assert.IsTrue(actionWasInvoked);
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var adaptivityContent = new AdaptivityContent(new List<IAdaptivityTask>());
        var taskToDelete = new AdaptivityTask(new List<IAdaptivityQuestion>(), null, "Task1");
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