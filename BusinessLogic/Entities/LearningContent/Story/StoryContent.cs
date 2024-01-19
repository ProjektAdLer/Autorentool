namespace BusinessLogic.Entities.LearningContent.Story;

public class StoryContent : IStoryContent
{
    private StoryContent()
    {
        Name = "";
        UnsavedChanges = false;
        StoryText = "";
    }
    public StoryContent(string name, bool unsavedChanges, string storyText)
    {
        Name = name;
        UnsavedChanges = unsavedChanges;
        StoryText = storyText;
    }

    public bool Equals(ILearningContent? other)
    {
        throw new NotImplementedException();
    }

    public string Name { get; set; }
    public bool UnsavedChanges { get; set; }
    public string StoryText { get; set; }
}