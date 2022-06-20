using System.IO;
using System.IO.Abstractions.TestingHelpers;
using AuthoringTool.DataAccess.WorldExport;
using AuthoringTool.DataAccess.XmlClasses;
using AuthoringTool.DataAccess.XmlClasses.Entities;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities;

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
    
    [Test]
    public void RolesXmlRole_Serialize_XmlFileWritten()
    {
        //Arrange 
        var mockFileSystem = new MockFileSystem();
        var backupFileGen = new BackupFileGenerator(mockFileSystem);
        backupFileGen.CreateBackupFolders();
        var curWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        
        var rolesRole = new RolesXmlRole();
        rolesRole.SetParameters("", "", "5", "student", "$@NULL@$", "5", "student");
        var rolesRolesDefinition = new RolesXmlRolesDefinition();
        rolesRolesDefinition.SetParameters(rolesRole);

        //Act
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        rolesRolesDefinition.Serialize();
        
        //Assert
        var pathXmlFile = Path.Join(curWorkDir, "XMLFilesForExport", "roles.xml");
        Assert.That(mockFileSystem.FileExists(pathXmlFile), Is.True);
    }
}