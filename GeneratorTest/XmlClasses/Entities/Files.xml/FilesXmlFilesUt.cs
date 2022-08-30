using System.IO.Abstractions.TestingHelpers;
using Generator.WorldExport;
using Generator.XmlClasses.Entities.Files.xml;
using NUnit.Framework;

namespace GeneratorTest.XmlClasses.Entities.Files.xml;

[TestFixture]
public class FilesXmlFilesUt
{
   
    [Test]
    public void FilesXmlFiles_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var filesFiles = new FilesXmlFiles();
        
        var file1 = new FilesXmlFile();
        var file2 = new FilesXmlFile();

        var list = new List<FilesXmlFile>
        {
            file1,
            file2 
        };

        //Act
        filesFiles.File = (list);

        //Assert
        Assert.That(filesFiles.File, Is.EqualTo(list));
    }

    [Test]
    public void FilesXmlFiles_Serialize_XmlFileWritten()
    {
        //Arrange 
        var mockFileSystem = new MockFileSystem();
        var backupGenerator = new BackupFileGenerator(mockFileSystem);
        backupGenerator.CreateBackupFolders();
        var curWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        
        //Arrange
        var filesFiles = new FilesXmlFiles();
    
        var file1 = new FilesXmlFile();
        var file2 = new FilesXmlFile();

        var list = new List<FilesXmlFile>
        {
            file1,
            file2 
        };
        filesFiles.File = (list);
        
        //Act
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        filesFiles.Serialize();
        
        //Assert
        var pathXmlFile = Path.Join(curWorkDir, "XMLFilesForExport", "files.xml");
        Assert.That(mockFileSystem.FileExists(pathXmlFile), Is.True);
    }
    
    
}