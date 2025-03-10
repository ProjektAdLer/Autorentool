using Shared;

namespace Presentation.PresentationLogic.LearningContent.Story;

public interface IStoryContentViewModel : ILearningContentViewModel
{
    public List<string> StoryText { get; set; }
    NpcMood NpcMood { get; set; }
}