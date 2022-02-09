using AuthoringTool.API;
using AuthoringTool.API.Configuration;
using NSubstitute;
using NUnit.Framework;

namespace AuthoringToolTest.API;

[TestFixture]
public class AuthoringToolFactoryUt
{
    
    // beware in this test we currently use the real implementations no fakes.
    // -> can lead to side effects 
    [Test]
    public void CreateAuthoringTool()
    {
        var systemUnderTest = CreateAuthoringToolFactory();
        var stubAuthoringToolConfiguration = Substitute.For<IAuthoringToolConfiguration>();

        IAuthoringTool result = systemUnderTest.CreateAuthoringTool(stubAuthoringToolConfiguration);
        
        AssertArchitectstructureIsCorrect(result);
    }

    private static void AssertArchitectstructureIsCorrect(IAuthoringTool result)
    {
        Assert.That((result as AuthoringTool.API.AuthoringTool).BusinessLogic, Is.Not.Null);
        Assert.That((result as AuthoringTool.API.AuthoringTool).PresentationLogic, Is.Not.Null);
        Assert.That((result as AuthoringTool.API.AuthoringTool).PresentationLogic.BusinessLogic, Is.Not.Null);
        Assert.That((result as AuthoringTool.API.AuthoringTool).PresentationLogic.BusinessLogic.DataAccess, Is.Not.Null);
    }

    private static AuthoringTool.API.AuthoringToolFactory CreateAuthoringToolFactory()
    {
        return new AuthoringTool.API.AuthoringToolFactory();
    }
}