using Presentation.PresentationLogic.LearningContent.AdaptivityContent.Question;
using Shared.Adaptivity;

namespace Presentation.PresentationLogic.LearningContent.AdaptivityContent;

public interface IAdaptivityTaskViewModel
{
    /// <summary>
    /// The main questions this task contains.
    /// </summary>
    public ICollection<IAdaptivityQuestionViewModel> Questions { get; set; }
    /// <summary>
    /// The minimum required difficulty of question required to answer to complete this task.
    /// </summary>
    public QuestionDifficulty? MinimumRequiredDifficulty { get; set; }

    Guid Id { get; set; }
    string Name { get; set; }
    bool UnsavedChanges { get; set; }
}