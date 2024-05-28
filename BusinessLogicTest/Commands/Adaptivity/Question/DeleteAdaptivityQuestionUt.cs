using BusinessLogic.Commands.Adaptivity.Question;
using BusinessLogic.Entities.LearningContent.Adaptivity;
using BusinessLogic.Entities.LearningContent.Adaptivity.Question;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using Shared.Adaptivity;
using TestHelpers;

namespace BusinessLogicTest.Commands.Adaptivity.Question;

[TestFixture]
public class DeleteAdaptivityQuestionUt
{
    [Test]
    public void Execute_DeletesAdaptivityQuestion()
    {
        // Arrange
        var adaptivityTask = EntityProvider.GetAdaptivityTask();
        var question = EntityProvider.GetMultipleChoiceSingleResponseQuestion();
        adaptivityTask.Questions.Add(question);
        var questionsCount = adaptivityTask.Questions.Count;
        var actionWasInvoked = false;
        Action<AdaptivityTask> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteAdaptivityQuestion(adaptivityTask, question, mappingAction,
            new NullLogger<DeleteAdaptivityQuestion>());

        // Assert before execution
        Assert.Multiple(() =>
        {
            Assert.That(adaptivityTask.Questions, Has.Count.EqualTo(questionsCount));
            Assert.That(adaptivityTask.Questions, Contains.Item(question));
            Assert.That(actionWasInvoked, Is.False);
        });

        // Act
        command.Execute();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(adaptivityTask.Questions, Has.Count.EqualTo(questionsCount - 1));
            Assert.That(adaptivityTask.Questions, Does.Not.Contain(question));
            Assert.That(actionWasInvoked, Is.True);
        });
    }

    [Test]
    public void Execute_UpdatesMinimumRequiredDifficultyAfterDeletion()
    {
        var adaptivityTask = new AdaptivityTask(new List<IAdaptivityQuestion>() { }, null, "taskname");
        ;
        var question1 = EntityProvider.GetMultipleChoiceSingleResponseQuestion();
        question1.Difficulty = QuestionDifficulty.Easy;
        var question2 = EntityProvider.GetMultipleChoiceSingleResponseQuestion();
        question2.Difficulty = QuestionDifficulty.Medium;

        adaptivityTask.Questions.Add(question1);
        adaptivityTask.Questions.Add(question2);
        adaptivityTask.MinimumRequiredDifficulty = QuestionDifficulty.Easy;

        var actionWasInvoked = false;
        Action<AdaptivityTask> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteAdaptivityQuestion(adaptivityTask, question1, mappingAction,
            new NullLogger<DeleteAdaptivityQuestion>());

        Assert.Multiple(() =>
        {
            Assert.That(adaptivityTask.Questions, Has.Count.EqualTo(2));
            Assert.That(adaptivityTask.Questions, Contains.Item(question1));
            Assert.That(adaptivityTask.Questions, Contains.Item(question2));
            Assert.That(adaptivityTask.MinimumRequiredDifficulty, Is.EqualTo(QuestionDifficulty.Easy));
            Assert.That(actionWasInvoked, Is.False);
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(adaptivityTask.Questions, Has.Count.EqualTo(1));
            Assert.That(adaptivityTask.Questions, Does.Not.Contain(question1));
            Assert.That(adaptivityTask.Questions, Contains.Item(question2));
            Assert.That(adaptivityTask.MinimumRequiredDifficulty, Is.EqualTo(QuestionDifficulty.Medium));
            Assert.That(actionWasInvoked, Is.True);
        });
    }

    [Test]
    public void Undo_UndoesDeleteAdaptivityQuestion()
    {
        // Arrange
        var adaptivityTask = EntityProvider.GetAdaptivityTask();
        var question = EntityProvider.GetMultipleChoiceSingleResponseQuestion();
        adaptivityTask.Questions.Add(question);
        var questionsCount = adaptivityTask.Questions.Count;
        var actionWasInvoked = false;
        Action<AdaptivityTask> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteAdaptivityQuestion(adaptivityTask, question, mappingAction,
            new NullLogger<DeleteAdaptivityQuestion>());

        command.Execute();

        // Assert before undo
        Assert.Multiple(() =>
        {
            Assert.That(adaptivityTask.Questions, Has.Count.EqualTo(questionsCount - 1));
            Assert.That(adaptivityTask.Questions, Does.Not.Contain(question));
            Assert.That(actionWasInvoked, Is.True);
        });

        // Act
        command.Undo();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(adaptivityTask.Questions, Has.Count.EqualTo(questionsCount));
            Assert.That(adaptivityTask.Questions, Contains.Item(question));
            Assert.That(actionWasInvoked, Is.True);
        });
    }

    [Test]
    public void Redo_RedoDeletesAdaptivityQuestion()
    {
        // Arrange
        var adaptivityTask = EntityProvider.GetAdaptivityTask();
        var question = EntityProvider.GetMultipleChoiceSingleResponseQuestion();
        adaptivityTask.Questions.Add(question);
        var questionsCount = adaptivityTask.Questions.Count;
        var actionWasInvoked = false;
        Action<AdaptivityTask> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteAdaptivityQuestion(adaptivityTask, question, mappingAction,
            new NullLogger<DeleteAdaptivityQuestion>());

        command.Execute();
        command.Undo();

        // Assert before redo
        Assert.Multiple(() =>
        {
            Assert.That(adaptivityTask.Questions, Has.Count.EqualTo(questionsCount));
            Assert.That(adaptivityTask.Questions, Contains.Item(question));
            Assert.That(actionWasInvoked, Is.True);
        });

        // Act
        command.Redo();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(adaptivityTask.Questions, Has.Count.EqualTo(questionsCount - 1));
            Assert.That(adaptivityTask.Questions, Does.Not.Contain(question));
            Assert.That(actionWasInvoked, Is.True);
        });
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var adaptivityTask = EntityProvider.GetAdaptivityTask();
        var question = adaptivityTask.Questions.First();
        var actionWasInvoked = false;
        Action<AdaptivityTask> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteAdaptivityQuestion(adaptivityTask, question, mappingAction,
            new NullLogger<DeleteAdaptivityQuestion>());

        // Act and Assert
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.Multiple(() =>
        {
            Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
            Assert.That(actionWasInvoked, Is.False);
        });
    }
}