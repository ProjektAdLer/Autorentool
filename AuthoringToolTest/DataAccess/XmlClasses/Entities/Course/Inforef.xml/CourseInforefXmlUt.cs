using System.IO;
using System.IO.Abstractions.TestingHelpers;
using AuthoringTool.DataAccess.WorldExport;
using AuthoringTool.DataAccess.XmlClasses.Entities.Course.Inforef.xml;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities.Course.Inforef.xml;

[TestFixture]
public class CourseInforefXmlUt
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
    
    [Test]
    public void CourseInforefXmlInforef_StandardConstructor_AllParametersSet()
    {
        //Arrange
        var inforefRole = new CourseInforefXmlRole();
        var inforefRoleref = new CourseInforefXmlRoleref();
        inforefRoleref.Role = inforefRole;
        var systemUnderTest = new CourseInforefXmlInforef();

        //Act
        systemUnderTest.Roleref = inforefRoleref;
        
        //Assert
        Assert.That(systemUnderTest.Roleref, Is.EqualTo(inforefRoleref));
    }
    
    [Test]

    public void CourseInforefXmlInforef_Serialize_XmlFileWritten()
    {
        //Arrange 
        var mockFileSystem = new MockFileSystem();
        var backupFileGen = new BackupFileGenerator(mockFileSystem);
        backupFileGen.CreateBackupFolders();
        var curWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        
        var inforefRole = new CourseInforefXmlRole();
        var inforefRoleref = new CourseInforefXmlRoleref();
        inforefRoleref.Role = inforefRole;
        
        var systemUnderTest = new CourseInforefXmlInforef();
        systemUnderTest.Roleref = inforefRoleref;

        //Act
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        systemUnderTest.Serialize();
        
        //Assert
        var pathXmlFile = Path.Join(curWorkDir, "XMLFilesForExport", "course", "inforef.xml");
        Assert.That(mockFileSystem.FileExists(pathXmlFile), Is.True);
    }
    
}