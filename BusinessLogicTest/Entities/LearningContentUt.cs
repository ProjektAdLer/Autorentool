using BusinessLogic.Entities;
using NUnit.Framework;

namespace BusinessLogicTest.Entities;

[TestFixture]
public class LearningContentUt
{
    [Test]
    public void Constructor_SetsAllParameters()
    {
        var name = "foobar";
        var type = "barbaz";
        var content = new byte[] { 0x00, 0x0f, 0xf0, 0xff };
        
        var systemUnderTest = new LearningContent(name, type, content);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(name));
            Assert.That(systemUnderTest.Type, Is.EqualTo(type));
            Assert.That(systemUnderTest.Content, Is.EqualTo(content));
        });
    }
}