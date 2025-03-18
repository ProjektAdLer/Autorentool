namespace PersistEntities.LearningContent;

public interface ILinkContentPe : ILearningContentPe
{
    string Link { get; set; }
    bool IsDeleted { get; set; }
}