using System.IO.Abstractions.TestingHelpers;
using System.Text;
using Generator.DSL;
using Generator.WorldExport;
using Generator.XmlClasses;
using NSubstitute;
using NUnit.Framework;

namespace GeneratorTest.WorldExport;

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
        var mockReadDsl = Substitute.For<IReadDsl>();
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
            Assert.That(systemUnderTest.XmlEntityManager, Is.Not.Null);
            systemUnderTest.XmlEntityManager.Received().GetFactories(mockReadDsl, Path.Join(currWorkDir, "XMLFilesForExport", "Hello World"));
            
        });

    }
    
    [Test]
    public void BackupFileGenerator_WriteBackupFile_FilesCreated_AndTempDirectoryDeleted()
    {
        //Arrange
        var curDirectory = Directory.GetCurrentDirectory();
        
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
        using (FileStream fs = File.Create(Path.Join(curDirectory, "XMLFilesForExport", "File.txt")))     
        {    
            // Add some text to file    
            Byte[] title = new UTF8Encoding(true).GetBytes("New Text File");    
            fs.Write(title, 0, title.Length);
        }
        using (FileStream fs = File.Create(Path.Join(curDirectory, "EmptyWorld.mbz")))     
        {    
            // Add some text to file    
            Byte[] title = new UTF8Encoding(true).GetBytes("New Text File");    
            fs.Write(title, 0, title.Length);
        }
        
        //Act
        systemUnderTest.WriteBackupFile(Path.Join(curDirectory, "XMLFilesForTesting"));
        var tempDir = systemUnderTest.GetTempDir();
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(File.Exists(Path.Join(curDirectory, "XMLFilesForTesting")), Is.True);
            Assert.That(Directory.Exists(Path.Join(curDirectory, "XMLFilesForExport", "course")), Is.False);
            Assert.That(Directory.Exists(Path.Join(curDirectory, "XMLFilesForExport")), Is.False);
            Assert.That(File.Exists(Path.Join(curDirectory, "XMLFilesForExport", "File.txt")), Is.False);
        });
        
        //Delete all Files and Folders created during the Test
        if (File.Exists(Path.Join(curDirectory, "XMLFilesForTesting")))
        {
            File.Delete(Path.Join(curDirectory, "XMLFilesForTesting"));
        }
           
    }

    [Test]
    public void BackupFileGenerator_GetTempDir_ReturnsTempDirectory()
    {
        //Arrange
        
        var mockFileSystem = new MockFileSystem();
        var systemUnderTest = new BackupFileGenerator(mockFileSystem);
        
        //Act
        var tempDir =  systemUnderTest.GetTempDir();

        //Assert
        Assert.That(tempDir, Is.Not.Empty);

    }
    
}