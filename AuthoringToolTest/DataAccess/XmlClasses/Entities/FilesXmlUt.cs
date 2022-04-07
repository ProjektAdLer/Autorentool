using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;
using AuthoringTool.DataAccess.XmlClasses;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities;

[TestFixture]
public class FilesXmlUt
{
    //There aren´t any Parameters to set yet.
    //Will be needed later, then the Test gets changed. 
    [Test]
    public void FilesXmlFiles_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var filesFiles = new FilesXmlFiles();
        
        //Act
        filesFiles.SetParameters();
        
        //Assert
        Assert.That(filesFiles, Is.EqualTo(filesFiles));
    }
    
    [Test]
    public void FilesXmlFiles_Serialize_XmlFileWritten()
    {
        //Arrange 
        var mockFileSystem = new MockFileSystem();
        var backupFileGen = new BackupFileGenerator(mockFileSystem);
        backupFileGen.CreateBackupFolders();
        var filesFiles = new FilesXmlFiles();
        filesFiles.SetParameters();

        //Act
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        filesFiles.Serialize();
        
        //Assert
        Assert.That(mockFileSystem.FileExists("C:\\XMLFilesForExport\\files.xml"), Is.True);
    }
    
    
}