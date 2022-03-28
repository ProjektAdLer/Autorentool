using AuthoringTool.DataAccess.XmlClasses;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses;

[TestFixture]
public class RolesXmlUt
{
    [Test]
    public void RolesXmlRolesDefinition_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var rolesRole = new RolesXmlRole();
        rolesRole.SetParameters("", "", "5", "student", "$@NULL@$", "5", "student");
        var rolesRolesDefinition = new RolesXmlRolesDefinition();
        rolesRolesDefinition.SetParameters(rolesRole);
        
        //Act
        rolesRolesDefinition.SetParameters(rolesRole);
        
        //Assert
        Assert.That(rolesRolesDefinition.Role, Is.EqualTo(rolesRole));
        
    }
    
    [Test]
    public void RolesXmlRole_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var rolesRole = new RolesXmlRole();

        //Act
        rolesRole.SetParameters("", "", "5", "student", "$@NULL@$", "5", "student");
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(rolesRole.Name, Is.EqualTo(""));
            Assert.That(rolesRole.Description, Is.EqualTo(""));
            Assert.That(rolesRole.Id, Is.EqualTo("5"));
            Assert.That(rolesRole.Shortname, Is.EqualTo("student"));
            Assert.That(rolesRole.Nameincourse, Is.EqualTo("$@NULL@$"));
            Assert.That(rolesRole.Sortorder, Is.EqualTo("5"));
            Assert.That(rolesRole.Archetype, Is.EqualTo("student"));
        });
    }
}