using AuthoringTool.DataAccess.XmlClasses.course;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities.course;

[TestFixture]
public class CourseCourseXmlUt
{
    [Test]
    public void CourseCourseXmlCategory_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var courseCategory = new CourseCourseXmlCategory();
        
        //Act
        courseCategory.SetParameters("Miscellaneous", "$@NULL@$", "1"); 
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(courseCategory.Name, Is.EqualTo("Miscellaneous"));
            Assert.That(courseCategory.Description, Is.EqualTo("$@NULL@$"));
            Assert.That(courseCategory.Id, Is.EqualTo("1"));
        });
    }
    
    [Test]
    public void CourseCourseXmlCourse_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var courseCategory = new CourseCourseXmlCategory();
        courseCategory.SetParameters("Miscellaneous", "$@NULL@$", "1"); 
        var courseCourse = new CourseCourseXmlCourse();
        
        //Act
        courseCourse.SetParameters(courseCategory);
        
        //Assert
        Assert.That(courseCourse.Category, Is.EqualTo(courseCategory));
    }
}