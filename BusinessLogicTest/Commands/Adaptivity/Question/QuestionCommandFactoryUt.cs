using BusinessLogic.Commands.Adaptivity.Question;
using BusinessLogic.Entities.LearningContent.Adaptivity;
using BusinessLogic.Entities.LearningContent.Adaptivity.Question;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using Shared.Adaptivity;
using TestHelpers;

namespace BusinessLogicTest.Commands.Adaptivity.Question;

[TestFixture]
public class QuestionCommandFactoryUt
{
    [SetUp]
    public void SetUp()
    {
        _factory = new QuestionCommandFactory(new NullLoggerFactory());
    }

    private QuestionCommandFactory _factory = null!;

    [Test]
    public void GetCreateMultipleChoiceSingleResponseQuestionCommand_ReturnsCreateMultipleChoiceSingleResponseQuestion()
    {
        // Arrange
        var adaptivityTask = new AdaptivityTask(new List<IAdaptivityQuestion>(), QuestionDifficulty.Medium, "Task1");
        var difficulty = QuestionDifficulty.Medium;
        var questionTitle = "QuestionTitle";
        var questionText = "Question";
        var correctChoice = new Choice("Choice1");
        var choices = new List<Choice>() {correctChoice};
        var expectedCompletionTime = 10;
        Action<AdaptivityTask> mappingAction = _ => { };

        // Act
        var result = _factory.GetCreateMultipleChoiceSingleResponseQuestionCommand(adaptivityTask, difficulty,
            questionTitle, questionText, choices, correctChoice, expectedCompletionTime, mappingAction);

        // Assert
        Assert.That(result, Is.InstanceOf<CreateMultipleChoiceSingleResponseQuestion>());
        var resultCasted = result as CreateMultipleChoiceSingleResponseQuestion;
        Assert.Multiple(() =>
        {
            Assert.That(resultCasted!.AdaptivityTask, Is.EqualTo(adaptivityTask));
            Assert.That(resultCasted.Question.Difficulty, Is.EqualTo(difficulty));
            Assert.That(resultCasted.Question.Title, Is.EqualTo(questionTitle));
            Assert.That(resultCasted.Question.Text, Is.EqualTo(questionText));
            Assert.That(resultCasted.Question.Choices, Is.EqualTo(choices));
            Assert.That(resultCasted.Question.CorrectChoice, Is.EqualTo(correctChoice));
            Assert.That(resultCasted.Question.ExpectedCompletionTime, Is.EqualTo(expectedCompletionTime));
            Assert.That(resultCasted.MappingAction, Is.EqualTo(mappingAction));
        });
    }

    [Test]
    public void
        GetCreateMultipleChoiceMultipleResponseQuestionCommand_ReturnsCreateMultipleChoiceMultipleResponseQuestion()
    {
        // Arrange
        var adaptivityTask = new AdaptivityTask(new List<IAdaptivityQuestion>(), QuestionDifficulty.Medium, "Task1");
        var difficulty = QuestionDifficulty.Medium;
        var questionTitle = "QuestionTitle";
        var questionText = "Question";
        var choice = new Choice("Choice1");
        var choices = new List<Choice>() {choice};
        var correctChoices = new List<Choice>() {choice};
        var expectedCompletionTime = 10;
        Action<AdaptivityTask> mappingAction = _ => { };

        // Act
        var result = _factory.GetCreateMultipleChoiceMultipleResponseQuestionCommand(adaptivityTask, difficulty,
            questionTitle, questionText, choices, correctChoices, expectedCompletionTime, mappingAction);

        // Assert
        Assert.That(result, Is.InstanceOf<CreateMultipleChoiceMultipleResponseQuestion>());
        var resultCasted = result as CreateMultipleChoiceMultipleResponseQuestion;
        Assert.Multiple(() =>
        {
            Assert.That(resultCasted!.AdaptivityTask, Is.EqualTo(adaptivityTask));
            Assert.That(resultCasted.Question.Difficulty, Is.EqualTo(difficulty));
            Assert.That(resultCasted.Question.Title, Is.EqualTo(questionTitle));
            Assert.That(resultCasted.Question.Text, Is.EqualTo(questionText));
            Assert.That(resultCasted.Question.Choices, Is.EqualTo(choices));
            Assert.That(resultCasted.Question.CorrectChoices, Is.EqualTo(correctChoices));
            Assert.That(resultCasted.Question.ExpectedCompletionTime, Is.EqualTo(expectedCompletionTime));
            Assert.That(resultCasted.MappingAction, Is.EqualTo(mappingAction));
        });
    }

