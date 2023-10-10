namespace Presentation.PresentationLogic.LearningContent.AdaptivityContent.Question;

/// <summary>
/// Represents a multiple choice question with either single or multiple responses.
/// </summary>
public interface IMultipleChoiceQuestionViewModel : IAdaptivityQuestionViewModel
{
    /// <summary>
    /// Choices for the question to pick response from.
    /// </summary>
    public ICollection<ChoiceViewModel> Choices { get; set; }

    /// <summary>
    /// Correct choices from <see cref="Choices"/>.
    /// May be single item for <see cref="MultipleChoiceSingleResponseQuestionViewModel"/>
    /// or multiple items for <see cref="MultipleChoiceMultipleResponseQuestionViewModel"/>.
    /// </summary>
    public ICollection<ChoiceViewModel> CorrectChoices { get; }

    /// <summary>
    /// The main question text of the question.
    /// </summary>
    public string Text { get; set; }
}