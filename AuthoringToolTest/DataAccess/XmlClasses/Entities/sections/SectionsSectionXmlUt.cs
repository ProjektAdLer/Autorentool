using AuthoringTool.DataAccess.XmlClasses.sections;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities.sections;

[TestFixture]
public class SectionsSectionXmlUt
{
    [Test]
    public void SectionsSectioneXmlSection_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var sectionSection = new SectionsSectionXmlSection();
        
        //Act
        sectionSection.SetParameters("160","1");
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(sectionSection.Id, Is.EqualTo("160"));
            Assert.That(sectionSection.Number, Is.EqualTo("1"));
            
        });
    }
}