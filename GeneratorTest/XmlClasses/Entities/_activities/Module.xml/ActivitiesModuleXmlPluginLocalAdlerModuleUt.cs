using Generator.XmlClasses.Entities._activities.Module.xml;
using NUnit.Framework;

namespace GeneratorTest.XmlClasses.Entities._activities.Module.xml;

[TestFixture]

public class ActivitiesModuleXmlPluginLocalAdlerModuleUt
{
    [Test]
    public void Constructor_AllParametersSet()
    {
        // Arrange
        var systemUnderTest = new ActivitiesModuleXmlPluginLocalAdlerModule();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.AdlerModule, Is.Not.Null);
            Assert.That(systemUnderTest.AdlerModule?.ScoreMax, Is.EqualTo("0"));
        });
    }
}