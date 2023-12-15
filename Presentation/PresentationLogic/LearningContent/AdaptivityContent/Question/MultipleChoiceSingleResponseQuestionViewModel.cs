using Shared.Adaptivity;

namespace Presentation.PresentationLogic.LearningContent.AdaptivityContent.Question;

/// <summary>
/// Represents a question with multiple different choices but only one response (and therefore only one correct choice).
/// </summary>
public class MultipleChoiceSingleResponseQuestionViewModel : IMultipleChoiceQuestionViewModel
{
    public MultipleChoiceSingleResponseQuestionViewModel(int expectedCompletionTime, string text,
        ChoiceViewModel correctChoice, QuestionDifficulty difficulty, ICollection<ChoiceViewModel> choices,
        ICollection<IAdaptivityRuleViewModel>? rules = null)
    {
        Id = Guid.NewGuid();
        ExpectedCompletionTime = expectedCompletionTime;
        Text = text;
        CorrectChoice = correctChoice;
        Difficulty = difficulty;
        Choices = choices;
        Rules = rules ?? new List<IAdaptivityRuleViewModel>();
        UnsavedChanges = true;
    }

    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private MultipleChoiceSingleResponseQuestionViewModel()
    {
        Id = Guid.Empty;
        ExpectedCompletionTime = 0;
        Choices = null!;
        Text = null!;
        CorrectChoice = null!;
        Difficulty = QuestionDifficulty.Easy;
        Rules = null!;
        UnsavedChanges = false;
    }

    public ChoiceViewModel CorrectChoice { get; set; }

    public Guid Id { get; private set; }
    public int ExpectedCompletionTime { get; set; }
    public QuestionDifficulty Difficulty { get; set; }
    public ICollection<IAdaptivityRuleViewModel> Rules { get; set; }

    private bool InternalUnsavedChanges { get; set; }

    public bool UnsavedChanges
    {
        get => InternalUnsavedChanges || Rules.Any(rule => rule.UnsavedChanges) ||
               Choices.Any(choice => choice.UnsavedChanges) || CorrectChoice.UnsavedChanges;
        set => InternalUnsavedChanges = value;
    }

    public ICollection<ChoiceViewModel> Choices { get; set; }
    public ICollection<ChoiceViewModel> CorrectChoices => new List<ChoiceViewModel> {CorrectChoice};
    public string Text { get; set; }
}