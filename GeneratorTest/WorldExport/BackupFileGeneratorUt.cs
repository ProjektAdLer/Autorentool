﻿using System.IO.Abstractions.TestingHelpers;
using System.Text;
using Generator.ATF;
using Generator.WorldExport;
using Generator.XmlClasses;
using NSubstitute;
using NUnit.Framework;
using Shared.Configuration;

namespace GeneratorTest.WorldExport;

[TestFixture]
public class BackupFileGeneratorUt
{
    [Test]
    // ANF-ID: [GHO11]
    public void BackupFileGenerator_CreateBackupFolders_EveryFolderCreated()
    {
        //Arrange
        var mockFileSystem = new MockFileSystem();
        var systemUnderTest = new BackupFileGenerator(mockFileSystem);
        var fullDirPath = mockFileSystem.Path.Join(ApplicationPaths.BackupFolder, "XMLFilesForExport");

        //Act
        systemUnderTest.CreateBackupFolders();


        //Assert
        // var directoryNames = mockFileSystem.Directory.GetDirectories(currWorkDir);
        // Assert.That(directoryNames, Contains.Item(fullDirPath));
        Assert.That(mockFileSystem.FileExists(fullDirPath));

        mockFileSystem.Directory.SetCurrentDirectory(fullDirPath);
        var directoryNamesOneLevelDeeper =
            mockFileSystem.Directory.GetDirectories(mockFileSystem.Directory.GetCurrentDirectory());
        Assert.Multiple(() =>
        {
            Assert.That(directoryNamesOneLevelDeeper, Contains.Item(Path.Join(fullDirPath, "course")));
            Assert.That(directoryNamesOneLevelDeeper, Contains.Item(Path.Join(fullDirPath, "sections")));
            Assert.That(directoryNamesOneLevelDeeper, Contains.Item(Path.Join(fullDirPath, "files")));
            Assert.That(directoryNamesOneLevelDeeper, Contains.Item(Path.Join(fullDirPath, "activities")));
        });
    }

    [Test]
    // ANF-ID: [GHO11]
    public void BackupFileGenerator_GetTempDir_TemporaryDirectoryCreatedAndReturned()
    {
        //Arrange
        var mockFileSystem = new MockFileSystem();
        var systemUnderTest = new BackupFileGenerator(mockFileSystem);
        var curWorkDir = mockFileSystem.Directory.GetCurrentDirectory();

        //Act
        var tempFolder = systemUnderTest.GetTempDir();

        //Assert
        var fileSystemTempDir = mockFileSystem.Directory.GetDirectories(curWorkDir);
        var concreteTempDir = mockFileSystem.Directory.GetDirectories(fileSystemTempDir[0]);
        Assert.That(tempFolder, Is.EqualTo(concreteTempDir[0]));
    }

    [Test]
    // ANF-ID: [GHO11]
    public void BackupFileGenerator_DirectoryCopy_TargetDirectoryCopied()
    {
        //Arrange
        var mockFileSystem = new MockFileSystem();
        var systemUnderTest = new BackupFileGenerator(mockFileSystem);
        systemUnderTest.CreateBackupFolders();
        var fullDirPath = mockFileSystem.Path.Join(ApplicationPaths.BackupFolder, "XMLFilesForExport");
        var fullPathFile = mockFileSystem.Path.Join(fullDirPath, "course.xml");
        mockFileSystem.AddFile(fullPathFile, "encoding=UTF-8");
        var tempDir = systemUnderTest.GetTempDir();

        //Act
        systemUnderTest.DirectoryCopy(fullDirPath, tempDir);

        //Assert
        var copiedDirectoriesInTemp = mockFileSystem.Directory.GetDirectories(tempDir);
        var copiedFile = mockFileSystem.AllFiles;
        Assert.Multiple(() =>
        {
            Assert.That(copiedDirectoriesInTemp[0], Is.EqualTo(mockFileSystem.Path.Join(tempDir, "activities")));
            Assert.That(copiedDirectoriesInTemp[1], Is.EqualTo(mockFileSystem.Path.Join(tempDir, "files")));
            Assert.That(copiedDirectoriesInTemp[2], Is.EqualTo(mockFileSystem.Path.Join(tempDir, "course")));
            Assert.That(copiedDirectoriesInTemp[3], Is.EqualTo(mockFileSystem.Path.Join(tempDir, "sections")));
            Assert.That(copiedFile, Contains.Item(fullPathFile));
        });
    }

