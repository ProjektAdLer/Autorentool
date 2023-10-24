using System.IO.Abstractions.TestingHelpers;
using AutoMapper;
using BusinessLogic.Entities;
using Generator.API;
using Generator.ATF;
using Generator.WorldExport;
using NSubstitute;
using NUnit.Framework;
using PersistEntities;

namespace GeneratorTest.API;

[TestFixture]
public class WorldGeneratorUt
{
    [Test]
    public void WorldGenerator_DefaultConstructor_AllParametersSet()
    {
        // Arrange
        var mockBackupFileGen = Substitute.For<IBackupFileGenerator>();
        var mockCreateAtf = Substitute.For<ICreateAtf>();
        var mockReadAtf = Substitute.For<IReadAtf>();
        var mockMapper = Substitute.For<IMapper>();
        var mockFileSystem = new MockFileSystem();

        // Act
        var systemUnderTest =
            new WorldGenerator(mockBackupFileGen, mockCreateAtf, mockReadAtf, mockMapper, mockFileSystem);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.BackupFile, Is.EqualTo(mockBackupFileGen));
            Assert.That(systemUnderTest.CreateAtf, Is.EqualTo(mockCreateAtf));
            Assert.That(systemUnderTest.ReadAtf, Is.EqualTo(mockReadAtf));
            Assert.That(systemUnderTest.Mapper, Is.EqualTo(mockMapper));
        });
    }

    [Test]
    public void WorldGenerator_ConstructBackup_AllMethodCallsReceived()
    {
        // Arrange
        var mockBackupFileGen = Substitute.For<IBackupFileGenerator>();
        var mockCreateAtf = Substitute.For<ICreateAtf>();
        var mockReadAtf = Substitute.For<IReadAtf>();
        var mockMapper = Substitute.For<IMapper>();
        var mockFileSystem = new MockFileSystem();

        var systemUnderTest =
            new WorldGenerator(mockBackupFileGen, mockCreateAtf, mockReadAtf, mockMapper, mockFileSystem);

        // Act
        systemUnderTest.ConstructBackup(Arg.Any<LearningWorld>(), "DestinationPath");


        // Assert
        Assert.Multiple(() =>
        {
            mockCreateAtf.Received().GenerateAndExportLearningWorldJson(Arg.Any<LearningWorldPe>());
            mockReadAtf.Received().ReadLearningWorld("", Arg.Any<DocumentRootJson?>());
            mockBackupFileGen.Received().WriteXmlFiles(Arg.Any<IReadAtf>());
            mockBackupFileGen.Received().WriteBackupFile("DestinationPath");
        });
    }

    [Test]
    public void ExtractAtfFromBackup_CallsBackupFileGen()
    {
        // Arrange
        var mockBackupFileGen = Substitute.For<IBackupFileGenerator>();
        var mockCreateAtf = Substitute.For<ICreateAtf>();
        var mockReadAtf = Substitute.For<IReadAtf>();
        var mockMapper = Substitute.For<IMapper>();
        var mockFileSystem = new MockFileSystem();

        var systemUnderTest =
            new WorldGenerator(mockBackupFileGen, mockCreateAtf, mockReadAtf, mockMapper, mockFileSystem);

        // Act
        systemUnderTest.ExtractAtfFromBackup("DestinationPath");

        // Assert
        mockBackupFileGen.Received().ExtractAtfFromBackup("DestinationPath");
    }

    [Test]
    public void ExtractAtfFromBackup_ReturnsPath()
    {
        // Arrange
        var mockBackupFileGen = Substitute.For<IBackupFileGenerator>();
        mockBackupFileGen.ExtractAtfFromBackup("DestinationPath").Returns("PathToAtf");
        var mockCreateAtf = Substitute.For<ICreateAtf>();
        var mockReadAtf = Substitute.For<IReadAtf>();
        var mockMapper = Substitute.For<IMapper>();
        var mockFileSystem = new MockFileSystem();

        var systemUnderTest =
            new WorldGenerator(mockBackupFileGen, mockCreateAtf, mockReadAtf, mockMapper, mockFileSystem);

        // Act
        var result = systemUnderTest.ExtractAtfFromBackup("DestinationPath");

        // Assert
        Assert.That(result, Is.EqualTo("PathToAtf"));
    }

    [Test]
    public void ExtractAtfFromBackup()
    {
        //TODO: Implement
    }
}