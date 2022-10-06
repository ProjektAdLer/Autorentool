using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Security.Cryptography;
using DataAccess.Persistence;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace DataAccessTest.Persistence;

[TestFixture]

public class ContentFileHandlerUt
{
    private string ContentFilesFolderPath => Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "AdLerAuthoring", "ContentFiles");

    [Test]
    public async Task LoadContentAsync_WithFilepath_NoDuplicateFile_CopiesFileCreatesHashFileAndReturnsCorrectObject()
    {
        const string filepath = "foobar.png";
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { filepath, new MockFileData(new byte[] {0x42, 0x24, 0x53, 0x54})},
        });

        var systemUnderTest = CreateTestableContentFileHandler(fileSystem: fileSystem);

        var objActual = await systemUnderTest.LoadContentAsync(filepath);
        Assert.Multiple(() =>
        {
            Assert.That(objActual.Name, Is.EqualTo("foobar.png"));
            Assert.That(objActual.Type, Is.EqualTo("png"));
            Assert.That(objActual.Filepath, Is.EqualTo(Path.Join(ContentFilesFolderPath, "foobar.png")));
            Assert.That(fileSystem.File.Exists(Path.Join(ContentFilesFolderPath, "foobar.png")), Is.True);
            Assert.That(fileSystem.File.Exists(Path.Join(ContentFilesFolderPath, "foobar.png.hash")), Is.True);
        });
    }
    
    [Test]
    public async Task LoadContentAsync_WithFilepath_DuplicateFile_DoesNotCopyAndReturnsCorrectObject()
    {
        const string filepath = "foobar.png";
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { filepath, new MockFileData(new byte[] {0x42, 0x24, 0x53, 0x54})},
            { Path.Join(ContentFilesFolderPath, "a.png"), new MockFileData(new byte[] {0x42, 0x24, 0x53, 0x54})},
        });

        var systemUnderTest = CreateTestableContentFileHandler(fileSystem: fileSystem);

        var objActual = await systemUnderTest.LoadContentAsync(filepath);
        Assert.Multiple(() =>
        {
            Assert.That(objActual.Name, Is.EqualTo("a.png"));
            Assert.That(objActual.Type, Is.EqualTo("png"));
            Assert.That(objActual.Filepath, Is.EqualTo(Path.Join(ContentFilesFolderPath, "a.png")));
            Assert.That(fileSystem.File.Exists(Path.Join(ContentFilesFolderPath, "foobar.png")), Is.False);
            //two files: the content itself and its hash file
            Assert.That(fileSystem.Directory.EnumerateFiles(ContentFilesFolderPath).Count(), Is.EqualTo(2));
        });
    }
    
    [Test]
    public async Task LoadContentAsync_WithFilepath_DuplicateFileNameWithDifferentContent_CopiesAndReturnsCorrectObject()
    {
        const string filepath = "foobar.png";
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { filepath, new MockFileData(new byte[] {0x42, 0x24, 0x53, 0x54})},
            { Path.Join(ContentFilesFolderPath, "foobar.png"), new MockFileData(new byte[] {0x00, 0x53, 0x00, 0x53,})},
        });

        var systemUnderTest = CreateTestableContentFileHandler(fileSystem: fileSystem);

        var objActual = await systemUnderTest.LoadContentAsync(filepath);
        Assert.Multiple(() =>
        {
            Assert.That(objActual.Name, Is.EqualTo("foobar(1).png"));
            Assert.That(objActual.Type, Is.EqualTo("png"));
            Assert.That(objActual.Filepath, Is.EqualTo(Path.Join(ContentFilesFolderPath, "foobar(1).png")));
            Assert.That(fileSystem.File.Exists(Path.Join(ContentFilesFolderPath, "foobar(1).png")), Is.True);
            Assert.That(fileSystem.Directory.EnumerateFiles(ContentFilesFolderPath).Count(), Is.EqualTo(4));
        });
    }
    
    
    [Test]
    public async Task LoadContentAsync_WithStream_NoDuplicateFile_CopiesFileAndReturnsCorrectObject()
    {
        var fileSystem = new MockFileSystem();
        const string filepath = "foobar.png";
        var stream = new MemoryStream(new byte[] {0x42, 0x24, 0x53, 0x54});


        var systemUnderTest = CreateTestableContentFileHandler(fileSystem: fileSystem);

        var objActual = await systemUnderTest.LoadContentAsync(filepath, stream);
        Assert.Multiple(() =>
        {
            Assert.That(objActual.Name, Is.EqualTo("foobar.png"));
            Assert.That(objActual.Type, Is.EqualTo("png"));
            Assert.That(objActual.Filepath, Is.EqualTo(Path.Join(ContentFilesFolderPath, "foobar.png")));
            Assert.That(fileSystem.File.Exists(Path.Join(ContentFilesFolderPath, "foobar.png")), Is.True);
        });
    }

    [Test]
    public async Task LoadContentAsync_WithStream_DuplicateFile_DoesNotCopyAndReturnsCorrectObject()
    {
        var fileSystem = new MockFileSystem();
        const string filepath = "foobar.png";
        var stream = new MemoryStream(new byte[] {0x42, 0x24, 0x53, 0x54});
        fileSystem.AddFile(Path.Join(ContentFilesFolderPath, "a.txt"), new MockFileData(new byte[] {0x42, 0x24, 0x53, 0x54}));
        
        var systemUnderTest = CreateTestableContentFileHandler(fileSystem: fileSystem);

        var objActual = await systemUnderTest.LoadContentAsync(filepath, stream);
        Assert.Multiple(() =>
        {
            Assert.That(objActual.Name, Is.EqualTo("foobar.png"));
            Assert.That(objActual.Type, Is.EqualTo("png"));
            Assert.That(objActual.Filepath, Is.EqualTo(Path.Join(ContentFilesFolderPath, "a.txt")));
            Assert.That(fileSystem.File.Exists(Path.Join(ContentFilesFolderPath, "foobar.png")), Is.False);
            //two files: the content itself and its hash file
            Assert.That(fileSystem.Directory.EnumerateFiles(ContentFilesFolderPath).Count(), Is.EqualTo(2));
        });
    }

    [Test]
    public async Task ExistsAlreadyInContentFilesFolderAsync_WithExactSameFile_ReturnsPath()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile("a.txt", new MockFileData(new byte[] {0x42, 0x24, 0x53, 0x54}));
        fileSystem.AddFile(Path.Join(ContentFilesFolderPath, "a.txt"), new MockFileData(new byte[] {0x42, 0x24, 0x53, 0x54}));
        fileSystem.AddFile(Path.Join(ContentFilesFolderPath, "a.txt.hash"),
            new MockFileData(SHA256.HashData(new byte[] { 0x42, 0x24, 0x53, 0x54 })));
        
        var systemUnderTest = CreateTestableContentFileHandler(fileSystem: fileSystem);
        
        var (actual, _) = await systemUnderTest.ExistsAlreadyInContentFilesFolderAsync("a.txt");
        
        Assert.That(actual, Is.EqualTo(Path.Join(ContentFilesFolderPath, "a.txt")));
    }

    [Test]
    public async Task ExistsAlreadyInContentFilesFolderAsync_SameContentDifferentName_ReturnsPath()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile("foobar.txt", new MockFileData(new byte[] {0x42, 0x24, 0x53, 0x54}));
        fileSystem.AddFile(Path.Join(ContentFilesFolderPath, "a.txt"), new MockFileData(new byte[] {0x42, 0x24, 0x53, 0x54}));
        fileSystem.AddFile(Path.Join(ContentFilesFolderPath, "a.txt.hash"),
            new MockFileData(SHA256.HashData(new byte[] { 0x42, 0x24, 0x53, 0x54 })));
        
        var systemUnderTest = CreateTestableContentFileHandler(fileSystem: fileSystem);
        
        var (actual, _) = await systemUnderTest.ExistsAlreadyInContentFilesFolderAsync("foobar.txt");
        
        Assert.That(actual, Is.EqualTo(Path.Join(ContentFilesFolderPath, "a.txt")));
    }

    [Test]
    public async Task ExistsAlreadyInContentFilesFolderAsync_DifferentLength_ReturnsNull()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile("a.txt", new MockFileData(new byte[] {0x42, 0x24, 0x53, 0x54, 0x00}));
        fileSystem.AddFile(Path.Join(ContentFilesFolderPath, "a.txt"), new MockFileData(new byte[] {0x42, 0x24, 0x53, 0x54}));
        fileSystem.AddFile(Path.Join(ContentFilesFolderPath, "a.txt.hash"),
            new MockFileData(SHA256.HashData(new byte[] { 0x42, 0x24, 0x53, 0x54 })));
        
        var systemUnderTest = CreateTestableContentFileHandler(fileSystem: fileSystem);
        
        var (actual, _) = await systemUnderTest.ExistsAlreadyInContentFilesFolderAsync("a.txt");
        
        Assert.That(actual, Is.Null);
    }
    
    [Test]
    public async Task ExistsAlreadyInContentFilesFolderAsync_DifferentContent_ReturnsNull()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile("a.txt", new MockFileData(new byte[] {0x13, 0x37}));
        fileSystem.AddFile(Path.Join(ContentFilesFolderPath, "a.txt"), new MockFileData(new byte[] {0x42, 0x24, 0x53, 0x54}));
        fileSystem.AddFile(Path.Join(ContentFilesFolderPath, "a.txt.hash"),
            new MockFileData(SHA256.HashData(new byte[] { 0x42, 0x24, 0x53, 0x54 })));
        
        var systemUnderTest = CreateTestableContentFileHandler(fileSystem: fileSystem);
        
        var (actual, _) = await systemUnderTest.ExistsAlreadyInContentFilesFolderAsync("a.txt");
        
        Assert.That(actual, Is.Null);
    }
    
    [Test]
    public async Task ExistsAlreadyInContentFilesFolderAsync_NoFilesInContentFolder_ReturnsNull()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile("a.txt", new MockFileData(new byte[] {0x42, 0x24, 0x53, 0x54, 0x00}));
        
        var systemUnderTest = CreateTestableContentFileHandler(fileSystem: fileSystem);
        
        var (actual, _) = await systemUnderTest.ExistsAlreadyInContentFilesFolderAsync("a.txt");
        
        Assert.That(actual, Is.Null);
    }
    
    [Test]
    public async Task ExistsAlreadyInContentFilesFolderAsync_HashButNoRealFile_SilentlyDeletesHashFile_ThenReturnsNull()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile("a.txt", new MockFileData(new byte[] {0x42, 0x24, 0x53, 0x54, 0x00}));
        fileSystem.AddFile(Path.Join(ContentFilesFolderPath, "a.txt.hash"),
            new MockFileData(SHA256.HashData(new byte[] { 0x42, 0x24, 0x53, 0x54 })));
        
        var systemUnderTest = CreateTestableContentFileHandler(fileSystem: fileSystem);
        
        var (actual, _) = await systemUnderTest.ExistsAlreadyInContentFilesFolderAsync("a.txt");
        
        Assert.Multiple(() =>
        {
            Assert.That(actual, Is.Null);
            Assert.That(fileSystem.File.Exists(Path.Join(ContentFilesFolderPath, "a.txt.hash")), Is.False);
        });
    }
    
    [Test]
    public async Task ExistsAlreadyInContentFilesFolderAsync_RealFileButNoHash_SilentlyCreatesHashFile_ThenReturnsPath()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile("a.txt", new MockFileData(new byte[] {0x42, 0x24, 0x53, 0x54}));
        fileSystem.AddFile(Path.Join(ContentFilesFolderPath, "a.txt"), new MockFileData(new byte[] {0x42, 0x24, 0x53, 0x54}));
        
        var systemUnderTest = CreateTestableContentFileHandler(fileSystem: fileSystem);
        
        var (actual, _) = await systemUnderTest.ExistsAlreadyInContentFilesFolderAsync("a.txt");
        
        Assert.Multiple(() =>
        {
            Assert.That(actual, Is.EqualTo(Path.Join(ContentFilesFolderPath, "a.txt")));
            Assert.That(fileSystem.File.Exists(Path.Join(ContentFilesFolderPath, "a.txt.hash")), Is.True);
        });
    }

    private ContentFileHandler CreateTestableContentFileHandler(ILogger<ContentFileHandler>? logger = null,
        IFileSystem? fileSystem = null)
    {
        logger ??= Substitute.For<ILogger<ContentFileHandler>>();
        fileSystem ??= new MockFileSystem();
        return new ContentFileHandler(logger, fileSystem);
    }
}