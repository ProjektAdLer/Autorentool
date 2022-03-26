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
        });
    }

    [Test]
    public void ConstructBackup_ConstructBackup_CallsBackupFileGenerator()
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
    
    private static AuthoringTool.DataAccess.API.DataAccess CreateTestableDataAccess(IAuthoringToolConfiguration? configuration=null,
        IBackupFileGenerator? backupFileConstructor=null, IFileSaveHandler<LearningWorld>? fileSaveHandlerWorld=null)
    {
        configuration ??= Substitute.For<IAuthoringToolConfiguration>();
        backupFileConstructor ??= Substitute.For<IBackupFileGenerator>();
        fileSaveHandlerWorld ??= Substitute.For<IFileSaveHandler<LearningWorld>>();
        return new AuthoringTool.DataAccess.API.DataAccess(configuration, backupFileConstructor, fileSaveHandlerWorld);
    }
    
}