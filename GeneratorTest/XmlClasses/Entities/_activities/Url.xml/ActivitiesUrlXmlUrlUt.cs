using Generator.XmlClasses.Entities._activities.Url.xml;
using NUnit.Framework;

namespace GeneratorTest.XmlClasses.Entities._activities.Url.xml;

[TestFixture]
public class ActivitiesUrlXmlUrlUt
{
    [Test]
    public void ActivitiesUrlXmlUrlUt_StandardConstructor_AllParametersSet()
    {
        // Arrange
        
        // Act
        var systemUnderTest = new ActivitiesUrlXmlUrl();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(""));
            Assert.That(systemUnderTest.Intro, Is.EqualTo(""));
            Assert.That(systemUnderTest.Introformat, Is.EqualTo("1"));
            Assert.That(systemUnderTest.Externalurl, Is.EqualTo(""));
            Assert.That(systemUnderTest.Display, Is.EqualTo("1"));
            Assert.That(systemUnderTest.Displayoptions, Is.EqualTo("a:1:{s:10:\"printintro\";i:1;}"));
            Assert.That(systemUnderTest.Parameters, Is.EqualTo("a:0:{}"));
            Assert.That(systemUnderTest.Timemodified, Is.EqualTo(""));
            Assert.That(systemUnderTest.Id, Is.EqualTo(""));
        });
        
    }
}