namespace BusinessLogic.Entities.LearningContent.Adaptivity;

/// <summary>
/// LearningContent for an AdLer adaptivity element.
/// </summary>
public interface IAdaptivityContent : ILearningContent, IOriginator
{
    /// <summary>
    /// The main questions the content contains.
    /// </summary>
    public ICollection<IAdaptivityTask> Tasks { get; set; }
}