using AuthoringTool.API;
using AuthoringTool.API.Configuration;
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

        //Act 
        var systemUnderTest = CreateStandardDataAccess(mockConfiguration);
        
        //Assert
        Assert.That(systemUnderTest.Configuration, Is.EqualTo(mockConfiguration));
        Assert.That(systemUnderTest.BackupFile, Is.Not.Null);
    }

    [Test]
    public void ConstructBackup_BackupFile_AllMethods()
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
    
    private static AuthoringTool.DataAccess.API.DataAccess CreateStandardDataAccess(IAuthoringToolConfiguration fakeConfiguration=null)
    {
        fakeConfiguration ??= Substitute.For<IAuthoringToolConfiguration>();
        return new AuthoringTool.DataAccess.API.DataAccess(fakeConfiguration);
    }
    private static AuthoringTool.DataAccess.API.DataAccess CreateTestableDataAccess(IAuthoringToolConfiguration fakeConfiguration=null, IBackupFileGenerator fakeBackupFile=null)
    {
        fakeConfiguration ??= Substitute.For<IAuthoringToolConfiguration>();
        fakeBackupFile ??= Substitute.For<IBackupFileGenerator>();
        return new AuthoringTool.DataAccess.API.DataAccess(fakeConfiguration, fakeBackupFile);
    }
    
}