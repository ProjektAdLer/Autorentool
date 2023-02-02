using System.Collections;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using AutoMapper;
using BusinessLogic.Entities;
using DataAccess.Persistence;
using NSubstitute;
using NUnit.Framework;
using PersistEntities;
using Shared;
using Shared.Configuration;

namespace DataAccessTest.API;

[TestFixture]
public class DataAccessUt
{
    [Test]
    public void DataAccess_Standard_AllPropertiesInitialized()
    {
        //Arrange 
        var mockConfiguration = Substitute.For<IAuthoringToolConfiguration>();
        var mockFileSaveHandlerWorld = Substitute.For<IXmlFileHandler<WorldPe>>();
        var mockFileSaveHandlerSpace = Substitute.For<IXmlFileHandler<SpacePe>>();
        var mockFileSaveHandlerElement = Substitute.For<IXmlFileHandler<ElementPe>>();
        var mockContentHandler = Substitute.For<IContentFileHandler>();
        var mockFileSystem = new MockFileSystem();

        //Act 
        var systemUnderTest = CreateTestableDataAccess(mockConfiguration, mockFileSaveHandlerWorld,
            mockFileSaveHandlerSpace, mockFileSaveHandlerElement, mockContentHandler, mockFileSystem);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Configuration, Is.EqualTo(mockConfiguration));
            Assert.That(systemUnderTest.XmlHandlerWorld, Is.EqualTo(mockFileSaveHandlerWorld));
            Assert.That(systemUnderTest.XmlHandlerSpace, Is.EqualTo(mockFileSaveHandlerSpace));
            Assert.That(systemUnderTest.XmlHandlerElement, Is.EqualTo(mockFileSaveHandlerElement));
            Assert.That(systemUnderTest.XmlHandlerContent, Is.EqualTo(mockContentHandler));
            Assert.That(systemUnderTest.FileSystem, Is.EqualTo(mockFileSystem));
        });
    }

    [Test]
    public void DataAccess_SaveWorldToFile_CallsFileSaveHandlerWorld()
    {
        var mockFileSaveHandlerWorld = Substitute.For<IXmlFileHandler<WorldPe>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerWorld: mockFileSaveHandlerWorld);

        var world = new World("f", "f", "f", "f", "f", "f");
        systemUnderTest.SaveWorldToFile(
            world,
            "C:/nonsense");

        mockFileSaveHandlerWorld.Received().SaveToDisk(Arg.Any<WorldPe>(), "C:/nonsense");
    }

    [Test]
    public void DataAccess_LoadWorldFromFile_CallsFileSaveHandlerWorld()
    {
        var mockFileSaveHandlerWorld = Substitute.For<IXmlFileHandler<WorldPe>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerWorld: mockFileSaveHandlerWorld);

        systemUnderTest.LoadWorld("C:/nonsense");

        mockFileSaveHandlerWorld.Received().LoadFromDisk("C:/nonsense");
    }
    
    [Test]
    public void DataAccess_LoadWorldFromStream_CallsFileSaveHandlerWorld()
    {
        var mockFileSaveHandlerWorld = Substitute.For<IXmlFileHandler<WorldPe>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerWorld: mockFileSaveHandlerWorld);
        var stream = Substitute.For<Stream>();

        systemUnderTest.LoadWorld(stream);

        mockFileSaveHandlerWorld.Received().LoadFromStream(stream);
    }

    [Test]
    public void DataAccess_SaveSpaceToFile_CallsFileSaveHandlerSpace()
    {
        var mockFileSaveHandlerSpace = Substitute.For<IXmlFileHandler<SpacePe>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerSpace: mockFileSaveHandlerSpace);

        var space = new Space("f", "f", "f", "f", "f", 5);
        systemUnderTest.SaveSpaceToFile(
            space,
            "C:/nonsense");

        mockFileSaveHandlerSpace.Received().SaveToDisk(Arg.Any<SpacePe>(), "C:/nonsense");
    }

    [Test]
    public void DataAccess_LoadSpaceFromFile_CallsFileSaveHandlerSpace()
    {
        var mockFileSaveHandlerSpace = Substitute.For<IXmlFileHandler<SpacePe>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerSpace: mockFileSaveHandlerSpace);

        systemUnderTest.LoadSpace("C:/nonsense");

        mockFileSaveHandlerSpace.Received().LoadFromDisk("C:/nonsense");
    }
    
    [Test]
    public void DataAccess_LoadSpaceFromStream_CallsFileSaveHandlerWorld()
    {
        var mockFileSaveHandlerSpace = Substitute.For<IXmlFileHandler<SpacePe>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerSpace: mockFileSaveHandlerSpace);
        var stream = Substitute.For<Stream>();

        systemUnderTest.LoadSpace(stream);

        mockFileSaveHandlerSpace.Received().LoadFromStream(stream);
    }

    [Test]
    public void DataAccess_SaveElementToFile_CallsFileSaveHandlerElement()
    {
        var mockFileSaveHandlerElement = Substitute.For<IXmlFileHandler<ElementPe>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerElement: mockFileSaveHandlerElement);

        var content = new Content("a", "b", "");
        var element = new Element("f","f", content, "url","f",
            "f", "f", ElementDifficultyEnum.Easy);
        systemUnderTest.SaveElementToFile(
            element,
            "C:/nonsense");

        mockFileSaveHandlerElement.Received().SaveToDisk(Arg.Any<ElementPe>(), "C:/nonsense");
    }

    [Test]
    public void DataAccess_LoadElementFromFile_CallsFileSaveHandlerElement()
    {
        var mockFileSaveHandlerElement = Substitute.For<IXmlFileHandler<ElementPe>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerElement: mockFileSaveHandlerElement);

        systemUnderTest.LoadElement("C:/nonsense");

        mockFileSaveHandlerElement.Received().LoadFromDisk("C:/nonsense");
    }
    
    [Test]
    public void DataAccess_LoadElementFromStream_CallsFileSaveHandlerElement()
    {
        var mockFileSaveHandlerElement = Substitute.For<IXmlFileHandler<ElementPe>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerElement: mockFileSaveHandlerElement);
        var stream = Substitute.For<Stream>();

        systemUnderTest.LoadElement(stream);

        mockFileSaveHandlerElement.Received().LoadFromStream(stream);
    }
    
    [Test]
    public void DataAccess_LoadContentFromFile_CallsFileSaveHandlerElement()
    {
        var mockContentFileHandler = Substitute.For<IContentFileHandler>();
        var systemUnderTest = CreateTestableDataAccess(contentHandler: mockContentFileHandler);

        systemUnderTest.LoadContent("C:/nonsense");

        mockContentFileHandler.Received().LoadContentAsync("C:/nonsense");
    }
    
    [Test]
    public void DataAccess_LoadContentFromStream_CallsFileSaveHandlerElement()
    {
        var mockContentFileHandler = Substitute.For<IContentFileHandler>();
        var systemUnderTest = CreateTestableDataAccess(contentHandler: mockContentFileHandler);
        var stream = Substitute.For<MemoryStream>();

        systemUnderTest.LoadContent("filename.extension", stream);

        mockContentFileHandler.Received().LoadContentAsync("filename.extension", stream);
    }

    [Test]
    [TestCaseSource(typeof(FindSuitableNewSavePathTestCases))]
    public void DataAccess_FindSuitableNewSavePath_FindsSuitablePath(IFileSystem mockFileSystem, string targetFolder,
        string fileName, string fileEnding, string expectedSavePath)
    {
        var systemUnderTest = CreateTestableDataAccess(fileSystem: mockFileSystem);

        var actualSavePath = systemUnderTest.FindSuitableNewSavePath(targetFolder, fileName, fileEnding);
        
        Assert.That(actualSavePath, Is.EqualTo(expectedSavePath));
    }

    [Test]
    public void DataAccess_FindSuitableNewSavePath_ThrowsWhenEmptyParameters()
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
                    {Path.Combine("directory", "foo.bar"), emptyFile}
                }),
                "directory", "foo", "bar", Path.Join("directory", "foo_1.bar")
            };
            yield return new object[] //multiple files present
            {
                new MockFileSystem(new Dictionary<string, MockFileData>
                {
                    {Path.Combine("directory", "foo.bar"), emptyFile},
                    {Path.Combine("directory", "foo_1.bar"), emptyFile},
                    {Path.Combine("directory", "foo_2.bar"), emptyFile}
                }),
                "directory", "foo", "bar", Path.Join("directory", "foo_3.bar")
            };
            yield return new object[] //irrelevant files present
            {
                new MockFileSystem(new Dictionary<string, MockFileData>
                {
                    {Path.Combine("directory", "poo.bar"), emptyFile}
                }),
                "directory", "foo", "bar", Path.Join("directory", "foo.bar")
            };
        }
    }

    private static DataAccess.API.DataAccess CreateTestableDataAccess(
        IAuthoringToolConfiguration? configuration = null,
        IXmlFileHandler<WorldPe>? fileSaveHandlerWorld = null,
        IXmlFileHandler<SpacePe>? fileSaveHandlerSpace = null,
        IXmlFileHandler<ElementPe>? fileSaveHandlerElement = null,
        IContentFileHandler? contentHandler = null,
        IFileSystem? fileSystem = null,
        IMapper? mapper = null)
    {
        configuration ??= Substitute.For<IAuthoringToolConfiguration>();
        fileSaveHandlerWorld ??= Substitute.For<IXmlFileHandler<WorldPe>>();
        fileSaveHandlerSpace ??= Substitute.For<IXmlFileHandler<SpacePe>>();
        fileSaveHandlerElement ??= Substitute.For<IXmlFileHandler<ElementPe>>();
        contentHandler ??= Substitute.For<IContentFileHandler>();
        fileSystem ??= new MockFileSystem();
        mapper ??= Substitute.For<IMapper>();
        return new DataAccess.API.DataAccess(configuration, fileSaveHandlerWorld,
            fileSaveHandlerSpace, fileSaveHandlerElement, contentHandler, fileSystem, mapper);
    }
}