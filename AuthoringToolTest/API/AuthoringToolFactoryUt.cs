using AuthoringTool.API;
using AuthoringTool.API.Configuration;
using log4net;
using NSubstitute;
using NUnit.Framework;

namespace AuthoringToolTest.API;

[TestFixture]
public class AuthoringToolFactoryUt
{
    
    // beware in this test we currently use the real implementations no fakes.
    // -> can lead to side effects 
    //      example effect: somebody implements in the DataAccessAccess constructor the creation for a DataBase
    //      -> This DB will be build each time the test runs.
    [Test]
    public void CreateAuthoringTool_Normal_CorrectArchitecture()
    {
        var systemUnderTest = CreateAuthoringToolFactory();
        var configuration = CreateAuthoringToolConfiguration(systemUnderTest);

        IAuthoringTool result = systemUnderTest.CreateAuthoringTool(configuration);
        
        AssertArchitectureIsCorrect(result);
    }
    private static void AssertArchitectureIsCorrect(IAuthoringTool result)
    {
        Assert.That((result as AuthoringTool.API.AuthoringTool).BusinessLogic, Is.Not.Null);
        Assert.That((result as AuthoringTool.API.AuthoringTool).PresentationLogic, Is.Not.Null);
        Assert.That((result as AuthoringTool.API.AuthoringTool).PresentationLogic.BusinessLogic, Is.Not.Null);
        Assert.That((result as AuthoringTool.API.AuthoringTool).PresentationLogic.BusinessLogic.DataAccess, Is.Not.Null);
    }
    
    
    // beware in this test we currently use the real implementations no fakes.
    // -> can lead to side effects 
    //      example effect: somebody implements in the DataAccessAccess constructor the creation for a DataBase
    //      -> This DB will be build each time the test runs.
    [Test]
    public void CreateAuthoringTool_Normal_ConfigurationInEachLayer()
    {
        var systemUnderTest = CreateAuthoringToolFactory();
        var configuration = CreateAuthoringToolConfiguration(systemUnderTest);

        IAuthoringTool result = systemUnderTest.CreateAuthoringTool(configuration);
        
        AssertConfigurationIsInEachLayer(result, configuration);
    }

    private static void AssertConfigurationIsInEachLayer(IAuthoringTool result, IAuthoringToolConfiguration configuration)
    {
        Assert.That((result as AuthoringTool.API.AuthoringTool).Configuration, Is.EqualTo(configuration));
        Assert.That((result as AuthoringTool.API.AuthoringTool).BusinessLogic.Configuration, Is.EqualTo(configuration));
        Assert.That((result as AuthoringTool.API.AuthoringTool).PresentationLogic.Configuration, Is.EqualTo(configuration));
        Assert.That((result as AuthoringTool.API.AuthoringTool).PresentationLogic.BusinessLogic.DataAccess.Configuration,
            Is.EqualTo(configuration));
    }


    private static IAuthoringToolConfiguration CreateAuthoringToolConfiguration(
        AuthoringToolFactory authoringToolFactory,
        ILog fakeLogger = null)
    {
        fakeLogger ??= Substitute.For<ILog>();
        return authoringToolFactory.CreateAuthoringToolConfiguration(fakeLogger);
    }
    
    

    private static AuthoringTool.API.AuthoringToolFactory CreateAuthoringToolFactory()
    {
        return new AuthoringTool.API.AuthoringToolFactory();
    }
}