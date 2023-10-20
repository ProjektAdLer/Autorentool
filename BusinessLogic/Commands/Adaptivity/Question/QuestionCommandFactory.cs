using BusinessLogic.Entities.LearningContent.Adaptivity;
using BusinessLogic.Entities.LearningContent.Adaptivity.Question;
using Microsoft.Extensions.Logging;
using Shared.Adaptivity;

namespace BusinessLogic.Commands.Adaptivity.Question;

public class QuestionCommandFactory : IQuestionCommandFactory
{
    public QuestionCommandFactory(ILoggerFactory loggerFactory)
    {
        LoggerFactory = loggerFactory;
    }

    private ILoggerFactory LoggerFactory { get; }

    public ICreateMultipleChoiceSingleResponseQuestion GetCreateMultipleChoiceSingleResponseQuestionCommand(
        AdaptivityTask adaptivityTask, QuestionDifficulty difficulty, string questionText, ICollection<Choice> choices,
        Choice correctChoice, int expectedCompletionTime, Action<AdaptivityTask> mappingAction)
    {
        return new CreateMultipleChoiceSingleResponseQuestion(adaptivityTask, difficulty, questionText, choices,
            correctChoice, expectedCompletionTime, mappingAction,
            LoggerFactory.CreateLogger<CreateMultipleChoiceSingleResponseQuestion>());
    }

    public ICreateMultipleChoiceMultipleResponseQuestion GetCreateMultipleChoiceMultipleResponseQuestionCommand(
        AdaptivityTask adaptivityTask, QuestionDifficulty difficulty, string questionText, ICollection<Choice> choices,
        ICollection<Choice> correctChoices, int expectedCompletionTime, Action<AdaptivityTask> mappingAction)
    {
        return new CreateMultipleChoiceMultipleResponseQuestion(adaptivityTask, difficulty, questionText, choices,
            correctChoices, expectedCompletionTime, mappingAction,
            LoggerFactory.CreateLogger<CreateMultipleChoiceMultipleResponseQuestion>());
    }

    public IEditMultipleChoiceSingleResponseQuestion GetEditMultipleChoiceSingleResponseQuestionCommand(
        MultipleChoiceSingleResponseQuestion question, string questionText, ICollection<Choice> choices,
        Choice correctChoice, int expectedCompletionTime, Action<MultipleChoiceSingleResponseQuestion> mappingAction)
    {
        return new EditMultipleChoiceSingleResponseQuestion(question, questionText, choices, correctChoice,
            expectedCompletionTime, mappingAction,
            LoggerFactory.CreateLogger<EditMultipleChoiceSingleResponseQuestion>());
    }

    public IEditMultipleChoiceMultipleResponseQuestion GetEditMultipleChoiceMultipleResponseQuestionCommand(
        MultipleChoiceMultipleResponseQuestion question, string questionText, ICollection<Choice> choices,
        ICollection<Choice> correctChoices, int expectedCompletionTime,
        Action<MultipleChoiceMultipleResponseQuestion> mappingAction)
    {
        return new EditMultipleChoiceMultipleResponseQuestion(question, questionText, choices, correctChoices,
            expectedCompletionTime, mappingAction,
            LoggerFactory.CreateLogger<EditMultipleChoiceMultipleResponseQuestion>());
    }

    public IEditMultipleChoiceQuestionWithTypeChange GetEditMultipleChoiceQuestionWithTypeChangeCommand(
        AdaptivityTask task, IMultipleChoiceQuestion question, bool isSingleResponse, string text,
        ICollection<Choice> choices, ICollection<Choice> correctChoices, int expectedCompletionTime,
        Action<AdaptivityTask> mappingAction)
    {
        return new EditMultipleChoiceQuestionWithTypeChange(task, question, isSingleResponse, text, choices,
            correctChoices, expectedCompletionTime, mappingAction,
            LoggerFactory.CreateLogger<EditMultipleChoiceQuestionWithTypeChange>());
    }

    public IDeleteAdaptivityQuestion GetDeleteCommand(AdaptivityTask task, IAdaptivityQuestion question,
        Action<AdaptivityTask> mappingAction)
    {
        return new DeleteAdaptivityQuestion(task, question, mappingAction,
            LoggerFactory.CreateLogger<DeleteAdaptivityQuestion>());
    }
}