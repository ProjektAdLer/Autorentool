using System.IO.Abstractions.TestingHelpers;
using Generator.XmlClasses;
using Generator.XmlClasses.Entities.Files.xml;
using NUnit.Framework;

namespace GeneratorTest.XmlClasses;

[TestFixture]
public class XmlFileManagerUt
{
    [Test]
    public void XmlFileManager_GetXmlFilesList_ListReturned()
    {
        // Arrange
        var mockFileSystem = new MockFileSystem();
        
        // Act
        var systemUnderTest = new XmlFileManager(mockFileSystem);
        var list = systemUnderTest.GetXmlFilesList();

        // Assert
        Assert.That(list, Is.Not.Null);
    }
    
    [Test]
    public void XmlFileManager_SetXmlFilesList_ListSet()
    {
        // Arrange
        var mockFileSystem = new MockFileSystem();
        var xmlFile = new FilesXmlFile();
        var fileList = new List<FilesXmlFile>();
        
        fileList.Add(xmlFile);
            // Act
        var systemUnderTest = new XmlFileManager(mockFileSystem);
        systemUnderTest.SetXmlFilesList(fileList);

        // Assert
        Assert.That(systemUnderTest.FilesXmlFilesList, Is.EqualTo(fileList));
    }

    [Test]
    public void XmlFileManager_CalculateHashCheckSumAndFileSize_AndGetHashCheckSumGetFilesize_ReturnsValue()
    {
        // Arrange
        var mockFileSystem = new MockFileSystem();
        mockFileSystem.AddFile(@"C:/test.txt", new MockFileData("Hello World"));
        
        // Act
        var systemUnderTest = new XmlFileManager(mockFileSystem);
        systemUnderTest.CalculateHashCheckSumAndFileSize(@"C:/test.txt");
        var checksum = systemUnderTest.GetHashCheckSum();
        var fileSize = systemUnderTest.GetFileSize();

        // Assert
        Assert.Multiple(()=>
        {
            Assert.That(checksum, Is.Not.Null);
            Assert.That(fileSize, Is.Not.Null);
        });

    }
    
    [Test]
    public void XmlFileManager_CreateFolderAndFiles_FilesAndFoldersCreated()
    {
        // Arrange
        var mockFileSystem = new MockFileSystem();
        var currWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        var txtPath = mockFileSystem.Path.Join(currWorkDir, "test.txt");
        var dirPath = mockFileSystem.Path.Join(currWorkDir, "XMLFilesForExport", "files", "08");
        mockFileSystem.AddFile(txtPath, new MockFileData("Hello World"));
        
        // Act
        var systemUnderTest = new XmlFileManager(mockFileSystem);
        systemUnderTest.CreateFolderAndFiles(txtPath, "08");

        // Assert
        Assert.Multiple(()=>
        {
            Assert.That(mockFileSystem.Directory.Exists(dirPath), Is.True);
            Assert.That(mockFileSystem.File.Exists(txtPath), Is.False);
        });

    }
    
}