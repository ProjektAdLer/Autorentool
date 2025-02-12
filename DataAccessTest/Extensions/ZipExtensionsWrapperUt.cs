using DataAccess.Extensions;
using NUnit.Framework;

using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.IO.Compression;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace DataAccessTest.Extensions;
[TestFixture]
public class ZipExtensionsUnitTest
{
    private MockFileSystem _mockFileSystem;
    private string _basePath;

    [SetUp]
    public void SetUp()
    {
        _mockFileSystem = new MockFileSystem();
        _basePath = Environment.OSVersion.Platform == PlatformID.Win32NT ? "C:" : "/";
    }

    public ZipExtensionsWrapper CreateZipExtensionWrapper(IFileSystem? fileSystem = null)
    {
        fileSystem ??= new MockFileSystem();
        return new ZipExtensionsWrapper(fileSystem);
    }

    [Test]
    public void GetZipArchive_ValidArchive_ReturnsZipArchive()
    {
        var archivePath = "myZipFile.zip";
        var memoryStream = CreateMemmoryStreamWithZipArchive();
        _mockFileSystem.AddFile(archivePath, new MockFileData(memoryStream.ToArray()));

        var systemUnderTest = CreateZipExtensionWrapper(_mockFileSystem);
        var result = systemUnderTest.GetZipArchive(archivePath);

        Assert.That(result, Is.TypeOf<ZipArchive>());
    }
    [Test]
    public void GetZipArchive_NonExistentFile_ThrowsFileNotFoundException()
    {
        var nonExistentFilePath = Path.Combine(_basePath, "test", "nonexistent.zip");

        var systemUnderTest = CreateZipExtensionWrapper(_mockFileSystem);

        Assert.Throws<FileNotFoundException>(() => systemUnderTest.GetZipArchive(nonExistentFilePath));
    }
    [Test]
    public void GetZipArchive_NotAZipFile_ThrowsInvalidDataException()
    {
        var invalidZipFilePath = Path.Combine(_basePath,"test","invalid.zip");
        _mockFileSystem.AddFile(invalidZipFilePath, new MockFileData("Not a zip file"));
        
        var systemUnderTest = CreateZipExtensionWrapper(_mockFileSystem);
        
        Assert.Throws<InvalidDataException>(() => systemUnderTest.GetZipArchive( invalidZipFilePath));
    }
    [Test]
    public void GetZipArchive_NullArgument_ThrowsArgumentNullException()
    {
        var systemUnderTest = CreateZipExtensionWrapper(_mockFileSystem);
        
        Assert.Throws<ArgumentNullException>(() => systemUnderTest.GetZipArchive( null));
    }
    
