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
        NpcName = "";
        NpcMood = NpcMood.Happy;
    }

    public StoryContent(string name, bool unsavedChanges, List<string> storyText, string npcName, NpcMood npcMood)
    {
        Name = name;
        UnsavedChanges = unsavedChanges;
        StoryText = storyText;
        NpcName = npcName;
        NpcMood = npcMood;
    }

    public bool UnsavedChanges { get; set; }

    public bool Equals(ILearningContent? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        if (other is not StoryContent otherCast) return false;
        return Name == other.Name && StoryText.SequenceEqual(otherCast.StoryText) && NpcName == otherCast.NpcName &&
               NpcMood == otherCast.NpcMood;
    }

    public string Name { get; set; }
    public List<string> StoryText { get; set; }
    public string NpcName { get; set; }
    public NpcMood NpcMood { get; set; }
}