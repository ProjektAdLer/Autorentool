using System.IO.Abstractions.TestingHelpers;
using Generator.WorldExport;
using Generator.XmlClasses.Entities._course.Roles.xml;
using NUnit.Framework;

namespace GeneratorTest.XmlClasses.Entities._course.Roles.xml;

[TestFixture]
public class CourseRoleXmlRolesUt
{
    [Test]
    public void CourseRoleXmlRoles_StandardConstructor_AllParametersSet()
    {
        //Arrange
        
        //Act
        var systemUnderTest = new CourseRolesXmlRoles();
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.RoleOverrides, Is.EqualTo(""));
            Assert.That(systemUnderTest.RoleAssignments, Is.EqualTo(""));
            
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
        
        var systemUnderTest = new CourseRolesXmlRoles();

        //Act
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        systemUnderTest.Serialize();
        
        //Assert
        var pathXmlFile = Path.Join(curWorkDir, "XMLFilesForExport", "course", "roles.xml");
        Assert.That(mockFileSystem.FileExists(pathXmlFile), Is.True);
    }
}