using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Security.Cryptography;
using BusinessLogic.ErrorManagement.DataAccess;
using DataAccess.Persistence;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using PersistEntities.LearningContent;

namespace DataAccessTest.Persistence;

[TestFixture]
public class ContentFileHandlerUt
{
    private string ContentFilesFolderPath => Path.Join(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "AdLerAuthoring", "ContentFiles");

    [Test]
    public async Task LoadContentAsync_WithFilepath_NoDuplicateFile_CopiesFileCreatesHashFileAndReturnsCorrectObject()
    {
        const string filepath = "foobar.png";
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { filepath, new MockFileData(new byte[] { 0x42, 0x24, 0x53, 0x54 }) },
        });

        var systemUnderTest = CreateTestableContentFileHandler(fileSystem: fileSystem);

        var objActual = await systemUnderTest.LoadContentAsync(filepath);
        Assert.That(objActual, Is.TypeOf<FileContentPe>());
        var objActualAsFileContentPe = (FileContentPe)objActual;
        Assert.Multiple(() =>
        {
            Assert.That(objActual.Name, Is.EqualTo("foobar.png"));
            Assert.That(objActualAsFileContentPe.Type, Is.EqualTo("png"));
            Assert.That(objActualAsFileContentPe.Filepath, Is.EqualTo(Path.Join(ContentFilesFolderPath, "foobar.png")));
            Assert.That(fileSystem.File.Exists(Path.Join(ContentFilesFolderPath, "foobar.png")), Is.True);
            Assert.That(fileSystem.File.Exists(Path.Join(ContentFilesFolderPath, "foobar.png.hash")), Is.True);
        });
    }

    [Test]
    public async Task LoadContentAsync_WithFilepath_DuplicateFile_DoesNotCopyAndThrowsHashExistsException()
    {
        const string filepath = "foobar.png";
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { filepath, new MockFileData(new byte[] { 0x42, 0x24, 0x53, 0x54 }) },
            { Path.Join(ContentFilesFolderPath, "a.png"), new MockFileData(new byte[] { 0x42, 0x24, 0x53, 0x54 }) },
        });

        var systemUnderTest = CreateTestableContentFileHandler(fileSystem: fileSystem);

        Assert.Multiple(() =>
        {
            Assert.That(async () => await systemUnderTest.LoadContentAsync(filepath),
                Throws.TypeOf<HashExistsException>());
            //two files: the content itself and its hash file
            Assert.That(fileSystem.Directory.EnumerateFiles(ContentFilesFolderPath).Count(), Is.EqualTo(2));
            Assert.That(fileSystem.File.Exists(Path.Join(ContentFilesFolderPath, "foobar.png")), Is.False);
        });
    }

    [Test]
    public async Task
        LoadContentAsync_WithFilepath_DuplicateFileNameWithDifferentContent_CopiesAndReturnsCorrectObject()
    {
        const string filepath = "foobar.png";
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { filepath, new MockFileData(new byte[] { 0x42, 0x24, 0x53, 0x54 }) },
            {
                Path.Join(ContentFilesFolderPath, "foobar.png"),
                new MockFileData(new byte[] { 0x00, 0x53, 0x00, 0x53, })
            },
        });

        var systemUnderTest = CreateTestableContentFileHandler(fileSystem: fileSystem);

        var objActual = await systemUnderTest.LoadContentAsync(filepath);
        Assert.That(objActual, Is.TypeOf<FileContentPe>());
        var objActualAsFileContentPe = (FileContentPe)objActual;
        Assert.Multiple(() =>
        {
            Assert.That(objActual.Name, Is.EqualTo("foobar(1).png"));
            Assert.That(objActualAsFileContentPe.Type, Is.EqualTo("png"));
            Assert.That(objActualAsFileContentPe.Filepath,
                Is.EqualTo(Path.Join(ContentFilesFolderPath, "foobar(1).png")));
            Assert.That(fileSystem.File.Exists(Path.Join(ContentFilesFolderPath, "foobar(1).png")), Is.True);
            Assert.That(fileSystem.Directory.EnumerateFiles(ContentFilesFolderPath).Count(), Is.EqualTo(4));
        });
    }

    [Test]
    public void LoadContentAsync_WithFilepath_EmptyFile_ThrowsException()
    {
        const string filepath = "foobar.png";
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { filepath, new MockFileData(Array.Empty<byte>()) },
        });

        var systemUnderTest = CreateTestableContentFileHandler(fileSystem: fileSystem);

        Assert.ThrowsAsync(
            Is.TypeOf<IOException>()
                .And.Message.EqualTo($"File at path {filepath} is empty.")
                .And.InnerException.Not.Null
                .And.InnerException.TypeOf<IOException>()
                .And.InnerException.Message.EqualTo("The given stream is empty."),
            async () => await systemUnderTest.LoadContentAsync(filepath));
    }


    [Test]
    public async Task LoadContentAsync_WithStream_NoDuplicateFile_CopiesFileAndReturnsCorrectObject()
    {
        var fileSystem = new MockFileSystem();
        const string filepath = "foobar.png";
        var stream = new MemoryStream(new byte[] { 0x42, 0x24, 0x53, 0x54 });


        var systemUnderTest = CreateTestableContentFileHandler(fileSystem: fileSystem);

        var objActual = await systemUnderTest.LoadContentAsync(filepath, stream);
        Assert.That(objActual, Is.TypeOf<FileContentPe>());
        var objActualAsFileContentPe = (FileContentPe)objActual;
        Assert.Multiple(() =>
        {
            Assert.That(objActual.Name, Is.EqualTo("foobar.png"));
            Assert.That(objActualAsFileContentPe.Type, Is.EqualTo("png"));
            Assert.That(objActualAsFileContentPe.Filepath, Is.EqualTo(Path.Join(ContentFilesFolderPath, "foobar.png")));
            Assert.That(fileSystem.File.Exists(Path.Join(ContentFilesFolderPath, "foobar.png")), Is.True);
        });
    }

    [Test]
    public async Task LoadContentAsync_WithStream_DuplicateFile_DoesNotCopyAndThrowsHashExistsException()
    {
        var fileSystem = new MockFileSystem();
        const string filepath = "foobar.png";
        var stream = new MemoryStream(new byte[] { 0x42, 0x24, 0x53, 0x54 });
        fileSystem.AddFile(Path.Join(ContentFilesFolderPath, "a.txt"),
            new MockFileData(new byte[] { 0x42, 0x24, 0x53, 0x54 }));

        var systemUnderTest = CreateTestableContentFileHandler(fileSystem: fileSystem);


        Assert.Multiple(() =>
        {
            Assert.That(async () => await systemUnderTest.LoadContentAsync(filepath, stream),
                Throws.TypeOf<HashExistsException>());
            Assert.That(fileSystem.File.Exists(Path.Join(ContentFilesFolderPath, "foobar.png")), Is.False);
            //two files: the content itself and its hash file
            Assert.That(fileSystem.Directory.EnumerateFiles(ContentFilesFolderPath).Count(), Is.EqualTo(2));
        });
    }

    [Test]
    public void LoadContentAsync_WithStream_EmptyStream_ThrowsException()
    {
        var fileSystem = new MockFileSystem();
        var stream = new MemoryStream();
#pragma warning disable NUnit2046
        Assert.That(stream.Length, Is.EqualTo(0));
#pragma warning restore NUnit2046

        var systemUnderTest = CreateTestableContentFileHandler(fileSystem: fileSystem);

        Assert.ThrowsAsync(
            Is.TypeOf<IOException>()
                .And.Message.EqualTo("The given stream is empty."),
            async () => await systemUnderTest.LoadContentAsync("foo", stream));
    }

    [Test]
    public async Task GetFilePathOfExistingCopyAndHashAsync_WithExactSameFile_ReturnsPath()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile("a.txt", new MockFileData(new byte[] { 0x42, 0x24, 0x53, 0x54 }));
        fileSystem.AddFile(Path.Join(ContentFilesFolderPath, "a.txt"),
            new MockFileData(new byte[] { 0x42, 0x24, 0x53, 0x54 }));
        fileSystem.AddFile(Path.Join(ContentFilesFolderPath, "a.txt.hash"),
            new MockFileData(SHA256.HashData(new byte[] { 0x42, 0x24, 0x53, 0x54 })));

        var systemUnderTest = CreateTestableContentFileHandler(fileSystem: fileSystem);

        var (actual, _) = await systemUnderTest.GetFilePathOfExistingCopyAndHashAsync("a.txt");

        Assert.That(actual, Is.EqualTo(Path.Join(ContentFilesFolderPath, "a.txt")));
    }

    [Test]
    public async Task GetFilePathOfExistingCopyAndHashAsync_SameContentDifferentName_ReturnsPath()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile("foobar.txt", new MockFileData(new byte[] { 0x42, 0x24, 0x53, 0x54 }));
        fileSystem.AddFile(Path.Join(ContentFilesFolderPath, "a.txt"),
            new MockFileData(new byte[] { 0x42, 0x24, 0x53, 0x54 }));
        fileSystem.AddFile(Path.Join(ContentFilesFolderPath, "a.txt.hash"),
            new MockFileData(SHA256.HashData(new byte[] { 0x42, 0x24, 0x53, 0x54 })));

        var systemUnderTest = CreateTestableContentFileHandler(fileSystem: fileSystem);

        var (actual, _) = await systemUnderTest.GetFilePathOfExistingCopyAndHashAsync("foobar.txt");

        Assert.That(actual, Is.EqualTo(Path.Join(ContentFilesFolderPath, "a.txt")));
    }

    [Test]
    public async Task GetFilePathOfExistingCopyAndHashAsync_DifferentLength_ReturnsNull()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile("a.txt", new MockFileData(new byte[] { 0x42, 0x24, 0x53, 0x54, 0x00 }));
        fileSystem.AddFile(Path.Join(ContentFilesFolderPath, "a.txt"),
            new MockFileData(new byte[] { 0x42, 0x24, 0x53, 0x54 }));
        fileSystem.AddFile(Path.Join(ContentFilesFolderPath, "a.txt.hash"),
            new MockFileData(SHA256.HashData(new byte[] { 0x42, 0x24, 0x53, 0x54 })));

        var systemUnderTest = CreateTestableContentFileHandler(fileSystem: fileSystem);

        var (actual, _) = await systemUnderTest.GetFilePathOfExistingCopyAndHashAsync("a.txt");

        Assert.That(actual, Is.Null);
    }

    [Test]
    public async Task GetFilePathOfExistingCopyAndHashAsync_DifferentContent_ReturnsNull()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile("a.txt", new MockFileData(new byte[] { 0x13, 0x37 }));
        fileSystem.AddFile(Path.Join(ContentFilesFolderPath, "a.txt"),
            new MockFileData(new byte[] { 0x42, 0x24, 0x53, 0x54 }));
        fileSystem.AddFile(Path.Join(ContentFilesFolderPath, "a.txt.hash"),
            new MockFileData(SHA256.HashData(new byte[] { 0x42, 0x24, 0x53, 0x54 })));

        var systemUnderTest = CreateTestableContentFileHandler(fileSystem: fileSystem);

        var (actual, _) = await systemUnderTest.GetFilePathOfExistingCopyAndHashAsync("a.txt");

        Assert.That(actual, Is.Null);
    }

    [Test]
    public async Task GetFilePathOfExistingCopyAndHashAsync_NoFilesInContentFolder_ReturnsNull()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile("a.txt", new MockFileData(new byte[] { 0x42, 0x24, 0x53, 0x54, 0x00 }));

        var systemUnderTest = CreateTestableContentFileHandler(fileSystem: fileSystem);

        var (actual, _) = await systemUnderTest.GetFilePathOfExistingCopyAndHashAsync("a.txt");

        Assert.That(actual, Is.Null);
    }

    [Test]
    public async Task GetFilePathOfExistingCopyAndHashAsync_HashButNoRealFile_SilentlyDeletesHashFile_ThenReturnsNull()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile("a.txt", new MockFileData(new byte[] { 0x42, 0x24, 0x53, 0x54, 0x00 }));
        fileSystem.AddFile(Path.Join(ContentFilesFolderPath, "a.txt.hash"),
            new MockFileData(SHA256.HashData(new byte[] { 0x42, 0x24, 0x53, 0x54 })));

        var systemUnderTest = CreateTestableContentFileHandler(fileSystem: fileSystem);

        var (actual, _) = await systemUnderTest.GetFilePathOfExistingCopyAndHashAsync("a.txt");

        Assert.Multiple(() =>
        {
            Assert.That(actual, Is.Null);
            Assert.That(fileSystem.File.Exists(Path.Join(ContentFilesFolderPath, "a.txt.hash")), Is.False);
        });
    }

    [Test]
    public async Task GetFilePathOfExistingCopyAndHashAsync_RealFileButNoHash_SilentlyCreatesHashFile_ThenReturnsPath()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile("a.txt", new MockFileData(new byte[] { 0x42, 0x24, 0x53, 0x54 }));
        fileSystem.AddFile(Path.Join(ContentFilesFolderPath, "a.txt"),
            new MockFileData(new byte[] { 0x42, 0x24, 0x53, 0x54 }));

        var systemUnderTest = CreateTestableContentFileHandler(fileSystem: fileSystem);

        var (actual, _) = await systemUnderTest.GetFilePathOfExistingCopyAndHashAsync("a.txt");

        Assert.Multiple(() =>
        {
            Assert.That(actual, Is.EqualTo(Path.Join(ContentFilesFolderPath, "a.txt")));
            Assert.That(fileSystem.File.Exists(Path.Join(ContentFilesFolderPath, "a.txt.hash")), Is.True);
        });
    }

    [Test]
    public void GetFilePathOfExistingCopyAndHashAsync_EmptyOrNullFilePath_ThrowsArgumentException()
    {
        var systemUnderTest = CreateTestableContentFileHandler();

        Assert.ThrowsAsync(
            Is.TypeOf<ArgumentException>()
                .And.Message.EqualTo("Path cannot be null or whitespace. (Parameter 'filepath')"),
            async () => await systemUnderTest.GetFilePathOfExistingCopyAndHashAsync(null!));
        Assert.ThrowsAsync(
            Is.TypeOf<ArgumentException>()
                .And.Message.EqualTo("Path cannot be null or whitespace. (Parameter 'filepath')"),
            async () => await systemUnderTest.GetFilePathOfExistingCopyAndHashAsync(""));
    }

    private ContentFileHandler CreateTestableContentFileHandler(ILogger<ContentFileHandler>? logger = null,
        IFileSystem? fileSystem = null, IXmlFileHandler<List<LinkContentPe>>? xmlFileHandler = null)
    {
        logger ??= Substitute.For<ILogger<ContentFileHandler>>();
        fileSystem ??= new MockFileSystem();
        xmlFileHandler ??= Substitute.For<IXmlFileHandler<List<LinkContentPe>>>();
        return new ContentFileHandler(logger, fileSystem, xmlFileHandler);
    }
}