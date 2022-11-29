using Generator.XmlClasses.Entities._activities.Label.xml;
using NUnit.Framework;

namespace GeneratorTest.XmlClasses.Entities._activities.Label.xml;

[TestFixture]
public class ActivitiesLabelXmlLabelUt
{
    [Test]
    public void ActivitiesLabelXmlLabel_StandardConstructor_AllParametersSet()
    {
        // Arrange

        // Act
        var systemUnderTest = new ActivitiesLabelXmlLabel();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(""));
            Assert.That(systemUnderTest.Intro, Is.EqualTo(""));
            Assert.That(systemUnderTest.Introformat, Is.EqualTo("1"));
            Assert.That(systemUnderTest.Timemodified, Is.EqualTo(""));
            Assert.That(systemUnderTest.Id, Is.EqualTo(""));
        });
    }
}