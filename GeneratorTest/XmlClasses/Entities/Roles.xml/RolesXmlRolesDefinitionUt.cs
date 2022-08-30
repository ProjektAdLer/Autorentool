using System.IO.Abstractions.TestingHelpers;
using Generator.WorldExport;
using Generator.XmlClasses.Entities.Roles.xml;
using NUnit.Framework;

namespace GeneratorTest.XmlClasses.Entities.Roles.xml;

[TestFixture]
public class RolesXmlRolesDefinitionUt
{
    [Test]
    public void RolesXmlRolesDefinition_StandardConstructor_AllParametersSet()
    {
        //Arrange

        //Act
        var systemUnderTest = new RolesXmlRole();
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(""));
            Assert.That(systemUnderTest.Description, Is.EqualTo(""));
            Assert.That(systemUnderTest.Id, Is.EqualTo("5"));
            Assert.That(systemUnderTest.Shortname, Is.EqualTo("student"));
            Assert.That(systemUnderTest.NameInCourse, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.Sortorder, Is.EqualTo("5"));
            Assert.That(systemUnderTest.Archetype, Is.EqualTo("student"));
        });
    }
    
     
    [Test]
    public void RolesXmlRolesDefinition_Serialize_XmlFileWritten()
    {
        //Arrange 
        var mockFileSystem = new MockFileSystem();
        var backupFileGen = new BackupFileGenerator(mockFileSystem);
        backupFileGen.CreateBackupFolders();
        var curWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        
        var rolesRole = new RolesXmlRole();
        var systemUnderTest = new RolesXmlRolesDefinition();
        systemUnderTest.Role = rolesRole;

        //Act
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        systemUnderTest.Serialize();
        
        //Assert
        var pathXmlFile = Path.Join(curWorkDir, "XMLFilesForExport", "roles.xml");
        Assert.That(mockFileSystem.FileExists(pathXmlFile), Is.True);
    }
    
    
}