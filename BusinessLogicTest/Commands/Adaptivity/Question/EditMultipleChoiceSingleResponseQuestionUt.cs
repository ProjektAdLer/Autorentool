using BusinessLogic.Commands.Adaptivity.Question;
using BusinessLogic.Entities.LearningContent.Adaptivity.Question;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using TestHelpers;

namespace BusinessLogicTest.Commands.Adaptivity.Question;

[TestFixture]
public class EditMultipleChoiceSingleResponseQuestionUt
{
    [Test]
    public void Execute_EditsMultipleChoiceSingleResponseQuestion()
    {
        // Arrange
        var question = EntityProvider.GetMultipleChoiceSingleResponseQuestion();
        var questionText = "NewQuestionText";
        var choice1 = new Choice("Choice1");
        var choice2 = new Choice("Choice2");
        var correctChoice = new Choice("CorrectChoice");
        var choices = new List<Choice>() {choice1, correctChoice, choice2};
        var expectedCompletionTime = 10;
        var actionWasInvoked = false;
        Action<MultipleChoiceSingleResponseQuestion> mappingAction = _ => actionWasInvoked = true;

        var command = new EditMultipleChoiceSingleResponseQuestion(question, questionText,
            choices, correctChoice, expectedCompletionTime, mappingAction,
            new NullLogger<EditMultipleChoiceSingleResponseQuestion>());

        // Assert before execution
        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(question.Text, Is.Not.EqualTo(questionText));
            Assert.That(question.Choices, Is.Not.EqualTo(choices));
            Assert.That(question.Choices.Count, Is.Not.EqualTo(3));
            Assert.That(question.Choices.First(), Is.Not.EqualTo(choice1));
            Assert.That(question.CorrectChoice, Is.Not.EqualTo(correctChoice));
        });

        // Act
        command.Execute();

        // Assert after execution
        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(question.Text, Is.EqualTo(questionText));
            Assert.That(question.Choices, Is.EqualTo(choices));
            Assert.That(question.Choices.Count, Is.EqualTo(3));
            Assert.That(question.Choices.First(), Is.EqualTo(choice1));
            Assert.That(question.CorrectChoice, Is.EqualTo(correctChoice));
        });
    }

    [Test]
    public void Undo_UndoesEditMultipleChoiceSingleResponseQuestion()
    {
        // Arrange
        var question = EntityProvider.GetMultipleChoiceSingleResponseQuestion();
        var oldQuestionText = question.Text;
        var oldChoices = question.Choices;
        var oldCorrectChoice = question.CorrectChoice;
        var oldExpectedCompletionTime = question.ExpectedCompletionTime;

        var questionText = "NewQuestionText";
        var choice1 = new Choice("Choice1");
        var choice2 = new Choice("Choice2");
        var correctChoice = new Choice("CorrectChoice");
        var choices = new List<Choice>() {choice1, correctChoice, choice2};
        var expectedCompletionTime = 10;
        var actionWasInvoked = false;
        Action<MultipleChoiceSingleResponseQuestion> mappingAction = _ => actionWasInvoked = true;

        var command = new EditMultipleChoiceSingleResponseQuestion(question, questionText,
            choices, correctChoice, expectedCompletionTime, mappingAction,
            new NullLogger<EditMultipleChoiceSingleResponseQuestion>());

        command.Execute();

        // Assert before undo
        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(question.Text, Is.EqualTo(questionText));
            Assert.That(question.Choices, Is.EqualTo(choices));
            Assert.That(question.Choices.Count, Is.EqualTo(3));
            Assert.That(question.Choices.First(), Is.EqualTo(choice1));
            Assert.That(question.CorrectChoice, Is.EqualTo(correctChoice));
        });

        // Act
        command.Undo();

        // Assert after undo
        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(question.Text, Is.EqualTo(oldQuestionText));
            Assert.That(question.Choices, Is.EqualTo(oldChoices));
            Assert.That(question.Choices.Count, Is.EqualTo(oldChoices.Count));
            Assert.That(question.Choices.First(), Is.EqualTo(oldChoices.First()));
            Assert.That(question.CorrectChoice, Is.EqualTo(oldCorrectChoice));
        });
    }

    [Test]
    public void Redo_RedoEditsMultipleChoiceSingleResponseQuestion()
    {
        // Arrange
        var question = EntityProvider.GetMultipleChoiceSingleResponseQuestion();
        var oldQuestionText = question.Text;
        var oldChoices = question.Choices;
        var oldCorrectChoice = question.CorrectChoice;
        var oldExpectedCompletionTime = question.ExpectedCompletionTime;

        var questionText = "NewQuestionText";
        var choice1 = new Choice("Choice1");
        var choice2 = new Choice("Choice2");
        var correctChoice = new Choice("CorrectChoice");
        var choices = new List<Choice>() {choice1, correctChoice, choice2};
        var expectedCompletionTime = 10;
        var actionWasInvoked = false;
        Action<MultipleChoiceSingleResponseQuestion> mappingAction = _ => actionWasInvoked = true;

        var command = new EditMultipleChoiceSingleResponseQuestion(question, questionText,
            choices, correctChoice, expectedCompletionTime, mappingAction,
            new NullLogger<EditMultipleChoiceSingleResponseQuestion>());

        command.Execute();
        command.Undo();

        // Assert before redo
        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(question.Text, Is.EqualTo(oldQuestionText));
            Assert.That(question.Choices, Is.EqualTo(oldChoices));
            Assert.That(question.Choices.Count, Is.EqualTo(oldChoices.Count));
            Assert.That(question.Choices.First(), Is.EqualTo(oldChoices.First()));
            Assert.That(question.CorrectChoice, Is.EqualTo(oldCorrectChoice));
        });

        // Act
        command.Redo();

        // Assert after redo
        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(question.Text, Is.EqualTo(questionText));
            Assert.That(question.Choices, Is.EqualTo(choices));
            Assert.That(question.Choices.Count, Is.EqualTo(3));
            Assert.That(question.Choices.First(), Is.EqualTo(choice1));
            Assert.That(question.CorrectChoice, Is.EqualTo(correctChoice));
        });
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var question = EntityProvider.GetMultipleChoiceSingleResponseQuestion();
        var questionText = "NewQuestionText";
        var choice1 = new Choice("Choice1");
        var choice2 = new Choice("Choice2");
        var correctChoice = new Choice("CorrectChoice");
        var choices = new List<Choice>() {choice1, correctChoice, choice2};
        var expectedCompletionTime = 10;
        var actionWasInvoked = false;
        Action<MultipleChoiceSingleResponseQuestion> mappingAction = _ => actionWasInvoked = true;

        var command = new EditMultipleChoiceSingleResponseQuestion(question, questionText,
            choices, correctChoice, expectedCompletionTime, mappingAction,
            new NullLogger<EditMultipleChoiceSingleResponseQuestion>());

        // Act and Assert
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.Multiple(() =>
        {
            Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
            Assert.That(actionWasInvoked, Is.False);
        });
    }
}