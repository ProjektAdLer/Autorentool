using System.IO.Abstractions.TestingHelpers;
using Generator.WorldExport;
using Generator.XmlClasses.Entities._course.Inforef.xml;
using NUnit.Framework;

namespace GeneratorTest.XmlClasses.Entities._course.Inforef.xml;

[TestFixture]
public class CourseInforefXmlInforefUt
{

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