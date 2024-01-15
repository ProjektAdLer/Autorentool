using NUnit.Framework;
using Presentation.PresentationLogic.Topic;

namespace PresentationTest.PresentationLogic.Topic;

[TestFixture]
public class TopicViewModelUt
{
    [Test]
    public void Constructor_InitializesAllProperties()
    {
        var name = "topic name";

        var systemUnderTest = new TopicViewModel(name);

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(name));
            Assert.That(systemUnderTest.UnsavedChanges, Is.False);
        });
    }
}