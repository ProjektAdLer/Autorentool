using NUnit.Framework;
using Presentation.PresentationLogic.Content;
using Presentation.PresentationLogic.Element.ActivationElement;
using Presentation.PresentationLogic.Space;
using Shared;

namespace PresentationTest.PresentationLogic.Element.ActivationElement;

[TestFixture]

public class H5PActivationElementViewModelUt
{
    [Test]
    public void Constructor_InitializesAllProperties()
    {
        var name = "qwert";
        var shortname = "asdf";
        var parent = new SpaceViewModel("foo", "bar", "baz", "", "", 2);
        var content = new ContentViewModel("bar", "foo", "");
        var url = "url";
        var authors = "trewq";
        var description = "fdsa";
        var goals = "barfoo";
        var workload = 10;
        var points = 6;
        var difficulty = ElementDifficultyEnum.Medium;
        var positionX = 5f;
        var positionY = 21f;

        var systemUnderTest = new H5PActivationElementViewModel(name, shortname, parent, content, url, authors,
            description, goals, difficulty, workload, points, positionX, positionY);

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(name));
            Assert.That(systemUnderTest.Shortname, Is.EqualTo(shortname));
            Assert.That(systemUnderTest.Parent, Is.EqualTo(parent));
            Assert.That(systemUnderTest.Content, Is.EqualTo(content));
            Assert.That(systemUnderTest.Url, Is.EqualTo(url));
            Assert.That(systemUnderTest.Authors, Is.EqualTo(authors));
            Assert.That(systemUnderTest.Description, Is.EqualTo(description));
            Assert.That(systemUnderTest.Goals, Is.EqualTo(goals));
            Assert.That(systemUnderTest.Workload, Is.EqualTo(workload));
            Assert.That(systemUnderTest.Points, Is.EqualTo(points));
            Assert.That(systemUnderTest.Difficulty, Is.EqualTo(difficulty));
            Assert.That(systemUnderTest.PositionX, Is.EqualTo(positionX));
            Assert.That(systemUnderTest.PositionY, Is.EqualTo(positionY));
        });
    }
}