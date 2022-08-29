
using System.IO.Abstractions.TestingHelpers;
using AuthoringToolLib.Entities;
using AutoMapper;
using Generator.API;
using Generator.DSL;
using Generator.PersistEntities;
using Generator.WorldExport;
using NSubstitute;
using NUnit.Framework;

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
        var worldGenerator = new WorldGenerator(mockBackupFileGen, mockCreateDsl, mockReadDsl, mockFilesystem,  mockMapper);

        // Assert
        Assert.Multiple(()=>
        {
            Assert.That(worldGenerator.BackupFile, Is.EqualTo(mockBackupFileGen));
            Assert.That(worldGenerator.CreateDsl, Is.EqualTo(mockCreateDsl));
            Assert.That(worldGenerator.ReadDsl, Is.EqualTo(mockReadDsl));
            Assert.That(worldGenerator.Mapper, Is.EqualTo(mockMapper));
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
        var worldGenerator = new WorldGenerator(mockBackupFileGen, mockCreateDsl, mockReadDsl, mockFilesystem,  mockMapper);

        // Act
        worldGenerator.ConstructBackup(Arg.Any<LearningWorld>(), "SomePath");
        
        
        
        // Assert
        Assert.Multiple(()=>
        {
            mockCreateDsl.Received().WriteLearningWorld(Arg.Any<LearningWorldPe>());
            mockReadDsl.Received().ReadLearningWorld("", Arg.Any<DocumentRootJson?>());
            mockBackupFileGen.Received().CreateBackupFolders();
            mockBackupFileGen.Received().WriteXmlFiles(Arg.Any<IReadDsl>(), "");
            mockBackupFileGen.Received().WriteBackupFile("SomePath");
            
        });
    }
}