    [Test]
    public void GetEditMultipleChoiceSingleResponseQuestionCommand_ReturnsEditMultipleChoiceSingleResponseQuestion()
    {
        // Arrange
        var oldCorrectChoice = new Choice("OldCorrectChoice");
        var question = new MultipleChoiceSingleResponseQuestion("OldQuestionTitle", 10,
            new List<Choice>() {oldCorrectChoice}, "OldQuestionText", oldCorrectChoice, QuestionDifficulty.Easy,
            new List<IAdaptivityRule>());
        var questionTitle = "NewQuestionTitle";
        var questionText = "NewQuestionText";
        var choices = new List<Choice>();
        var correctChoice = new Choice("NewCorrectChoice");
        var expectedCompletionTime = 20;
        Action<MultipleChoiceSingleResponseQuestion> mappingAction = _ => { };

        // Act
        var result = _factory.GetEditMultipleChoiceSingleResponseQuestionCommand(question, questionTitle, questionText,
            choices, correctChoice, expectedCompletionTime, mappingAction);

        // Assert
        Assert.That(result, Is.InstanceOf<EditMultipleChoiceSingleResponseQuestion>());
        var resultCasted = result as EditMultipleChoiceSingleResponseQuestion;
        Assert.Multiple(() =>
        {
            Assert.That(resultCasted!.Question, Is.EqualTo(question));
            Assert.That(resultCasted.Title, Is.EqualTo(questionTitle));
            Assert.That(resultCasted.QuestionText, Is.EqualTo(questionText));
            Assert.That(resultCasted.Choices, Is.EqualTo(choices));
            Assert.That(resultCasted.CorrectChoice, Is.EqualTo(correctChoice));
            Assert.That(resultCasted.ExpectedCompletionTime, Is.EqualTo(expectedCompletionTime));
            Assert.That(resultCasted.MappingAction, Is.EqualTo(mappingAction));
        });
    }

    [Test]
    public void GetEditMultipleChoiceMultipleResponseQuestionCommand_ReturnsEditMultipleChoiceMultipleResponseQuestion()
    {
        // Arrange
        var question = new MultipleChoiceMultipleResponseQuestion("OldQuestionTitle", 10, new List<Choice>(),
            new List<Choice>(), new List<IAdaptivityRule>(), "OldQuestionText", QuestionDifficulty.Easy);
        var questionTitle = "NewQuestionTitle";
        var questionText = "NewQuestionText";
        var choices = new List<Choice>();
        var correctChoices = new List<Choice>();
        var expectedCompletionTime = 20;
        Action<MultipleChoiceMultipleResponseQuestion> mappingAction = _ => { };

        // Act
        var result = _factory.GetEditMultipleChoiceMultipleResponseQuestionCommand(question, questionTitle,
            questionText, choices, correctChoices, expectedCompletionTime, mappingAction);

        // Assert
        Assert.That(result, Is.InstanceOf<EditMultipleChoiceMultipleResponseQuestion>());
        var resultCasted = result as EditMultipleChoiceMultipleResponseQuestion;
        Assert.Multiple(() =>
        {
            Assert.That(resultCasted!.Question, Is.EqualTo(question));
            Assert.That(resultCasted.Title, Is.EqualTo(questionTitle));
            Assert.That(resultCasted.QuestionText, Is.EqualTo(questionText));
            Assert.That(resultCasted.Choices, Is.EqualTo(choices));
            Assert.That(resultCasted.CorrectChoices, Is.EqualTo(correctChoices));
            Assert.That(resultCasted.ExpectedCompletionTime, Is.EqualTo(expectedCompletionTime));
            Assert.That(resultCasted.MappingAction, Is.EqualTo(mappingAction));
        });
    }

