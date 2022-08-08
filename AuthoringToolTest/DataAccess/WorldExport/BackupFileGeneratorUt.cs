using System.IO;
using System.IO.Abstractions.TestingHelpers;
using AuthoringTool.DataAccess;
using AuthoringTool.DataAccess.DSL;
using AuthoringTool.DataAccess.WorldExport;
using NSubstitute;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.WorldExport;

[TestFixture]
public class BackupFileGeneratorUt
{
    
    [Test]
    public void BackupFileGenerator_CreateBackupFolders_EveryFolderCreated()
    {
        //Arrange
        var mockFileSystem = new MockFileSystem();
        var backupFileGen = new BackupFileGenerator(mockFileSystem);
        var fullDirPath = mockFileSystem.Path.GetFullPath("XMLFilesForExport");
        var fullDirPathCourse = Path.Join(fullDirPath, "course");
        var fullDirPathSections = Path.Join(fullDirPath, "sections");
        var currWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        
        //Act
        backupFileGen.CreateBackupFolders();

        
        //Assert
        var directoryNames = mockFileSystem.Directory.GetDirectories(currWorkDir);
        Assert.That(directoryNames, Contains.Item(fullDirPath));
        
        mockFileSystem.Directory.SetCurrentDirectory(fullDirPath);
        var directoryNamesOneLevelDeeper = mockFileSystem.Directory.GetDirectories(mockFileSystem.Directory.GetCurrentDirectory());
        Assert.That(directoryNamesOneLevelDeeper, Contains.Item(fullDirPathCourse));
        Assert.That(directoryNamesOneLevelDeeper, Contains.Item(fullDirPathSections));
    }

    [Test]
    public void BackupFileGenerator_GetTempDir_TemporaryDirectoryCreatedAndReturned()
    {
        //Arrange
        var mockFileSystem = new MockFileSystem();
        var backupFileGen = new BackupFileGenerator(mockFileSystem);
        var curWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        
        //Act
        backupFileGen.GetTempDir();
        
        //Assert
        var fileSystemTempDir = mockFileSystem.Directory.GetDirectories(curWorkDir);
        var pathTempDir = Path.Join(curWorkDir, "temp");
        Assert.That(fileSystemTempDir, Contains.Item(pathTempDir));
    }

    [Test]
    public void BackupFileGenerator_DirectoryCopy_TargetDirectoryCopied()
    {
        //Arrange
        var mockFileSystem = new MockFileSystem();
        var backupFileGen = new BackupFileGenerator(mockFileSystem);
        backupFileGen.CreateBackupFolders();
        var fullPathFile = mockFileSystem.Path.Join(mockFileSystem.Path.GetFullPath("XMLFilesForExport"), "course.xml");
        mockFileSystem.AddFile(fullPathFile, "encoding=UTF-8");
        var tempDir = backupFileGen.GetTempDir();

        //Act
        backupFileGen.DirectoryCopy("XMLFilesForExport", tempDir);
        var fullDirPath = mockFileSystem.Path.GetFullPath("XMLFilesForExport");
        
        //Assert
        var currentDir = mockFileSystem.Directory.GetDirectories(tempDir);
        var copiedDirectory = mockFileSystem.Directory.GetDirectories(mockFileSystem.Directory.GetCurrentDirectory());
        var copiedFile = mockFileSystem.AllFiles;
        Assert.That(copiedDirectory, Contains.Item(fullDirPath));
        Assert.That(copiedFile, Contains.Item(fullPathFile));
    }

    [Test]
    public void BackupFileGenerator_WriteBackupFile_BackupFileCreated()
    {
        //Arrange 
        var mockReadDsl = Substitute.For<IReadDSL>();
        var mockFileSystem = new MockFileSystem();
        var mockEntityManager = Substitute.For<IXmlEntityManager>();
        var currWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        mockFileSystem.AddFile(Path.Join(currWorkDir, "XMLFilesForExport"), new MockFileData("Hello World"));
        
        //Act
        var systemUnderTest = new BackupFileGenerator(mockFileSystem, mockEntityManager);
        systemUnderTest.CreateBackupFolders();
        systemUnderTest.WriteXmlFiles(mockReadDsl, Path.Join(currWorkDir, "XMLFilesForExport", "Hello World"));
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.xmlEntityManager, Is.Not.Null);
            systemUnderTest.xmlEntityManager.Received().GetFactories(mockReadDsl, Path.Join(currWorkDir, "XMLFilesForExport", "Hello World"));
            
        });

    }
    /*
    [Test]
    public void BackupFileGenerator_WriteBackupFile_FilesCreated_AndTempDirectoryDeleted()
    {
        //Arrange
        var mockReadDsl = Substitute.For<IReadDSL>();
        var mockFileSystem = new MockFileSystem();
        var mockTarSystem = new mock
        var mockEntityManager = Substitute.For<IXmlEntityManager>();
        var currWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        mockFileSystem.AddFile(Path.Join(currWorkDir, "XMLFilesForExport"), new MockFileData("Hello World"));

        var systemUnderTest = new BackupFileGenerator(mockFileSystem, mockEntityManager);
        systemUnderTest.CreateBackupFolders();
        systemUnderTest.WriteXmlFiles(mockReadDsl, Path.Join(currWorkDir, "XMLFilesForExport", "Hello World"));

        //Act
        systemUnderTest.WriteBackupFile(Path.Join(currWorkDir, "XMLFilesForExport", "Hello World"));
        
        //Assert
           
    }*/
    
}