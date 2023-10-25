using System.Collections;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using AutoMapper;
using DataAccess.Persistence;
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
            mockFileSaveHandlerSpace, mockFileSaveHandlerElement, mockFileSaveHandlerLink, mockContentHandler, mockWorldSavePathsHandler,
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

        systemUnderTest.GetSavedLearningWorldPaths();

        mockWorldSavePathsHandler.Received().GetSavedLearningWorldPaths();
    }

    [Test]
    public void GetSavedLearningWorldPaths_ReturnsSavedLearningWorldPaths()
    {
        var mockWorldSavePathsHandler = Substitute.For<ILearningWorldSavePathsHandler>();
        var systemUnderTest = CreateTestableDataAccess(worldSavePathsHandler: mockWorldSavePathsHandler);
        var expectedPaths = new List<SavedLearningWorldPath>
        {
            new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000000"), Name = "name", Path = "C:/nonsense" },
            new() { Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), Name = "name2", Path = "C:/nonsense2" }
        };
        mockWorldSavePathsHandler.GetSavedLearningWorldPaths().Returns(expectedPaths);

        var actualPaths = systemUnderTest.GetSavedLearningWorldPaths();

        Assert.That(actualPaths, Is.EqualTo(expectedPaths));
    }

    [Test]
    public void AddSavedLearningWorldPath_CallsWorldSavePathsHandler()
    {
        var mockWorldSavePathsHandler = Substitute.For<ILearningWorldSavePathsHandler>();
        var systemUnderTest = CreateTestableDataAccess(worldSavePathsHandler: mockWorldSavePathsHandler);
        var path = new SavedLearningWorldPath();

        systemUnderTest.AddSavedLearningWorldPath(path);

        mockWorldSavePathsHandler.Received().AddSavedLearningWorldPath(path);
    }

    [Test]
    public void AddSavedLearningWorldPathByPathOnly_CallsWorldSavePathsHandler()
    {
        var mockWorldSavePathsHandler = Substitute.For<ILearningWorldSavePathsHandler>();
        var systemUnderTest = CreateTestableDataAccess(worldSavePathsHandler: mockWorldSavePathsHandler);
        var path = "C:/nonsense";

        systemUnderTest.AddSavedLearningWorldPathByPathOnly(path);

        mockWorldSavePathsHandler.Received().AddSavedLearningWorldPathByPathOnly(path);
    }

    [Test]
    public void AddSavedLearningWorldPathByPathOnly_ReturnsSavedLearningWorldPath()
    {
        var mockWorldSavePathsHandler = Substitute.For<ILearningWorldSavePathsHandler>();
        var systemUnderTest = CreateTestableDataAccess(worldSavePathsHandler: mockWorldSavePathsHandler);
        var path = "C:/nonsense";
        var expectedPath = new SavedLearningWorldPath();
        mockWorldSavePathsHandler.AddSavedLearningWorldPathByPathOnly(path).Returns(expectedPath);

        var actualPath = systemUnderTest.AddSavedLearningWorldPathByPathOnly(path);

        Assert.That(actualPath, Is.EqualTo(expectedPath));
    }

    [Test]
    public void UpdateIdOfSavedLearningWorldPath_CallsWorldSavePathsHandler()
    {
        var mockWorldSavePathsHandler = Substitute.For<ILearningWorldSavePathsHandler>();
        var systemUnderTest = CreateTestableDataAccess(worldSavePathsHandler: mockWorldSavePathsHandler);
        var path = new SavedLearningWorldPath();
        var id = Guid.Parse("00000000-0000-0000-0000-000000000000");

        systemUnderTest.UpdateIdOfSavedLearningWorldPath(path, id);

        mockWorldSavePathsHandler.Received().UpdateIdOfSavedLearningWorldPath(path, id);
    }

    [Test]
    public void RemoveSavedLearningWorldPath_CallsWorldSavePathsHandler()
    {
        var mockWorldSavePathsHandler = Substitute.For<ILearningWorldSavePathsHandler>();
        var systemUnderTest = CreateTestableDataAccess(worldSavePathsHandler: mockWorldSavePathsHandler);
        var path = new SavedLearningWorldPath();

        systemUnderTest.RemoveSavedLearningWorldPath(path);

        mockWorldSavePathsHandler.Received().RemoveSavedLearningWorldPath(path);
    }

    [Test]
    [TestCaseSource(typeof(FindSuitableNewSavePathTestCases))]
    public void FindSuitableNewSavePath_FindsSuitablePath(IFileSystem mockFileSystem, string targetFolder,
        string fileName, string fileEnding, string expectedSavePath)
    {
        var systemUnderTest = CreateTestableDataAccess(fileSystem: mockFileSystem);

        var actualSavePath = systemUnderTest.FindSuitableNewSavePath(targetFolder, fileName, fileEnding);

        Assert.That(actualSavePath, Is.EqualTo(expectedSavePath));
    }

    [Test]
    public void FindSuitableNewSavePath_ThrowsWhenEmptyParameters()
    {
        var systemUnderTest = CreateTestableDataAccess();

        var ex = Assert.Throws<ArgumentException>(() => systemUnderTest.FindSuitableNewSavePath("", "foo", "bar"));
        Assert.Multiple(() =>
        {
            Assert.That(ex!.Message, Is.EqualTo("targetFolder cannot be empty (Parameter 'targetFolder')"));
            Assert.That(ex.ParamName, Is.EqualTo("targetFolder"));
        });

        ex = Assert.Throws<ArgumentException>(() => systemUnderTest.FindSuitableNewSavePath("foo", "", "bar"));
        Assert.Multiple(() =>
        {
            Assert.That(ex!.Message, Is.EqualTo("fileName cannot be empty (Parameter 'fileName')"));
            Assert.That(ex.ParamName, Is.EqualTo("fileName"));
        });

        ex = Assert.Throws<ArgumentException>(() => systemUnderTest.FindSuitableNewSavePath("foo", "bar", ""));
        Assert.Multiple(() =>
        {
            Assert.That(ex!.Message, Is.EqualTo("fileEnding cannot be empty (Parameter 'fileEnding')"));
            Assert.That(ex.ParamName, Is.EqualTo("fileEnding"));
        });
    }

    private class FindSuitableNewSavePathTestCases : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return new object[] //no file present
            {
                new MockFileSystem(new Dictionary<string, MockFileData>()),
                "directory", "foo", "bar", Path.Join("directory", "foo.bar")
            };
            var emptyFile = new MockFileData("");
            yield return new object[] //file is present
            {
                new MockFileSystem(new Dictionary<string, MockFileData>
                {
                    { Path.Combine("directory", "foo.bar"), emptyFile }
                }),
                "directory", "foo", "bar", Path.Join("directory", "foo_1.bar")
            };
            yield return new object[] //multiple files present
            {
                new MockFileSystem(new Dictionary<string, MockFileData>
                {
                    { Path.Combine("directory", "foo.bar"), emptyFile },
                    { Path.Combine("directory", "foo_1.bar"), emptyFile },
                    { Path.Combine("directory", "foo_2.bar"), emptyFile }
                }),
                "directory", "foo", "bar", Path.Join("directory", "foo_3.bar")
            };
            yield return new object[] //irrelevant files present
            {
                new MockFileSystem(new Dictionary<string, MockFileData>
                {
                    { Path.Combine("directory", "poo.bar"), emptyFile }
                }),
                "directory", "foo", "bar", Path.Join("directory", "foo.bar")
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