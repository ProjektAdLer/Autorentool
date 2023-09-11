using PersistEntities.LearningContent.Question;
using Shared.Adaptivity;

namespace PersistEntities.LearningContent;

public class AdaptivityTaskPe : IAdaptivityTaskPe
{
    public AdaptivityTaskPe(ICollection<IAdaptivityQuestionPe> questions, QuestionDifficulty minimumRequiredDifficulty)
    {
        Questions = questions;
        MinimumRequiredDifficulty = minimumRequiredDifficulty;
        Id = Guid.NewGuid();
    }

    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private AdaptivityTaskPe()
    {
        Questions = null!;
        MinimumRequiredDifficulty = QuestionDifficulty.Easy;
        Id = Guid.Empty;
    }

    public ICollection<IAdaptivityQuestionPe> Questions { get; set; }
    public QuestionDifficulty MinimumRequiredDifficulty { get; set; }
    public Guid Id { get; set; }
}