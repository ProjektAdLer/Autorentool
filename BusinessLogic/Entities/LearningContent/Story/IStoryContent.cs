using Shared;

namespace BusinessLogic.Entities.LearningContent.Story;

public interface IStoryContent : ILearningContent
{
    public List<string> StoryText { get; set; }
    public NpcMood NpcMood { get; set; }
    string NpcName { get; set; }
}