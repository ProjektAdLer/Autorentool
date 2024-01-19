namespace PersistEntities.LearningContent.Story;

public class StoryContentPe : IStoryContentPe
{
    private StoryContentPe()
    {
        Name = "";
        UnsavedChanges = false;
        StoryText = "";
    }
    public StoryContentPe(string name, bool unsavedChanges, string storyText)
    {
        Name = name;
        UnsavedChanges = unsavedChanges;
        StoryText = storyText;
    }

    public bool Equals(ILearningContentPe? other)
    {
        throw new NotImplementedException();
    }

    public string Name { get; set; }
    public bool UnsavedChanges { get; set; }
    public string StoryText { get; set; }
}