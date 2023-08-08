using Generator.XmlClasses.Entities._sections.Section.xml;
using NUnit.Framework;

namespace GeneratorTest.XmlClasses.Entities._sections;

[TestFixture]
public class SectionsSectionXmlPluginLocalAdlerSectionUt
{
    [Test]
    public void SetParameters_ObjectsAreEqual()
    {
        //Arrange

        //Act
        var systemUnderTest = new SectionsSectionXmlPluginLocalAdlerSection();

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.AdlerSection, Is.Not.Null);
            Assert.That(systemUnderTest.AdlerSection!.RequiredPointsToComplete, Is.EqualTo("0"));
        });
    }
}