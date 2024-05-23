using Generator.XmlClasses.Entities._sections.Section.xml;
using NUnit.Framework;

namespace GeneratorTest.XmlClasses.Entities._sections;

[TestFixture]
public class SectionsSectionXmlAdlerSectionUt
{
    [Test]
    // ANF-ID: [GHO11]
    public void SetParameters_ObjectsAreEqual()
    {
        //Arrange

        //Act
        var systemUnderTest = new SectionsSectionXmlAdlerSection();

        //Assert
        Assert.Multiple(() => { Assert.That(systemUnderTest.RequiredPointsToComplete, Is.EqualTo("0")); });
    }
}