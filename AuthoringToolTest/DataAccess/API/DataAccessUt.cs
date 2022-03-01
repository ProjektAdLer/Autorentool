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
        Assert.That(systemUnderTest.EmptyWorld, Is.Not.Null);
    }

    [Test]
    public void ExportWorld_EmptyWorld_ExistingXmlStructureModified()
    {
        //Arrange
        var mockExportWorld = Substitute.For<IExportEmptyWorld>();
        var systemUnderTest = CreateTestableDataAccess(null, mockExportWorld);
        
        //Act
        systemUnderTest.ExportWorld();
        
        //Assert
        mockExportWorld.Received().ModifyExistingXMLStructure();
    }
    
    private static AuthoringTool.DataAccess.API.DataAccess CreateStandardDataAccess(IAuthoringToolConfiguration fakeConfiguration=null)
    {
        fakeConfiguration ??= Substitute.For<IAuthoringToolConfiguration>();
        return new AuthoringTool.DataAccess.API.DataAccess(fakeConfiguration);
    }
    private static AuthoringTool.DataAccess.API.DataAccess CreateTestableDataAccess(IAuthoringToolConfiguration fakeConfiguration=null, IExportEmptyWorld fakeExportWorld=null)
    {
        fakeConfiguration ??= Substitute.For<IAuthoringToolConfiguration>();
        fakeExportWorld ??= Substitute.For<IExportEmptyWorld>();
        return new AuthoringTool.DataAccess.API.DataAccess(fakeConfiguration, fakeExportWorld);
    }
    
}