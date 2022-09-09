
using System.IO.Abstractions.TestingHelpers;
using AutoMapper;
using BusinessLogic.Entities;
using Generator.API;
using Generator.DSL;
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
        var mockFilesystem = new MockFileSystem();
        var mockBackupFileGen = Substitute.For<IBackupFileGenerator>();
        var mockCreateDsl = Substitute.For<ICreateDsl>();
        var mockReadDsl = Substitute.For<IReadDsl>();
        var mockMapper = Substitute.For<IMapper>();

        // Act
        var systemUnderTest = new WorldGenerator(mockBackupFileGen, mockCreateDsl, mockReadDsl, mockFilesystem,  mockMapper);

        // Assert
        Assert.Multiple(()=>
        {
            Assert.That(systemUnderTest.BackupFile, Is.EqualTo(mockBackupFileGen));
            Assert.That(systemUnderTest.CreateDsl, Is.EqualTo(mockCreateDsl));
            Assert.That(systemUnderTest.ReadDsl, Is.EqualTo(mockReadDsl));
            Assert.That(systemUnderTest.Mapper, Is.EqualTo(mockMapper));
        });
    }

    [Test]
    public void WorldGenerator_ConstructBackup_AllMethodCallsReceived()
    {
        // Arrange
        var mockFilesystem = new MockFileSystem();
        var mockBackupFileGen = Substitute.For<IBackupFileGenerator>();
        var mockCreateDsl = Substitute.For<ICreateDsl>();
        var mockReadDsl = Substitute.For<IReadDsl>();
        var mockMapper = Substitute.For<IMapper>();
        
        var systemUnderTest = new WorldGenerator(mockBackupFileGen, mockCreateDsl, mockReadDsl, mockFilesystem, mockMapper);

        // Act
        systemUnderTest.ConstructBackup(Arg.Any<LearningWorld>(), "DestinationPath");
        
        
        
        // Assert
        Assert.Multiple(()=>
        {
            mockCreateDsl.Received().WriteLearningWorld(Arg.Any<LearningWorldPe>());
            mockReadDsl.Received().ReadLearningWorld("", Arg.Any<DocumentRootJson?>());
            mockBackupFileGen.Received().WriteXmlFiles(Arg.Any<IReadDsl>());
            mockBackupFileGen.Received().WriteBackupFile("DestinationPath");
            
        });
    }
}