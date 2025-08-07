using Shared;

namespace Presentation.PresentationLogic.LearningContent.Story;

public class StoryContentViewModel : IStoryContentViewModel
{
    // ReSharper disable once UnusedMember.Local
    private StoryContentViewModel()
    {
        Name = "";
        UnsavedChanges = false;
        StoryText = new List<string>();
        NpcName = "";
        NpcMood = NpcMood.Happy;
        ExitAfterStorySequence = false;
    }

    public StoryContentViewModel(string name = "", List<string>? storyText = null, string npcName = "",
        NpcMood npcMood = NpcMood.Happy, bool exitAfterStorySequence = false)
    {
        Name = name;
        UnsavedChanges = true;
        StoryText = storyText ?? new List<string>();
        NpcName = npcName;
        NpcMood = npcMood;
        ExitAfterStorySequence = exitAfterStorySequence;
    }

    public bool UnsavedChanges { get; set; }

    public string Name { get; init; }
    public List<string> StoryText { get; set; }
    public string NpcName { get; set; }
    public NpcMood NpcMood { get; set; }
    public bool ExitAfterStorySequence { get; set; }

    public bool Equals(ILearningContentViewModel? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        if (other is not StoryContentViewModel otherCast) return false;
        return Name == other.Name && StoryText.SequenceEqual(otherCast.StoryText) && NpcName == otherCast.NpcName &&
               NpcMood == otherCast.NpcMood;
    }
}