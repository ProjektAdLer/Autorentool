using System;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using AuthoringTool.DataAccess.WorldExport;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.ExportWorld;

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
        var fullDirPathCourse = fullDirPath + "\\course";
        var fullDirPathSections = fullDirPath + "\\sections";
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
    
}