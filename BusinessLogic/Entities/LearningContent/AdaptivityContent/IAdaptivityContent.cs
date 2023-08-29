namespace BusinessLogic.Entities.LearningContent.AdaptivityContent;

/// <summary>
/// LearningContent for an AdLer adaptivity element.
/// </summary>
public interface IAdaptivityContent : ILearningContent
{
    /// <summary>
    /// The main questions the content contains.
    /// </summary>
    public IEnumerable<IAdaptivityTask> Tasks { get; set; }

    /// <summary>
    /// The rules defining adaptivity on the content.
    /// </summary>
    public IEnumerable<IAdaptivityRule> Rules { get; set; }
}