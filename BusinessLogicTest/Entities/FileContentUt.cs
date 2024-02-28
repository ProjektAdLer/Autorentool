using BusinessLogic.Entities.LearningContent.FileContent;
using NUnit.Framework;

namespace BusinessLogicTest.Entities;

[TestFixture]
public class FileContentUt
{
    [Test]
    public void Constructor_SetsAllParameters()
    {
        var name = "foobar";
        var type = "barbaz";
        var content = "";

        var systemUnderTest = new FileContent(name, type, content);

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(name));
            Assert.That(systemUnderTest.Type, Is.EqualTo(type));
            Assert.That(systemUnderTest.Filepath, Is.EqualTo(content));
            Assert.That(systemUnderTest.PrimitiveH5P, Is.EqualTo(false));
        });
    }
}