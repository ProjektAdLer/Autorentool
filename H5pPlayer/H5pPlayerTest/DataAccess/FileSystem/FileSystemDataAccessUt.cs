﻿using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.IO.Compression;
using H5pPlayer.DataAccess.FileSystem;
using NSubstitute;

namespace H5pPlayerTest.DataAccess.FileSystem;

[TestFixture]
public class FileSystemDataAccessUt
{
    private string _basePath;

    [SetUp]
    public void SetUp()
    {
        _basePath = OperatingSystem.IsWindows() ? "C:" : "/";
    }

    [Test]
    public void ExtractZipFile_ShouldExtractFilesToDestination()
    {
        var zipFilePath = Path.Combine(_basePath, "test", "archive.zip");
        var destinationPath = Path.Combine(_basePath, "test", "output");
        var mockFileSystem = CreateFakeFileSystemWithZipFile(zipFilePath);
        var dataAccess = CreateTestableFileSystemDataAccess(mockFileSystem);

        // Act & Assert: Ensure no exception is thrown during the extraction process
        Assert.That(() => dataAccess.ExtractZipFile(zipFilePath, destinationPath), Throws.Nothing);

        // Assert: Verify that the extracted file exists in the destination directory
        var extractedFilePath = Path.Combine(destinationPath, "testfile.txt");
        Assert.That(mockFileSystem.FileExists(extractedFilePath), Is.True);

        // Assert: Verify the content of the extracted file matches the original content
        var extractedContent = mockFileSystem.File.ReadAllText(extractedFilePath);
        Assert.That(extractedContent, Is.EqualTo("Hello, World!"));
    }

    [Test]
    public void ExtractZipFile_ShouldThrowException_WhenZipFileDoesNotExist()
    {
        // Arrange: Set up a scenario where the ZIP file does not exist
        var zipFilePath = Path.Combine(_basePath, "test", "nonexistent.zip");
        var destinationPath = Path.Combine(_basePath, "test", "output");
        var mockFileSystem = new MockFileSystem();
        var dataAccess = CreateTestableFileSystemDataAccess(mockFileSystem);

        // Act & Assert: Ensure a FileNotFoundException is thrown when the ZIP file is missing
        Assert.That(() => dataAccess.ExtractZipFile(zipFilePath, destinationPath),
            Throws.InstanceOf<FileNotFoundException>());
    }

    [Test]
    public void ExtractZipFile_ShouldCreateDestinationDirectory_IfNotExists()
    {
        // Arrange: Set up a scenario where the destination directory does not exist
        var zipFilePath = Path.Combine(_basePath, "test", "archive.zip");
        var destinationPath = Path.Combine(_basePath, "test", "nonexistentoutput");
        var mockFileSystem = CreateFakeFileSystemWithZipFile(zipFilePath);
        var dataAccess = CreateTestableFileSystemDataAccess(mockFileSystem);

        // Act: Call the method to extract the ZIP file
        Assert.That(() => dataAccess.ExtractZipFile(zipFilePath, destinationPath), Throws.Nothing);

        // Assert: Verify that the destination directory was created
        Assert.That(mockFileSystem.Directory.Exists(destinationPath), Is.True);

        // Assert: Verify that the extracted file exists in the destination directory
        var extractedFilePath = Path.Combine(destinationPath, "testfile.txt");
        Assert.That(mockFileSystem.FileExists(extractedFilePath), Is.True);

        // Assert: Verify the content of the extracted file matches the original content
        var extractedContent = mockFileSystem.File.ReadAllText(extractedFilePath);
        Assert.That(extractedContent, Is.EqualTo("Hello, World!"));
    }

    private MockFileSystem CreateFakeFileSystemWithZipFile(string zipFilePath)
    {
        string zipContent = "Hello, World!";
        var zipFileData = CreateValidZipFile(zipContent);
        var mockFileSystem = new MockFileSystem();
        mockFileSystem.AddFile(zipFilePath, new MockFileData(zipFileData));
        return mockFileSystem;
    }

