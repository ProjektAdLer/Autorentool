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
        NpcMood = NpcMood.Friendly;
    }

    public StoryContentViewModel(string name = "", List<string>? storyText = null, NpcMood npcMood = NpcMood.Friendly)
    {
        Name = name;
        UnsavedChanges = true;
        StoryText = storyText ?? new List<string>();
        NpcMood = npcMood;
    }

    public bool UnsavedChanges { get; set; }

    public string Name { get; init; }
    public List<string> StoryText { get; set; }
    public NpcMood NpcMood { get; set; }

    public bool Equals(ILearningContentViewModel? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        if (other is not StoryContentViewModel otherCast) return false;
        return Name == other.Name && StoryText.SequenceEqual(otherCast.StoryText) && NpcMood == otherCast.NpcMood;
    }
}