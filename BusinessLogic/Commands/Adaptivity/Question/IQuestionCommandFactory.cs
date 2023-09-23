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
    ICreateMultipleChoiceSingleResponseQuestion GetCreateMultipleChoiceSingleResponseQuestionCommand(
        AdaptivityTask adaptivityTask,
        QuestionDifficulty difficulty, string questionText, ICollection<Choice> choices, Choice correctChoice,
        int expectedCompletionTime,
        Action<AdaptivityTask> mappingAction);

    /// <summary>
    /// Creates a command to create a multiple choice multiple response question.
    /// </summary>
    ICreateMultipleChoiceMultipleResponseQuestion GetCreateMultipleChoiceMultipleResponseQuestionCommand(
        AdaptivityTask adaptivityTask,
        QuestionDifficulty difficulty, string questionText, ICollection<Choice> choices,
        ICollection<Choice> correctChoices, int expectedCompletionTime, Action<AdaptivityTask> mappingAction);

    /// <summary>
    /// Creates a command to edit a multiple choice single response question.
    /// </summary>
    IEditMultipleChoiceSingleResponseQuestion GetEditMultipleChoiceSingleResponseQuestionCommand(
        MultipleChoiceSingleResponseQuestion question,
        string questionText, ICollection<Choice> choices, Choice correctChoice, int expectedCompletionTime,
        Action<MultipleChoiceSingleResponseQuestion> action);

    /// <summary>
    /// Creates a command to edit a multiple choice multiple response question.
    /// </summary>
    IEditMultipleChoiceMultipleResponseQuestion GetEditMultipleChoiceMultipleResponseQuestionCommand(
        MultipleChoiceMultipleResponseQuestion question,
        string questionText, ICollection<Choice> choices, ICollection<Choice> correctChoices,
        int expectedCompletionTime, Action<MultipleChoiceMultipleResponseQuestion> mappingAction);

    /// <summary>
    /// Creates a command to delete a question.
    /// </summary>
    IDeleteAdaptivityQuestion GetDeleteCommand(AdaptivityTask task, IAdaptivityQuestion question,
        Action<AdaptivityTask> mappingAction);
}