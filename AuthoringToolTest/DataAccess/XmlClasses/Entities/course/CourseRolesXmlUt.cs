using AuthoringTool.DataAccess.XmlClasses.course;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities.course;

[TestFixture]
public class CourseRolesXmlUt
{
    [Test]
    public void CourseRoleXmlRoles_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var rolesRoles = new CourseRolesXmlRoles();
        
        //Act
        rolesRoles.SetParameters("", "");
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(rolesRoles.Role_overrides, Is.EqualTo(""));
            Assert.That(rolesRoles.Role_assignments, Is.EqualTo(""));
            
        });
    }
}