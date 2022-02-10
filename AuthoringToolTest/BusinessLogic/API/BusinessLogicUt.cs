using AuthoringTool.API.Configuration;
using NUnit.Framework;
using AuthoringTool.BusinessLogic.API;
using AuthoringTool.DataAccess.API;
using NSubstitute;

namespace AuthoringToolTest.BusinessLogic.API;

[TestFixture]
public class BusinessLogicUt
{
    [Test]
    public void BusinessLogic_StandardConstructor_AllPropertiesInitialized()
    {
        IAuthoringToolConfiguration mockConfiguration = Substitute.For<IAuthoringToolConfiguration>();
        IDataAccess mockDataAccess = Substitute.For<IDataAccess>();  
        var systemUnderTest = CreateStandardBusinessLogic(mockConfiguration, mockDataAccess);
        
        Assert.AreEqual(mockConfiguration, systemUnderTest.Configuration);
        Assert.AreEqual(mockDataAccess, systemUnderTest.DataAccess);
    }

    private AuthoringTool.BusinessLogic.API.BusinessLogic CreateStandardBusinessLogic(
        IAuthoringToolConfiguration fakeConfiguration = null,
        IDataAccess fakeDataAccess = null)
    {
        fakeConfiguration ??= Substitute.For<IAuthoringToolConfiguration>();
        fakeDataAccess ??= Substitute.For<IDataAccess>();
        return new AuthoringTool.BusinessLogic.API.BusinessLogic(fakeConfiguration, fakeDataAccess);
    }
}