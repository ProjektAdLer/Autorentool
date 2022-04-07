using System.IO.Abstractions.TestingHelpers;
using AuthoringTool.DataAccess.WorldExport;
using AuthoringTool.DataAccess.XmlClasses.course;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities.course;

[TestFixture]
public class CourseInforefXmlUt
{
    [Test]
    public void CourseInforefXmlRole_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var inforefRole = new CourseInforefXmlRole();

        //Act
        inforefRole.SetParameters("5");
        
        //Assert
        Assert.That(inforefRole.Id, Is.EqualTo("5"));
    }
    
    [Test]
    public void CourseInforefXmlRoleref_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var inforefRole = new CourseInforefXmlRole();
        inforefRole.SetParameters("5");
        var inforefRoleref = new CourseInforefXmlRoleref();

        //Act
        inforefRoleref.SetParameters(inforefRole);
        
        //Assert
        Assert.That(inforefRoleref.Role, Is.EqualTo(inforefRole));
    }
    
    [Test]
    public void CourseInforefXmlInforef_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var inforefRole = new CourseInforefXmlRole();
        inforefRole.SetParameters("5");
        var inforefRoleref = new CourseInforefXmlRoleref();
        inforefRoleref.SetParameters(inforefRole);
        var inforefInforef = new CourseInforefXmlInforef();

        //Act
        inforefInforef.SetParameters(inforefRoleref);
        
        //Assert
        Assert.That(inforefInforef.Roleref, Is.EqualTo(inforefRoleref));
    }
    
    [Test]

    public void CourseInforefXmlInforef_Serialize_XmlFileWritten()
    {
        //Arrange 
        var mockFileSystem = new MockFileSystem();
        var backupFileGen = new BackupFileGenerator(mockFileSystem);
        backupFileGen.CreateBackupFolders();
        
        var inforefRole = new CourseInforefXmlRole();
        inforefRole.SetParameters("5");
        var inforefRoleref = new CourseInforefXmlRoleref();
        inforefRoleref.SetParameters(inforefRole);
        var inforefInforef = new CourseInforefXmlInforef();
        inforefInforef.SetParameters(inforefRoleref);

        //Act
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        inforefInforef.Serialize();
        
        //Assert
        Assert.That(mockFileSystem.FileExists("C:\\XMLFilesForExport\\course\\inforef.xml"), Is.True);
    }
    
}