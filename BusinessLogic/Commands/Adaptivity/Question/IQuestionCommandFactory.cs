using BusinessLogic.Entities.LearningContent.Adaptivity;
using BusinessLogic.Entities.LearningContent.Adaptivity.Question;
using Shared.Adaptivity;

namespace BusinessLogic.Commands.Adaptivity.Question;

/// <summary>
/// Factory for creating commands relating to adaptivity questions.
/// </summary>
public interface IQuestionCommandFactory
{
    /// <summary>
    /// Creates a command to create a multiple choice single response question.
    /// </summary>
    ICommand GetCreateMultipleChoiceSingleResponseQuestionCommand(AdaptivityTask adaptivityTask,
        QuestionDifficulty difficulty, string questionText, ICollection<Choice> choices, Choice correctChoice,
        int expectedCompletionTime,
        Action<AdaptivityTask> mappingAction);

    /// <summary>
    /// Creates a command to create a multiple choice multiple response question.
    /// </summary>
    ICommand GetCreateMultipleChoiceMultipleResponseQuestionCommand(AdaptivityTask adaptivityTask,
        QuestionDifficulty difficulty, string questionText, ICollection<Choice> choices,
        ICollection<Choice> correctChoices, int expectedCompletionTime, Action<AdaptivityTask> mappingAction);
}