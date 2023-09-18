using Shared.Adaptivity;

namespace Presentation.PresentationLogic.LearningContent.AdaptivityContent.Question;

/// <summary>
/// Represents a question with multiple different choices and multiple responses.
/// </summary>
public class MultipleChoiceMultipleResponseQuestionViewModel : IMultipleChoiceQuestionViewModel
{
    public MultipleChoiceMultipleResponseQuestionViewModel(int expectedCompletionTime, string text,
        QuestionDifficulty difficulty,
        ICollection<ChoiceViewModel> correctChoices,
        ICollection<ChoiceViewModel> choices,
        ICollection<IAdaptivityRuleViewModel>? rules = null)
    {
        Id = Guid.NewGuid();
        ExpectedCompletionTime = expectedCompletionTime;
        Choices = choices;
        CorrectChoices = correctChoices;
        Text = text;
        Difficulty = difficulty;
        Rules = rules ?? new List<IAdaptivityRuleViewModel>();
    }

    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private MultipleChoiceMultipleResponseQuestionViewModel()
    {
        Id = Guid.Empty;
        ExpectedCompletionTime = 0;
        Choices = null!;
        CorrectChoices = null!;
        Text = null!;
        Difficulty = QuestionDifficulty.Easy;
        Rules = null!;
    }

    public Guid Id { get; private set; }
    public int ExpectedCompletionTime { get; set; }
    public QuestionDifficulty Difficulty { get; set; }
    public ICollection<IAdaptivityRuleViewModel> Rules { get; set; }
    public ICollection<ChoiceViewModel> Choices { get; set; }
    public ICollection<ChoiceViewModel> CorrectChoices { get; set; }
    public string Text { get; set; }
}