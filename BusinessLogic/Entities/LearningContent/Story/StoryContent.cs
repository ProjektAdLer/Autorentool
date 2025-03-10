using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Shared;

namespace BusinessLogic.Entities.LearningContent.Story;

public class StoryContent : IStoryContent
{
    [UsedImplicitly]
    [ExcludeFromCodeCoverage(Justification =
        "Used by automapper, all properties are set after constructor, inaccessible to user code")]
    private StoryContent()
    {
        Name = "";
        UnsavedChanges = false;
        StoryText = new List<string>();
        NpcMood = NpcMood.Friendly;
    }

    public StoryContent(string name, bool unsavedChanges, List<string> storyText, NpcMood npcMood)
    {
        Name = name;
        UnsavedChanges = unsavedChanges;
        StoryText = storyText;
        NpcMood = npcMood;
    }

    public bool UnsavedChanges { get; set; }

    public bool Equals(ILearningContent? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        if (other is not StoryContent otherCast) return false;
        return Name == other.Name && StoryText.SequenceEqual(otherCast.StoryText) && NpcMood == otherCast.NpcMood;
    }

    public string Name { get; set; }
    public List<string> StoryText { get; set; }
    public NpcMood NpcMood { get; set; }
}