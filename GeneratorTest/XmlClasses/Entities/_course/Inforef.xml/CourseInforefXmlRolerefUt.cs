using Generator.XmlClasses.Entities._course.Inforef.xml;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities.Course.Inforef.xml;

[TestFixture]
public class CourseInforefXmlRolerefUt
{
    [Test]
    public void CourseInforefXmlRoleref_StandardConstructor_AllParametersSet()
    {
        //Arrange
        var inforefRole = new CourseInforefXmlRole();
        var systemUnderTest = new CourseInforefXmlRoleref();

        //Act
        systemUnderTest.Role = inforefRole;
        
        //Assert
        Assert.That(systemUnderTest.Role, Is.EqualTo(inforefRole));
    }
}