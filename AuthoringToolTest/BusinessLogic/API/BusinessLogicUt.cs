using NUnit.Framework;
using AuthoringTool.BusinessLogic.API;

namespace AuthoringToolTest.BusinessLogic.API;

[TestFixture]
public class BusinessLogicUt
{
    [Test]
    public void BusinessLogic_StandardConstructor_AllPropertiesInitialized()
    {
        var systemUnderTest = CreateStandardBusinessLogic();
        
        Assert.NotNull(systemUnderTest.DataAccess);
    }

    private AuthoringTool.BusinessLogic.API.BusinessLogic CreateStandardBusinessLogic()
    {
        return new AuthoringTool.BusinessLogic.API.BusinessLogic();
    }
}