using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace BusinessLogic.Entities.LearningContent.Story;

public class StoryContent : IStoryContent
{
    [UsedImplicitly]
    [ExcludeFromCodeCoverage(Justification = "Used by automapper, all properties are set after constructor, inaccessible to user code")]
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
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        if (other is not StoryContent otherCast) return false;
        return Name == other.Name && StoryText.SequenceEqual(otherCast.StoryText);
    }

    public string Name { get; set; }
    public List<string> StoryText { get; set; }
}