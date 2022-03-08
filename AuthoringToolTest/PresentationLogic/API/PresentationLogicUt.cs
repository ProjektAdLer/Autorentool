using AuthoringTool.API.Configuration;
using AuthoringTool.BusinessLogic.API;
using NSubstitute;
using NUnit.Framework;

namespace AuthoringToolTest.PresentationLogic.API;

[TestFixture]
public class PresentationLogicUt
{
    [Test]
    public void PresentationLogic_Standard_AllPropertiesInitialized()
    {
        //Arrange
        var mockConfiguration = Substitute.For<IAuthoringToolConfiguration>();
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        
        //Act
        var systemUnderTest = CreateStandardPresentationLogic(mockConfiguration, mockBusinessLogic);
        
        //Assert
        Assert.That(systemUnderTest.Configuration, Is.EqualTo(mockConfiguration));
        Assert.That(systemUnderTest.BusinessLogic, Is.EqualTo(mockBusinessLogic));
        
    }
    
    [Test]
    public void PresentationLogic_ExportWorldOnly_ExportWorld()
    {
        //Arrange
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var systemUnderTest = CreateTestablePresentationLogic(null,mockBusinessLogic);
        
        //Act
        systemUnderTest.ExportWorld();
        
        //Assert
        mockBusinessLogic.Received().ExportWorld();
    }

    private static AuthoringTool.PresentationLogic.API.PresentationLogic CreateStandardPresentationLogic(IAuthoringToolConfiguration fakeConfiguration=null, IBusinessLogic fakeBusinessLogic=null)
    {
        fakeConfiguration ??= Substitute.For<IAuthoringToolConfiguration>();
        fakeBusinessLogic ??= Substitute.For<IBusinessLogic>();
        return new AuthoringTool.PresentationLogic.API.PresentationLogic(fakeConfiguration, fakeBusinessLogic);
    }
    
    private static AuthoringTool.PresentationLogic.API.PresentationLogic CreateTestablePresentationLogic(IAuthoringToolConfiguration fakeConfiguration=null, IBusinessLogic fakeBusinessLogic=null)
    {
        fakeConfiguration ??= Substitute.For<IAuthoringToolConfiguration>();
        fakeBusinessLogic ??= Substitute.For<IBusinessLogic>();
        return new AuthoringTool.PresentationLogic.API.PresentationLogic(fakeConfiguration, fakeBusinessLogic);
    }
}