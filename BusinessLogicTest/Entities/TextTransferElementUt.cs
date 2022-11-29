using BusinessLogic.Entities;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Entities;

[TestFixture]

public class TextTransferElementUt
{
    [Test]
    public void Constructor_SetsAllParameters()
    {
        
        var name = "asdf";
        var shortname = "jkl;";
        var parent = new LearningSpace("foo", "bar", "", "", "", 3);
        var content = new LearningContent("a", "b", "");
        var url = "url";
        var authors = "ben and jerry";
        var description = "very cool element";
        var goals = "learn very many things";
        var difficulty = LearningElementDifficultyEnum.Medium;
        var workload = 5;
        var points = 6;
        var positionX = 5f;
        var positionY = 21f;

        var systemUnderTest = new TextTransferElement(name, shortname, parent, content, url,authors, description, goals,
            difficulty, workload, points, positionX, positionY);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(name));
            Assert.That(systemUnderTest.Shortname, Is.EqualTo(shortname));
            Assert.That(systemUnderTest.Parent, Is.EqualTo(parent));
            Assert.That(systemUnderTest.LearningContent, Is.EqualTo(content));
            Assert.That(systemUnderTest.Url, Is.EqualTo(url));
            Assert.That(systemUnderTest.Authors, Is.EqualTo(authors));
            Assert.That(systemUnderTest.Description, Is.EqualTo(description));
            Assert.That(systemUnderTest.Goals, Is.EqualTo(goals));
            Assert.That(systemUnderTest.Difficulty, Is.EqualTo(difficulty));
            Assert.That(systemUnderTest.Workload, Is.EqualTo(workload));
            Assert.That(systemUnderTest.Points, Is.EqualTo(points));
            Assert.That(systemUnderTest.PositionX, Is.EqualTo(positionX));
            Assert.That(systemUnderTest.PositionY, Is.EqualTo(positionY));
        });
    }
}