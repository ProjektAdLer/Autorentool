
namespace Presentation.PresentationLogic.LearningContent.Story;

public class StoryContentViewModel : IStoryContentViewModel
{
    private StoryContentViewModel()
    {
        Name = "";
        UnsavedChanges = false;
        StoryText = "";
    }
    public StoryContentViewModel(string name = "", string storyText = "")
    {
        Name = name;
        UnsavedChanges = true;
        StoryText = storyText;
    }

    public bool Equals(ILearningContentViewModel? other)
    {
        throw new NotImplementedException();
    }

    public string Name { get; init; }
    public bool UnsavedChanges { get; set; }
    public string StoryText { get; set; }
}