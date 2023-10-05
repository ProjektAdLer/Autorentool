using BusinessLogic.Commands.Adaptivity.Question;
using BusinessLogic.Entities.LearningContent.Adaptivity;
using BusinessLogic.Entities.LearningContent.Adaptivity.Question;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using TestHelpers;

namespace BusinessLogicTest.Commands.Adaptivity.Question;

[TestFixture]
public class EditMultipleChoiceQuestionWithTypeChangeUt
{
    [Test]
    public void Execute_MultipleResponseToSingleResponse_DeletesOldQuestionAndCreateSingleResponseQuestion()
    {
        // Arrange
        var task = EntityProvider.GetAdaptivityTask();
        var oldQuestion = EntityProvider.GetMultipleChoiceMultipleResponseQuestion();
        task.Questions.Add(oldQuestion);
        var questionsCount = task.Questions.Count;
        var isSingleResponse = true;
        var title = "NewQuestionTitle";
        var text = "NewQuestionText";
        var choice1 = new Choice("Choice1");
        var choice2 = new Choice("Choice2");
        var correctChoice = new Choice("CorrectChoice");
        var choices = new List<Choice>() {choice1, choice2, correctChoice};
        var correctChoices = new List<Choice>() {correctChoice};
        var expectedCompletionTime = 10;
        var actionWasInvoked = false;
        Action<AdaptivityTask> mappingAction = _ => actionWasInvoked = true;

        var command = new EditMultipleChoiceQuestionWithTypeChange(task, oldQuestion, isSingleResponse, title, text,
            choices, correctChoices, expectedCompletionTime, mappingAction,
            new NullLogger<EditMultipleChoiceQuestionWithTypeChange>());

        // Assert before execution
        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(task.Questions, Has.Count.EqualTo(questionsCount));
            Assert.That(task.Questions, Does.Contain(oldQuestion));
            Assert.That(oldQuestion.Title, Is.Not.EqualTo(title));
            Assert.That(oldQuestion.Text, Is.Not.EqualTo(text));
            Assert.That(oldQuestion.Choices, Is.Not.EqualTo(choices));
            Assert.That(oldQuestion.CorrectChoices, Is.Not.EqualTo(correctChoices));
        });

        // Act
        command.Execute();

        // Assert after execution
        Assert.That(command.SingleResponseQuestion, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(task.Questions, Has.Count.EqualTo(questionsCount));
            Assert.That(task.Questions, Does.Not.Contain(oldQuestion));
            Assert.That(task.Questions, Does.Contain(command.SingleResponseQuestion));
            Assert.That(command.SingleResponseQuestion!.Title, Is.EqualTo(title));
            Assert.That(command.SingleResponseQuestion.Text, Is.EqualTo(text));
            Assert.That(command.SingleResponseQuestion.Choices, Is.EqualTo(choices));
            Assert.That(command.SingleResponseQuestion.CorrectChoice, Is.EqualTo(correctChoice));
            Assert.That(command.SingleResponseQuestion.ExpectedCompletionTime, Is.EqualTo(expectedCompletionTime));
        });
    }

    [Test]
    public void Execute_SingleResponseToMultipleResponse_DeletesOldQuestionAndCreateMultipleResponseQuestion()
    {
        // Arrange
        var task = EntityProvider.GetAdaptivityTask();
        var oldQuestion = EntityProvider.GetMultipleChoiceSingleResponseQuestion();
        task.Questions.Add(oldQuestion);
        var questionsCount = task.Questions.Count;
        var isSingleResponse = false;
        var title = "NewQuestionTitle";
        var text = "NewQuestionText";
        var choice1 = new Choice("Choice1");
        var choice2 = new Choice("Choice2");
        var correctChoice1 = new Choice("CorrectChoice1");
        var correctChoice2 = new Choice("CorrectChoice2");
        var choices = new List<Choice>() {choice1, choice2};
        var correctChoices = new List<Choice>() {correctChoice1, correctChoice2};
        var expectedCompletionTime = 10;
        var actionWasInvoked = false;
        Action<AdaptivityTask> mappingAction = _ => actionWasInvoked = true;

        var command = new EditMultipleChoiceQuestionWithTypeChange(task, oldQuestion, isSingleResponse, title, text,
            choices, correctChoices, expectedCompletionTime, mappingAction,
            new NullLogger<EditMultipleChoiceQuestionWithTypeChange>());

        // Assert before execution
        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(task.Questions, Has.Count.EqualTo(questionsCount));
            Assert.That(task.Questions, Does.Contain(oldQuestion));
            Assert.That(oldQuestion.Title, Is.Not.EqualTo(title));
            Assert.That(oldQuestion.Text, Is.Not.EqualTo(text));
            Assert.That(oldQuestion.Choices, Is.Not.EqualTo(choices));
            Assert.That(oldQuestion.CorrectChoice, Is.Not.EqualTo(correctChoice1));
            Assert.That(oldQuestion.CorrectChoice, Is.Not.EqualTo(correctChoice2));
        });

        // Act
        command.Execute();

        // Assert after execution
        Assert.That(command.MultipleResponseQuestion, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(task.Questions, Has.Count.EqualTo(questionsCount));
            Assert.That(task.Questions, Does.Not.Contain(oldQuestion));
            Assert.That(task.Questions, Does.Contain(command.MultipleResponseQuestion));
            Assert.That(command.MultipleResponseQuestion!.Title, Is.EqualTo(title));
            Assert.That(command.MultipleResponseQuestion.Text, Is.EqualTo(text));
            Assert.That(command.MultipleResponseQuestion.Choices, Is.EqualTo(choices));
            Assert.That(command.MultipleResponseQuestion.CorrectChoices, Is.EqualTo(correctChoices));
            Assert.That(command.MultipleResponseQuestion.ExpectedCompletionTime, Is.EqualTo(expectedCompletionTime));
        });
    }

    [Test]
    public void Execute_InvalidParameters_ThrowsInvalidOperationException()
    {
        // Arrange
        var task = EntityProvider.GetAdaptivityTask();
        var oldQuestion = EntityProvider.GetMultipleChoiceSingleResponseQuestion();
        task.Questions.Add(oldQuestion);
        var isSingleResponse = true;
        var title = "NewQuestionTitle";
        var text = "NewQuestionText";
        var choices = new List<Choice>() {new Choice("Choice1"), new Choice("Choice2")};
        var correctChoices = new List<Choice>() {new Choice("CorrectChoice1"), new Choice("CorrectChoice2")};
        var expectedCompletionTime = 10;
        Action<AdaptivityTask> mappingAction = _ => { };

        // Act and Assert
        var ex = Assert.Throws<InvalidOperationException>(() =>
        {
            var command = new EditMultipleChoiceQuestionWithTypeChange(task, oldQuestion, isSingleResponse, title, text,
                choices, correctChoices, expectedCompletionTime, mappingAction,
                new NullLogger<EditMultipleChoiceQuestionWithTypeChange>());
        });
        Assert.That(ex!.Message,
            Is.EqualTo(
                $"Cannot change type of question {oldQuestion.Id} from {oldQuestion.GetType().Name} to {isSingleResponse}. The question has already this type."));
    }

    [Test]
    public void Execute_QuestionDoesNotExistInTask_ThrowsInvalidOperationException()
    {
        // Arrange
        var task = EntityProvider.GetAdaptivityTask();
        var oldQuestion = EntityProvider.GetMultipleChoiceSingleResponseQuestion();
        var questionsCount = task.Questions.Count;
        var isSingleResponse = false;
        var title = "NewQuestionTitle";
        var text = "NewQuestionText";
        var choices = new List<Choice>() {new Choice("Choice1"), new Choice("Choice2")};
        var correctChoices = new List<Choice>() {new Choice("CorrectChoice1"), new Choice("CorrectChoice2")};
        var expectedCompletionTime = 10;
        var actionWasInvoked = false;
        Action<AdaptivityTask> mappingAction = _ => actionWasInvoked = true;

        var command = new EditMultipleChoiceQuestionWithTypeChange(task, oldQuestion, isSingleResponse, title, text,
            choices, correctChoices, expectedCompletionTime, mappingAction,
            new NullLogger<EditMultipleChoiceQuestionWithTypeChange>());

        // Assert before execution
        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(task.Questions, Has.Count.EqualTo(questionsCount));
            Assert.That(task.Questions, Does.Not.Contain(oldQuestion));
        });

        // Act and Assert
        var ex = Assert.Throws<InvalidOperationException>(() => command.Execute());
        Assert.Multiple(() =>
        {
            Assert.That(ex!.Message,
                Is.EqualTo(
                    $"Cannot change type of question {oldQuestion.Id} from {oldQuestion.GetType().Name} to {isSingleResponse}. The question does not exist in the Task."));
            Assert.That(actionWasInvoked, Is.False);
        });
    }

    [Test]
    public void Undo_MultipleResponseToSingleResponse_UndoesDeletesOldQuestionAndCreateSingleResponseQuestion()
    {
        // Arrange
        var task = EntityProvider.GetAdaptivityTask();
        var oldQuestion = EntityProvider.GetMultipleChoiceMultipleResponseQuestion();
        task.Questions.Add(oldQuestion);
        var questionsCount = task.Questions.Count;
        var oldQuestionTitle = oldQuestion.Title;
        var oldQuestionText = oldQuestion.Text;
        var oldChoices = oldQuestion.Choices;
        var oldCorrectChoices = oldQuestion.CorrectChoices;
        var oldExpectedCompletionTime = oldQuestion.ExpectedCompletionTime;
        var isSingleResponse = true;
        var title = "NewQuestionTitle";
        var text = "NewQuestionText";
        var choice1 = new Choice("Choice1");
        var choice2 = new Choice("Choice2");
        var correctChoice = new Choice("CorrectChoice");
        var choices = new List<Choice>() {choice1, choice2, correctChoice};
        var correctChoices = new List<Choice>() {correctChoice};
        var expectedCompletionTime = 10;
        var actionWasInvoked = false;
        Action<AdaptivityTask> mappingAction = _ => actionWasInvoked = true;

        var command = new EditMultipleChoiceQuestionWithTypeChange(task, oldQuestion, isSingleResponse, title, text,
            choices, correctChoices, expectedCompletionTime, mappingAction,
            new NullLogger<EditMultipleChoiceQuestionWithTypeChange>());

        command.Execute();
        actionWasInvoked = false;

        // Assert before undo
        Assert.That(command.SingleResponseQuestion, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(task.Questions, Has.Count.EqualTo(questionsCount));
            Assert.That(task.Questions, Does.Not.Contain(oldQuestion));
            Assert.That(task.Questions, Does.Contain(command.SingleResponseQuestion));
            Assert.That(command.SingleResponseQuestion!.Title, Is.EqualTo(title));
            Assert.That(command.SingleResponseQuestion.Text, Is.EqualTo(text));
            Assert.That(command.SingleResponseQuestion.Choices, Is.EqualTo(choices));
            Assert.That(command.SingleResponseQuestion.CorrectChoice, Is.EqualTo(correctChoice));
            Assert.That(command.SingleResponseQuestion.ExpectedCompletionTime, Is.EqualTo(expectedCompletionTime));
        });

        // Act
        command.Undo();

        // Assert after undo
        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(task.Questions, Has.Count.EqualTo(questionsCount));
            Assert.That(task.Questions, Does.Contain(oldQuestion));
            Assert.That(task.Questions, Does.Not.Contain(command.SingleResponseQuestion));
            Assert.That(oldQuestion.Title, Is.EqualTo(oldQuestionTitle));
            Assert.That(oldQuestion.Text, Is.EqualTo(oldQuestionText));
            Assert.That(oldQuestion.Choices, Is.EqualTo(oldChoices));
            Assert.That(oldQuestion.CorrectChoices, Is.EqualTo(oldCorrectChoices));
            Assert.That(oldQuestion.ExpectedCompletionTime, Is.EqualTo(oldExpectedCompletionTime));
        });
    }

    [Test]
    public void Undo_SingleResponseToMultipleResponse_UndoesDeletesOldQuestionAndCreateMultipleResponseQuestion()
    {
        // Arrange
        var task = EntityProvider.GetAdaptivityTask();
        var oldQuestion = EntityProvider.GetMultipleChoiceSingleResponseQuestion();
        task.Questions.Add(oldQuestion);
        var questionsCount = task.Questions.Count;
        var oldQuestionTitle = oldQuestion.Title;
        var oldQuestionText = oldQuestion.Text;
        var oldChoices = oldQuestion.Choices;
        var oldCorrectChoice = oldQuestion.CorrectChoice;
        var oldExpectedCompletionTime = oldQuestion.ExpectedCompletionTime;
        var isSingleResponse = false;
        var title = "NewQuestionTitle";
        var text = "NewQuestionText";
        var choice1 = new Choice("Choice1");
        var choice2 = new Choice("Choice2");
        var correctChoice1 = new Choice("CorrectChoice1");
        var correctChoice2 = new Choice("CorrectChoice2");
        var choices = new List<Choice>() {choice1, choice2};
        var correctChoices = new List<Choice>() {correctChoice1, correctChoice2};
        var expectedCompletionTime = 10;
        var actionWasInvoked = false;
        Action<AdaptivityTask> mappingAction = _ => actionWasInvoked = true;

        var command = new EditMultipleChoiceQuestionWithTypeChange(task, oldQuestion, isSingleResponse, title, text,
            choices, correctChoices, expectedCompletionTime, mappingAction,
            new NullLogger<EditMultipleChoiceQuestionWithTypeChange>());

        command.Execute();
        actionWasInvoked = false;

        // Assert before undo
        Assert.That(command.MultipleResponseQuestion, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(task.Questions, Has.Count.EqualTo(questionsCount));
            Assert.That(task.Questions, Does.Not.Contain(oldQuestion));
            Assert.That(task.Questions, Does.Contain(command.MultipleResponseQuestion));
            Assert.That(command.MultipleResponseQuestion!.Title, Is.EqualTo(title));
            Assert.That(command.MultipleResponseQuestion.Text, Is.EqualTo(text));
            Assert.That(command.MultipleResponseQuestion.Choices, Is.EqualTo(choices));
            Assert.That(command.MultipleResponseQuestion.CorrectChoices, Is.EqualTo(correctChoices));
            Assert.That(command.MultipleResponseQuestion.ExpectedCompletionTime, Is.EqualTo(expectedCompletionTime));
        });

        // Act
        command.Undo();

        // Assert after undo
        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(task.Questions, Has.Count.EqualTo(questionsCount));
            Assert.That(task.Questions, Does.Contain(oldQuestion));
            Assert.That(task.Questions, Does.Not.Contain(command.MultipleResponseQuestion));
            Assert.That(oldQuestion.Title, Is.EqualTo(oldQuestionTitle));
            Assert.That(oldQuestion.Text, Is.EqualTo(oldQuestionText));
            Assert.That(oldQuestion.Choices, Is.EqualTo(oldChoices));
            Assert.That(oldQuestion.CorrectChoice, Is.EqualTo(oldCorrectChoice));
            Assert.That(oldQuestion.ExpectedCompletionTime, Is.EqualTo(oldExpectedCompletionTime));
        });
    }

    [Test]
    public void Redo_MultipleResponseToSingleResponse_RedoesDeletesOldQuestionAndCreateSingleResponseQuestion()
    {
        // Arrange
        var task = EntityProvider.GetAdaptivityTask();
        var oldQuestion = EntityProvider.GetMultipleChoiceMultipleResponseQuestion();
        task.Questions.Add(oldQuestion);
        var questionsCount = task.Questions.Count;
        var oldQuestionTitle = oldQuestion.Title;
        var oldQuestionText = oldQuestion.Text;
        var oldChoices = oldQuestion.Choices;
        var oldCorrectChoices = oldQuestion.CorrectChoices;
        var oldExpectedCompletionTime = oldQuestion.ExpectedCompletionTime;
        var isSingleResponse = true;
        var title = "NewQuestionTitle";
        var text = "NewQuestionText";
        var choice1 = new Choice("Choice1");
        var choice2 = new Choice("Choice2");
        var correctChoice = new Choice("CorrectChoice");
        var choices = new List<Choice>() {choice1, choice2, correctChoice};
        var correctChoices = new List<Choice>() {correctChoice};
        var expectedCompletionTime = 10;
        var actionWasInvoked = false;
        Action<AdaptivityTask> mappingAction = _ => actionWasInvoked = true;

        var command = new EditMultipleChoiceQuestionWithTypeChange(task, oldQuestion, isSingleResponse, title, text,
            choices, correctChoices, expectedCompletionTime, mappingAction,
            new NullLogger<EditMultipleChoiceQuestionWithTypeChange>());

        command.Execute();
        command.Undo();
        actionWasInvoked = false;

        // Assert before redo
        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(task.Questions, Has.Count.EqualTo(questionsCount));
            Assert.That(task.Questions, Does.Contain(oldQuestion));
            Assert.That(task.Questions, Does.Not.Contain(command.SingleResponseQuestion));
            Assert.That(oldQuestion.Title, Is.EqualTo(oldQuestionTitle));
            Assert.That(oldQuestion.Text, Is.EqualTo(oldQuestionText));
            Assert.That(oldQuestion.Choices, Is.EqualTo(oldChoices));
            Assert.That(oldQuestion.CorrectChoices, Is.EqualTo(oldCorrectChoices));
            Assert.That(oldQuestion.ExpectedCompletionTime, Is.EqualTo(oldExpectedCompletionTime));
        });

        // Act
        command.Redo();

        // Assert after redo
        Assert.That(command.SingleResponseQuestion, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(task.Questions, Has.Count.EqualTo(questionsCount));
            Assert.That(task.Questions, Does.Not.Contain(oldQuestion));
            Assert.That(task.Questions, Does.Contain(command.SingleResponseQuestion));
            Assert.That(command.SingleResponseQuestion!.Title, Is.EqualTo(title));
            Assert.That(command.SingleResponseQuestion.Text, Is.EqualTo(text));
            Assert.That(command.SingleResponseQuestion.Choices, Is.EqualTo(choices));
            Assert.That(command.SingleResponseQuestion.CorrectChoice, Is.EqualTo(correctChoice));
            Assert.That(command.SingleResponseQuestion.ExpectedCompletionTime, Is.EqualTo(expectedCompletionTime));
        });
    }

    [Test]
    public void Redo_SingleResponseToMultipleResponse_RedoesDeletesOldQuestionAndCreateMultipleResponseQuestion()
    {
        // Arrange
        var task = EntityProvider.GetAdaptivityTask();
        var oldQuestion = EntityProvider.GetMultipleChoiceSingleResponseQuestion();
        task.Questions.Add(oldQuestion);
        var questionsCount = task.Questions.Count;
        var oldQuestionTitle = oldQuestion.Title;
        var oldQuestionText = oldQuestion.Text;
        var oldChoices = oldQuestion.Choices;
        var oldCorrectChoice = oldQuestion.CorrectChoice;
        var oldExpectedCompletionTime = oldQuestion.ExpectedCompletionTime;
        var isSingleResponse = false;
        var title = "NewQuestionTitle";
        var text = "NewQuestionText";
        var choice1 = new Choice("Choice1");
        var choice2 = new Choice("Choice2");
        var correctChoice1 = new Choice("CorrectChoice1");
        var correctChoice2 = new Choice("CorrectChoice2");
        var choices = new List<Choice>() {choice1, choice2};
        var correctChoices = new List<Choice>() {correctChoice1, correctChoice2};
        var expectedCompletionTime = 10;
        var actionWasInvoked = false;
        Action<AdaptivityTask> mappingAction = _ => actionWasInvoked = true;

        var command = new EditMultipleChoiceQuestionWithTypeChange(task, oldQuestion, isSingleResponse, title, text,
            choices, correctChoices, expectedCompletionTime, mappingAction,
            new NullLogger<EditMultipleChoiceQuestionWithTypeChange>());

        command.Execute();
        command.Undo();
        actionWasInvoked = false;

        // Assert before redo
        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(task.Questions, Has.Count.EqualTo(questionsCount));
            Assert.That(task.Questions, Does.Contain(oldQuestion));
            Assert.That(task.Questions, Does.Not.Contain(command.MultipleResponseQuestion));
            Assert.That(oldQuestion.Title, Is.EqualTo(oldQuestionTitle));
            Assert.That(oldQuestion.Text, Is.EqualTo(oldQuestionText));
            Assert.That(oldQuestion.Choices, Is.EqualTo(oldChoices));
            Assert.That(oldQuestion.CorrectChoice, Is.EqualTo(oldCorrectChoice));
            Assert.That(oldQuestion.ExpectedCompletionTime, Is.EqualTo(oldExpectedCompletionTime));
        });

        // Act
        command.Redo();

        // Assert after redo
        Assert.That(command.MultipleResponseQuestion, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(task.Questions, Has.Count.EqualTo(questionsCount));
            Assert.That(task.Questions, Does.Not.Contain(oldQuestion));
            Assert.That(task.Questions, Does.Contain(command.MultipleResponseQuestion));
            Assert.That(command.MultipleResponseQuestion!.Title, Is.EqualTo(title));
            Assert.That(command.MultipleResponseQuestion.Text, Is.EqualTo(text));
            Assert.That(command.MultipleResponseQuestion.Choices, Is.EqualTo(choices));
            Assert.That(command.MultipleResponseQuestion.CorrectChoices, Is.EqualTo(correctChoices));
            Assert.That(command.MultipleResponseQuestion.ExpectedCompletionTime, Is.EqualTo(expectedCompletionTime));
        });
    }

    [Test]
    public void Undo_SingleResponseToMultipleResponse_MementoIsNull_ThrowsException()
    {
        // Arrange
        var task = EntityProvider.GetAdaptivityTask();
        var oldQuestion = EntityProvider.GetMultipleChoiceSingleResponseQuestion();
        task.Questions.Add(oldQuestion);
        var isSingleResponse = false;
        var title = "NewQuestionTitle";
        var text = "NewQuestionText";
        var choices = new List<Choice>() {new Choice("Choice1"), new Choice("Choice2")};
        var correctChoices = new List<Choice>() {new Choice("CorrectChoice1"), new Choice("CorrectChoice2")};
        var expectedCompletionTime = 10;
        var actionWasInvoked = false;
        Action<AdaptivityTask> mappingAction = _ => actionWasInvoked = true;

        var command = new EditMultipleChoiceQuestionWithTypeChange(task, oldQuestion, isSingleResponse, title, text,
            choices, correctChoices, expectedCompletionTime, mappingAction,
            new NullLogger<EditMultipleChoiceQuestionWithTypeChange>());

        // Act and Assert
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.Multiple(() =>
        {
            Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
            Assert.That(actionWasInvoked, Is.False);
        });
    }

    [Test]
    public void Undo_MultipleResponseToSingleResponse_MementoIsNull_ThrowsException()
    {
        // Arrange
        var task = EntityProvider.GetAdaptivityTask();
        var oldQuestion = EntityProvider.GetMultipleChoiceMultipleResponseQuestion();
        task.Questions.Add(oldQuestion);
        var isSingleResponse = true;
        var title = "NewQuestionTitle";
        var text = "NewQuestionText";
        var choices = new List<Choice>() {new Choice("Choice1"), new Choice("Choice2")};
        var correctChoices = new List<Choice>() {new Choice("CorrectChoice1"), new Choice("CorrectChoice2")};
        var expectedCompletionTime = 10;
        var actionWasInvoked = false;
        Action<AdaptivityTask> mappingAction = _ => actionWasInvoked = true;

        var command = new EditMultipleChoiceQuestionWithTypeChange(task, oldQuestion, isSingleResponse, title, text,
            choices, correctChoices, expectedCompletionTime, mappingAction,
            new NullLogger<EditMultipleChoiceQuestionWithTypeChange>());

        // Act and Assert
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.Multiple(() =>
        {
            Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
            Assert.That(actionWasInvoked, Is.False);
        });
    }


    //
    // [Test]
    // public void Undo_MementoIsNull_ThrowsException()
    // {
    //     var question = EntityProvider.GetMultipleChoiceSingleResponseQuestion();
    //     var questionTitle = "NewQuestionTitle";
    //     var questionText = "NewQuestionText";
    //     var choice1 = new Choice("Choice1");
    //     var choice2 = new Choice("Choice2");
    //     var correctChoice = new Choice("CorrectChoice");
    //     var choices = new List<Choice>() {choice1, correctChoice, choice2};
    //     var expectedCompletionTime = 10;
    //     var actionWasInvoked = false;
    //     Action<MultipleChoiceSingleResponseQuestion> mappingAction = _ => actionWasInvoked = true;
    //
    //     var command = new EditMultipleChoiceSingleResponseQuestion(question, questionTitle, questionText, choices,
    //         correctChoice, expectedCompletionTime, mappingAction,
    //         new NullLogger<EditMultipleChoiceSingleResponseQuestion>());
    //
    //     // Act and Assert
    //     var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
    //     Assert.Multiple(() =>
    //     {
    //         Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
    //         Assert.That(actionWasInvoked, Is.False);
    //     });
    // }
}

