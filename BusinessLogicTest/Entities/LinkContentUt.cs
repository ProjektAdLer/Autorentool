using BusinessLogic.Entities.LearningContent.LinkContent;
using NUnit.Framework;

namespace BusinessLogicTest.Entities;

[TestFixture]
public class LinkContentUt
{
    [Test]
    public void Constructor_SetsAllParameters()
    {
        var name = "foobar";
        var link = "https://www.google.com";

        var systemUnderTest = new LinkContent(name, link);

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(name));
            Assert.That(systemUnderTest.Link, Is.EqualTo(link));
        });
    }
}