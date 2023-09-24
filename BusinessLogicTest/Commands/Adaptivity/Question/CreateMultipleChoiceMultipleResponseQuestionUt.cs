using BusinessLogic.Commands.Adaptivity.Question;
using BusinessLogic.Entities.LearningContent.Adaptivity;
using BusinessLogic.Entities.LearningContent.Adaptivity.Question;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using Shared.Adaptivity;
using TestHelpers;

namespace BusinessLogicTest.Commands.Adaptivity.Question;

[TestFixture]
public class CreateMultipleChoiceMultipleResponseQuestionUt
{
    [Test]
    public void Execute_CreatesMultipleChoiceMultipleResponseQuestion()
    {
        // Arrange
        var adaptivityTask = EntityProvider.GetAdaptivityTask();
        var questionsCount = adaptivityTask.Questions.Count;
        var difficulty = QuestionDifficulty.Medium;
        var questionText = "Question";
        var choice1 = new Choice("Choice1");
        var choice2 = new Choice("Choice2");
        var correctChoice1 = new Choice("CorrectChoice1");
        var correctChoice2 = new Choice("CorrectChoice2");
        var choices = new List<Choice>() {choice1, correctChoice1, choice2, correctChoice2};
        var correctChoices = new List<Choice>() {correctChoice1, correctChoice2};
        var expectedCompletionTime = 10;
        var actionWasInvoked = false;
        Action<AdaptivityTask> mappingAction = _ => actionWasInvoked = true;

        var command = new CreateMultipleChoiceMultipleResponseQuestion(adaptivityTask, difficulty, questionText,
            choices, correctChoices, expectedCompletionTime, mappingAction,
            new NullLogger<CreateMultipleChoiceMultipleResponseQuestion>());

        // Assert before execution
        Assert.Multiple(() =>
        {
            Assert.That(adaptivityTask.Questions, Has.Count.EqualTo(questionsCount));
            Assert.That(adaptivityTask.Questions, Does.Not.Contain(command.Question));
            Assert.That(actionWasInvoked, Is.False);
        });

        // Act
        command.Execute();

        // Assert after execution
        Assert.Multiple(() =>
        {
            Assert.That(adaptivityTask.Questions, Has.Count.EqualTo(questionsCount + 1));
            Assert.That(adaptivityTask.Questions, Does.Contain(command.Question));
            Assert.That(actionWasInvoked, Is.True);
        });

        var createdQuestion = adaptivityTask.Questions.Last() as MultipleChoiceMultipleResponseQuestion;
        Assert.That(createdQuestion, Is.Not.Null);
        Assert.That(createdQuestion!.Text, Is.EqualTo(questionText));
        Assert.That(createdQuestion.Choices, Is.EqualTo(choices));
        Assert.That(createdQuestion.Choices.Count, Is.EqualTo(4));
        Assert.That(createdQuestion.Choices.First(), Is.EqualTo(choice1));
        Assert.That(createdQuestion.CorrectChoices, Is.EqualTo(correctChoices));
        Assert.That(createdQuestion.CorrectChoices.Count, Is.EqualTo(2));
        Assert.That(createdQuestion.CorrectChoices.First(), Is.EqualTo(correctChoice1));
    }

    [Test]
    public void Undo_UndoesCreateMultipleChoiceMultipleResponseQuestion()
    {
        // Arrange
        var adaptivityTask = EntityProvider.GetAdaptivityTask();
        var questionsCount = adaptivityTask.Questions.Count;
        var difficulty = QuestionDifficulty.Medium;
        var questionText = "Question";
        var choice1 = new Choice("Choice1");
        var choice2 = new Choice("Choice2");
        var correctChoice1 = new Choice("CorrectChoice1");
        var correctChoice2 = new Choice("CorrectChoice2");
        var choices = new List<Choice>() {choice1, correctChoice1, choice2, correctChoice2};
        var correctChoices = new List<Choice>() {correctChoice1, correctChoice2};
        var expectedCompletionTime = 10;
        var actionWasInvoked = false;
        Action<AdaptivityTask> mappingAction = _ => actionWasInvoked = true;

        var command = new CreateMultipleChoiceMultipleResponseQuestion(adaptivityTask, difficulty, questionText,
            choices, correctChoices, expectedCompletionTime, mappingAction,
            new NullLogger<CreateMultipleChoiceMultipleResponseQuestion>());

        // Act
        command.Execute();
        actionWasInvoked = false; // Reset actionWasInvoked

        // Assert before undo
        Assert.Multiple(() =>
        {
            Assert.That(adaptivityTask.Questions, Has.Count.EqualTo(questionsCount + 1));
            Assert.That(adaptivityTask.Questions, Does.Contain(command.Question));
            Assert.That(actionWasInvoked, Is.False);
        });

        // Undo
        command.Undo();

        // Assert after undo
        Assert.Multiple(() =>
        {
            Assert.That(adaptivityTask.Questions, Has.Count.EqualTo(questionsCount));
            Assert.That(adaptivityTask.Questions, Does.Not.Contain(command.Question));
            Assert.That(actionWasInvoked, Is.True);
        });
    }

