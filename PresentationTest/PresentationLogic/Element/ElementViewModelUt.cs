using NUnit.Framework;
using Presentation.PresentationLogic.Content;
using Presentation.PresentationLogic.Element;
using Presentation.PresentationLogic.Space;
using Shared;

namespace PresentationTest.PresentationLogic.Element;

[TestFixture]
public class ElementViewModelUt
{
    [Test]
    public void Constructor_InitializesAllProperties()
    {
        var name = "asdf";
        var shortname = "jkl;";
        var parent = new SpaceViewModel("foo", "bar", "baz", "", "", 2);
        var content = new ContentViewModel("bar", "foo", "");
        var url = "url";
        var authors = "ben and jerry";
        var description = "very cool element";
        var goals = "learn very many things";
        var workload = 5;
        var points = 6;
        var difficulty = ElementDifficultyEnum.Easy;
        var positionX = 5f;
        var positionY = 21f;

        var systemUnderTest = new ElementViewModel(name, shortname, content, url, authors,
            description, goals, difficulty, parent, workload, points, positionX, positionY);
        
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

    [Test]
    public void FileEnding_ReturnsCorrectEnding()
    {
        const string expectedFileEnding = "aef";
        var systemUnderTest = new ElementViewModel("foo", "foo", null!,
            "url","foo",  "foo", "foo", ElementDifficultyEnum.Medium);
        Assert.That(systemUnderTest.FileEnding, Is.EqualTo(expectedFileEnding));
    }
}