using AuthoringTool.API.Configuration;
using log4net;
using NSubstitute;
using NUnit.Framework;

namespace AuthoringToolTest.API.Configuration;

[TestFixture]
public class AuthoringToolConfigurationUt
{
    [Test]
    public void AuthoringToolConfiguration_Standard_AllPropertiesInitialized()
    {
        ILog mockLogger = Substitute.For<ILog>();
        var systemUnderTest = CreateStandardAuthoringToolConfiguration(mockLogger);
        
        Assert.That(systemUnderTest.Logger, Is.EqualTo(mockLogger));
    }

    private static IAuthoringToolConfiguration CreateStandardAuthoringToolConfiguration(ILog fakeLogger = null)
    { 
        fakeLogger ??= Substitute.For<ILog>();
        return new AuthoringToolConfiguration(fakeLogger);
    }
}