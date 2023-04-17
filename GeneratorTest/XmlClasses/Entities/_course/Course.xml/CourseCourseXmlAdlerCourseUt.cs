using Generator.XmlClasses.Entities._course.Course.xml;
using NUnit.Framework;

namespace GeneratorTest.XmlClasses.Entities._course.Course.xml;

[TestFixture]

public class CourseCourseXmlAdlerCourseUt
{
   [Test]
   public void Constructor_AllParametersSet()
   {
      // Arrange
      var systemUnderTest = new CourseCourseXmlAdlerCourse();

      // Assert
      Assert.Multiple(() =>
      {
         Assert.That(systemUnderTest.Foo, Is.EqualTo("bar"));
      });
   }
}