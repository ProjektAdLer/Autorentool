using Generator.XmlClasses.Entities._activities.Module.xml;
using NUnit.Framework;

namespace GeneratorTest.XmlClasses.Entities._activities.Module.xml;

[TestFixture]

public class ActivitiesModuleXmlAdlerScoreUt
{
    [Test]
    public void Constructor_AllParametersSet()
    {
        // Arrange
        var systemUnderTest = new ActivitiesModuleXmlAdlerScore();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.ScoreMax, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Timecreated, Is.EqualTo("0"));
            Assert.That(systemUnderTest.TimeModified, Is.EqualTo("0"));
        });
    }
}