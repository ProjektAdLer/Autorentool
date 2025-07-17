using System.Collections;
using BusinessLogic.Entities.LearningContent;
using BusinessLogic.Entities.LearningContent.Story;
using NSubstitute;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Entities.LearningContent.Story;

[TestFixture]
public class StoryContentUt
{
    [Test]
    public void Constructor_AllPropertiesSet()
    {
        var name = "a name";
        var unsavedChanges = true;
        var storyText = new List<string> { "a story", "and another story" };
        var npcName = "a npc name";
        var npcMood = NpcMood.Welcome;
        
        var storyContent = new StoryContent(name, unsavedChanges, storyText, npcName, npcMood);
        
        Assert.Multiple(() =>
        {
            Assert.That(storyContent.Name, Is.EqualTo(name));
            Assert.That(storyContent.UnsavedChanges, Is.EqualTo(unsavedChanges));
            Assert.That(storyContent.StoryText, Is.EquivalentTo(storyText));
            Assert.That(storyContent.NpcName, Is.EqualTo(npcName));
            Assert.That(storyContent.NpcMood, Is.EqualTo(npcMood));
        });
    }
    
    [Test]
    public void Equals_SameNameAndStoryText_ReturnsTrue()
    {
        var name = "a name";
        var unsavedChanges = true;
        var storyText = new List<string> { "a story", "and another story" };
        var npcName = "a npc name";
        var npcMood = NpcMood.Welcome;
        
        var storyContent = new StoryContent(name, unsavedChanges, storyText, npcName, npcMood);
        var otherStoryContent = new StoryContent(name, unsavedChanges, storyText, npcName, npcMood);

        Assert.Multiple(() =>
        {
            Assert.That(ReferenceEquals(storyContent, otherStoryContent), Is.False);
            Assert.That(storyContent, Is.EqualTo(otherStoryContent));
        });
    }

    [Test]
    public void Equals_ReferenceEqual_ReturnsTrue()
    {
        var name = "a name";
        var unsavedChanges = true;
        var storyText = new List<string> { "a story", "and another story" };
        var npcName = "a npc name";
        var npcMood = NpcMood.Welcome;
        
        var storyContent = new StoryContent(name, unsavedChanges, storyText, npcName, npcMood);
        
        Assert.Multiple(() =>
        {
            // ReSharper disable once EqualExpressionComparison
            Assert.That(ReferenceEquals(storyContent, storyContent), Is.True);
#pragma warning disable NUnit2010
            Assert.That(storyContent.Equals(storyContent), Is.True);
#pragma warning restore NUnit2010
        });
    }
    
    [Test]
    public void Equals_DifferentType_ReturnsFalse()
    {
        var name = "a name";
        var unsavedChanges = true;
        var storyText = new List<string> { "a story", "and another story" };
        var npcName = "a npc name";
        var npcMood = NpcMood.Welcome;
        
        var storyContent = new StoryContent(name, unsavedChanges, storyText, npcName, npcMood);
        var otherStoryContent = Substitute.For<ILearningContent>();

        Assert.Multiple(() =>
        {
            Assert.That(ReferenceEquals(storyContent, otherStoryContent), Is.False);
            Assert.That(storyContent.Equals(otherStoryContent), Is.False);
        });
    }
    
    [Test]
    public void Equals_WithNull_ReturnsFalse()
    {
        var storyContent = GetStoryContent();

#pragma warning disable NUnit2010
        Assert.That(storyContent.Equals(null), Is.False);
#pragma warning restore NUnit2010
    }

    [Test]
    [TestCaseSource(typeof(UnequalStoryContent))]
    public void Equals_UnequalProperties_ReturnsFalse(StoryContent other)
    {
        var storyContent = GetStoryContent();
     
        Assert.That(storyContent, Is.Not.EqualTo(other));
    }

    private class UnequalStoryContent : IEnumerable<StoryContent>
    {
        public IEnumerator<StoryContent> GetEnumerator()
        {
            yield return new StoryContent("different name", false, new List<string> { "a story", "and another story" }, "a npc name", NpcMood.Welcome);
            yield return new StoryContent("a name", false, new List<string> { "a different story", "and another story" }, "different npc name", NpcMood.Tired);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    private static StoryContent GetStoryContent()
    {
        var name = "a name";
        var unsavedChanges = true;
        var storyText = new List<string> { "a story", "and another story" };
        var npcName = "a npc name";
        var npcMood = NpcMood.Welcome;
        
        var storyContent = new StoryContent(name, unsavedChanges, storyText, npcName, npcMood);
        return storyContent;
    }
}