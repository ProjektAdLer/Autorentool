using AuthoringTool.DataAccess.XmlClasses.Entities.Course.Course.xml;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities.Course.Course.xml;

[TestFixture]
public class CourseCourseXmlCategoryUt
{
    [Test]
    public void CourseCourseXmlCategory_StandardConstructor_AllParametersSet()
    {
        //Arrange

        //Act
        var systemUnderTest = new CourseCourseXmlCategory();
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo("Miscellaneous"));
            Assert.That(systemUnderTest.Description, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.Id, Is.EqualTo("1"));
        });
    }
}