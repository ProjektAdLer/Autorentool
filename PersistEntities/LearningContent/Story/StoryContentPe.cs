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
        NpcName = "";
        NpcMood = NpcMood.Happy;
        ExitAfterStorySequence = false;
    }

    public StoryContentPe(string name, bool unsavedChanges, List<string> storyText, string npcName, NpcMood npcMood,
        bool exitAfterStorySequence)
    {
        Name = name;
        UnsavedChanges = unsavedChanges;
        StoryText = storyText;
        NpcName = npcName;
        NpcMood = npcMood;
        ExitAfterStorySequence = exitAfterStorySequence;
    }

    public bool UnsavedChanges { get; set; }

    public string Name { get; set; }
    public List<string> StoryText { get; set; }
    public string NpcName { get; set; }
    public NpcMood NpcMood { get; set; }
    public bool ExitAfterStorySequence { get; set; }

    public bool Equals(ILearningContentPe? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        if (other is not StoryContentPe otherCast) return false;
        return Name == other.Name && StoryText.SequenceEqual(otherCast.StoryText) && NpcName == otherCast.NpcName &&
               NpcMood == otherCast.NpcMood;
    }
}