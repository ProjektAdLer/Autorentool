using Presentation.PresentationLogic.LearningContent.AdaptivityContent.Question;
using Shared.Adaptivity;

namespace Presentation.PresentationLogic.LearningContent.AdaptivityContent;

public class AdaptivityTaskViewModel : IAdaptivityTaskViewModel
{
    public AdaptivityTaskViewModel(ICollection<IAdaptivityQuestionViewModel> questions, QuestionDifficulty minimumRequiredDifficulty, string name)
    {
        Questions = questions;
        MinimumRequiredDifficulty = minimumRequiredDifficulty;
        Name = name;
        Id = Guid.NewGuid();
        UnsavedChanges = true;
    }

    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private AdaptivityTaskViewModel()
    {
        Questions = null!;
        MinimumRequiredDifficulty = QuestionDifficulty.Easy;
        Name = "";
        Id = Guid.Empty;
        UnsavedChanges = false;
    }

    public ICollection<IAdaptivityQuestionViewModel> Questions { get; set; }
    public QuestionDifficulty? MinimumRequiredDifficulty { get; set; }
    public string Name { get; set; }

    private bool InternalUnsavedChanges { get; set; }

    public bool UnsavedChanges
    {
        get => InternalUnsavedChanges || Questions.Any(question => question.UnsavedChanges);
        set => InternalUnsavedChanges = value;
    }

    public Guid Id { get; set; }
}