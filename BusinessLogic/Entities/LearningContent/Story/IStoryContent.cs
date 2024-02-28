namespace BusinessLogic.Entities.LearningContent.Story;

public interface IStoryContent : ILearningContent
{
    public List<string> StoryText { get; set; }
}