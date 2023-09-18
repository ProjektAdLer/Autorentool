namespace BusinessLogic.Entities.LearningContent.AdaptivityContent;

/// <summary>
/// LearningContent for an AdLer adaptivity element.
/// </summary>
public interface IAdaptivityContent : ILearningContent
{
    /// <summary>
    /// The main questions the content contains.
    /// </summary>
    public ICollection<IAdaptivityTask> Tasks { get; set; }
}