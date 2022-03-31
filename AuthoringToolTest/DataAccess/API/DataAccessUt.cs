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
        var mockFileSaveHandlerWorld = Substitute.For<IFileSaveHandler<LearningWorld>>();

        //Act 
        var systemUnderTest = CreateTestableDataAccess(mockConfiguration, mockBackupFileConstructor,
            mockFileSaveHandlerWorld);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Configuration, Is.EqualTo(mockConfiguration));
            Assert.That(systemUnderTest.BackupFile, Is.EqualTo(mockBackupFileConstructor));
            Assert.That(systemUnderTest.SaveHandlerWorld, Is.EqualTo(mockFileSaveHandlerWorld));
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
        mockBackupFile.Received().WriteXMLFiles();
        mockBackupFile.Received().WriteBackupFile();
    }

    [Test]
    public void DataAccess_SaveLearningWorldToFile_CallsFileSaveHandlerWorld()
    {
        var mockFileSaveHandlerWorld = Substitute.For<IFileSaveHandler<LearningWorld>>();
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
        var mockFileSaveHandlerWorld = Substitute.For<IFileSaveHandler<LearningWorld>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerWorld: mockFileSaveHandlerWorld);

        systemUnderTest.LoadLearningWorldFromFile("C:/nonsense");

        mockFileSaveHandlerWorld.Received().LoadFromDisk("C:/nonsense");
    }

    [Test]
    public void DataAccess_SaveLearningSpaceToFile_CallsFileSaveHandlerSpace()
    {
        var mockFileSaveHandlerSpace = Substitute.For<IFileSaveHandler<LearningSpace>>();
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
        var mockFileSaveHandlerSpace = Substitute.For<IFileSaveHandler<LearningSpace>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerSpace: mockFileSaveHandlerSpace);

        systemUnderTest.LoadLearningSpaceFromFile("C:/nonsense");

        mockFileSaveHandlerSpace.Received().LoadFromDisk("C:/nonsense");
    }

    [Test]
    public void DataAccess_SaveLearningElementToFile_CallsFileSaveHandlerElement()
    {
        var mockFileSaveHandlerElement = Substitute.For<IFileSaveHandler<LearningElement>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerElement: mockFileSaveHandlerElement);

        var learningElement = new LearningElement("f", "f", "f", "f", "f", "f", "f");
        systemUnderTest.SaveLearningElementToFile(
            learningElement,
            "C:/nonsense");

        mockFileSaveHandlerElement.Received().SaveToDisk(learningElement, "C:/nonsense");
    }

    [Test]
    public void DataAccess_LoadLearningElementFromFile_CallsFileSaveHandlerElement()
    {
        var mockFileSaveHandlerElement = Substitute.For<IFileSaveHandler<LearningElement>>();
        var systemUnderTest = CreateTestableDataAccess(fileSaveHandlerElement: mockFileSaveHandlerElement);

        systemUnderTest.LoadLearningElementFromFile("C:/nonsense");

        mockFileSaveHandlerElement.Received().LoadFromDisk("C:/nonsense");
    }

    private static AuthoringTool.DataAccess.API.DataAccess CreateTestableDataAccess(
        IAuthoringToolConfiguration? configuration = null,
        IBackupFileGenerator? backupFileConstructor = null,
        IFileSaveHandler<LearningWorld>? fileSaveHandlerWorld = null,
        IFileSaveHandler<LearningSpace>? fileSaveHandlerSpace = null,
        IFileSaveHandler<LearningElement>? fileSaveHandlerElement = null)
    {
        configuration ??= Substitute.For<IAuthoringToolConfiguration>();
        backupFileConstructor ??= Substitute.For<IBackupFileGenerator>();
        fileSaveHandlerWorld ??= Substitute.For<IFileSaveHandler<LearningWorld>>();
        fileSaveHandlerSpace ??= Substitute.For<IFileSaveHandler<LearningSpace>>();
        fileSaveHandlerElement ??= Substitute.For<IFileSaveHandler<LearningElement>>();
        return new AuthoringTool.DataAccess.API.DataAccess(configuration, backupFileConstructor, fileSaveHandlerWorld,
            fileSaveHandlerSpace, fileSaveHandlerElement);
    }
}