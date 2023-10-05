using BusinessLogic.Commands.Adaptivity.Task;
using BusinessLogic.Entities.LearningContent.Adaptivity;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using Shared.Adaptivity;
using TestHelpers;

namespace BusinessLogicTest.Commands.Adaptivity.Task;

[TestFixture]
public class EditAdaptivityTaskUt
{
    [Test]
    public void Execute_EditsAdaptivityTask()
    {
        // Arrange
        var adaptivityTask = EntityProvider.GetAdaptivityTask();
        var oldName = adaptivityTask.Name;
        var name = "NewTaskName";
        Assert.That(oldName, Is.Not.EqualTo(name));
        var minimumRequiredDifficulty = QuestionDifficulty.Medium;
        var oldMinimumRequiredDifficulty = adaptivityTask.MinimumRequiredDifficulty;
        Assert.That(oldMinimumRequiredDifficulty, Is.Not.EqualTo(minimumRequiredDifficulty));
        var actionWasInvoked = false;
        Action<AdaptivityTask> mappingAction = _ => actionWasInvoked = true;

        var command = new EditAdaptivityTask(adaptivityTask, name, minimumRequiredDifficulty, mappingAction,
            new NullLogger<EditAdaptivityTask>());

        // Assert before execution
        Assert.Multiple(() =>
        {
            Assert.That(adaptivityTask.Name, Is.EqualTo(oldName));
            Assert.That(adaptivityTask.MinimumRequiredDifficulty, Is.EqualTo(oldMinimumRequiredDifficulty));
            Assert.That(actionWasInvoked, Is.False);
        });

        // Act
        command.Execute();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(adaptivityTask.Name, Is.EqualTo(name));
            Assert.That(adaptivityTask.MinimumRequiredDifficulty, Is.EqualTo(minimumRequiredDifficulty));
            Assert.That(actionWasInvoked, Is.True);
        });
    }

    [Test]
    public void Undo_UndoesEditAdaptivityTask()
    {
        // Arrange
        var adaptivityTask = EntityProvider.GetAdaptivityTask();
        var oldName = adaptivityTask.Name;
        var name = "NewTaskName";
        Assert.That(oldName, Is.Not.EqualTo(name));
        var oldMinimumRequiredDifficulty = adaptivityTask.MinimumRequiredDifficulty;
        var minimumRequiredDifficulty = QuestionDifficulty.Medium;
        Assert.That(oldMinimumRequiredDifficulty, Is.Not.EqualTo(minimumRequiredDifficulty));
        var actionWasInvoked = false;
        Action<AdaptivityTask> mappingAction = _ => actionWasInvoked = true;

        var command = new EditAdaptivityTask(adaptivityTask, name, minimumRequiredDifficulty, mappingAction,
            new NullLogger<EditAdaptivityTask>());

        command.Execute();

        // Act
        command.Undo();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(adaptivityTask.Name, Is.EqualTo(oldName));
            Assert.That(adaptivityTask.MinimumRequiredDifficulty, Is.EqualTo(oldMinimumRequiredDifficulty));
            Assert.That(actionWasInvoked, Is.True);
        });
    }

    [Test]
    public void Redo_RedoEditsAdaptivityTask()
    {
        // Arrange
        var adaptivityTask = EntityProvider.GetAdaptivityTask();
        var name = "NewTaskName";
        var minimumRequiredDifficulty = QuestionDifficulty.Medium;
        var actionWasInvoked = false;
        Action<AdaptivityTask> mappingAction = _ => actionWasInvoked = true;

        var command = new EditAdaptivityTask(adaptivityTask, name, minimumRequiredDifficulty, mappingAction,
            new NullLogger<EditAdaptivityTask>());

        command.Execute();
        command.Undo();

        // Act
        command.Redo();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(adaptivityTask.Name, Is.EqualTo(name));
            Assert.That(adaptivityTask.MinimumRequiredDifficulty, Is.EqualTo(minimumRequiredDifficulty));
            Assert.That(actionWasInvoked, Is.True);
        });
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        // Arrange
        var adaptivityTask = EntityProvider.GetAdaptivityTask();
        var name = "NewTaskName";
        var minimumRequiredDifficulty = QuestionDifficulty.Medium;
        var actionWasInvoked = false;
        Action<AdaptivityTask> mappingAction = _ => actionWasInvoked = true;

        var command = new EditAdaptivityTask(adaptivityTask, name, minimumRequiredDifficulty, mappingAction, null!);

        // Act and Assert
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.Multiple(() =>
        {
            Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
            Assert.That(actionWasInvoked, Is.False);
        });
    }
}