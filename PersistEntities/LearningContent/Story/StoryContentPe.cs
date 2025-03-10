using Shared;

namespace PersistEntities.LearningContent.Story;

public class StoryContentPe : IStoryContentPe
{
    // ReSharper disable once UnusedMember.Local
    private StoryContentPe()
    {
        Name = "";
        UnsavedChanges = false;
        StoryText = new List<string>();
        NpcMood = NpcMood.Friendly;
    }

    public StoryContentPe(string name, bool unsavedChanges, List<string> storyText, NpcMood npcMood)
    {
        Name = name;
        UnsavedChanges = unsavedChanges;
        StoryText = storyText;
        NpcMood = npcMood;
    }

    public bool UnsavedChanges { get; set; }

    public string Name { get; set; }
    public List<string> StoryText { get; set; }
    public NpcMood NpcMood { get; set; }

    public bool Equals(ILearningContentPe? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        if (other is not StoryContentPe otherCast) return false;
        return Name == other.Name && StoryText.SequenceEqual(otherCast.StoryText) && NpcMood == otherCast.NpcMood;
    }
}