    [Test]
    // ANF-ID: [GHO11]
    public void BackupFileGenerator_WriteXmlFiles_GetFactoriesReceived()
    {
        //Arrange 
        var mockReadAtf = Substitute.For<IReadAtf>();
        var mockFileSystem = new MockFileSystem();
        var mockEntityManager = Substitute.For<IXmlEntityManager>();

        var systemUnderTest = new BackupFileGenerator(mockFileSystem, mockEntityManager);
        systemUnderTest.CreateBackupFolders();

        //Act
        systemUnderTest.WriteXmlFiles(mockReadAtf);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.XmlEntityManager, Is.Not.Null);
            systemUnderTest.XmlEntityManager.Received().GetFactories(mockReadAtf);
        });
    }

    [Test]
    // ANF-ID: [GHO11]
    public void BackupFileGenerator_WriteBackupFile_FilesCreated_AndTempDirectoryDeleted()
    {
        var curDirectory = ApplicationPaths.BackupFolder;
        try
        {
            //Arrange

            if (Directory.Exists(Path.Join(curDirectory, "XMLFilesForExport")))
            {
                var dir = new DirectoryInfo(Path.Join(curDirectory, "XMLFilesForExport"));
                dir.Delete(true);
            }

            if (File.Exists(Path.Join(curDirectory, "XMLFilesForTesting")))
            {
                File.Delete(Path.Join(curDirectory, "XMLFilesForTesting"));
            }

            var mockEntityManager = Substitute.For<IXmlEntityManager>();
            var systemUnderTest = new BackupFileGenerator(entityManager: mockEntityManager);

            //Create a File and 2 Folders 
            Directory.CreateDirectory(Path.Join(curDirectory, "XMLFilesForExport"));
            Directory.CreateDirectory(Path.Join(curDirectory, "XMLFilesForExport", "course"));
            using (var fs = File.Create(Path.Join(curDirectory, "XMLFilesForExport", "File.txt")))
            {
                // Add some text to file    
                var title = new UTF8Encoding(true).GetBytes("New Text File");
                fs.Write(title, 0, title.Length);
            }

            using (var fs = File.Create(Path.Join(curDirectory, "EmptyWorld.mbz")))
            {
                // Add some text to file    
                var title = new UTF8Encoding(true).GetBytes("New Text File");
                fs.Write(title, 0, title.Length);
            }

            //Act
            systemUnderTest.WriteBackupFile(Path.Join(curDirectory, "XMLFilesForTesting"));

            //Assert
            Assert.That(File.Exists(Path.Join(curDirectory, "XMLFilesForTesting")), Is.True);
        }
        finally
        {
            //Delete all Files and Folders created during the Test
            if (File.Exists(Path.Join(curDirectory, "XMLFilesForTesting")))
            {
                File.Delete(Path.Join(curDirectory, "XMLFilesForTesting"));
            }
        }
    }

    [Test]
    // ANF-ID: [GHO11]
    public void BackupFileGenerator_GetTempDir_ReturnsTempDirectory()
    {
        //Arrange

        var mockFileSystem = new MockFileSystem();
        var systemUnderTest = new BackupFileGenerator(mockFileSystem);

        //Act
        var tempDir = systemUnderTest.GetTempDir();

        //Assert
        Assert.That(tempDir, Is.Not.Empty);
    }

    /// <summary>
    /// This is a regression test for https://github.com/ProjektAdLer/Autorentool/issues/253
    /// </summary>
    [Test]
    // ANF-ID: [GHO11]
    public void WriteBackupFile_MbzFileAlreadyExists_OverwritesFile()
    {
        //we can't mock the filesystem here unfortunately because sharpziplib which we use for creating the tar file
        //does not support substituting the file system
        const string mbzName = "boobaz.mbz";
        const string contents = "OverwriteMePlease";
        const string directory = "XMLFilesForExport";
        try
        {
            long streamLengthAfterWrite;
            Directory.CreateDirectory(directory);
            var joinedPath = Path.Join(directory, mbzName);
            using (var stream = File.OpenWrite(joinedPath))
            {
                using (var writer = new StreamWriter(stream))
                {
                    writer.WriteLine(contents);
                }
            }

            using (var stream = File.OpenRead(joinedPath))
                streamLengthAfterWrite = stream.Length;


            var systemUnderTest = new BackupFileGenerator();

            systemUnderTest.WriteBackupFile(joinedPath);

            using (var stream = File.OpenRead(joinedPath))
            {
                //comparing stream length has to be enough of a sanity check here because 
                Assert.That(stream, Has.Length.Not.EqualTo(streamLengthAfterWrite));
            }
        }
        finally
        {
            if (Directory.Exists(directory))
                Directory.Delete(directory, recursive: true);
        }
    }

    [Test]
    // ANF-ID: [AHO22]
    public void ExtractAtfFromBackup_ThrowsException_WhenBackupFileDoesNotExist()
    {
        //Arrange
        var mockFileSystem = new MockFileSystem();
        var systemUnderTest = new BackupFileGenerator(mockFileSystem);

        //Act
        TestDelegate testDelegate = () => systemUnderTest.ExtractAtfFromBackup("backupFile");

        //Assert
        Assert.That(testDelegate, Throws.TypeOf<FileNotFoundException>());
    }

    [Test]
    public void ExtractAtfFromBackup_CopiesTheAtfToATempDirectory()
    {
        //TODO implement
    }
}