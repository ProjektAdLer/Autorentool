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
        UnsavedChanges = true;
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
        UnsavedChanges = false;
    }

    public Guid Id { get; private set; }
    public int ExpectedCompletionTime { get; set; }
    public QuestionDifficulty Difficulty { get; set; }
    public ICollection<IAdaptivityRuleViewModel> Rules { get; set; }

    // ReSharper disable once MemberCanBePrivate.Global - disabled because we need a public property so automapper will map it
    public bool InternalUnsavedChanges { get; private set; }

    public bool UnsavedChanges
    {
        get => InternalUnsavedChanges || Rules.Any(rule => rule.UnsavedChanges) || Choices.Any(choice => choice.UnsavedChanges) || CorrectChoices.Any(choice => choice.UnsavedChanges);
        set => InternalUnsavedChanges = value;
    }

    public ICollection<ChoiceViewModel> Choices { get; set; }
    public ICollection<ChoiceViewModel> CorrectChoices { get; set; }
    public string Text { get; set; }
}