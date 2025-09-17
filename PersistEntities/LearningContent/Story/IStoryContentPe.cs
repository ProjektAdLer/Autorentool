using Shared;

namespace PersistEntities.LearningContent.Story;

public interface IStoryContentPe : ILearningContentPe
{
    public List<string> StoryText { get; set; }
    NpcMood NpcMood { get; set; }
    string NpcName { get; set; }
    bool ExitAfterStorySequence { get; set; }
}