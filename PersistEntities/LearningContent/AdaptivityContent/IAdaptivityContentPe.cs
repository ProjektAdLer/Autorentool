namespace PersistEntities.LearningContent;

/// <summary>
/// LearningContent for an AdLer adaptivity element.
/// </summary>
public interface IAdaptivityContentPe : ILearningContentPe
{
    /// <summary>
    /// The main questions the content contains.
    /// </summary>
    public ICollection<IAdaptivityTaskPe> Tasks { get; set; }
}