    [Test]
    public void
        GetEditMultipleChoiceQuestionWithTypeChangeCommand_MultipleResponseToSingleResponse_ReturnsEditMultipleChoiceQuestionWithTypeChange()
    {
        var task = EntityProvider.GetAdaptivityTask();
        var question = EntityProvider.GetMultipleChoiceMultipleResponseQuestion();
        var isSingleResponse = true;
        var title = "NewTitle";
        var text = "NewText";
        var choice1 = new Choice("Choice1");
        var choice2 = new Choice("Choice2");
        var correctChoice = new Choice("CorrectChoice");
        var choices = new List<Choice>() {choice1, choice2, correctChoice};
        var correctChoices = new List<Choice>() {correctChoice};
        var expectedCompletionTime = 20;
        Action<AdaptivityTask> mappingAction = _ => { };

        // Act
        var result = _factory.GetEditMultipleChoiceQuestionWithTypeChangeCommand(task, question, isSingleResponse,
            title, text, choices, correctChoices, expectedCompletionTime, mappingAction);

        // Assert
        Assert.That(result, Is.InstanceOf<EditMultipleChoiceQuestionWithTypeChange>());
        var resultCasted = result as EditMultipleChoiceQuestionWithTypeChange;
        Assert.Multiple(() =>
        {
            Assert.That(resultCasted!.Task, Is.EqualTo(task));
            Assert.That(resultCasted.Question, Is.EqualTo(question));
            Assert.That(resultCasted.IsSingleResponse, Is.EqualTo(isSingleResponse));
            Assert.That(resultCasted.MappingAction, Is.EqualTo(mappingAction));
            Assert.That(resultCasted.MultipleResponseQuestion, Is.Null);
            Assert.That(resultCasted.SingleResponseQuestion, Is.Not.Null);
            Assert.That(resultCasted.SingleResponseQuestion!.Title, Is.EqualTo(title));
            Assert.That(resultCasted.SingleResponseQuestion.Text, Is.EqualTo(text));
            Assert.That(resultCasted.SingleResponseQuestion.Choices, Is.EqualTo(choices));
            Assert.That(resultCasted.SingleResponseQuestion.CorrectChoice, Is.EqualTo(correctChoices.First()));
            Assert.That(resultCasted.SingleResponseQuestion.ExpectedCompletionTime, Is.EqualTo(expectedCompletionTime));
        });
    }

    [Test]
    public void
        GetEditMultipleChoiceQuestionWithTypeChangeCommand_SingleResponseToMultipleResponse_ReturnsEditMultipleChoiceQuestionWithTypeChange()
    {
        var task = EntityProvider.GetAdaptivityTask();
        var question = EntityProvider.GetMultipleChoiceSingleResponseQuestion();
        var isSingleResponse = false;
        var title = "NewTitle";
        var text = "NewText";
        var choice1 = new Choice("Choice1");
        var choice2 = new Choice("Choice2");
        var correctChoice = new Choice("CorrectChoice");
        var choices = new List<Choice>() {choice1, choice2, correctChoice};
        var correctChoices = new List<Choice>() {correctChoice};
        var expectedCompletionTime = 20;
        Action<AdaptivityTask> mappingAction = _ => { };

        // Act
        var result = _factory.GetEditMultipleChoiceQuestionWithTypeChangeCommand(task, question, isSingleResponse,
            title, text, choices, correctChoices, expectedCompletionTime, mappingAction);

        // Assert
        Assert.That(result, Is.InstanceOf<EditMultipleChoiceQuestionWithTypeChange>());
        var resultCasted = result as EditMultipleChoiceQuestionWithTypeChange;
        Assert.Multiple(() =>
        {
            Assert.That(resultCasted!.Task, Is.EqualTo(task));
            Assert.That(resultCasted.Question, Is.EqualTo(question));
            Assert.That(resultCasted.IsSingleResponse, Is.EqualTo(isSingleResponse));
            Assert.That(resultCasted.MappingAction, Is.EqualTo(mappingAction));
            Assert.That(resultCasted.MultipleResponseQuestion, Is.Not.Null);
            Assert.That(resultCasted.SingleResponseQuestion, Is.Null);
            Assert.That(resultCasted.MultipleResponseQuestion!.Title, Is.EqualTo(title));
            Assert.That(resultCasted.MultipleResponseQuestion.Text, Is.EqualTo(text));
            Assert.That(resultCasted.MultipleResponseQuestion.Choices, Is.EqualTo(choices));
            Assert.That(resultCasted.MultipleResponseQuestion.CorrectChoices, Is.EqualTo(correctChoices));
            Assert.That(resultCasted.MultipleResponseQuestion.ExpectedCompletionTime,
                Is.EqualTo(expectedCompletionTime));
        });
    }

    [Test]
    public void GetDeleteCommand_ReturnsDeleteAdaptivityQuestion()
    {
        // Arrange
        var question = new MultipleChoiceMultipleResponseQuestion("OldQuestionTitle", 10, new List<Choice>(),
            new List<Choice>(), new List<IAdaptivityRule>(), "OldQuestionText", QuestionDifficulty.Easy);
        var task = new AdaptivityTask(new List<IAdaptivityQuestion>() {question}, QuestionDifficulty.Easy, "Task1");
        Action<AdaptivityTask> mappingAction = _ => { };

        // Act
        var result = _factory.GetDeleteCommand(task, question, mappingAction);

        // Assert
        Assert.That(result, Is.InstanceOf<DeleteAdaptivityQuestion>());
        var resultCasted = result as DeleteAdaptivityQuestion;
        Assert.Multiple(() =>
        {
            Assert.That(resultCasted!.Task, Is.EqualTo(task));
            Assert.That(resultCasted.Question, Is.EqualTo(question));
            Assert.That(resultCasted.MappingAction, Is.EqualTo(mappingAction));
        });
    }
}