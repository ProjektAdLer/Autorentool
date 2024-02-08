namespace PersistEntities.LearningContent.Story;

public interface IStoryContentPe : ILearningContentPe
{
    public List<string> StoryText { get; set; }
}