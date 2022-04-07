using System.IO;
using System.IO.Abstractions.TestingHelpers;
using AuthoringTool.DataAccess.WorldExport;
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
    
    [Test]

    public void CourseInforefXmlInforef_Serialize_XmlFileWritten()
    {
        //Arrange 
        var mockFileSystem = new MockFileSystem();
        var backupFileGen = new BackupFileGenerator(mockFileSystem);
        backupFileGen.CreateBackupFolders();
        var curWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        
        var rolesRoles = new CourseRolesXmlRoles();
        rolesRoles.SetParameters("", "");

        //Act
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        rolesRoles.Serialize();
        
        //Assert
        var pathXmlFile = Path.Join(curWorkDir, "XMLFilesForExport\\course\\roles.xml");
        Assert.That(mockFileSystem.FileExists(pathXmlFile), Is.True);
    }
}