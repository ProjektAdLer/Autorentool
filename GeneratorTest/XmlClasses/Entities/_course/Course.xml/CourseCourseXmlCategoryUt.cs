using Generator.XmlClasses.Entities._course.Course.xml;
using NUnit.Framework;

namespace GeneratorTest.XmlClasses.Entities._course.Course.xml;

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