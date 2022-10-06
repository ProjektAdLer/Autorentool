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
        var mockFileSaveHandlerWorld = Substitute.For<IXmlFileHandler<LearningWorldPe>>();
        var mockFileSaveHandlerSpace = Substitute.For<IXmlFileHandler<LearningSpacePe>>();
        var mockFileSaveHandlerElement = Substitute.For<IXmlFileHandler<LearningElementPe>>();
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
    public void DataAccess_SaveLearningWorldToFile_CallsFileSaveHandlerWorld()
    {
        var mockFileSaveHandlerWorld = Substitute.For<IXmlFileHandler<LearningWorldPe>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerWorld: mockFileSaveHandlerWorld);

        var learningWorld = new LearningWorld("f", "f", "f", "f", "f", "f");
        systemUnderTest.SaveLearningWorldToFile(
            learningWorld,
            "C:/nonsense");

        mockFileSaveHandlerWorld.Received().SaveToDisk(Arg.Any<LearningWorldPe>(), "C:/nonsense");
    }

    [Test]
    public void DataAccess_LoadLearningWorldFromFile_CallsFileSaveHandlerWorld()
    {
        var mockFileSaveHandlerWorld = Substitute.For<IXmlFileHandler<LearningWorldPe>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerWorld: mockFileSaveHandlerWorld);

        systemUnderTest.LoadLearningWorld("C:/nonsense");

        mockFileSaveHandlerWorld.Received().LoadFromDisk("C:/nonsense");
    }
    
    [Test]
    public void DataAccess_LoadLearningWorldFromStream_CallsFileSaveHandlerWorld()
    {
        var mockFileSaveHandlerWorld = Substitute.For<IXmlFileHandler<LearningWorldPe>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerWorld: mockFileSaveHandlerWorld);
        var stream = Substitute.For<Stream>();

        systemUnderTest.LoadLearningWorld(stream);

        mockFileSaveHandlerWorld.Received().LoadFromStream(stream);
    }

    [Test]
    public void DataAccess_SaveLearningSpaceToFile_CallsFileSaveHandlerSpace()
    {
        var mockFileSaveHandlerSpace = Substitute.For<IXmlFileHandler<LearningSpacePe>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerSpace: mockFileSaveHandlerSpace);

        var learningSpace = new LearningSpace("f", "f", "f", "f", "f", 5);
        systemUnderTest.SaveLearningSpaceToFile(
            learningSpace,
            "C:/nonsense");

        mockFileSaveHandlerSpace.Received().SaveToDisk(Arg.Any<LearningSpacePe>(), "C:/nonsense");
    }

    [Test]
    public void DataAccess_LoadLearningSpaceFromFile_CallsFileSaveHandlerSpace()
    {
        var mockFileSaveHandlerSpace = Substitute.For<IXmlFileHandler<LearningSpacePe>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerSpace: mockFileSaveHandlerSpace);

        systemUnderTest.LoadLearningSpace("C:/nonsense");

        mockFileSaveHandlerSpace.Received().LoadFromDisk("C:/nonsense");
    }
    
    [Test]
    public void DataAccess_LoadLearningSpaceFromStream_CallsFileSaveHandlerWorld()
    {
        var mockFileSaveHandlerSpace = Substitute.For<IXmlFileHandler<LearningSpacePe>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerSpace: mockFileSaveHandlerSpace);
        var stream = Substitute.For<Stream>();

        systemUnderTest.LoadLearningSpace(stream);

        mockFileSaveHandlerSpace.Received().LoadFromStream(stream);
    }

    [Test]
    public void DataAccess_SaveLearningElementToFile_CallsFileSaveHandlerElement()
    {
        var mockFileSaveHandlerElement = Substitute.For<IXmlFileHandler<LearningElementPe>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerElement: mockFileSaveHandlerElement);

        var learningContent = new LearningContent("a", "b", "");
        var learningElement = new LearningElement("f","f", learningContent, "f",
            "f", "f", LearningElementDifficultyEnum.Easy);
        systemUnderTest.SaveLearningElementToFile(
            learningElement,
            "C:/nonsense");

        mockFileSaveHandlerElement.Received().SaveToDisk(Arg.Any<LearningElementPe>(), "C:/nonsense");
    }

    [Test]
    public void DataAccess_LoadLearningElementFromFile_CallsFileSaveHandlerElement()
    {
        var mockFileSaveHandlerElement = Substitute.For<IXmlFileHandler<LearningElementPe>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerElement: mockFileSaveHandlerElement);

        systemUnderTest.LoadLearningElement("C:/nonsense");

        mockFileSaveHandlerElement.Received().LoadFromDisk("C:/nonsense");
    }
    
    [Test]
    public void DataAccess_LoadLearningElementFromStream_CallsFileSaveHandlerElement()
    {
        var mockFileSaveHandlerElement = Substitute.For<IXmlFileHandler<LearningElementPe>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerElement: mockFileSaveHandlerElement);
        var stream = Substitute.For<Stream>();

        systemUnderTest.LoadLearningElement(stream);

        mockFileSaveHandlerElement.Received().LoadFromStream(stream);
    }
    
    [Test]
    public void DataAccess_LoadLearningContentFromFile_CallsFileSaveHandlerElement()
    {
        var mockContentFileHandler = Substitute.For<IContentFileHandler>();
        var systemUnderTest = CreateTestableDataAccess(contentHandler: mockContentFileHandler);

        systemUnderTest.LoadLearningContent("C:/nonsense");

        mockContentFileHandler.Received().LoadContentAsync("C:/nonsense");
    }
    
    [Test]
    public void DataAccess_LoadLearningContentFromStream_CallsFileSaveHandlerElement()
    {
        var mockContentFileHandler = Substitute.For<IContentFileHandler>();
        var systemUnderTest = CreateTestableDataAccess(contentHandler: mockContentFileHandler);
        var stream = Substitute.For<Stream>();

        systemUnderTest.LoadLearningContent("filename.extension", stream);

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
        IXmlFileHandler<LearningWorldPe>? fileSaveHandlerWorld = null,
        IXmlFileHandler<LearningSpacePe>? fileSaveHandlerSpace = null,
        IXmlFileHandler<LearningElementPe>? fileSaveHandlerElement = null,
        IContentFileHandler? contentHandler = null,
        IFileSystem? fileSystem = null,
        IMapper? mapper = null)
    {
        configuration ??= Substitute.For<IAuthoringToolConfiguration>();
        fileSaveHandlerWorld ??= Substitute.For<IXmlFileHandler<LearningWorldPe>>();
        fileSaveHandlerSpace ??= Substitute.For<IXmlFileHandler<LearningSpacePe>>();
        fileSaveHandlerElement ??= Substitute.For<IXmlFileHandler<LearningElementPe>>();
        contentHandler ??= Substitute.For<IContentFileHandler>();
        fileSystem ??= new MockFileSystem();
        mapper ??= Substitute.For<IMapper>();
        return new DataAccess.API.DataAccess(configuration, fileSaveHandlerWorld,
            fileSaveHandlerSpace, fileSaveHandlerElement, contentHandler, fileSystem, mapper);
    }
}