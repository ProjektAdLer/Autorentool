using BusinessLogic.Commands.Adaptivity.Question;
using BusinessLogic.Entities.LearningContent.Adaptivity;
using BusinessLogic.Entities.LearningContent.Adaptivity.Question;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using Shared.Adaptivity;

namespace BusinessLogicTest.Commands.Adaptivity.Question;

[TestFixture]
public class CreateMultipleChoiceSingleResponseQuestionUt
{
    [Test]
    public void Execute_CreatesMultipleChoiceSingleResponseQuestion()
    {
        // Arrange
        var adaptivityTask = new AdaptivityTask(new List<IAdaptivityQuestion>(), QuestionDifficulty.Medium, "Task1");
        var difficulty = QuestionDifficulty.Medium;
        var questionText = "Question";
        var choice1 = new Choice("Choice1");
        var choice2 = new Choice("Choice2");
        var correctChoice = new Choice("CorrectChoice");
        var choices = new List<Choice>() {choice1, choice2, correctChoice};
        var expectedCompletionTime = 10;
        var actionWasInvoked = false;
        Action<AdaptivityTask> mappingAction = _ => actionWasInvoked = true;

        var command = new CreateMultipleChoiceSingleResponseQuestion(adaptivityTask, difficulty, questionText,
            choices, correctChoice, expectedCompletionTime, mappingAction,
            new NullLogger<CreateMultipleChoiceSingleResponseQuestion>());

        // Assert before execution
        Assert.Multiple(() =>
        {
            Assert.That(adaptivityTask.Questions, Is.Empty);
            Assert.That(actionWasInvoked, Is.False);
        });

        // Act
        command.Execute();

        // Assert after execution
        Assert.Multiple(() =>
        {
            Assert.That(adaptivityTask.Questions, Has.Count.EqualTo(1));
            Assert.That(actionWasInvoked, Is.True);
        });

        var createdQuestion = adaptivityTask.Questions.First() as MultipleChoiceSingleResponseQuestion;
        Assert.That(createdQuestion, Is.Not.Null);
        Assert.That(createdQuestion!.Text, Is.EqualTo(questionText));
        Assert.That(createdQuestion.Choices, Is.EqualTo(choices));
        Assert.That(createdQuestion.Choices.Count, Is.EqualTo(3));
        Assert.That(createdQuestion.Choices.First(), Is.EqualTo(choice1));
        Assert.That(createdQuestion.CorrectChoice, Is.EqualTo(correctChoice));
    }

    [Test]
    public void Undo_UndoesCreateMultipleChoiceSingleResponseQuestion()
    {
        // Arrange
        var adaptivityTask = new AdaptivityTask(new List<IAdaptivityQuestion>(), QuestionDifficulty.Medium, "Task1");
        var difficulty = QuestionDifficulty.Medium;
        var questionText = "Question";
        var choice1 = new Choice("Choice1");
        var choice2 = new Choice("Choice2");
        var correctChoice = new Choice("CorrectChoice");
        var choices = new List<Choice>() {choice1, choice2, correctChoice};
        var expectedCompletionTime = 10;
        var actionWasInvoked = false;
        Action<AdaptivityTask> mappingAction = _ => actionWasInvoked = true;

        var command = new CreateMultipleChoiceSingleResponseQuestion(adaptivityTask, difficulty, questionText,
            choices, correctChoice, expectedCompletionTime, mappingAction,
            new NullLogger<CreateMultipleChoiceSingleResponseQuestion>());

        // Act
        command.Execute();
        actionWasInvoked = false; // Reset actionWasInvoked

        // Assert before undo
        Assert.Multiple(() =>
        {
            Assert.That(adaptivityTask.Questions, Has.Count.EqualTo(1));
            Assert.That(actionWasInvoked, Is.False);
        });

        // Undo
        command.Undo();

        // Assert after undo
        Assert.Multiple(() =>
        {
            Assert.That(adaptivityTask.Questions, Is.Empty);
            Assert.That(actionWasInvoked, Is.True);
        });
    }

    [Test]
    public void Redo_RedoCreatesMultipleChoiceSingleResponseQuestion()
    {
        // Arrange
        var adaptivityTask = new AdaptivityTask(new List<IAdaptivityQuestion>(), QuestionDifficulty.Medium, "Task1");
        var difficulty = QuestionDifficulty.Medium;
        var questionText = "Question";
        var choice1 = new Choice("Choice1");
        var choice2 = new Choice("Choice2");
        var correctChoice = new Choice("CorrectChoice");
        var choices = new List<Choice>() {choice1, choice2, correctChoice};
        var expectedCompletionTime = 10;
        var actionWasInvoked = false;
        Action<AdaptivityTask> mappingAction = _ => actionWasInvoked = true;

        var command = new CreateMultipleChoiceSingleResponseQuestion(adaptivityTask, difficulty, questionText,
            choices, correctChoice, expectedCompletionTime, mappingAction,
            new NullLogger<CreateMultipleChoiceSingleResponseQuestion>());

        // Execute and undo
        command.Execute();
        command.Undo();
        actionWasInvoked = false; // Reset actionWasInvoked

        // Assert before redo
        Assert.Multiple(() =>
        {
            Assert.That(adaptivityTask.Questions, Is.Empty);
            Assert.That(actionWasInvoked, Is.False);
        });

        // Redo
        command.Redo();

        // Assert after redo
        Assert.Multiple(() =>
        {
            Assert.That(adaptivityTask.Questions, Has.Count.EqualTo(1));
            Assert.That(actionWasInvoked, Is.True);
        });

        var createdQuestion = adaptivityTask.Questions.First() as MultipleChoiceSingleResponseQuestion;
        Assert.That(createdQuestion, Is.Not.Null);
        Assert.That(createdQuestion!.Text, Is.EqualTo(questionText));
        Assert.That(createdQuestion.Choices, Is.EqualTo(choices));
        Assert.That(createdQuestion.Choices.Count, Is.EqualTo(3));
        Assert.That(createdQuestion.Choices.First(), Is.EqualTo(choice1));
        Assert.That(createdQuestion.CorrectChoice, Is.EqualTo(correctChoice));
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        // Arrange
        var adaptivityTask = new AdaptivityTask(new List<IAdaptivityQuestion>(), QuestionDifficulty.Medium, "Task1");
        var difficulty = QuestionDifficulty.Medium;
        var questionText = "Question";
        var choices = new List<Choice>();
        var correctChoice = new Choice("CorrectChoice");
        var expectedCompletionTime = 10;
        Action<AdaptivityTask> mappingAction = _ => { };

        var command = new CreateMultipleChoiceSingleResponseQuestion(adaptivityTask, difficulty, questionText,
            choices, correctChoice, expectedCompletionTime, mappingAction,
            new NullLogger<CreateMultipleChoiceSingleResponseQuestion>());

        // Act and assert
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
    }
}