    [Test]
    [TestCaseSource(nameof(GetInvalidPathChars))]
    public void GetZipArchive_InvalidCharactersInPath_ThrowsArgumentException(char badChar)
    {
        string validPath = Path.Combine("Temp", "Invalid");
        var invalidPath = validPath + badChar;

        var systemUnderTest = CreateZipExtensionWrapper(_mockFileSystem);
        Assert.Throws<ArgumentException>(() => systemUnderTest.GetZipArchive( invalidPath));
    }
    private static IEnumerable<char> GetInvalidPathChars()
    {
        return Path.GetInvalidPathChars();
    }
    [Test]
    public void GetZipArchive_InvalidPathFormat_ThrowsNotSupportedException()
    {
        var invalidFormatPath = "://InvalidPath/file.zip";
        var systemUnderTest = CreateZipExtensionWrapper(_mockFileSystem);
        if (OperatingSystem.IsWindows())
        {
            Assert.Throws<NotSupportedException>(() => systemUnderTest.GetZipArchive(invalidFormatPath));
        }
        else
        {
            Assert.Throws<ArgumentException>(() => systemUnderTest.GetZipArchive(invalidFormatPath));
        }
    }
    /// <summary>
    /// Implementing tested with OpenRead. It would be better if a mock file would be constructed using the TestingHelpers that we are not allowed to access.
    /// </summary>
    [Test]
    public void GetZipArchiveAccessIsDenied_ThrowsUnauthorizedAccessException() 
    {
        var fileSystemMock = NSubstitute.Substitute.For<IFileSystem>();
        string archivePath = "test.zip";
        fileSystemMock.File
            .OpenRead(Arg.Any<string>())
            .Returns(  _=> throw new UnauthorizedAccessException("Simulated Unauthorized Access"));

        var systemUnderTest = CreateZipExtensionWrapper(fileSystemMock);
        
        Assert.Throws<UnauthorizedAccessException>(() =>
        {
            systemUnderTest.GetZipArchive(archivePath);
        });
    }
    /// <summary>
    /// Implementing tested with OpenRead. It would be better if a mock file would be constructed using the TestingHelpers that we are not allowed to access.
    /// </summary>
    [Test]
    public void GetZipArchiveFileCannotBeRead_ThrowsIOException()
    {
        var fileSystemMock = NSubstitute.Substitute.For<IFileSystem>();
        string archivePath = "test.zip";
        fileSystemMock.File
            .OpenRead(Arg.Any<string>())
            .Returns( _=> throw new IOException("Simulated IO Exception"));
        
        var systemUnderTest = CreateZipExtensionWrapper(fileSystemMock);
        
        Assert.Throws<IOException>(() =>
        {
            systemUnderTest.GetZipArchive(archivePath);
        });
    }
    [Test]
    public void GetZipArchive_LargeArchive_Success()
    {
        var archivePath = "largeArchive.zip";
        var memoryStream = CreateLargeArchive();
        _mockFileSystem.AddFile(archivePath, new MockFileData(memoryStream.ToArray()));
        
        var systemUnderTest = CreateZipExtensionWrapper(_mockFileSystem);
        var result = systemUnderTest.GetZipArchive(archivePath);

        Assert.That(result, Is.TypeOf<ZipArchive>());
    }
    [Test]
    public void GetZipArchive_WhitespacePath_ThrowsArgumentException()
    {
        var systemUnderTest = CreateZipExtensionWrapper(_mockFileSystem);

        Assert.Throws<ArgumentException>(() => systemUnderTest.GetZipArchive("   "));
    }
    /// <summary>
    /// Implementing tested with OpenRead. TestingHelper doesn't implement PathTooLong exception.
    /// </summary>
    [Test]
    public void GetZipArchive_PathTooLong_ThrowsPathTooLongException()
    {
        var longPath = new string('a', 260) + ".zip";
        var mockFileSystem = Substitute.For<IFileSystem>();
        mockFileSystem.File.OpenRead(Arg.Is<string>(path => path.Length > 259))
            .Throws<PathTooLongException>();

        var systemUnderTest = CreateZipExtensionWrapper(mockFileSystem);

        Assert.Throws<PathTooLongException>(() => systemUnderTest.GetZipArchive(longPath));
    }
    private static MemoryStream CreateLargeArchive()
    {
        var memoryStream = new MemoryStream();
        using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
        {
            for (int i = 0; i < 1000; i++)
            {
                var entry = archive.CreateEntry($"file{i}.txt");
                using var entryStream = entry.Open();
                using var writer = new StreamWriter(entryStream);
                writer.Write($"This is file {i}");
            }
        }

        memoryStream.Seek(0, SeekOrigin.Begin);
        return memoryStream;
    }

