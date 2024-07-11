using BusinessLogic.Commands.Adaptivity.Question;
using BusinessLogic.Entities.LearningContent.Adaptivity.Question;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using TestHelpers;

namespace BusinessLogicTest.Commands.Adaptivity.Question;

[TestFixture]
public class EditMultipleChoiceMultipleResponseQuestionUt
{
    [Test]
    // ANF-ID: [AWA0008]
    public void Execute_EditsMultipleChoiceMultipleResponseQuestion()
    {
        // Arrange
        var question = EntityProvider.GetMultipleChoiceMultipleResponseQuestion();
        var questionText = "NewQuestionText";
        var choice1 = new Choice("Choice1");
        var choice2 = new Choice("Choice2");
        var correctChoice1 = new Choice("CorrectChoice1");
        var correctChoice2 = new Choice("CorrectChoice2");
        var choices = new List<Choice>() { choice1, correctChoice1, choice2, correctChoice2 };
        var correctChoices = new List<Choice>() { correctChoice1, correctChoice2 };
        var expectedCompletionTime = 10;
        var actionWasInvoked = false;
        Action<MultipleChoiceMultipleResponseQuestion> mappingAction = _ => actionWasInvoked = true;

        var command = new EditMultipleChoiceMultipleResponseQuestion(question, questionText,
            choices, correctChoices, expectedCompletionTime, mappingAction,
            new NullLogger<EditMultipleChoiceMultipleResponseQuestion>());

        // Assert before execution
        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(question.Text, Is.Not.EqualTo(questionText));
            Assert.That(question.Choices, Is.Not.EqualTo(choices));
            Assert.That(question.Choices, Has.Count.Not.EqualTo(4));
            Assert.That(question.Choices.First(), Is.Not.EqualTo(choice1));
            Assert.That(question.CorrectChoices, Is.Not.EqualTo(correctChoices));
            Assert.That(question.CorrectChoices, Has.Count.Not.EqualTo(2));
            Assert.That(question.CorrectChoices.First(), Is.Not.EqualTo(correctChoice1));
        });

        // Act
        command.Execute();

        // Assert after execution
        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(question.Text, Is.EqualTo(questionText));
            Assert.That(question.Choices, Is.EqualTo(choices));
            Assert.That(question.Choices, Has.Count.EqualTo(4));
            Assert.That(question.Choices.First(), Is.EqualTo(choice1));
            Assert.That(question.CorrectChoices, Is.EqualTo(correctChoices));
            Assert.That(question.CorrectChoices, Has.Count.EqualTo(2));
            Assert.That(question.CorrectChoices.First(), Is.EqualTo(correctChoice1));
        });
    }

    [Test]
    public void Undo_UndoesEditMultipleChoiceMultipleResponseQuestion()
    {
        // Arrange
        var question = EntityProvider.GetMultipleChoiceMultipleResponseQuestion();
        var oldQuestionText = question.Text;
        var oldChoices = question.Choices;
        var oldCorrectChoices = question.CorrectChoices;
        var oldExpectedCompletionTime = question.ExpectedCompletionTime;

        var questionText = "NewQuestionText";
        var choice1 = new Choice("Choice1");
        var choice2 = new Choice("Choice2");
        var correctChoice1 = new Choice("CorrectChoice1");
        var correctChoice2 = new Choice("CorrectChoice2");
        var choices = new List<Choice>() { choice1, correctChoice1, choice2, correctChoice2 };
        var correctChoices = new List<Choice>() { correctChoice1, correctChoice2 };
        var expectedCompletionTime = 10;
        var actionWasInvoked = false;
        Action<MultipleChoiceMultipleResponseQuestion> mappingAction = _ => actionWasInvoked = true;

        var command = new EditMultipleChoiceMultipleResponseQuestion(question, questionText,
            choices, correctChoices, expectedCompletionTime, mappingAction,
            new NullLogger<EditMultipleChoiceMultipleResponseQuestion>());

        command.Execute();

        // Assert before undo
        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(question.Text, Is.EqualTo(questionText));
            Assert.That(question.Choices, Is.EqualTo(choices));
            Assert.That(question.Choices.Count, Is.EqualTo(4));
            Assert.That(question.Choices.First(), Is.EqualTo(choice1));
            Assert.That(question.CorrectChoices, Is.EqualTo(correctChoices));
            Assert.That(question.CorrectChoices.Count, Is.EqualTo(2));
            Assert.That(question.CorrectChoices.First(), Is.EqualTo(correctChoice1));
        });

        // Act
        command.Undo();

        // Assert after undo
        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(question.Text, Is.EqualTo(oldQuestionText));
            Assert.That(question.Choices, Is.EqualTo(oldChoices));
            Assert.That(question.Choices, Has.Count.EqualTo(oldChoices.Count));
            Assert.That(question.Choices.First(), Is.EqualTo(oldChoices.First()));
            Assert.That(question.CorrectChoices, Is.EqualTo(oldCorrectChoices));
            Assert.That(question.CorrectChoices, Has.Count.EqualTo(oldCorrectChoices.Count));
            Assert.That(question.CorrectChoices.First(), Is.EqualTo(oldCorrectChoices.First()));
        });
    }

    [Test]
    public void Redo_RedoEditsMultipleChoiceMultipleResponseQuestion()
    {
        // Arrange
        var question = EntityProvider.GetMultipleChoiceMultipleResponseQuestion();
        var oldQuestionText = question.Text;
        var oldChoices = question.Choices;
        var oldCorrectChoices = question.CorrectChoices;
        var oldExpectedCompletionTime = question.ExpectedCompletionTime;

        var questionText = "NewQuestionText";
        var choice1 = new Choice("Choice1");
        var choice2 = new Choice("Choice2");
        var correctChoice1 = new Choice("CorrectChoice1");
        var correctChoice2 = new Choice("CorrectChoice2");
        var choices = new List<Choice>() { choice1, correctChoice1, choice2, correctChoice2 };
        var correctChoices = new List<Choice>() { correctChoice1, correctChoice2 };
        var expectedCompletionTime = 10;
        var actionWasInvoked = false;
        Action<MultipleChoiceMultipleResponseQuestion> mappingAction = _ => actionWasInvoked = true;

        var command = new EditMultipleChoiceMultipleResponseQuestion(question, questionText,
            choices, correctChoices, expectedCompletionTime, mappingAction,
            new NullLogger<EditMultipleChoiceMultipleResponseQuestion>());

        command.Execute();
        command.Undo();

        // Assert before redo
        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(question.Text, Is.EqualTo(oldQuestionText));
            Assert.That(question.Choices, Is.EqualTo(oldChoices));
            Assert.That(question.Choices, Has.Count.EqualTo(oldChoices.Count));
            Assert.That(question.Choices.First(), Is.EqualTo(oldChoices.First()));
            Assert.That(question.CorrectChoices, Is.EqualTo(oldCorrectChoices));
            Assert.That(question.CorrectChoices, Has.Count.EqualTo(oldCorrectChoices.Count));
            Assert.That(question.CorrectChoices.First(), Is.EqualTo(oldCorrectChoices.First()));
        });

        // Act
        command.Redo();

        // Assert after redo
        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(question.Text, Is.EqualTo(questionText));
            Assert.That(question.Choices, Is.EqualTo(choices));
            Assert.That(question.Choices, Has.Count.EqualTo(4));
            Assert.That(question.Choices.First(), Is.EqualTo(choice1));
            Assert.That(question.CorrectChoices, Is.EqualTo(correctChoices));
            Assert.That(question.CorrectChoices, Has.Count.EqualTo(2));
            Assert.That(question.CorrectChoices.First(), Is.EqualTo(correctChoice1));
        });
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var question = EntityProvider.GetMultipleChoiceMultipleResponseQuestion();
        var questionText = "NewQuestionText";
        var choices = new List<Choice>() { new Choice("Choice1"), new Choice("Choice2") };
        var correctChoices = new List<Choice>() { new Choice("CorrectChoice1"), new Choice("CorrectChoice2") };
        var expectedCompletionTime = 10;
        var actionWasInvoked = false;
        Action<MultipleChoiceMultipleResponseQuestion> mappingAction = _ => actionWasInvoked = true;

        var command = new EditMultipleChoiceMultipleResponseQuestion(question, questionText,
            choices, correctChoices, expectedCompletionTime, mappingAction,
            new NullLogger<EditMultipleChoiceMultipleResponseQuestion>());

        // Act and Assert
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.Multiple(() =>
        {
            Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
            Assert.That(actionWasInvoked, Is.False);
        });
    }
}