using Generator.XmlClasses.Entities._activities.Resource.xml;
using NUnit.Framework;

namespace GeneratorTest.XmlClasses.Entities._activities.Resource.xml;

[TestFixture]
public class ActivitiesResourceXmlResourceUt
{
    [Test]
    public void ActivitiesResourceXmlResource_StandardConstructor_AllParametersSet()
    {
        // Arrange
        
        // Act
        var systemUnderTest = new ActivitiesResourceXmlResource();
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(""));
            Assert.That(systemUnderTest.Intro, Is.EqualTo(""));
            Assert.That(systemUnderTest.IntroFormat, Is.EqualTo("1"));
            Assert.That(systemUnderTest.TobeMigrated, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Legacyfiles, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Legacyfileslast, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.Display, Is.EqualTo("0"));
            Assert.That(systemUnderTest.DisplayOptions, Is.EqualTo(""));
            Assert.That(systemUnderTest.FilterFiles, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Revision, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Timemodified, Is.EqualTo(""));
            Assert.That(systemUnderTest.Id, Is.EqualTo(""));
            
        });
    }
}