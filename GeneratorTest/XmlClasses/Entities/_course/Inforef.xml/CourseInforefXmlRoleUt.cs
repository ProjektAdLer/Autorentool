using Generator.XmlClasses.Entities._course.Inforef.xml;
using NUnit.Framework;

namespace GeneratorTest.XmlClasses.Entities._course.Inforef.xml;

[TestFixture]
public class CourseInforefXmlRoleUt
{
    [Test]
    public void CourseInforefXmlRole_StandardConstructor_AllParametersSet()
    {
        //Arrange

        //Act
        var systemUnderTest = new CourseInforefXmlRole();
        
        //Assert
        Assert.That(systemUnderTest.Id, Is.EqualTo("5"));
    }
    
   
}