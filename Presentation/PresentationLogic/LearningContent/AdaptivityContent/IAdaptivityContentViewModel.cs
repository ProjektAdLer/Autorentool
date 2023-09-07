namespace Presentation.PresentationLogic.LearningContent.AdaptivityContent;

/// <summary>
/// LearningContent for an AdLer adaptivity element.
/// </summary>
public interface IAdaptivityContentViewModel : ILearningContentViewModel
{
    /// <summary>
    /// The main questions the content contains.
    /// </summary>
    public ICollection<IAdaptivityTaskViewModel> Tasks { get; set; }

}