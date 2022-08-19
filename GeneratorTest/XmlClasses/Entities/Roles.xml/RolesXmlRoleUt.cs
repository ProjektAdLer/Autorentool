using Generator.XmlClasses.Entities.Roles.xml;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities.Roles.xml;

[TestFixture]
public class RolesXmlRolesUt
{
    
    [Test]
    public void RolesXmlRoles_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var rolesRole = new RolesXmlRole();
        var systemUnderTest = new RolesXmlRolesDefinition();
        
        //Act
        systemUnderTest.Role = rolesRole;
        
        //Assert
        Assert.That(systemUnderTest.Role, Is.EqualTo(rolesRole));
        
    }
   
}