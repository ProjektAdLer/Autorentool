using System.Collections;
using System.Data;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using AutoMapper;
using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using BusinessLogic.Entities.LearningContent.Adaptivity;
using BusinessLogic.Entities.LearningContent.Adaptivity.Action;
using BusinessLogic.Entities.LearningContent.FileContent;
using BusinessLogic.Entities.LearningContent.LinkContent;
using DataAccess.Extensions;
using DataAccess.Persistence;
using DataAccessTest.Resources;
using NSubstitute;
using NUnit.Framework;
using PersistEntities;
using PersistEntities.LearningContent;
using Shared.Configuration;
using TestHelpers;

namespace DataAccessTest.API;

[TestFixture]
public class DataAccessUt
{
    [Test]
    public void Standard_AllPropertiesInitialized()
    {
        //Arrange 
        var mockConfiguration = Substitute.For<IApplicationConfiguration>();
        var mockFileSaveHandlerWorld = Substitute.For<IXmlFileHandler<LearningWorldPe>>();
        var mockFileSaveHandlerLink = Substitute.For<IXmlFileHandler<List<LinkContentPe>>>();
        var mockContentHandler = Substitute.For<IContentFileHandler>();
        var mockWorldSavePathsHandler = Substitute.For<ILearningWorldSavePathsHandler>();
        var mockFileSystem = new MockFileSystem();

        //Act 
        var systemUnderTest = CreateTestableDataAccess(mockConfiguration, mockFileSaveHandlerWorld,
            mockFileSaveHandlerLink, mockContentHandler, mockWorldSavePathsHandler, mockFileSystem);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Configuration, Is.EqualTo(mockConfiguration));
            Assert.That(systemUnderTest.XmlHandlerWorld, Is.EqualTo(mockFileSaveHandlerWorld));
            Assert.That(systemUnderTest.XmlHandlerLink, Is.EqualTo(mockFileSaveHandlerLink));
            Assert.That(systemUnderTest.ContentFileHandler, Is.EqualTo(mockContentHandler));
            Assert.That(systemUnderTest.WorldSavePathsHandler, Is.EqualTo(mockWorldSavePathsHandler));
            Assert.That(systemUnderTest.FileSystem, Is.EqualTo(mockFileSystem));
        });
    }

    [Test]
    // ANF-ID: [ASE6]
    public void SaveLearningWorldToFile_CallsFileSaveHandlerWorld()
    {
        var mockFileSaveHandlerWorld = Substitute.For<IXmlFileHandler<LearningWorldPe>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerWorld: mockFileSaveHandlerWorld);

        var learningWorld = EntityProvider.GetLearningWorld();
        systemUnderTest.SaveLearningWorldToFile(
            learningWorld,
            "C:/nonsense");

        mockFileSaveHandlerWorld.Received().SaveToDisk(Arg.Any<LearningWorldPe>(), "C:/nonsense");
    }

    [Test]
    // ANF-ID: [ASE2]
    public void LoadLearningWorldFromFile_CallsFileSaveHandlerWorld()
    {
        var mockFileSaveHandlerWorld = Substitute.For<IXmlFileHandler<LearningWorldPe>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerWorld: mockFileSaveHandlerWorld);

        systemUnderTest.LoadLearningWorld("C:/nonsense");

        mockFileSaveHandlerWorld.Received().LoadFromDisk("C:/nonsense");
    }

    [Test]
    // ANF-ID: [ASE2]
    public void LoadLearningWorldFromStream_CallsFileSaveHandlerWorld()
    {
        var mockFileSaveHandlerWorld = Substitute.For<IXmlFileHandler<LearningWorldPe>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerWorld: mockFileSaveHandlerWorld);
        var stream = Substitute.For<Stream>();

        systemUnderTest.LoadLearningWorld(stream);

        mockFileSaveHandlerWorld.Received().LoadFromStream(stream);
    }

    [Test]
    public void LoadLearningContentFromFile_CallsFileSaveHandlerElement()
    {
        var mockContentFileHandler = Substitute.For<IContentFileHandler>();
        var systemUnderTest = CreateTestableDataAccess(contentHandler: mockContentFileHandler);

        _ = systemUnderTest.LoadLearningContentAsync("C:/nonsense");

        mockContentFileHandler.Received().LoadContentAsync("C:/nonsense");
    }

    [Test]
    public void LoadLearningContentFromStream_CallsFileSaveHandlerElement()
    {
        var mockContentFileHandler = Substitute.For<IContentFileHandler>();
        var systemUnderTest = CreateTestableDataAccess(contentHandler: mockContentFileHandler);
        var stream = Substitute.For<MemoryStream>();

        _ = systemUnderTest.LoadLearningContentAsync("filename.extension", stream);

        mockContentFileHandler.Received().LoadContentAsync("filename.extension", stream);
    }

    [Test]
    public void GetSavedLearningWorldPaths_CallsWorldSavePathsHandler()
    {
        var mockWorldSavePathsHandler = Substitute.For<ILearningWorldSavePathsHandler>();
        var systemUnderTest = CreateTestableDataAccess(worldSavePathsHandler: mockWorldSavePathsHandler);
        var expectedFileInfos = new List<IFileInfo>
        {
            Substitute.For<IFileInfo>(),
            Substitute.For<IFileInfo>(),
            Substitute.For<IFileInfo>(),
        };
        mockWorldSavePathsHandler.GetSavedLearningWorldPaths().Returns(expectedFileInfos);

        var actualPaths = systemUnderTest.GetSavedLearningWorldPaths();

        Assert.That(actualPaths, Is.EqualTo(expectedFileInfos));
    }

    [Test]
    [TestCaseSource(typeof(FindSuitableNewSavePathTestCases))]
    public void FindSuitableNewSavePath_FindsSuitablePath(IFileSystem mockFileSystem, string targetFolder,
        string fileName, string fileEnding, string expectedSavePath, int expectedIterations)
    {
        var systemUnderTest = CreateTestableDataAccess(fileSystem: mockFileSystem);

        var actualSavePath =
            systemUnderTest.FindSuitableNewSavePath(targetFolder, fileName, fileEnding, out var iterations);

        Assert.That(actualSavePath, Is.EqualTo(expectedSavePath));
        Assert.That(iterations, Is.EqualTo(expectedIterations));
    }

    [Test]
    public void FindSuitableNewSavePath_ThrowsWhenEmptyParameters()
    {
        var systemUnderTest = CreateTestableDataAccess();

        var ex = Assert.Throws<ArgumentException>(
            () => systemUnderTest.FindSuitableNewSavePath("", "foo", "bar", out _));
        Assert.Multiple(() =>
        {
            Assert.That(ex!.Message, Is.EqualTo("targetFolder cannot be empty (Parameter 'targetFolder')"));
            Assert.That(ex.ParamName, Is.EqualTo("targetFolder"));
        });

        ex = Assert.Throws<ArgumentException>(() => systemUnderTest.FindSuitableNewSavePath("foo", "", "bar", out _));
        Assert.Multiple(() =>
        {
            Assert.That(ex!.Message, Is.EqualTo("fileName cannot be empty (Parameter 'fileName')"));
            Assert.That(ex.ParamName, Is.EqualTo("fileName"));
        });

        ex = Assert.Throws<ArgumentException>(() => systemUnderTest.FindSuitableNewSavePath("foo", "bar", "", out _));
        Assert.Multiple(() =>
        {
            Assert.That(ex!.Message, Is.EqualTo("fileEnding cannot be empty (Parameter 'fileEnding')"));
            Assert.That(ex.ParamName, Is.EqualTo("fileEnding"));
        });
    }

    [Test]
    // ANF-ID: [ASN0002]
    public async Task ImportLearningWorldFromArchiveAsync_CopiesContentOverCorrectly()
    {
        var basePath = Environment.OSVersion.Platform == PlatformID.Win32NT ? "C:" : "/";
        var fileSystem = Environment.OSVersion.Platform == PlatformID.Win32NT
            ? ResourceHelper.PrepareWindowsFileSystemWithResources()
            : ResourceHelper.PrepareUnixFileSystemWithResources();

        var xmlHandlerWorlds = Substitute.For<IXmlFileHandler<LearningWorldPe>>();
        var learningWorldPe = PersistEntityProvider.GetLearningWorld();
        xmlHandlerWorlds.LoadFromDisk(Arg.Any<string>()).Returns(learningWorldPe);

        var mapper = Substitute.For<IMapper>();
        var learningWorld = EntityProvider.GetLearningWorldWithSpaceWithElement();
        mapper.Map<LearningWorld>(learningWorldPe).Returns(learningWorld);
        learningWorld.Name = "import_test";

        var ele1 = learningWorld.LearningSpaces.First().ContainedLearningElements.First();
        ele1.LearningContent = new FileContent("adler_logo.png", "png", "C:\\bogus_path\\adler_logo.png");

        var ele2 = EntityProvider.GetLearningElement();
        ele2.LearningContent = new LinkContent("rick", "https://www.youtube.com/watch?v=dQw4w9WgXcQ");
        learningWorld.LearningSpaces.First().LearningSpaceLayout.LearningElements.Add(3, ele2);

        var space2 = EntityProvider.GetLearningSpaceWithElement();
        space2.ContainedLearningElements.First().LearningContent =
            new FileContent("regex.txt", "txt", "C:\\bogus_path\\regex.txt");
        learningWorld.LearningSpaces.Add(space2);

        var adaptivityElement = EntityProvider.GetLearningElement();
        var adaptivityContent = EntityProvider.GetAdaptivityContentFullStructure();
        adaptivityContent.Tasks.First().Questions.First().Rules.First().Action =
            new ContentReferenceAction(new FileContent("adafile.png", "png", "C:\\bogus_path\\adafile.png"), "comment");
        adaptivityElement.LearningContent = adaptivityContent;
        learningWorld.LearningSpaces.First().LearningSpaceLayout.LearningElements.Add(4, adaptivityElement);
        
        var contentFileHandler = Substitute.For<IContentFileHandler>();
        var logoFcPe = PersistEntityProvider.GetFileContent("adler_logo.png", "png",
            Path.Join(ApplicationPaths.ContentFolder, "adler_logo.png"));
        var regexFcPe = PersistEntityProvider.GetFileContent("regex.txt", "txt",
            Path.Join(ApplicationPaths.ContentFolder, "regex.txt"));
        var adaptivityFcPe = PersistEntityProvider.GetFileContent("adafile.png", "png",
            Path.Join(ApplicationPaths.ContentFolder, "adafile.png"));
        contentFileHandler.LoadContentAsync(Arg.Is<string>(s => s.EndsWith("adler_logo.png")), Arg.Any<byte[]>())
            .Returns(logoFcPe);
        contentFileHandler.LoadContentAsync(Arg.Is<string>(s => s.EndsWith("regex.txt")), Arg.Any<byte[]>())
            .Returns(regexFcPe);
        contentFileHandler.LoadContentAsync(Arg.Is<string>(s => s.EndsWith("adafile.png")), Arg.Any<byte[]>())
            .Returns(adaptivityFcPe);

        var systemUnderTest = CreateTestableDataAccess(mapper: mapper, fileSaveHandlerWorld: xmlHandlerWorlds,
            fileSystem: fileSystem, contentHandler: contentFileHandler);

        var loadedWorld =
            await systemUnderTest.ImportLearningWorldFromArchiveAsync(Path.Combine(basePath, "zips",
                "import_test.zip"));

        await contentFileHandler.Received()
            .LoadContentAsync(Arg.Is<string>(s => s.EndsWith("adler_logo.png")), Arg.Any<byte[]>());
        await contentFileHandler.Received()
            .LoadContentAsync(Arg.Is<string>(s => s.EndsWith("regex.txt")), Arg.Any<byte[]>());
        await contentFileHandler.Received()
            .LoadContentAsync(Arg.Is<string>(s => s.EndsWith("adafile.png")), Arg.Any<byte[]>());

        Assert.That(loadedWorld, Is.EqualTo(learningWorld));
        Assert.That(
            (loadedWorld.LearningSpaces.First().ContainedLearningElements.First().LearningContent as FileContent)!
            .Filepath, Is.EqualTo(Path.Join(ApplicationPaths.ContentFolder, "adler_logo.png")));
        Assert.That(
            (loadedWorld.LearningSpaces.ElementAt(1).ContainedLearningElements.First().LearningContent as FileContent)!
            .Filepath, Is.EqualTo(Path.Join(ApplicationPaths.ContentFolder, "regex.txt")));
        
        var actualAdaptivityElement = loadedWorld.LearningSpaces.First().ContainedLearningElements.First(x => x.LearningContent is AdaptivityContent);
        var actualAdaptivityContent = actualAdaptivityElement.LearningContent as AdaptivityContent;
        var referenceAction = (ContentReferenceAction)actualAdaptivityContent!.Tasks.First().Questions.First()
            .Rules.First().Action;
        var adaptivityFileContent = (FileContent)referenceAction.Content!;
        Assert.That(adaptivityFileContent.Filepath, 
            Is.EqualTo(Path.Join(ApplicationPaths.ContentFolder, "adafile.png")));

    }

    [Test]
    // ANF-ID: [ASN0001]
    public async Task ExportLearningWorldToArchive_ConstructsZipCorrectly()
    {
        var basePath = Environment.OSVersion.Platform == PlatformID.Win32NT ? "C:" : "/";

        var mapper = Substitute.For<IMapper>();
        var filesystem = new MockFileSystem();
        
        var xmlHandlerWorlds = Substitute.For<IXmlFileHandler<LearningWorldPe>>();
        var learningWorldPe = PersistEntityProvider.GetLearningWorld();
        
        var learningWorld = EntityProvider.GetLearningWorldWithSpaceWithElement();
        
        var element = learningWorld.LearningSpaces.First().ContainedLearningElements.First();
        element.LearningContent = new FileContent("adler_logo.png", "png",
            Path.Join(ApplicationPaths.ContentFolder, "adler_logo.png"));
        
        var adaptivityElement = EntityProvider.GetLearningElement();
        var adaptivityContent = EntityProvider.GetAdaptivityContentFullStructure();
        adaptivityContent.Tasks.First().Questions.First().Rules.First().Action =
            new ContentReferenceAction(new FileContent("adafile.png", "png", Path.Join(ApplicationPaths.ContentFolder, "adafile.png")), "comment");
        adaptivityElement.LearningContent = adaptivityContent;
        learningWorld.LearningSpaces.First().LearningSpaceLayout.LearningElements.Add(4, adaptivityElement);
        
        mapper.Map<LearningWorldPe>(learningWorld).Returns(learningWorldPe);
        var expectedWorldData = new byte[] { 21, 22 };
        xmlHandlerWorlds
            .When(xmlfh => xmlfh.SaveToDisk(learningWorldPe, Arg.Any<string>()))
            .Do(ci => filesystem.AddFile(ci.Arg<string>(), new MockFileData(expectedWorldData)));

        var expectedPngData = new byte[] { 123, 69, 255, 123 };
        var expectedHashData = new byte[] { 69, 69, 69, 69 };
        filesystem.AddFile(Path.Join(ApplicationPaths.ContentFolder, "adler_logo.png"),
            new MockFileData(expectedPngData));
        filesystem.AddFile(Path.Join(ApplicationPaths.ContentFolder, "adler_logo.png.hash"),
            new MockFileData(expectedHashData));
        
        var adaPngData = new byte[] { 10, 20, 30, 40 };
        var adaHashData = new byte[] { 88, 88, 88, 88 };
        filesystem.AddFile(
            Path.Join(ApplicationPaths.ContentFolder, "adafile.png"),
            new MockFileData(adaPngData));
        filesystem.AddFile(
            Path.Join(ApplicationPaths.ContentFolder, "adafile.png.hash"),
            new MockFileData(adaHashData));

        var systemUnderTest = CreateTestableDataAccess(mapper: mapper, fileSaveHandlerWorld: xmlHandlerWorlds,
            fileSystem: filesystem);

        await systemUnderTest.ExportLearningWorldToArchiveAsync(learningWorld,
            Path.Combine(basePath, "export_test.zip"));

        var expectedZip = ZipExtensions.GetZipArchive(filesystem, Path.Combine(basePath, "export_test.zip"));
        Assert.That(expectedZip.Entries, Has.Count.EqualTo(5));

        var png1 = expectedZip.Entries[1];
        using var pngMemStream = new MemoryStream();
        png1.Open().CopyTo(pngMemStream);
        var pngData1 = pngMemStream.ToArray();
        var hash1 = expectedZip.Entries[2];
        using var hashMemStream = new MemoryStream();
        hash1.Open().CopyTo(hashMemStream);
        var hashData1 = hashMemStream.ToArray();
        var adaPng = expectedZip.Entries[3];
        using var adaPngMemStream = new MemoryStream();
        adaPng.Open().CopyTo(adaPngMemStream);
        var adapngResult = adaPngMemStream.ToArray();
        var adaHash = expectedZip.Entries[4];
        using var adaHashMemStream = new MemoryStream();
        adaHash.Open().CopyTo(adaHashMemStream);
        var adaHashResult = adaHashMemStream.ToArray();
        var world = expectedZip.Entries[0];
        using var worldMemStream = new MemoryStream();
        world.Open().CopyTo(worldMemStream);
        var worldData = worldMemStream.ToArray();

        Assert.Multiple(() =>
        {
            Assert.That(png1.Name, Is.EqualTo("adler_logo.png"));
            Assert.That(png1.Length, Is.EqualTo(4));
            Assert.That(pngData1, Is.EqualTo(expectedPngData));
            Assert.That(hash1.Name, Is.EqualTo("adler_logo.png.hash"));
            Assert.That(hash1.Length, Is.EqualTo(4));
            Assert.That(hashData1, Is.EqualTo(expectedHashData));
            Assert.That(adaPng.Name, Is.EqualTo("adafile.png"));
            Assert.That(adaPng.Length, Is.EqualTo(4));
            Assert.That(adapngResult, Is.EqualTo(adaPngData));
            Assert.That(adaHash.Name, Is.EqualTo("adafile.png.hash"));
            Assert.That(adaHash.Length, Is.EqualTo(4));
            Assert.That(adaHashResult, Is.EqualTo(adaHashData));
            Assert.That(world.Name, Is.EqualTo("world.awf"));
            Assert.That(world.Length, Is.EqualTo(2));
            Assert.That(worldData, Is.EqualTo(expectedWorldData));
        });
    }

    [Test]
    public void GetAllContent_CallsContentFileHandlerAndMapper()
    {
        var contentFileHandler = Substitute.For<IContentFileHandler>();
        var content = new List<ILearningContentPe>()
            { PersistEntityProvider.GetFileContent(), PersistEntityProvider.GetLinkContent() };
        var contentEntities = new List<ILearningContent>()
            { EntityProvider.GetFileContent(), EntityProvider.GetLinkContent() };
        var mapper = Substitute.For<IMapper>();
        contentFileHandler.GetAllContent().Returns(content);
        mapper.Map<ILearningContent>(content[0]).Returns(contentEntities[0]);
        mapper.Map<ILearningContent>(content[1]).Returns(contentEntities[1]);

        var systemUnderTest = CreateTestableDataAccess(contentHandler: contentFileHandler, mapper: mapper);

        var retval = systemUnderTest.GetAllContent();

        Assert.That(retval, Is.EquivalentTo(contentEntities));
        contentFileHandler.Received().GetAllContent();
        mapper.Received().Map<ILearningContent>(content[0]);
        mapper.Received().Map<ILearningContent>(content[1]);
    }

    [Test]
    // ANF-ID: [AWA0037]
    public void RemoveContent_CallsContentFileHandlerAndMapper()
    {
        var contentFileHandler = Substitute.For<IContentFileHandler>();
        var mapper = Substitute.For<IMapper>();
        var content = EntityProvider.GetFileContent();
        var contentPe = PersistEntityProvider.GetFileContent();
        mapper.Map<ILearningContentPe>(content).Returns(contentPe);

        var systemUnderTest = CreateTestableDataAccess(contentHandler: contentFileHandler, mapper: mapper);

        systemUnderTest.RemoveContent(content);

        contentFileHandler.Received().RemoveContent(contentPe);
        mapper.Received().Map<ILearningContentPe>(content);
    }

    [Test]
    // ANF-ID: [AWA0042]
    public void SaveLink_CallsContentFileHandlerAndMapper()
    {
        var contentFileHandler = Substitute.For<IContentFileHandler>();
        var mapper = Substitute.For<IMapper>();
        var link = EntityProvider.GetLinkContent();
        var linkPe = PersistEntityProvider.GetLinkContent();
        mapper.Map<LinkContentPe>(link).Returns(linkPe);

        var systemUnderTest = CreateTestableDataAccess(contentHandler: contentFileHandler, mapper: mapper);

        systemUnderTest.SaveLink(link);

        contentFileHandler.Received().SaveLink(linkPe);
        mapper.Received().Map<LinkContentPe>(link);
    }

    [Test]
    public void GetContentFilesFolderPath_CallsContentFileHandler()
    {
        var contentFileHandler = Substitute.For<IContentFileHandler>();
        contentFileHandler.ContentFilesFolderPath.Returns("foobarbaz");

        var systemUnderTest = CreateTestableDataAccess(contentHandler: contentFileHandler);

        Assert.That(systemUnderTest.GetContentFilesFolderPath(), Is.EqualTo("foobarbaz"));
    }

    [Test]
    public void GetFileInfoForPath_CallsFileSystem()
    {
        var filesystem = Substitute.For<IFileSystem>();
        var fileInfo = Substitute.For<IFileInfo>();
        filesystem.FileInfo.New("foo").Returns(fileInfo);
        var systemUnderTest = CreateTestableDataAccess(fileSystem: filesystem);

        var actualFileInfo = systemUnderTest.GetFileInfoForPath("foo");

        Assert.That(actualFileInfo, Is.EqualTo(fileInfo));
        filesystem.FileInfo.Received().New("foo");
    }

    [Test]
    // ANF-ID: [AWA0037]
    public void DeleteFileByPath_CallsFileSystem()
    {
        var filesystem = Substitute.For<IFileSystem>();
        var systemUnderTest = CreateTestableDataAccess(fileSystem: filesystem);

        systemUnderTest.DeleteFileByPath("foo");

        filesystem.File.Received().Delete("foo");
    }

    private class FindSuitableNewSavePathTestCases : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return new object[] //no file present
            {
                new MockFileSystem(new Dictionary<string, MockFileData>()),
                "directory", "foo", "bar", Path.Join("directory", "foo.bar"), 0
            };
            var emptyFile = new MockFileData("");
            yield return new object[] //file is present
            {
                new MockFileSystem(new Dictionary<string, MockFileData>
                {
                    { Path.Combine("directory", "foo.bar"), emptyFile }
                }),
                "directory", "foo", "bar", Path.Join("directory", "foo_1.bar"), 1
            };
            yield return new object[] //multiple files present
            {
                new MockFileSystem(new Dictionary<string, MockFileData>
                {
                    { Path.Combine("directory", "foo.bar"), emptyFile },
                    { Path.Combine("directory", "foo_1.bar"), emptyFile },
                    { Path.Combine("directory", "foo_2.bar"), emptyFile }
                }),
                "directory", "foo", "bar", Path.Join("directory", "foo_3.bar"), 3
            };
            yield return new object[] //irrelevant files present
            {
                new MockFileSystem(new Dictionary<string, MockFileData>
                {
                    { Path.Combine("directory", "poo.bar"), emptyFile }
                }),
                "directory", "foo", "bar", Path.Join("directory", "foo.bar"), 0
            };
        }
    }

    private static DataAccess.API.DataAccess CreateTestableDataAccess(
        IApplicationConfiguration? configuration = null,
        IXmlFileHandler<LearningWorldPe>? fileSaveHandlerWorld = null,
        IXmlFileHandler<List<LinkContentPe>>? fileSaveHandlerLink = null,
        IContentFileHandler? contentHandler = null,
        ILearningWorldSavePathsHandler? worldSavePathsHandler = null,
        IFileSystem? fileSystem = null,
        IMapper? mapper = null)
    {
        configuration ??= Substitute.For<IApplicationConfiguration>();
        fileSaveHandlerWorld ??= Substitute.For<IXmlFileHandler<LearningWorldPe>>();
        fileSaveHandlerLink ??= Substitute.For<IXmlFileHandler<List<LinkContentPe>>>();
        contentHandler ??= Substitute.For<IContentFileHandler>();
        worldSavePathsHandler ??= Substitute.For<ILearningWorldSavePathsHandler>();
        fileSystem ??= new MockFileSystem();
        mapper ??= Substitute.For<IMapper>();
        return new DataAccess.API.DataAccess(configuration, fileSaveHandlerWorld, fileSaveHandlerLink, contentHandler,
            worldSavePathsHandler, fileSystem, mapper);
    }
}