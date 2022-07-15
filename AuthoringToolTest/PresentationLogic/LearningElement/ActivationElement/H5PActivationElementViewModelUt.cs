using AuthoringTool.PresentationLogic.LearningContent;
using AuthoringTool.PresentationLogic.LearningElement;
using AuthoringTool.PresentationLogic.LearningElement.ActivationElement;
using AuthoringTool.PresentationLogic.LearningWorld;
using NUnit.Framework;

namespace AuthoringToolTest.PresentationLogic.LearningElement.ActivationElement;

[TestFixture]

public class H5PActivationElementViewModelUt
{
    [Test]
    public void Constructor_InitializesAllProperties()
    {
        var Name = "qwert";
        var Shortname = "asdf";
        var Parent = new LearningWorldViewModel("foo", "bar", "baz", "", "", "");
        var Content = new LearningContentViewModel("bar", "foo", new byte[] {0x01, 0x02});
        var Authors = "trewq";
        var Description = "fdsa";
        var Goals = "barfoo";
        var Workload = 10;
        var Difficulty = LearningElementDifficultyEnum.Medium;
        var PositionX = 5f;
        var PositionY = 21f;

        var systemUnderTest = new H5PActivationElementViewModel(Name, Shortname, Parent, Content, Authors,
            Description, Goals, Difficulty, Workload, PositionX, PositionY);

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(Name));
            Assert.That(systemUnderTest.Shortname, Is.EqualTo(Shortname));
            Assert.That(systemUnderTest.Parent, Is.EqualTo(Parent));
            Assert.That(systemUnderTest.LearningContent, Is.EqualTo(Content));
            Assert.That(systemUnderTest.Authors, Is.EqualTo(Authors));
            Assert.That(systemUnderTest.Description, Is.EqualTo(Description));
            Assert.That(systemUnderTest.Goals, Is.EqualTo(Goals));
            Assert.That(systemUnderTest.Workload, Is.EqualTo(Workload));
            Assert.That(systemUnderTest.Difficulty, Is.EqualTo(Difficulty));
            Assert.That(systemUnderTest.PositionX, Is.EqualTo(PositionX));
            Assert.That(systemUnderTest.PositionY, Is.EqualTo(PositionY));
        });
    }
}