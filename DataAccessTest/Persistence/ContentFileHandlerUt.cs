using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using BusinessLogic.ErrorManagement.DataAccess;
using DataAccess.Persistence;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using PersistEntities.LearningContent;
using TestHelpers;

namespace DataAccessTest.Persistence;

[TestFixture]
public class ContentFileHandlerUt
{
    private string ContentFilesFolderPath => Path.Join(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "AdLerAuthoring", "ContentFiles");

    [Test]
    // ANF-ID: [AWA0036]
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
    // ANF-ID: [AWA0036]
    public void LoadContentAsync_WithFilepath_DuplicateFile_DoesNotCopyAndThrowsHashExistsException()
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
    // ANF-ID: [AWA0036]
    public async Task
        LoadContentAsync_WithFilepath_DuplicateFileNameWithDifferentContent_CopiesAndReturnsCorrectObject()
    {
        const string filepath = "foobar.png";
        var fileSystem = CreateFileSystemWithByteFileData(filepath);

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

    private MockFileSystem CreateFileSystemWithByteFileData(string filepath)
    {
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { filepath, new MockFileData(new byte[] { 0x42, 0x24, 0x53, 0x54 }) },
            {
                Path.Join(ContentFilesFolderPath, "foobar.png"),
                new MockFileData(new byte[] { 0x00, 0x53, 0x00, 0x53, })
            },
        });
        return fileSystem;
    }

    [Test]
    // ANF-ID: [AWA0036]
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
    // ANF-ID: [ASE7]
    public async Task LoadContentAsync_WithFilePath_GeneratePersistFileForFileContent()
    {
        const string filepath = "foobar.png";
        var mockFileSystem = CreateFileSystemWithByteFileData(filepath);
        var systemUnderTest = CreateTestableContentFileHandler(fileSystem: mockFileSystem);

        await systemUnderTest.LoadContentAsync(filepath);
        
        systemUnderTest.XmlFileHandlerFileContent!.Received().SaveToDisk(Arg.Any<FileContentPe>(), Arg.Any<string>());
    }



    [Test]
    // ANF-ID: [AWA0036]
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
    // ANF-ID: [AWA0036]
    public void LoadContentAsync_WithStream_DuplicateFile_DoesNotCopyAndThrowsHashExistsException()
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
    // ANF-ID: [AWA0036]
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
    // ANF-ID: [ASE7]
    public async Task LoadContentAsync_WithStream_GeneratePersistFileForFileContent()
    {
        var stream = new MemoryStream(new byte[] { 0x42, 0x24, 0x53, 0x54 });
        const string filepath = "foobar.png";
        var mockFileSystem = CreateFileSystemWithByteFileData(filepath);
        var systemUnderTest = CreateTestableContentFileHandler(fileSystem: mockFileSystem);

        await systemUnderTest.LoadContentAsync(filepath, stream);
        
        systemUnderTest.XmlFileHandlerFileContent!.Received().SaveToDisk(Arg.Any<FileContentPe>(), Arg.Any<string>());
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
    // ANF-ID: [AWA0036]
    public async Task LoadContentAsync_WithByteArray_NoDuplicateFile_CopiesFileAndReturnsCorrectObject()
    {
        const string filepath = "foobar.png";
        var fileSystem = new MockFileSystem();
        var byteArray = new byte[] { 0x42, 0x24, 0x53, 0x54 };
        fileSystem.AddFile(filepath, new MockFileData(byteArray));
        using var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(byteArray);
        
        var systemUnderTest = CreateTestableContentFileHandler(fileSystem: fileSystem);
        
        var objActual = await systemUnderTest.LoadContentAsync(filepath, hash);
        
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
    // ANF-ID: [ASE7]
    public async Task LoadContentAsync_WithByteArray_GeneratePersistFileForFileContent()
    {
        var byteArray = new byte[] { 0x42, 0x24, 0x53, 0x54 };
        const string filepath = "foobar.png";
        var mockFileSystem = CreateFileSystemWithByteFileData(filepath);
        var systemUnderTest = CreateTestableContentFileHandler(fileSystem: mockFileSystem);

        await systemUnderTest.LoadContentAsync(filepath, byteArray);
        
        systemUnderTest.XmlFileHandlerFileContent!.Received().SaveToDisk(Arg.Any<FileContentPe>(), Arg.Any<string>());
    }




    [Test]
    // ANF-ID: [AWA0036]
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
    // ANF-ID: [AWA0036]
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
    // ANF-ID: [AWA0036]
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
    // ANF-ID: [AWA0036]
    public async Task GetFilePathOfExistingCopyAndHashAsync_NoFilesInContentFolder_ReturnsNull()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile("a.txt", new MockFileData(new byte[] { 0x42, 0x24, 0x53, 0x54, 0x00 }));

        var systemUnderTest = CreateTestableContentFileHandler(fileSystem: fileSystem);

        var (actual, _) = await systemUnderTest.GetFilePathOfExistingCopyAndHashAsync("a.txt");

        Assert.That(actual, Is.Null);
    }

    [Test]
    // ANF-ID: [AWA0036]
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
    // ANF-ID: [AWA0036]
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
    // ANF-ID: [AWA0036]
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

    [Test]
    // ANF-ID: [AWA0042]
    public void SaveLink_FileExistsButLinkDoesntExistYet_WritesIntoFile()
    {
        var fileSystem = new MockFileSystem();
        var xmlFileHandlerLink = Substitute.For<IXmlFileHandler<List<LinkContentPe>>>();
        var linkFilePath = fileSystem.Path.Combine(ContentFilesFolderPath, ".linkstore");
        fileSystem.AddFile(linkFilePath, "");
        var randomLink = PersistEntityProvider.GetLinkContent("somerandmname", "somerandomurl");
        xmlFileHandlerLink.LoadFromDisk(linkFilePath).Returns(new List<LinkContentPe>{randomLink});
        var systemUnderTest = CreateTestableContentFileHandler(fileSystem: fileSystem, xmlFileHandler: xmlFileHandlerLink);

        var link = PersistEntityProvider.GetLinkContent();
        
        systemUnderTest.SaveLink(link);
     
        xmlFileHandlerLink.Received().SaveToDisk(Arg.Is<List<LinkContentPe>>(li => li.Contains(link) && li.Count == 2), linkFilePath);
    }

    [Test]
    // ANF-ID: [AWA0042]
    public void SaveLink_FileDoesNotExist_WritesIntoFile()
    {
        var fileSystem = new MockFileSystem();
        var xmlFileHandlerLink = Substitute.For<IXmlFileHandler<List<LinkContentPe>>>();
        var linkFilePath = fileSystem.Path.Combine(ContentFilesFolderPath, ".linkstore");
        var systemUnderTest = CreateTestableContentFileHandler(fileSystem: fileSystem, xmlFileHandler: xmlFileHandlerLink);

        var link = PersistEntityProvider.GetLinkContent();
        
        systemUnderTest.SaveLink(link);
     
        xmlFileHandlerLink.Received().SaveToDisk(Arg.Is<List<LinkContentPe>>(li => li.Contains(link) && li.Count == 1), linkFilePath);
    }

    [Test]
    // ANF-ID: [AWA0042]
    public void SaveLink_FileExistsAndLinkExists_DoesNothing()
    {
        var fileSystem = new MockFileSystem();
        var xmlFileHandlerLink = Substitute.For<IXmlFileHandler<List<LinkContentPe>>>();
        var linkFilePath = fileSystem.Path.Combine(ContentFilesFolderPath, ".linkstore");
        fileSystem.AddFile(linkFilePath, "");
        var link = PersistEntityProvider.GetLinkContent();
        xmlFileHandlerLink.LoadFromDisk(linkFilePath).Returns(new List<LinkContentPe> {link});
        var systemUnderTest = CreateTestableContentFileHandler(fileSystem: fileSystem, xmlFileHandler: xmlFileHandlerLink);

        
        systemUnderTest.SaveLink(link);
        
        xmlFileHandlerLink.DidNotReceive().SaveToDisk(Arg.Any<List<LinkContentPe>>(), Arg.Any<string>());
    }

    [Test]
    // ANF-ID: [AWA0042]
    public void SaveLink_FileExistsAndLinkWithSameNameExists_OverwritesName()
    {
        var fileSystem = new MockFileSystem();
        var xmlFileHandlerLink = Substitute.For<IXmlFileHandler<List<LinkContentPe>>>();
        var linkFilePath = fileSystem.Path.Combine(ContentFilesFolderPath, ".linkstore");
        fileSystem.AddFile(linkFilePath, "");
        var link = PersistEntityProvider.GetLinkContent();
        var oldLinkName = link.Name;
        var dupLink = PersistEntityProvider.GetLinkContent(link: "differentlink");
        xmlFileHandlerLink.LoadFromDisk(linkFilePath).Returns(new List<LinkContentPe> {dupLink});
        var systemUnderTest = CreateTestableContentFileHandler(fileSystem: fileSystem, xmlFileHandler: xmlFileHandlerLink);

        
        systemUnderTest.SaveLink(link);
        
        xmlFileHandlerLink.Received().SaveToDisk(Arg.Is<List<LinkContentPe>>(li => li.Contains(link) && li.Count == 2), linkFilePath);
        Assert.That(link.Name, Is.EqualTo(oldLinkName + " (1)"));
    }

    [Test]
    // ANF-ID: [AWA0042]
    public void SaveLinks_SavesAllLinks()
    {
        var fileSystem = new MockFileSystem();
        var xmlFileHandlerLink = Substitute.For<IXmlFileHandler<List<LinkContentPe>>>();
        var linkFilePath = fileSystem.Path.Combine(ContentFilesFolderPath, ".linkstore");
        var links = new List<LinkContentPe>
        {
            PersistEntityProvider.GetLinkContent(),
            PersistEntityProvider.GetLinkContent(link: "foobar"),
            PersistEntityProvider.GetLinkContent(name: "barbouruas", link:"onemorelink")
        };
        
        var systemUnderTest = CreateTestableContentFileHandler(fileSystem: fileSystem, xmlFileHandler: xmlFileHandlerLink);
        
        systemUnderTest.SaveLinks(links);
        
        xmlFileHandlerLink.Received().SaveToDisk(Arg.Is<List<LinkContentPe>>(li => li.Count == 3 && li.SequenceEqual(links)), linkFilePath);
        
    }

    [Test]
    // ANF-ID: [AWA0048,AWA0049,ASN0028]
    public void GetAllContent_ReturnsAllContentFilesInContentFilesFolder()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile(Path.Join(ContentFilesFolderPath, "a.txt"), new MockFileData(new byte[] { 0x42, 0x24, 0x53, 0x54 }));
        fileSystem.AddFile(Path.Join(ContentFilesFolderPath, "b.txt"), new MockFileData(new byte[] { 0x42, 0x24, 0x53, 0x54 }));
        fileSystem.AddFile(Path.Join(ContentFilesFolderPath, "c.txt"), new MockFileData(new byte[] { 0x42, 0x24, 0x53, 0x54 }));
        fileSystem.AddFile(Path.Join(ContentFilesFolderPath, "c.txt.hash"), new MockFileData(new byte[] { 0x42, 0x24, 0x53, 0x54 }));
        fileSystem.AddFile(Path.Join(ContentFilesFolderPath, ".foobar"), new MockFileData(new byte[] { 0x42, 0x24, 0x53, 0x54 }));
        
        var systemUnderTest = CreateTestableContentFileHandler(fileSystem: fileSystem);
        
        var actual = systemUnderTest.GetAllContent().ToList();
        
        Assert.That(actual, Has.Count.EqualTo(3));
        Assert.That(actual, Has.Exactly(1).Matches<FileContentPe>(f => f.Name == "a.txt"));
        Assert.That(actual, Has.Exactly(1).Matches<FileContentPe>(f => f.Name == "b.txt"));
        Assert.That(actual, Has.Exactly(1).Matches<FileContentPe>(f => f.Name == "c.txt"));
    }
    
    [Test]
    // ANF-ID: [ASN0028]
    public void GetAllContent_LoadH5PsFromXmlPersistence()
    {
        var fileNames = new[] { "a.h5p", "a.xml", "b.txt", "c.h5p", "c.xml", "c.h5p.hash", ".foobar" };
        var fileSystem = CreateFakeFileSystem(ContentFilesFolderPath, fileNames);
        var systemUnderTest = CreateTestableContentFileHandler(
            fileSystem: fileSystem);

        systemUnderTest.GetAllContent();
        
        systemUnderTest.XmlFileHandlerFileContent!.Received(2).LoadFromDisk( Arg.Any<string>());
    }


    [Test]
    // ANF-ID: [ASN0028]
    public void GetAllContent_LoadH5PsFromXmlPersistence_XmlFileNotFound_GeneratesXmlFile()
    {
        var fileNames = new[] { "a.h5p", "b.txt", "c.h5p", "c.h5p.hash", ".foobar" };
        var fileSystem = CreateFakeFileSystem(ContentFilesFolderPath, fileNames);
        var mockXmlFileHandlerFileContent = CreateFakeXmlFileHandlerFileContentWithLoadFromDisk();
        var systemUnderTest = CreateTestableContentFileHandler(
            fileSystem: fileSystem,
            xmlFileHandlerFileContent: mockXmlFileHandlerFileContent);
       
        var result = systemUnderTest.GetAllContent();
        systemUnderTest.XmlFileHandlerFileContent!.Received().SaveToDisk(Arg.Any<FileContentPe>(), Arg.Any<string>());
        Assert.That(result.Count(), Is.EqualTo(2));
    }

    private static IXmlFileHandler<FileContentPe> CreateFakeXmlFileHandlerFileContentWithLoadFromDisk()
    {
        var mockXmlFileHandlerFileContent = Substitute.For<IXmlFileHandler<FileContentPe>>();
        var fakeFileContentPe = PersistEntityProvider.GetFileContent();
        mockXmlFileHandlerFileContent.LoadFromDisk(Arg.Any<string>()).Returns(fakeFileContentPe);
        return mockXmlFileHandlerFileContent;
    }


    private static MockFileSystem CreateFakeFileSystem(string baseFolder, IEnumerable<string> fileNames)
    {
        var fileSystem = new MockFileSystem();
        var dummyContent = new byte[] { 0x42, 0x24, 0x53, 0x54 };

        foreach (var fileName in fileNames)
        {
            var fullPath = Path.Combine(baseFolder, fileName);
            fileSystem.AddFile(fullPath, new MockFileData(dummyContent));
        }

        return fileSystem;
    }



    

    [Test]
    // ANF-ID: [AWA0037,AWA0043]
    public void RemoveContent_FileContent_DeletesFileFromContentFilesFolder()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile(Path.Join(ContentFilesFolderPath, "a.txt"), new MockFileData(new byte[] { 0x42, 0x24, 0x53, 0x54 }));
        
        var systemUnderTest = CreateTestableContentFileHandler(fileSystem: fileSystem);
        
        systemUnderTest.RemoveContent(new FileContentPe("a.txt", "png", Path.Join(ContentFilesFolderPath, "a.txt")));
        
        Assert.That(fileSystem.File.Exists(Path.Join(ContentFilesFolderPath, "a.txt")), Is.False);
    }

    [Test]
    // ANF-ID: [AWA0037,AWA0043]
    public void RemoveContent_LinkContent_DeletesLinkFromLinkstore()
    {
        var fileSystem = new MockFileSystem();
        fileSystem.AddFile(Path.Join(ContentFilesFolderPath, ".linkstore"), "");
        var xmlFileHandlerLink = Substitute.For<IXmlFileHandler<List<LinkContentPe>>>();
        var link = PersistEntityProvider.GetLinkContent();
        xmlFileHandlerLink.LoadFromDisk(Arg.Any<string>()).Returns(new List<LinkContentPe> {link});
        
        var systemUnderTest = CreateTestableContentFileHandler(fileSystem: fileSystem, xmlFileHandler: xmlFileHandlerLink);
        
        systemUnderTest.RemoveContent(link);
        
        xmlFileHandlerLink.Received().SaveToDisk(Arg.Is<List<LinkContentPe>>(li => !li.Contains(link)), Arg.Any<string>());
    }
    [Test]
    // ANF-ID: [ASE7]
    public void EditH5PFileContent_GeneratePersistFileForFileContent()
    {
        var stubFileContentPe = Substitute.For<IFileContentPe>();
        var systemUnderTest = CreateTestableContentFileHandler();

        systemUnderTest.EditH5PFileContent(stubFileContentPe);
        
        systemUnderTest.XmlFileHandlerFileContent!.Received().SaveToDisk(Arg.Any<FileContentPe>(), Arg.Any<string>());
    }


    
    

    private ContentFileHandler CreateTestableContentFileHandler(
        ILogger<ContentFileHandler>? logger = null,
        IFileSystem? fileSystem = null, 
        IXmlFileHandler<List<LinkContentPe>>? xmlFileHandler = null,
        IXmlFileHandler<FileContentPe>? xmlFileHandlerFileContent = null)
    {
        logger ??= Substitute.For<ILogger<ContentFileHandler>>();
        fileSystem ??= new MockFileSystem();
        xmlFileHandler ??= Substitute.For<IXmlFileHandler<List<LinkContentPe>>>();
        xmlFileHandlerFileContent ??= Substitute.For<IXmlFileHandler<FileContentPe>>();
        return new ContentFileHandler(logger, fileSystem, xmlFileHandler, xmlFileHandlerFileContent);
    }
}