    private byte[] CreateValidZipFile(string content)
    {
        using (var memoryStream = new MemoryStream())
        {
            using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, leaveOpen: true))
            {
                var entry = zipArchive.CreateEntry("testfile.txt");
                using (var writer = new StreamWriter(entry.Open()))
                {
                    writer.Write(content);
                }
            }

            return memoryStream.ToArray();
        }
    }

    [Test]
    public void DirectoryExists_DirectoryExists_ReturnsTrue()
    {
        var mockFileSystem = new MockFileSystem();
        var testDirectory = Path.Combine(_basePath, "test");
        mockFileSystem.AddDirectory(testDirectory);
        var systemUnderTest = CreateTestableFileSystemDataAccess(mockFileSystem);
        
        var result = systemUnderTest.DirectoryExists(testDirectory);
        
        Assert.That(result, Is.True);
    }
    
    [Test]
    public void DirectoryExists_DirectoryNotExists_ReturnsFalse()
    {
        var mockFileSystem = new MockFileSystem();
        var testDirectory = Path.Combine(_basePath, "test");
        var systemUnderTest = CreateTestableFileSystemDataAccess(mockFileSystem);
        
        var result = systemUnderTest.DirectoryExists(testDirectory);
        
        Assert.That(result, Is.False);
    }

    
    [Test]
    public void DeleteAllFilesAndDirectoriesIn_ShouldThrowIfDirectoryDoesNotExist()
    {
        var mockFileSystem = new MockFileSystem();
        var nonExistentDirectory = Path.Combine(_basePath, "nonexistent");

        var dataAccess = CreateTestableFileSystemDataAccess(mockFileSystem);

        var ex = Assert.Throws<DirectoryNotFoundException>(() =>
            dataAccess.DeleteAllFilesAndDirectoriesIn(nonExistentDirectory));

        Assert.That(ex.Message, Is.EqualTo($"The directory '{nonExistentDirectory}' does not exist."));
    }


    [Test]
    public void DeleteAllFilesAndDirectoriesIn()
    {
        var mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { Path.Combine(_basePath, "test", "file1.txt"), new MockFileData("File 1") },
            { Path.Combine(_basePath, "test", "ile2.txt"), new MockFileData("File 2") },
            { Path.Combine(_basePath, "test", "file3.txt"), new MockFileData("File 3") },
            { Path.Combine(_basePath, "test", "ile4.txt"), new MockFileData("File 4") }
        });

        var directoryToClean = Path.Combine(_basePath, "test");

        var dataAccess = CreateTestableFileSystemDataAccess(mockFileSystem);

        dataAccess.DeleteAllFilesAndDirectoriesIn(directoryToClean);

        Assert.Multiple(() =>
        {
            // Files and subdirectories should be deleted
            Assert.That(mockFileSystem.File.Exists(Path.Combine(_basePath, "test", "ile1.txt")), Is.False);
            Assert.That(mockFileSystem.File.Exists(Path.Combine(_basePath, "test", "file2.txt")), Is.False);
            Assert.That(mockFileSystem.File.Exists(Path.Combine(_basePath, "test", "subdir", "file3.txt")), Is.False);
            Assert.That(mockFileSystem.File.Exists(Path.Combine(_basePath, "test", "subdir2", "file4.txt")), Is.False);

            // The directory itself should not be deleted
            Assert.That(mockFileSystem.Directory.Exists(directoryToClean), Is.True);
        });
    }

    [Test]
    public void DeleteDirectoryRecursively_ThrowsExceptionIfPathIsNull()
    {
        var systemUnderTest = CreateTestableFileSystemDataAccess();

        Assert.Throws<ArgumentNullException>(() => systemUnderTest.DeleteAllFilesAndDirectoriesIn(null!));
    }

    [Test]
    public void DeleteDirectoryRecursively_ThrowsExceptionIfPathIsEmpty()
    {
        var systemUnderTest = CreateTestableFileSystemDataAccess();

        Assert.Throws<ArgumentException>(() => systemUnderTest.DeleteAllFilesAndDirectoriesIn(string.Empty));
    }

    [Test]
    public void DeleteAllFilesAndDirectoriesIn_ShouldNotThrowIfDirectoryIsEmpty()
    {
        var mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { Path.Combine(_basePath, "emptyDir"), new MockDirectoryData() }
        });
        var directoryToClean = Path.Combine(_basePath, "emptyDir");

        var dataAccess = CreateTestableFileSystemDataAccess(mockFileSystem);

        Assert.DoesNotThrow(() => dataAccess.DeleteAllFilesAndDirectoriesIn(directoryToClean));
        Assert.That(mockFileSystem.Directory.Exists(directoryToClean), Is.True);
    }

    [Test]
    public void DeleteAllFilesAndDirectoriesIn_ShouldDeleteOnlyInsideTargetDirectory()
    {
        var mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { Path.Combine(_basePath, "test", "file1.txt"), new MockFileData("File 1") },
            { Path.Combine(_basePath, "test", "file2.txt"), new MockFileData("File 2") },
            { Path.Combine(_basePath, "outside", "file3.txt"), new MockFileData("File 3") }
        });
        var directoryToClean = Path.Combine(_basePath, "test");
        var dataAccess = CreateTestableFileSystemDataAccess(mockFileSystem);

        dataAccess.DeleteAllFilesAndDirectoriesIn(directoryToClean);

        Assert.Multiple(() =>
        {
            Assert.That(mockFileSystem.File.Exists(Path.Combine(_basePath, "test", "file1.txt")), Is.False);
            Assert.That(mockFileSystem.File.Exists(Path.Combine(_basePath, "test", "file2.txt")), Is.False);
            Assert.That(mockFileSystem.File.Exists(Path.Combine(_basePath, "outside", "file3.txt")), Is.True);
        });
    }

    [Test]
    public void DeleteAllFilesAndDirectoriesIn_ShouldIgnoreHiddenFiles()
    {
        var mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { Path.Combine(_basePath, "test", "file1.txt"), new MockFileData("File 1") },
            {
                Path.Combine(_basePath, "test", ".hiddenfile"),
                new MockFileData("Hidden File") { Attributes = FileAttributes.Hidden }
            }
        });
        var directoryToClean = Path.Combine(_basePath, "test");
        var dataAccess = CreateTestableFileSystemDataAccess(mockFileSystem);

        dataAccess.DeleteAllFilesAndDirectoriesIn(directoryToClean);

        Assert.Multiple(() =>
        {
            Assert.That(mockFileSystem.File.Exists(Path.Combine(_basePath, "test", "file1.txt")), Is.False);
            Assert.That(mockFileSystem.File.Exists(Path.Combine(_basePath, "test", ".hiddenfile")), Is.False);
        });
    }

    [Test]
    public void DeleteAllFilesAndDirectoriesIn_ShouldThrowUnauthorizedAccessException_WhenFileIsReadOnly()
    {
        var mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            {
                Path.Combine(_basePath, "test", "readonlyfile.txt"),
                new MockFileData("Readonly File") { Attributes = FileAttributes.ReadOnly }
            }
        });
        var directoryToClean = Path.Combine(_basePath, "test");
        var dataAccess = CreateTestableFileSystemDataAccess(mockFileSystem);

        var ex = Assert.Throws<UnauthorizedAccessException>(() =>
            dataAccess.DeleteAllFilesAndDirectoriesIn(directoryToClean));

        Assert.That(ex.Message, Does.Contain("Access to the path"));
    }

    private static FileSystemDataAccess CreateTestableFileSystemDataAccess(IFileSystem? mockFileSystem = null!)
    {
        mockFileSystem ??= Substitute.For<IFileSystem>();
        return new FileSystemDataAccess(mockFileSystem);
    }
}