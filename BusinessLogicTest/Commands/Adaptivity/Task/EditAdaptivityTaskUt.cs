using BusinessLogic.Commands.Adaptivity.Task;
using BusinessLogic.Entities.LearningContent.Adaptivity;
using BusinessLogic.Entities.LearningContent.Adaptivity.Question;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using Shared.Adaptivity;

namespace BusinessLogicTest.Commands.Adaptivity.Task;

[TestFixture]
public class EditAdaptivityTaskUt
{
    [Test]
    public void Execute_EditsAdaptivityTask()
    {
        // Arrange
        var adaptivityTask = new AdaptivityTask(new List<IAdaptivityQuestion>(), QuestionDifficulty.Easy, "Task1");
        var name = "NewTaskName";
        var minimumRequiredDifficulty = QuestionDifficulty.Medium;
        var actionWasInvoked = false;
        Action<AdaptivityTask> mappingAction = _ => actionWasInvoked = true;

        var command = new EditAdaptivityTask(adaptivityTask, name, minimumRequiredDifficulty, mappingAction,
            new NullLogger<EditAdaptivityTask>());

        // Act
        command.Execute();

        // Assert
        Assert.That(adaptivityTask.Name, Is.EqualTo(name));
        Assert.That(adaptivityTask.MinimumRequiredDifficulty, Is.EqualTo(minimumRequiredDifficulty));
        Assert.IsTrue(actionWasInvoked);
    }

    [Test]
    public void Undo_UndoesEditAdaptivityTask()
    {
        // Arrange
        var adaptivityTask = new AdaptivityTask(new List<IAdaptivityQuestion>(), QuestionDifficulty.Easy, "Task1");
        var name = "NewTaskName";
        var minimumRequiredDifficulty = QuestionDifficulty.Medium;
        var actionWasInvoked = false;
        Action<AdaptivityTask> mappingAction = _ => actionWasInvoked = true;

        var command = new EditAdaptivityTask(adaptivityTask, name, minimumRequiredDifficulty, mappingAction,
            new NullLogger<EditAdaptivityTask>());

        command.Execute();

        // Act
        command.Undo();

        // Assert
        Assert.That(adaptivityTask.Name, Is.EqualTo("Task1"));
        Assert.That(adaptivityTask.MinimumRequiredDifficulty, Is.EqualTo(QuestionDifficulty.Easy));
        Assert.IsTrue(actionWasInvoked);
    }

    [Test]
    public void Redo_RedoEditsAdaptivityTask()
    {
        // Arrange
        var adaptivityTask = new AdaptivityTask(new List<IAdaptivityQuestion>(), QuestionDifficulty.Easy, "Task1");
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
        Assert.That(adaptivityTask.Name, Is.EqualTo(name));
        Assert.That(adaptivityTask.MinimumRequiredDifficulty, Is.EqualTo(minimumRequiredDifficulty));
        Assert.IsTrue(actionWasInvoked);
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        // Arrange
        var adaptivityTask = new AdaptivityTask(new List<IAdaptivityQuestion>(), QuestionDifficulty.Easy, "Task1");
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