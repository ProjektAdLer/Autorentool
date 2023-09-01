namespace Presentation.PresentationLogic.LearningContent.AdaptivityContent;

/// <summary>
/// LearningContent for an AdLer adaptivity element.
/// </summary>
public interface IAdaptivityContentViewModel : ILearningContentViewModel
{
    /// <summary>
    /// The main questions the content contains.
    /// </summary>
    public IEnumerable<IAdaptivityTaskViewModel> Tasks { get; set; }

    /// <summary>
    /// The rules defining adaptivity on the content.
    /// </summary>
    public IEnumerable<IAdaptivityRuleViewModel> Rules { get; set; }
}