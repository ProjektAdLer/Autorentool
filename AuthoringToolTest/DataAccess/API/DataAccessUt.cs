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
        var mockBackupFile = Substitute.For<IConstructBackupFile>();
        var systemUnderTest = CreateTestableDataAccess(null, mockBackupFile);
        
        //Act
        systemUnderTest.ConstructBackup();
        
        //Assert
        mockBackupFile.Received().CreateXMLFiles();
        //mockBackupFile.Received().OverwriteEncoding();
        mockBackupFile.Received().CreateBackupFile();
    }
    
    private static AuthoringTool.DataAccess.API.DataAccess CreateStandardDataAccess(IAuthoringToolConfiguration fakeConfiguration=null)
    {
        fakeConfiguration ??= Substitute.For<IAuthoringToolConfiguration>();
        return new AuthoringTool.DataAccess.API.DataAccess(fakeConfiguration);
    }
    private static AuthoringTool.DataAccess.API.DataAccess CreateTestableDataAccess(IAuthoringToolConfiguration fakeConfiguration=null, IConstructBackupFile fakeBackupFile=null)
    {
        fakeConfiguration ??= Substitute.For<IAuthoringToolConfiguration>();
        fakeBackupFile ??= Substitute.For<IConstructBackupFile>();
        return new AuthoringTool.DataAccess.API.DataAccess(fakeConfiguration, fakeBackupFile);
    }
    
}