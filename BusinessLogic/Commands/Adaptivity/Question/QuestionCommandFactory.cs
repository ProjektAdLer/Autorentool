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

    public ICommand GetCreateMultipleChoiceSingleResponseQuestionCommand(AdaptivityTask adaptivityTask,
        QuestionDifficulty difficulty, string questionText, ICollection<Choice> choices, Choice correctChoice,
        int expectedCompletionTime, Action<AdaptivityTask> mappingAction)
    {
        return new CreateMultipleChoiceSingleResponseQuestion(adaptivityTask, difficulty, questionText, choices,
            correctChoice, expectedCompletionTime, mappingAction,
            LoggerFactory.CreateLogger<CreateMultipleChoiceSingleResponseQuestion>());
    }

    public ICommand GetCreateMultipleChoiceMultipleResponseQuestionCommand(AdaptivityTask adaptivityTask,
        QuestionDifficulty difficulty, string questionText, ICollection<Choice> choices,
        ICollection<Choice> correctChoices,
        int expectedCompletionTime, Action<AdaptivityTask> mappingAction)
    {
        return new CreateMultipleChoiceMultipleResponseQuestion(adaptivityTask, difficulty, questionText, choices,
            correctChoices, expectedCompletionTime, mappingAction,
            LoggerFactory.CreateLogger<CreateMultipleChoiceMultipleResponseQuestion>());
    }
}