    private static MemoryStream CreateMemmoryStreamWithZipArchive()
    {
        MemoryStream? memoryStream = null;
        try
        {
            memoryStream = new MemoryStream();
            using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                var demoFile = archive.CreateEntry("test.txt");
                using (var entryStream = demoFile.Open())
                using (var streamWriter = new StreamWriter(entryStream))
                {
                    streamWriter.Write("Hello, world!");
                }
            }

            return memoryStream;
        }
        catch
        {
            memoryStream?.Dispose();
            throw;
        }
    }
    [Test]
    public void ExtractToDirectory_ValidArchiveAndDestination_ExtractsFilesSuccessfully()
    {
        var mockFile = new MockFileData("Mock zip content");
        _mockFileSystem.AddFile(Path.Combine("mockArchive.zip"), mockFile);
        var mockArchive = GetMockZipArchive(new[] { Path.Combine("file1.txt"), Path.Combine("folder","file2.txt") });

        var systemUnderTest = CreateZipExtensionWrapper(_mockFileSystem);
        systemUnderTest.ExtractToDirectory(mockArchive, Path.Combine("extracted"));

        Assert.Multiple(() =>
        {
            Assert.That(_mockFileSystem.Directory.Exists(Path.Combine("extracted")), Is.True);
            Assert.That(_mockFileSystem.File.Exists(Path.Combine("extracted", "file1.txt")), Is.True);
            Assert.That(_mockFileSystem.File.Exists(Path.Combine("extracted", "folder", "file2.txt")), Is.True);
        });
    }

    [Test]
    public void ExtractToDirectory_NullDestination_ThrowsArgumentNullException()
    {
        var mockArchive = GetMockZipArchive(new[] { Path.Combine("file1.txt") });

        var systemUnderTest = CreateZipExtensionWrapper(_mockFileSystem);
        
        Assert.That(() => systemUnderTest.ExtractToDirectory(mockArchive, null), Throws.TypeOf<ArgumentNullException>());
    }
    [Test]
    public void ExtractToDirectory_WhitespaceDestination_ThrowsArgumentException()
    {
        var mockArchive = GetMockZipArchive(new[] { Path.Combine("file1.txt") });
        
        var systemUnderTest = CreateZipExtensionWrapper(_mockFileSystem);

        Assert.Throws<ArgumentException>(() => systemUnderTest.ExtractToDirectory(mockArchive, "   "));
    }
    [Test]
    public void ExtractToDirectory_EmptyArchive_CreatesEmptyDirectory()
    {
        var mockArchive = GetMockZipArchive(Array.Empty<string>());

        var systemUnderTest = CreateZipExtensionWrapper(_mockFileSystem);
        systemUnderTest.ExtractToDirectory(mockArchive, Path.Combine("emptyExtract"));
        
        Assert.That(_mockFileSystem.Directory.Exists(Path.Combine("emptyExtract")), Is.True);
        Assert.That(_mockFileSystem.Directory.GetFiles(Path.Combine("emptyExtract")), Is.Empty);
    }
    /// <summary>
    /// Implementing tested with substituted mockFileSystem. It would be better if a mock file would be constructed using the TestingHelpers that we are not allowed to access.
    /// </summary>
    [Test]
    public void ExtractToDirectory_NonWritableDirectory_ThrowsUnauthorizedAccessException()
    {
        var mockArchive = GetMockZipArchive(new[] { Path.Combine("file1.txt") });
        var mockFileSystem = Substitute.For<IFileSystem>();
        mockFileSystem.Directory.Exists(Path.Combine("nonWritable")).Returns(true);
        mockFileSystem.Directory.CreateDirectory(Arg.Any<string>()).Returns(_ => throw new UnauthorizedAccessException("Simulated Unauthorized Access"));

        var systemUnderTest = new ZipExtensionsWrapper(mockFileSystem);

        Assert.Throws<UnauthorizedAccessException>(() => systemUnderTest.ExtractToDirectory(mockArchive, Path.Combine("nonWritable")));
    }

    
    private ZipArchive GetMockZipArchive(string[] fileNames)
    {
        var memoryStream = new MemoryStream();
        using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
        {
            foreach (var fileName in fileNames)
            {
                var entry = archive.CreateEntry(fileName);
                using var entryStream = entry.Open();
                using var writer = new StreamWriter(entryStream);
                writer.Write("Mock content");
            }
        }

        memoryStream.Seek(0, SeekOrigin.Begin);
        return new ZipArchive(memoryStream, ZipArchiveMode.Read);
    }
   
    [Test]
    public async Task CreateFromDirectoryAsync_NullSourcePath_ThrowsArgumentNullException()
    {
        string sourcePath = null;
        string destinationPath = Path.Combine("output.zip");
        
        var systemUnderTest = CreateZipExtensionWrapper();
        
        Assert.That(
            async () => await systemUnderTest.CreateFromDirectoryAsync(sourcePath, destinationPath),
            Throws.ArgumentNullException);
    }

    [Test]
    public async Task CreateFromDirectoryAsync_NullDestinationPath_ThrowsArgumentNullException()
    {
        string sourcePath = Path.Combine("source");
        string destinationPath = null;
        
        var systemUnderTest = CreateZipExtensionWrapper();
        Assert.That(
            async () => await systemUnderTest.CreateFromDirectoryAsync(sourcePath, destinationPath),
            Throws.ArgumentNullException);
    }

    [Test]
    public async Task CreateFromDirectoryAsync_SourcePathDoesNotExist_ThrowsDirectoryNotFoundException()
    {
        string sourcePath = Path.Combine("nonexistent");
        string destinationPath = Path.Combine("output.zip");
        
        var systemUnderTest = CreateZipExtensionWrapper();
       
        Assert.That(
            async () => await systemUnderTest.CreateFromDirectoryAsync(sourcePath, destinationPath),
            Throws.InstanceOf<DirectoryNotFoundException>());
    }

    /// <summary>
    /// Should be discussed further on 29.01.
    /// </summary>
    [Test]
    public void CreateFromDirectoryAsync_NoWritePermission_ThrowsUnauthorizedAccessException()
    {
        string sourcePath = Path.Combine("source");
        string destinationPath =Path.Combine("output.zip");

        _mockFileSystem.AddDirectory(sourcePath);
        _mockFileSystem.AddFile(Path.Combine(sourcePath, "file1.txt"), new MockFileData("File content"));

        var fileSystemMock = Substitute.For<IFileSystem>();
        fileSystemMock.FileSystemWatcher.Returns(_mockFileSystem.FileSystemWatcher);
        fileSystemMock.Directory.Returns(_mockFileSystem.Directory);
        fileSystemMock.File.OpenWrite(Arg.Any<string>())
            .Throws(new UnauthorizedAccessException("Simulated Unauthorized Access"));

        var systemUnderTest = CreateZipExtensionWrapper(fileSystemMock);

        var exception = Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
            await systemUnderTest.CreateFromDirectoryAsync(sourcePath, destinationPath));

        Assert.That(exception, Is.Not.Null);
        Assert.That(exception.Message, Contains.Substring("Simulated Unauthorized Access"));
    }
    

    

    [Test]
    public async Task CreateFromDirectoryAsync_SubdirectoriesAreIncluded_CreatesArchiveWithHierarchy()
    {
        string sourcePath = @"C:\source";
        string destinationPath = @"C:\output.zip";
    
        string subdirectoryPath = Path.Combine(sourcePath, "subdir");
        _mockFileSystem.AddDirectory(sourcePath);
        _mockFileSystem.AddDirectory(subdirectoryPath);
        _mockFileSystem.AddFile(Path.Combine(sourcePath, "file1.txt"), new MockFileData("File1 content"));
        _mockFileSystem.AddFile(Path.Combine(subdirectoryPath, "file2.txt"), new MockFileData("File2 content"));

        var systemUnderTest = CreateZipExtensionWrapper(_mockFileSystem);
        await systemUnderTest.CreateFromDirectoryAsync(sourcePath, destinationPath);
        var archive = OpenZipArchive(destinationPath);
        Assert.Multiple(()=>
        {
          Assert.That(_mockFileSystem.File.Exists(destinationPath), Is.True);
          Assert.That(archive.Entries.Count, Is.EqualTo(2));
          Assert.That(archive.Entries.Any(e => e.FullName == "file1.txt"), Is.True);
          Assert.That(archive.Entries.Any(e => e.FullName == "subdir/file2.txt" || e.FullName == @"subdir\file2.txt"), Is.True);  
        });
        
    }




    [Test]
    public async Task CreateFromDirectoryAsync_EmptySourceDirectory_CreatesEmptyArchive()
    {
        string sourcePath = Path.Combine("source");
        string destinationPath = Path.Combine("output.zip");
        _mockFileSystem.AddDirectory(sourcePath);
        
        var systemUnderTest = CreateZipExtensionWrapper(_mockFileSystem);

        await systemUnderTest.CreateFromDirectoryAsync(sourcePath, destinationPath);
        var archive = OpenZipArchive(destinationPath);
        Assert.Multiple(() =>
        {
            Assert.That(_mockFileSystem.File.Exists(destinationPath), Is.True);
            Assert.That(archive.Entries.Count, Is.EqualTo(0));
        });

    }
    [Test]
    public async Task CreateFromDirectoryAsync_EmptySourcePath_ThrowsArgumentException()
    {
        var systemUnderTest = CreateZipExtensionWrapper();

        var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
            await systemUnderTest.CreateFromDirectoryAsync(string.Empty, Path.Combine("destination.zip")));

        Assert.That(exception!.ParamName, Is.EqualTo(Path.Combine("path")));
    }

    [Test]
    public async Task CreateFromDirectoryAsync_EmptyDestinationPath_ThrowsArgumentException()
    {
        var systemUnderTest = CreateZipExtensionWrapper();

        var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
            await systemUnderTest.CreateFromDirectoryAsync(Path.Combine("source"), string.Empty));

        Assert.That(exception!.ParamName, Is.EqualTo(Path.Combine("path")));
    }
    [Test]
    public async Task CreateFromDirectoryAsync_WhitespaceSourcePath_ThrowsArgumentException()
    {
        var systemUnderTest = CreateZipExtensionWrapper();
        var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
            await systemUnderTest.CreateFromDirectoryAsync("   ", Path.Combine("destination.zip")));

        Assert.That(exception!.ParamName, Is.EqualTo(Path.Combine("path")));
    }
    [Test]
    public async Task CreateFromDirectoryAsync_WhitespaceDestinationPath_ThrowsArgumentException()
    {
        var systemUnderTest = CreateZipExtensionWrapper();
        var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
            await systemUnderTest.CreateFromDirectoryAsync(Path.Combine("source"), "   "));

        Assert.That(exception!.ParamName, Is.EqualTo(Path.Combine("path")));
    }

    [Test]
    public async Task CreateFromDirectoryAsync_SourceContainsFiles_CreateArchiveWithFiles()
    {
        string sourcePath = Path.Combine("source"); 
        string destinationPath = Path.Combine("output.zip");

        AddFilesToMockFileSystem(sourcePath, "File1 content", "File2 content");

        var systemUnderTest = CreateZipExtensionWrapper(_mockFileSystem);
        await systemUnderTest.CreateFromDirectoryAsync(sourcePath, destinationPath);
        var archive = OpenZipArchive(destinationPath);
        var archiveEntries = archive.Entries.Select(e => Path.GetFileName(e.FullName)).ToList();
        
        Assert.Multiple(() =>
        {
            Assert.That(_mockFileSystem.File.Exists(destinationPath), Is.True, "Archive file was not created.");
            Assert.That(archive.Entries.Count, Is.EqualTo(2), "Archive does not contain the expected number of files.");
            Assert.That(archiveEntries.Contains("file1.txt"), Is.True, "file1.txt was not found in the archive.");
            Assert.That(archiveEntries.Contains("file2.txt"), Is.True, "file2.txt was not found in the archive.");
        });
    }



    [Test]
    public async Task CreateFromDirectoryAsync_DestinationAlreadyExists_OverwritesFile()
    {
        string sourcePath = Path.Combine("source");
        string destinationPath = Path.Combine("output.zip");
        AddFilesToMockFileSystem(sourcePath, "file1content","file2content");
        _mockFileSystem.AddFile(destinationPath, new MockFileData("Existing content"));

        var systemUnderTest = CreateZipExtensionWrapper(_mockFileSystem);
        await systemUnderTest.CreateFromDirectoryAsync(sourcePath, destinationPath);
        var archive = OpenZipArchive(destinationPath);
        var archiveEntries = archive.Entries.Select(e => Path.GetFileName(e.FullName)).ToList();

        Assert.Multiple(() =>
        {
            Assert.That(_mockFileSystem.File.Exists(destinationPath), Is.True, "Archive file was not created.");
            Assert.That(archive.Entries.Count, Is.EqualTo(2), "Archive does not contain the expected number of files.");
            Assert.That(archiveEntries.Contains("file1.txt"), Is.True, "file1.txt was not found in the archive.");
            Assert.That(archiveEntries.Contains("file2.txt"), Is.True, "file2.txt was not found in the archive.");
        });
    }
    [Test]
    public async Task CreateFromDirectoryAsync_HiddenFilesInSource_IncludesHiddenFilesInArchive()
    {
        string sourcePath = Path.Combine("source");
        string destinationPath = Path.Combine("output.zip");

        _mockFileSystem.AddDirectory(sourcePath);
        string hiddenFilePath = Path.Combine(sourcePath, "hidden.txt");
        _mockFileSystem.AddFile(hiddenFilePath, new MockFileData("Hidden file content")
        {
            Attributes = FileAttributes.Hidden
        });

        var systemUnderTest = CreateZipExtensionWrapper(_mockFileSystem);
        await systemUnderTest.CreateFromDirectoryAsync(sourcePath, destinationPath);
        var archive = OpenZipArchive(destinationPath);

        var archiveEntries = archive.Entries.Select(e => Path.GetFileName(e.FullName)).ToList();

        Assert.Multiple(() =>
        {
            Assert.That(_mockFileSystem.File.Exists(destinationPath), Is.True);
            Assert.That(archive.Entries.Count, Is.EqualTo(1));
            Assert.That(archiveEntries.Contains("hidden.txt"), Is.True);
        });
    }
    [Test]
    public async Task CreateFromDirectoryAsync_SourcePathIsFile_ThrowsDirectoryNotFoundException()
    {
        string sourcePath = Path.Combine("source","file.txt");
        string destinationPath = Path.Combine("output.zip");
        _mockFileSystem.AddFile(sourcePath, new MockFileData("This is a file"));

        var systemUnderTest = CreateZipExtensionWrapper(_mockFileSystem);

        Assert.That(
            async () => await systemUnderTest.CreateFromDirectoryAsync(sourcePath, destinationPath),
            Throws.InstanceOf<DirectoryNotFoundException>().With.Message.Contains("Could not find a part of the path"));
    }
    
    [Test]
    public async Task CreateFromDirectoryAsync_SymlinksInSource_IgnoresSymlinks()
    {
        string sourcePath = Path.Combine("source");
        string destinationPath = Path.Combine("output.zip");

        _mockFileSystem.AddDirectory(sourcePath);
        _mockFileSystem.AddFile(Path.Combine(sourcePath, "file1.txt"), new MockFileData("File1 content"));
        _mockFileSystem.AddFile(Path.Combine(sourcePath, "symlink"), new MockFileData(string.Empty)
        {
            Attributes = FileAttributes.ReparsePoint
        });

        var systemUnderTest = CreateZipExtensionWrapper(_mockFileSystem);
        await systemUnderTest.CreateFromDirectoryAsync(sourcePath, destinationPath);
        var archive = OpenZipArchive(destinationPath);

        var archiveEntries = archive.Entries.Select(e => Path.GetFileName(e.FullName)).ToList();

        Assert.Multiple(() =>
        {
            Assert.That(_mockFileSystem.File.Exists(destinationPath), Is.True);
            Assert.That(archive.Entries.Count, Is.EqualTo(1));
            Assert.That(archiveEntries.Contains("file1.txt"), Is.True);
        });
    }
    [Test]
    public async Task CreateFromDirectoryAsync_SourceContainsReadOnlyFiles_Success()
    {
        var sourcePath = Path.Combine("source");
        var destinationPath = Path.Combine("destination.zip");
        _mockFileSystem.AddDirectory(sourcePath);
        _mockFileSystem.AddFile(Path.Combine(sourcePath, "readonly.txt"), new MockFileData("Read-only content")
        {
            Attributes = FileAttributes.ReadOnly
        });

        var systemUnderTest = CreateZipExtensionWrapper(_mockFileSystem);
        await systemUnderTest.CreateFromDirectoryAsync(sourcePath, destinationPath);
        var archive = OpenZipArchive(destinationPath);

        var archiveEntries = archive.Entries.Select(e => Path.GetFileName(e.FullName)).ToList();

        Assert.Multiple(() =>
        {
            Assert.That(_mockFileSystem.File.Exists(destinationPath), Is.True);
            Assert.That(archiveEntries.Contains("readonly.txt"), Is.True);
        });
    }
    [Test]
    public async Task CreateFromDirectoryAsync_SourceContainsFilesWithLongPaths_Success()
    {
        var sourcePath = Path.Combine("source");
        var destinationPath = Path.Combine("destination.zip");
        _mockFileSystem.AddDirectory(sourcePath);
        var longFileName = new string('a', 255) + ".txt";
        _mockFileSystem.AddFile(Path.Combine(sourcePath, longFileName), new MockFileData("Long path content"));

        var systemUnderTest = CreateZipExtensionWrapper(_mockFileSystem);
        await systemUnderTest.CreateFromDirectoryAsync(sourcePath, destinationPath);

        Assert.That(_mockFileSystem.File.Exists(destinationPath), Is.True);
    }

    [Test]
    public async Task CreateFromDirectoryAsync_SourceContainsSpecialCharacterFiles_Success()
    {
        var sourcePath = Path.Combine("source");
        var destinationPath = Path.Combine("destination.zip");
        _mockFileSystem.AddDirectory(sourcePath);
        var specialCharFileName = "file@#$!.txt";
        _mockFileSystem.AddFile(Path.Combine(sourcePath, specialCharFileName), new MockFileData("Special characters"));

        var systemUnderTest = CreateZipExtensionWrapper(_mockFileSystem);
        await systemUnderTest.CreateFromDirectoryAsync(sourcePath, destinationPath);
        var archive = OpenZipArchive(destinationPath);

        var archiveEntries = archive.Entries.Select(e => Path.GetFileName(e.FullName)).ToList();

        Assert.That(_mockFileSystem.File.Exists(destinationPath), Is.True);
        Assert.That(archiveEntries.Contains(specialCharFileName), Is.True);
    }

    [Test]
    public async Task CreateFromDirectoryAsync_FilesAddedDuringOperation_IgnoresNewFiles()
    {
        var sourcePath = Path.Combine("source");
        var destinationPath = Path.Combine("destination.zip");
        _mockFileSystem.AddDirectory(sourcePath);
        _mockFileSystem.AddFile(Path.Combine(sourcePath, "file1.txt"), new MockFileData("Initial content"));

        var systemUnderTest = CreateZipExtensionWrapper(_mockFileSystem);
        var task = systemUnderTest.CreateFromDirectoryAsync(sourcePath, destinationPath);
        await Task.Delay(10);
        _mockFileSystem.AddFile(Path.Combine(sourcePath, "file2.txt"), new MockFileData("New content"));
        await task;
        var archive = OpenZipArchive(destinationPath);

        var archiveEntries = archive.Entries.Select(e => Path.GetFileName(e.FullName)).ToList();

        Assert.Multiple(() =>
        {
            Assert.That(_mockFileSystem.File.Exists(destinationPath), Is.True, "Archive file was not created.");
            Assert.That(archive.Entries.Count, Is.EqualTo(1), "Archive should contain only one file.");
            Assert.That(archiveEntries.Contains("file1.txt"), Is.True, "file1.txt was not found in the archive.");
        });
    }

    [Test]
    public async Task CreateFromDirectoryAsync_FilesRemovedDuringOperation_DoesNotFail()
    {
        var sourcePath = Path.Combine("source");
        var destinationPath = Path.Combine("destination.zip");
        _mockFileSystem.AddDirectory(sourcePath);
        _mockFileSystem.AddFile(Path.Combine(sourcePath, "file1.txt"), new MockFileData("Initial content"));

        var systemUnderTest = CreateZipExtensionWrapper(_mockFileSystem);
        var task = systemUnderTest.CreateFromDirectoryAsync(sourcePath, destinationPath);
        await Task.Delay(10);
        _mockFileSystem.RemoveFile(Path.Combine(sourcePath, "file1.txt"));
        await task;
        var archive = OpenZipArchive(destinationPath);

        var archiveEntries = archive.Entries.Select(e => Path.GetFileName(e.FullName)).ToList();

        Assert.Multiple(() =>
        {
            Assert.That(_mockFileSystem.File.Exists(destinationPath), Is.True, "Archive file was not created.");
            Assert.That(archive.Entries.Count, Is.EqualTo(1), "Archive should contain only one file.");
            Assert.That(archiveEntries.Contains("file1.txt"), Is.True, "file1.txt was not found in the archive.");
        });
    }

    [Test]
    public async Task CreateFromDirectoryAsync_DestinationDriveOutOfSpace_ThrowsIOException()
    {
        var sourcePath = Path.Combine("source");
        var destinationPath = Path.Combine("destination.zip");
        _mockFileSystem.AddDirectory(sourcePath);
        _mockFileSystem.AddFile(Path.Combine(sourcePath, "file.txt"), new MockFileData("Some content"));

        var limitedFileSystem = Substitute.For<IFileSystem>();
        limitedFileSystem.FileSystemWatcher.Returns(_mockFileSystem.FileSystemWatcher);
        limitedFileSystem.Directory.Returns(_mockFileSystem.Directory);
        limitedFileSystem.File.OpenWrite(Arg.Any<string>()).Returns(_ => throw new IOException("No space left on device"));

        var systemUnderTest = CreateZipExtensionWrapper(limitedFileSystem);

        Assert.ThrowsAsync<IOException>(async () =>
            await systemUnderTest.CreateFromDirectoryAsync(sourcePath, destinationPath));
    }

    
    [Test]
    public async Task CreateFromDirectoryAsync_SourceContainsVeryLargeFiles_Success()
    {
        var sourcePath = Path.Combine("source");
        var destinationPath = Path.Combine("destination.zip");
        _mockFileSystem.AddDirectory(sourcePath);
        _mockFileSystem.AddFile(Path.Combine(sourcePath, "largefile.bin"), new MockFileData(new byte[1024 * 1024 * 500])); // 500MB

        var systemUnderTest = CreateZipExtensionWrapper(_mockFileSystem);
        await systemUnderTest.CreateFromDirectoryAsync(sourcePath, destinationPath);
        var archive = OpenZipArchive(destinationPath);

        var archiveEntries = archive.Entries.Select(e => Path.GetFileName(e.FullName)).ToList();

        Assert.Multiple(() =>
        {
            Assert.That(_mockFileSystem.File.Exists(destinationPath), Is.True, "Archive file was not created.");
            Assert.That(archive.Entries.Count, Is.EqualTo(1), "Archive should contain only one file.");
            Assert.That(archiveEntries.Contains("largefile.bin"), Is.True, "largefile.bin was not found in the archive.");
        });
    }


    
    private void AddFilesToMockFileSystem(string path, string content, string content2)
    {
        _mockFileSystem.AddDirectory(path);
        _mockFileSystem.AddFile(Path.Combine(path, "file1.txt"), new MockFileData(content));
        _mockFileSystem.AddFile(Path.Combine(path, "file2.txt"), new MockFileData(content2));
    }

    private ZipArchive OpenZipArchive(string path)
    {
        var stream = _mockFileSystem.File.OpenRead(path);
        return new ZipArchive(stream, ZipArchiveMode.Read);
    }
    

}