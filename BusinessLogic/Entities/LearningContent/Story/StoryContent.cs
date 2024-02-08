namespace BusinessLogic.Entities.LearningContent.Story;

public class StoryContent : IStoryContent
{
    private StoryContent()
    {
        Name = "";
        UnsavedChanges = false;
        StoryText = new List<string>();
    }

    public StoryContent(string name, bool unsavedChanges, List<string> storyText)
    {
        Name = name;
        UnsavedChanges = unsavedChanges;
        StoryText = storyText;
    }

    public bool UnsavedChanges { get; set; }

    public bool Equals(ILearningContent? other)
    {
        throw new NotImplementedException();
    }

    public string Name { get; set; }
    public List<string> StoryText { get; set; }
}