    [Test]
    public void Redo_RedoCreatesMultipleChoiceMultipleResponseQuestion()
    {
        // Arrange
        var adaptivityTask = EntityProvider.GetAdaptivityTask();
        var questionsCount = adaptivityTask.Questions.Count;
        var difficulty = QuestionDifficulty.Medium;
        var questionText = "Question";
        var choice1 = new Choice("Choice1");
        var choice2 = new Choice("Choice2");
        var correctChoice1 = new Choice("CorrectChoice1");
        var correctChoice2 = new Choice("CorrectChoice2");
        var choices = new List<Choice>() {choice1, correctChoice1, choice2, correctChoice2};
        var correctChoices = new List<Choice>() {correctChoice1, correctChoice2};
        var expectedCompletionTime = 10;
        var actionWasInvoked = false;
        Action<AdaptivityTask> mappingAction = _ => actionWasInvoked = true;

        var command = new CreateMultipleChoiceMultipleResponseQuestion(adaptivityTask, difficulty, questionText,
            choices, correctChoices, expectedCompletionTime, mappingAction,
            new NullLogger<CreateMultipleChoiceMultipleResponseQuestion>());

        // Execute and undo
        command.Execute();
        command.Undo();
        actionWasInvoked = false; // Reset actionWasInvoked

        // Assert before redo
        Assert.Multiple(() =>
        {
            Assert.That(adaptivityTask.Questions, Has.Count.EqualTo(questionsCount));
            Assert.That(adaptivityTask.Questions, Does.Not.Contain(command.Question));
            Assert.That(actionWasInvoked, Is.False);
        });

        // Redo
        command.Redo();

        // Assert after redo
        Assert.Multiple(() =>
        {
            Assert.That(adaptivityTask.Questions, Has.Count.EqualTo(questionsCount + 1));
            Assert.That(adaptivityTask.Questions, Does.Contain(command.Question));
            Assert.That(actionWasInvoked, Is.True);
        });

        var createdQuestion = adaptivityTask.Questions.Last() as MultipleChoiceMultipleResponseQuestion;
        Assert.That(createdQuestion, Is.Not.Null);
        Assert.That(createdQuestion!.Text, Is.EqualTo(questionText));
        Assert.That(createdQuestion.Choices, Is.EqualTo(choices));
        Assert.That(createdQuestion.Choices.Count, Is.EqualTo(4));
        Assert.That(createdQuestion.Choices.First(), Is.EqualTo(choice1));
        Assert.That(createdQuestion.CorrectChoices, Is.EqualTo(correctChoices));
        Assert.That(createdQuestion.CorrectChoices.Count, Is.EqualTo(2));
        Assert.That(createdQuestion.CorrectChoices.First(), Is.EqualTo(correctChoice1));
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        // Arrange
        var adaptivityTask = EntityProvider.GetAdaptivityTask();
        var difficulty = QuestionDifficulty.Medium;
        var questionText = "Question";
        var choices = new List<Choice>();
        var correctChoices = new List<Choice>();
        var expectedCompletionTime = 10;
        Action<AdaptivityTask> mappingAction = _ => { };

        var command = new CreateMultipleChoiceMultipleResponseQuestion(adaptivityTask, difficulty, questionText,
            choices, correctChoices, expectedCompletionTime, mappingAction,
            new NullLogger<CreateMultipleChoiceMultipleResponseQuestion>());

        // Act and assert
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
    }
}