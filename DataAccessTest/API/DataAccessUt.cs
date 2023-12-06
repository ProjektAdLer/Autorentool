using System.Collections;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using AutoMapper;
using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent.FileContent;
using BusinessLogic.Entities.LearningContent.LinkContent;
using DataAccess.Persistence;
using DataAccessTest.Resources;
using JetBrains.Annotations;
using NSubstitute;
using NUnit.Framework;
using PersistEntities;
using PersistEntities.LearningContent;
using Shared;
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
        var mockFileSaveHandlerSpace = Substitute.For<IXmlFileHandler<LearningSpacePe>>();
        var mockFileSaveHandlerElement = Substitute.For<IXmlFileHandler<LearningElementPe>>();
        var mockFileSaveHandlerLink = Substitute.For<IXmlFileHandler<List<LinkContentPe>>>();
        var mockContentHandler = Substitute.For<IContentFileHandler>();
        var mockWorldSavePathsHandler = Substitute.For<ILearningWorldSavePathsHandler>();
        var mockFileSystem = new MockFileSystem();

        //Act 
        var systemUnderTest = CreateTestableDataAccess(mockConfiguration, mockFileSaveHandlerWorld,
            mockFileSaveHandlerSpace, mockFileSaveHandlerElement, mockFileSaveHandlerLink, mockContentHandler,
            mockWorldSavePathsHandler,
            mockFileSystem);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Configuration, Is.EqualTo(mockConfiguration));
            Assert.That(systemUnderTest.XmlHandlerWorld, Is.EqualTo(mockFileSaveHandlerWorld));
            Assert.That(systemUnderTest.XmlHandlerSpace, Is.EqualTo(mockFileSaveHandlerSpace));
            Assert.That(systemUnderTest.XmlHandlerElement, Is.EqualTo(mockFileSaveHandlerElement));
            Assert.That(systemUnderTest.XmlHandlerLink, Is.EqualTo(mockFileSaveHandlerLink));
            Assert.That(systemUnderTest.ContentFileHandler, Is.EqualTo(mockContentHandler));
            Assert.That(systemUnderTest.WorldSavePathsHandler, Is.EqualTo(mockWorldSavePathsHandler));
            Assert.That(systemUnderTest.FileSystem, Is.EqualTo(mockFileSystem));
        });
    }

    [Test]
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
    public void LoadLearningWorldFromFile_CallsFileSaveHandlerWorld()
    {
        var mockFileSaveHandlerWorld = Substitute.For<IXmlFileHandler<LearningWorldPe>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerWorld: mockFileSaveHandlerWorld);

        systemUnderTest.LoadLearningWorld("C:/nonsense");

        mockFileSaveHandlerWorld.Received().LoadFromDisk("C:/nonsense");
    }

    [Test]
    public void LoadLearningWorldFromStream_CallsFileSaveHandlerWorld()
    {
        var mockFileSaveHandlerWorld = Substitute.For<IXmlFileHandler<LearningWorldPe>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerWorld: mockFileSaveHandlerWorld);
        var stream = Substitute.For<Stream>();

        systemUnderTest.LoadLearningWorld(stream);

        mockFileSaveHandlerWorld.Received().LoadFromStream(stream);
    }

    [Test]
    public void SaveLearningSpaceToFile_CallsFileSaveHandlerSpace()
    {
        var mockFileSaveHandlerSpace = Substitute.For<IXmlFileHandler<LearningSpacePe>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerSpace: mockFileSaveHandlerSpace);

        var learningSpace = EntityProvider.GetLearningSpace();
        systemUnderTest.SaveLearningSpaceToFile(
            learningSpace,
            "C:/nonsense");

        mockFileSaveHandlerSpace.Received().SaveToDisk(Arg.Any<LearningSpacePe>(), "C:/nonsense");
    }

    [Test]
    public void LoadLearningSpaceFromFile_CallsFileSaveHandlerSpace()
    {
        var mockFileSaveHandlerSpace = Substitute.For<IXmlFileHandler<LearningSpacePe>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerSpace: mockFileSaveHandlerSpace);

        systemUnderTest.LoadLearningSpace("C:/nonsense");

        mockFileSaveHandlerSpace.Received().LoadFromDisk("C:/nonsense");
    }

    [Test]
    public void LoadLearningSpaceFromStream_CallsFileSaveHandlerWorld()
    {
        var mockFileSaveHandlerSpace = Substitute.For<IXmlFileHandler<LearningSpacePe>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerSpace: mockFileSaveHandlerSpace);
        var stream = Substitute.For<Stream>();

        systemUnderTest.LoadLearningSpace(stream);

        mockFileSaveHandlerSpace.Received().LoadFromStream(stream);
    }

    [Test]
    public void SaveLearningElementToFile_CallsFileSaveHandlerElement()
    {
        var mockFileSaveHandlerElement = Substitute.For<IXmlFileHandler<LearningElementPe>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerElement: mockFileSaveHandlerElement);

        var learningContent = EntityProvider.GetFileContent();
        var learningElement = EntityProvider.GetLearningElement(content: learningContent);
        systemUnderTest.SaveLearningElementToFile(
            learningElement,
            "C:/nonsense");

        mockFileSaveHandlerElement.Received().SaveToDisk(Arg.Any<LearningElementPe>(), "C:/nonsense");
    }

    [Test]
    public void LoadLearningElementFromFile_CallsFileSaveHandlerElement()
    {
        var mockFileSaveHandlerElement = Substitute.For<IXmlFileHandler<LearningElementPe>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerElement: mockFileSaveHandlerElement);

        systemUnderTest.LoadLearningElement("C:/nonsense");

        mockFileSaveHandlerElement.Received().LoadFromDisk("C:/nonsense");
    }

    [Test]
    public void LoadLearningElementFromStream_CallsFileSaveHandlerElement()
    {
        var mockFileSaveHandlerElement = Substitute.For<IXmlFileHandler<LearningElementPe>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerElement: mockFileSaveHandlerElement);
        var stream = Substitute.For<Stream>();

        systemUnderTest.LoadLearningElement(stream);

        mockFileSaveHandlerElement.Received().LoadFromStream(stream);
    }

    [Test]
    public void LoadLearningContentFromFile_CallsFileSaveHandlerElement()
    {
        var mockContentFileHandler = Substitute.For<IContentFileHandler>();
        var systemUnderTest = CreateTestableDataAccess(contentHandler: mockContentFileHandler);

        systemUnderTest.LoadLearningContentAsync("C:/nonsense");

        mockContentFileHandler.Received().LoadContentAsync("C:/nonsense");
    }

    [Test]
    public void LoadLearningContentFromStream_CallsFileSaveHandlerElement()
    {
        var mockContentFileHandler = Substitute.For<IContentFileHandler>();
        var systemUnderTest = CreateTestableDataAccess(contentHandler: mockContentFileHandler);
        var stream = Substitute.For<MemoryStream>();

        systemUnderTest.LoadLearningContentAsync("filename.extension", stream);

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
    public async Task ImportLearningWorldFromArchiveAsync_CopiesContentOverCorrectly()
    {
        var fileSystem = ResourceHelper.PrepareFileSystemWithResources();
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

        var contentFileHandler = Substitute.For<IContentFileHandler>();
        var logoFcPe = PersistEntityProvider.GetFileContent("adler_logo.png", "png",
            Path.Join(ApplicationPaths.ContentFolder, "adler_logo.png"));
        var regexFcPe = PersistEntityProvider.GetFileContent("regex.txt", "txt",
            Path.Join(ApplicationPaths.ContentFolder, "regex.txt"));
        contentFileHandler.LoadContentAsync(Arg.Is<string>(s => s.EndsWith("adler_logo.png")), Arg.Any<byte[]>())
            .Returns(logoFcPe);
        contentFileHandler.LoadContentAsync(Arg.Is<string>(s => s.EndsWith("regex.txt")), Arg.Any<byte[]>())
            .Returns(regexFcPe);

        var systemUnderTest = CreateTestableDataAccess(mapper: mapper, fileSaveHandlerWorld: xmlHandlerWorlds,
            fileSystem: fileSystem, contentHandler: contentFileHandler);

        var loadedWorld = await systemUnderTest.ImportLearningWorldFromArchiveAsync("C:\\zips\\import_test.zip");

        await contentFileHandler.Received()
            .LoadContentAsync(Arg.Is<string>(s => s.EndsWith("adler_logo.png")), Arg.Any<byte[]>());
        await contentFileHandler.Received()
            .LoadContentAsync(Arg.Is<string>(s => s.EndsWith("regex.txt")), Arg.Any<byte[]>());

        Assert.That(loadedWorld, Is.EqualTo(learningWorld));
        Assert.That(
            (loadedWorld.LearningSpaces.First().ContainedLearningElements.First().LearningContent as FileContent)!
            .Filepath, Is.EqualTo(Path.Join(ApplicationPaths.ContentFolder, "adler_logo.png")));
        Assert.That(
            (loadedWorld.LearningSpaces.ElementAt(1).ContainedLearningElements.First().LearningContent as FileContent)!
            .Filepath, Is.EqualTo(Path.Join(ApplicationPaths.ContentFolder, "regex.txt")));
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
        IXmlFileHandler<LearningSpacePe>? fileSaveHandlerSpace = null,
        IXmlFileHandler<LearningElementPe>? fileSaveHandlerElement = null,
        IXmlFileHandler<List<LinkContentPe>>? fileSaveHandlerLink = null,
        IContentFileHandler? contentHandler = null,
        ILearningWorldSavePathsHandler? worldSavePathsHandler = null,
        IFileSystem? fileSystem = null,
        IMapper? mapper = null)
    {
        configuration ??= Substitute.For<IApplicationConfiguration>();
        fileSaveHandlerWorld ??= Substitute.For<IXmlFileHandler<LearningWorldPe>>();
        fileSaveHandlerSpace ??= Substitute.For<IXmlFileHandler<LearningSpacePe>>();
        fileSaveHandlerElement ??= Substitute.For<IXmlFileHandler<LearningElementPe>>();
        fileSaveHandlerLink ??= Substitute.For<IXmlFileHandler<List<LinkContentPe>>>();
        contentHandler ??= Substitute.For<IContentFileHandler>();
        worldSavePathsHandler ??= Substitute.For<ILearningWorldSavePathsHandler>();
        fileSystem ??= new MockFileSystem();
        mapper ??= Substitute.For<IMapper>();
        return new DataAccess.API.DataAccess(configuration, fileSaveHandlerWorld,
            fileSaveHandlerSpace, fileSaveHandlerElement, fileSaveHandlerLink, contentHandler, worldSavePathsHandler,
            fileSystem, mapper);
    }
}