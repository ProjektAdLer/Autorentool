namespace Presentation.PresentationLogic.LearningContent.Story;

public class StoryContentViewModel : IStoryContentViewModel
{
    private StoryContentViewModel()
    {
        Name = "";
        UnsavedChanges = false;
        StoryText = new List<string>();
    }

    public StoryContentViewModel(string name = "", List<string>? storyText = null)
    {
        Name = name;
        UnsavedChanges = true;
        StoryText = storyText ?? new List<string>();
    }

    public bool UnsavedChanges { get; set; }

    public string Name { get; init; }
    public List<string> StoryText { get; set; }

    public bool Equals(ILearningContentViewModel? other)
    {
        throw new NotImplementedException();
    }
}