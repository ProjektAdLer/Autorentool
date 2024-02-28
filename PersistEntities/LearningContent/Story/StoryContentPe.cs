namespace PersistEntities.LearningContent.Story;

public class StoryContentPe : IStoryContentPe
{
    private StoryContentPe()
    {
        Name = "";
        UnsavedChanges = false;
        StoryText = new List<string>();
    }

    public StoryContentPe(string name, bool unsavedChanges, List<string> storyText)
    {
        Name = name;
        UnsavedChanges = unsavedChanges;
        StoryText = storyText;
    }

    public bool UnsavedChanges { get; set; }

    public string Name { get; set; }
    public List<string> StoryText { get; set; }

    public bool Equals(ILearningContentPe? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        if (other is not StoryContentPe otherCast) return false;
        return Name == other.Name && StoryText.SequenceEqual(otherCast.StoryText);
    }
}