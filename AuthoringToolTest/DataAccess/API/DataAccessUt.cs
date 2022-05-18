using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using AuthoringTool.API.Configuration;
using AuthoringTool.DataAccess.Persistence;
using AuthoringTool.DataAccess.WorldExport;
using AuthoringTool.Entities;
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
        var mockFileSaveHandlerWorld = Substitute.For<IXmlFileHandler<LearningWorld>>();

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
        var mockBackupFile = Substitute.For<IBackupFileGenerator>();
        var systemUnderTest = CreateTestableDataAccess(null, mockBackupFile);

        //Act
        systemUnderTest.ConstructBackup();

        //Assert
        mockBackupFile.Received().WriteXmlFiles();
        mockBackupFile.Received().WriteBackupFile();
    }

    [Test]
    public void DataAccess_SaveLearningWorldToFile_CallsFileSaveHandlerWorld()
    {
        var mockFileSaveHandlerWorld = Substitute.For<IXmlFileHandler<LearningWorld>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerWorld: mockFileSaveHandlerWorld);

        var learningWorld = new LearningWorld("f", "f", "f", "f", "f", "f");
        systemUnderTest.SaveLearningWorldToFile(
            learningWorld,
            "C:/nonsense");

        mockFileSaveHandlerWorld.Received().SaveToDisk(learningWorld, "C:/nonsense");
    }

    [Test]
    public void DataAccess_LoadLearningWorldFromFile_CallsFileSaveHandlerWorld()
    {
        var mockFileSaveHandlerWorld = Substitute.For<IXmlFileHandler<LearningWorld>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerWorld: mockFileSaveHandlerWorld);

        systemUnderTest.LoadLearningWorldFromFile("C:/nonsense");

        mockFileSaveHandlerWorld.Received().LoadFromDisk("C:/nonsense");
    }

    [Test]
    public void DataAccess_SaveLearningSpaceToFile_CallsFileSaveHandlerSpace()
    {
        var mockFileSaveHandlerSpace = Substitute.For<IXmlFileHandler<LearningSpace>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerSpace: mockFileSaveHandlerSpace);

        var learningSpace = new LearningSpace("f", "f", "f", "f", "f");
        systemUnderTest.SaveLearningSpaceToFile(
            learningSpace,
            "C:/nonsense");

        mockFileSaveHandlerSpace.Received().SaveToDisk(learningSpace, "C:/nonsense");
    }

    [Test]
    public void DataAccess_LoadLearningSpaceFromFile_CallsFileSaveHandlerSpace()
    {
        var mockFileSaveHandlerSpace = Substitute.For<IXmlFileHandler<LearningSpace>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerSpace: mockFileSaveHandlerSpace);

        systemUnderTest.LoadLearningSpaceFromFile("C:/nonsense");

        mockFileSaveHandlerSpace.Received().LoadFromDisk("C:/nonsense");
    }

    [Test]
    public void DataAccess_SaveLearningElementToFile_CallsFileSaveHandlerElement()
    {
        var mockFileSaveHandlerElement = Substitute.For<IXmlFileHandler<LearningElement>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerElement: mockFileSaveHandlerElement);

        var learningContent = new LearningContent("a", "b", Array.Empty<byte>());
        var learningElement = new LearningElement("f","f", "f", "f", "f", learningContent,"f", "f", "f");
        systemUnderTest.SaveLearningElementToFile(
            learningElement,
            "C:/nonsense");

        mockFileSaveHandlerElement.Received().SaveToDisk(learningElement, "C:/nonsense");
    }

    [Test]
    public void DataAccess_LoadLearningElementFromFile_CallsFileSaveHandlerElement()
    {
        var mockFileSaveHandlerElement = Substitute.For<IXmlFileHandler<LearningElement>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerElement: mockFileSaveHandlerElement);

        systemUnderTest.LoadLearningElementFromFile("C:/nonsense");

        mockFileSaveHandlerElement.Received().LoadFromDisk("C:/nonsense");
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
            yield return new object[] //file is present
            {
                new MockFileSystem(new Dictionary<string, MockFileData>
                {
                    {Path.Combine("directory", "foo.bar"), MockFileData.NullObject}
                }),
                "directory", "foo", "bar", Path.Join("directory", "foo_1.bar")
            };
            yield return new object[] //multiple files present
            {
                new MockFileSystem(new Dictionary<string, MockFileData>
                {
                    {Path.Combine("directory", "foo.bar"), MockFileData.NullObject},
                    {Path.Combine("directory", "foo_1.bar"), MockFileData.NullObject},
                    {Path.Combine("directory", "foo_2.bar"), MockFileData.NullObject}
                }),
                "directory", "foo", "bar", Path.Join("directory", "foo_3.bar")
            };
            yield return new object[] //irrelevant files present
            {
                new MockFileSystem(new Dictionary<string, MockFileData>
                {
                    {Path.Combine("directory", "poo.bar"), MockFileData.NullObject}
                }),
                "directory", "foo", "bar", Path.Join("directory", "foo.bar")
            };
        }
    }

    private static AuthoringTool.DataAccess.API.DataAccess CreateTestableDataAccess(
        IAuthoringToolConfiguration? configuration = null,
        IBackupFileGenerator? backupFileConstructor = null,
        IXmlFileHandler<LearningWorld>? fileSaveHandlerWorld = null,
        IXmlFileHandler<LearningSpace>? fileSaveHandlerSpace = null,
        IXmlFileHandler<LearningElement>? fileSaveHandlerElement = null,
        IContentFileHandler? contentHandler = null,
        IFileSystem? fileSystem = null)
    {
        configuration ??= Substitute.For<IAuthoringToolConfiguration>();
        backupFileConstructor ??= Substitute.For<IBackupFileGenerator>();
        fileSaveHandlerWorld ??= Substitute.For<IXmlFileHandler<LearningWorld>>();
        fileSaveHandlerSpace ??= Substitute.For<IXmlFileHandler<LearningSpace>>();
        fileSaveHandlerElement ??= Substitute.For<IXmlFileHandler<LearningElement>>();
        contentHandler ??= Substitute.For<IContentFileHandler>();
        fileSystem ??= new MockFileSystem();
        return new AuthoringTool.DataAccess.API.DataAccess(configuration, backupFileConstructor, fileSaveHandlerWorld,
            fileSaveHandlerSpace, fileSaveHandlerElement, contentHandler, fileSystem);
    }
}