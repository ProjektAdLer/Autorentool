namespace BusinessLogic.Entities.LearningContent.Story;

public interface IStoryContent : ILearningContent
{
    public string StoryText { get; set; }
}