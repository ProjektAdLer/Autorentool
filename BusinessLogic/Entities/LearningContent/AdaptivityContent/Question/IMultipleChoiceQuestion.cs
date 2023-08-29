namespace BusinessLogic.Entities.LearningContent.AdaptivityContent.Question;

/// <summary>
/// Represents a multiple choice question with either single or multiple responses.
/// </summary>
public interface IMultipleChoiceQuestion : IAdaptivityQuestion
{
    /// <summary>
    /// Choices for the question to pick response from.
    /// </summary>
    public IEnumerable<Choice> Choices { get; set; }

    /// <summary>
    /// Correct choices from <see cref="Choices"/>.
    /// May be single item for <see cref="MultipleChoiceSingleResponseQuestion"/>
    /// or multiple items for <see cref="MultipleChoiceMultipleResponseQuestion"/>.
    /// </summary>
    public IEnumerable<Choice> CorrectChoices { get; }

    /// <summary>
    /// The main question text of the question.
    /// </summary>
    public string Text { get; set; }
}