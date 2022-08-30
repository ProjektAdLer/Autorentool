using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using AuthoringTool.API.Configuration;
using AuthoringTool.DataAccess.DSL;
using AuthoringTool.DataAccess.Persistence;
using AuthoringTool.DataAccess.PersistEntities;
using AuthoringTool.DataAccess.WorldExport;
using NSubstitute;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.API;

[TestFixture]
public class DataAccessUt
{
    [Test]
    public void DataAccess_Standard_AllPropertiesInitialized()
    {
        //Arrange 
        var mockConfiguration = Substitute.For<IAuthoringToolConfiguration>();
        var mockBackupFileConstructor = Substitute.For<IBackupFileGenerator>();
        var mockFileSaveHandlerWorld = Substitute.For<IXmlFileHandler<LearningWorldPe>>();

        //Act 
        var systemUnderTest = CreateTestableDataAccess(mockConfiguration, mockBackupFileConstructor,
            mockFileSaveHandlerWorld);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Configuration, Is.EqualTo(mockConfiguration));
            Assert.That(systemUnderTest.BackupFile, Is.EqualTo(mockBackupFileConstructor));
            Assert.That(systemUnderTest.XmlHandlerWorld, Is.EqualTo(mockFileSaveHandlerWorld));
        });
    }

    [Test]
    public void DataAccess_ConstructBackup_CallsBackupFileGenerator()
    {
        //Arrange
        var mockReadDsl = Substitute.For<IReadDsl>();
        var mockCreateDsl = Substitute.For<ICreateDsl>();
        
        var mockBackupFile = Substitute.For<IBackupFileGenerator>();
        var systemUnderTest = CreateTestableDataAccess(backupFileConstructor: mockBackupFile, createDsl: mockCreateDsl,
            readDsl: mockReadDsl);
        var filepath = "this/path";
        var mockLearningWorld = Substitute.For<ILearningWorldPe>();


        //Act
        systemUnderTest.ConstructBackup(mockLearningWorld as LearningWorldPe, filepath);

        //Assert
        mockCreateDsl.Received().WriteLearningWorld(mockLearningWorld as LearningWorldPe);
        mockReadDsl.Received().ReadLearningWorld(mockCreateDsl.WriteLearningWorld(mockLearningWorld as LearningWorldPe));
        mockBackupFile.Received().CreateBackupFolders();
        mockBackupFile.Received().WriteXmlFiles(mockReadDsl as ReadDsl, mockCreateDsl.WriteLearningWorld(mockLearningWorld as LearningWorldPe));
        mockBackupFile.Received().WriteBackupFile(filepath);

    }

    [Test]
    public void DataAccess_SaveLearningWorldToFile_CallsFileSaveHandlerWorld()
    {
        var mockFileSaveHandlerWorld = Substitute.For<IXmlFileHandler<LearningWorldPe>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerWorld: mockFileSaveHandlerWorld);

        var learningWorld = new LearningWorldPe("f", "f", "f", "f", "f", "f");
        systemUnderTest.SaveLearningWorldToFile(
            learningWorld,
            "C:/nonsense");

        mockFileSaveHandlerWorld.Received().SaveToDisk(learningWorld, "C:/nonsense");
    }

    [Test]
    public void DataAccess_LoadLearningWorldFromFile_CallsFileSaveHandlerWorld()
    {
        var mockFileSaveHandlerWorld = Substitute.For<IXmlFileHandler<LearningWorldPe>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerWorld: mockFileSaveHandlerWorld);

        systemUnderTest.LoadLearningWorldFromFile("C:/nonsense");

        mockFileSaveHandlerWorld.Received().LoadFromDisk("C:/nonsense");
    }
    
    [Test]
    public void DataAccess_LoadLearningWorldFromStream_CallsFileSaveHandlerWorld()
    {
        var mockFileSaveHandlerWorld = Substitute.For<IXmlFileHandler<LearningWorldPe>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerWorld: mockFileSaveHandlerWorld);
        var stream = Substitute.For<Stream>();

        systemUnderTest.LoadLearningWorldFromStream(stream);

        mockFileSaveHandlerWorld.Received().LoadFromStream(stream);
    }

    [Test]
    public void DataAccess_SaveLearningSpaceToFile_CallsFileSaveHandlerSpace()
    {
        var mockFileSaveHandlerSpace = Substitute.For<IXmlFileHandler<LearningSpacePe>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerSpace: mockFileSaveHandlerSpace);

        var learningSpace = new LearningSpacePe("f", "f", "f", "f", "f");
        systemUnderTest.SaveLearningSpaceToFile(
            learningSpace,
            "C:/nonsense");

        mockFileSaveHandlerSpace.Received().SaveToDisk(learningSpace, "C:/nonsense");
    }

    [Test]
    public void DataAccess_LoadLearningSpaceFromFile_CallsFileSaveHandlerSpace()
    {
        var mockFileSaveHandlerSpace = Substitute.For<IXmlFileHandler<LearningSpacePe>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerSpace: mockFileSaveHandlerSpace);

        systemUnderTest.LoadLearningSpaceFromFile("C:/nonsense");

        mockFileSaveHandlerSpace.Received().LoadFromDisk("C:/nonsense");
    }
    
    [Test]
    public void DataAccess_LoadLearningSpaceFromStream_CallsFileSaveHandlerWorld()
    {
        var mockFileSaveHandlerSpace = Substitute.For<IXmlFileHandler<LearningSpacePe>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerSpace: mockFileSaveHandlerSpace);
        var stream = Substitute.For<Stream>();

        systemUnderTest.LoadLearningSpaceFromStream(stream);

        mockFileSaveHandlerSpace.Received().LoadFromStream(stream);
    }

    [Test]
    public void DataAccess_SaveLearningElementToFile_CallsFileSaveHandlerElement()
    {
        var mockFileSaveHandlerElement = Substitute.For<IXmlFileHandler<LearningElementPe>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerElement: mockFileSaveHandlerElement);

        var learningContent = new LearningContentPe("a", "b", Array.Empty<byte>());
        var learningElement = new LearningElementPe("f","f", learningContent, "f",
            "f", "f", LearningElementDifficultyEnumPe.Easy);
        systemUnderTest.SaveLearningElementToFile(
            learningElement,
            "C:/nonsense");

        mockFileSaveHandlerElement.Received().SaveToDisk(learningElement, "C:/nonsense");
    }

    [Test]
    public void DataAccess_LoadLearningElementFromFile_CallsFileSaveHandlerElement()
    {
        var mockFileSaveHandlerElement = Substitute.For<IXmlFileHandler<LearningElementPe>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerElement: mockFileSaveHandlerElement);

        systemUnderTest.LoadLearningElementFromFile("C:/nonsense");

        mockFileSaveHandlerElement.Received().LoadFromDisk("C:/nonsense");
    }
    
    [Test]
    public void DataAccess_LoadLearningElementFromStream_CallsFileSaveHandlerElement()
    {
        var mockFileSaveHandlerElement = Substitute.For<IXmlFileHandler<LearningElementPe>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerElement: mockFileSaveHandlerElement);
        var stream = Substitute.For<Stream>();

        systemUnderTest.LoadLearningElementFromStream(stream);

        mockFileSaveHandlerElement.Received().LoadFromStream(stream);
    }
    
    [Test]
    public void DataAccess_LoadLearningContentFromFile_CallsFileSaveHandlerElement()
    {
        var mockContentFileHandler = Substitute.For<IContentFileHandler>();
        var systemUnderTest = CreateTestableDataAccess(contentHandler: mockContentFileHandler);

        systemUnderTest.LoadLearningContentFromFile("C:/nonsense");

        mockContentFileHandler.Received().LoadFromDisk("C:/nonsense");
    }
    
    [Test]
    public void DataAccess_LoadLearningContentFromStream_CallsFileSaveHandlerElement()
    {
        var mockContentFileHandler = Substitute.For<IContentFileHandler>();
        var systemUnderTest = CreateTestableDataAccess(contentHandler: mockContentFileHandler);
        var stream = Substitute.For<Stream>();

        systemUnderTest.LoadLearningContentFromStream("filename.extension", stream);

        mockContentFileHandler.Received().LoadFromStream("filename.extension", stream);
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

    private static AuthoringTool.DataAccess.API.DataAccess CreateTestableDataAccess(
        IAuthoringToolConfiguration? configuration = null,
        IBackupFileGenerator? backupFileConstructor = null,
        IXmlFileHandler<LearningWorldPe>? fileSaveHandlerWorld = null,
        IXmlFileHandler<LearningSpacePe>? fileSaveHandlerSpace = null,
        IXmlFileHandler<LearningElementPe>? fileSaveHandlerElement = null,
        IContentFileHandler? contentHandler = null,
        ICreateDsl? createDsl = null,
        IReadDsl? readDsl = null,
        IFileSystem? fileSystem = null)
    {
        configuration ??= Substitute.For<IAuthoringToolConfiguration>();
        backupFileConstructor ??= Substitute.For<IBackupFileGenerator>();
        fileSaveHandlerWorld ??= Substitute.For<IXmlFileHandler<LearningWorldPe>>();
        fileSaveHandlerSpace ??= Substitute.For<IXmlFileHandler<LearningSpacePe>>();
        fileSaveHandlerElement ??= Substitute.For<IXmlFileHandler<LearningElementPe>>();
        contentHandler ??= Substitute.For<IContentFileHandler>();
        fileSystem ??= new MockFileSystem();
        createDsl ??= Substitute.For<ICreateDsl>();
        readDsl ??= Substitute.For<IReadDsl>();
        return new AuthoringTool.DataAccess.API.DataAccess(configuration, backupFileConstructor, fileSaveHandlerWorld,
            fileSaveHandlerSpace, fileSaveHandlerElement, contentHandler, createDsl, readDsl, fileSystem);
    }
}