// using BusinessLogic.Entities;
// using BusinessLogic.Entities.LearningContent.Adaptivity;
// using BusinessLogic.Entities.LearningContent.Adaptivity.Question;
// using Microsoft.Extensions.Logging;
//
// namespace BusinessLogic.Commands.Adaptivity.Question;
//
// public class EditMultipleChoiceQuestionWithTypeChange : IEditMultipleChoiceQuestionWithTypeChange
// {
//     private IMemento? _memento;
//
//     public EditMultipleChoiceQuestionWithTypeChange(AdaptivityTask task, IMultipleChoiceQuestion question,
//         bool isSingleResponse, string title, string text, ICollection<Choice> choices,
//         ICollection<Choice> correctChoices, int expectedCompletionTime, Action<AdaptivityTask> mappingAction,
//         ILogger<EditMultipleChoiceQuestionWithTypeChange> createLogger)
//     {
//         switch (question)
//         {
//             case MultipleChoiceMultipleResponseQuestion when isSingleResponse:
//                 SingleResponseQuestion = new MultipleChoiceSingleResponseQuestion(title, expectedCompletionTime,
//                     choices, text, correctChoices.First(), question.Difficulty, question.Rules);
//                 break;
//             case MultipleChoiceSingleResponseQuestion when !isSingleResponse:
//                 MultipleResponseQuestion = new MultipleChoiceMultipleResponseQuestion(title, expectedCompletionTime,
//                     choices, correctChoices, question.Rules, text, question.Difficulty);
//                 break;
//             default:
//                 throw new InvalidOperationException(
//                     $"Cannot change type of question {question.Id} from {question.GetType().Name} to {isSingleResponse}. The question has already this type.");
//         }
//
//         Task = task;
//         Question = question;
//         IsSingleResponse = isSingleResponse;
//         MappingAction = mappingAction;
//         Logger = createLogger;
//     }
//
//     internal AdaptivityTask Task { get; }
//     internal IMultipleChoiceQuestion Question { get; }
//     internal bool IsSingleResponse { get; }
//     internal MultipleChoiceSingleResponseQuestion? SingleResponseQuestion { get; }
//     internal MultipleChoiceMultipleResponseQuestion? MultipleResponseQuestion { get; }
//     internal Action<AdaptivityTask> MappingAction { get; }
//     private ILogger<EditMultipleChoiceQuestionWithTypeChange> Logger { get; }
//
//     public string Name => nameof(EditMultipleChoiceQuestionWithTypeChange);
//
//     public void Execute()
//     {
//         _memento = Question.GetMemento();
//
//         Logger.LogTrace(
//             "Editing MultipleChoiceQuestion {QuestionTitle} ({QuestionId}). Previous Values: Type {PreviousType} Title {PreviousTitle}, Text {PreviousText}, Choices {@PreviousChoices}, CorrectChoices {@PreviousCorrectChoices}, ExpectedCompletionTime {PreviousExpectedCompletionTime}",
//             Question.Title, Question.Id, Question.GetType().Name, Question.Title, Question.Text, Question.Choices,
//             Question.CorrectChoices, Question.ExpectedCompletionTime);
//
//         var questionToDelete = Task.Questions.FirstOrDefault(q => q.Id == Question.Id);
//         if (questionToDelete == null)
//         {
//             throw new InvalidOperationException(
//                 $"Cannot change type of question {Question.Id} from {Question.GetType().Name} to {IsSingleResponse}. The question does not exist in the Task.");
//         }
//
//         if (IsSingleResponse)
//         {
//             Task.Questions.Remove(questionToDelete);
//             Task.Questions.Add(SingleResponseQuestion!);
//         }
//         else if (!IsSingleResponse)
//         {
//             Task.Questions.Remove(questionToDelete);
//             Task.Questions.Add(MultipleResponseQuestion!);
//         }
//
//         // Does the new question need the same Id as the old one?
//
//         Logger.LogTrace(
//             "Edited MultipleChoiceQuestion {QuestionTitle} ({QuestionId}). Updated Values: Title {PreviousTitle}, Text {PreviousText}, Choices {@PreviousChoices}, CorrectChoices {@PreviousCorrectChoices}, ExpectedCompletionTime {PreviousExpectedCompletionTime}",
//             Question.Title, Question.Id, Question.Title, Question.Text, Question.Choices, Question.CorrectChoices,
//             Question.ExpectedCompletionTime);
//
//         MappingAction.Invoke(Task);
//     }
//
//     public void Undo()
//     {
//         if (_memento == null)
//         {
//             throw new InvalidOperationException("_memento is null");
//         }
//
//         Question.RestoreMemento(_memento);
//
//         Logger.LogTrace(
//             "Undoing edit of MultipleChoiceQuestion {QuestionTitle} ({QuestionId}). Restored Values: Title {PreviousTitle}, Text {PreviousText}, Choices {@PreviousChoices}, CorrectChoices {@PreviousCorrectChoices}, ExpectedCompletionTime {PreviousExpectedCompletionTime}",
//             Question.Title, Question.Id, Question.Title, Question.Text, Question.Choices, Question.CorrectChoices,
//             Question.ExpectedCompletionTime);
//
//         MappingAction.Invoke(Task);
//     }
//
//     public void Redo()
//     {
//         Logger.LogTrace("Redoing EditMultipleChoiceQuestionWithTypeChange");
//         Execute();
//     }
// }