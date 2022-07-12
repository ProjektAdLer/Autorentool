using System.IO;
using System.IO.Abstractions.TestingHelpers;
using AuthoringTool.DataAccess.WorldExport;
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

    /*
    [Test]
    public void BackupFileGenerator_WriteBackupFile_BackupFileCreated()
    {
        //Arrange 
        var mockFileSystem = new MockFileSystem();
        var backupFileGen = new BackupFileGenerator(mockFileSystem);
        backupFileGen.CreateBackupFolders();
        
        //Act
        backupFileGen.WriteBackupFile();
        
        //Assert
        Assert.That(Directory.GetFiles(backupFileGen.GetTempDir()), Is.Not.Null);
    }*/
    
}