using AuthoringToolLib.API.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace AuthoringToolLibTest.API.Configuration;

[TestFixture]
public class AuthoringToolConfigurationUt
{
    [Test]
    public void AuthoringToolConfiguration_Standard_AllPropertiesInitialized()
    {
        var mockLogger = Substitute.For<ILogger<AuthoringToolConfiguration>>();
        var systemUnderTest = CreateStandardAuthoringToolConfiguration(mockLogger);
        
        Assert.That(systemUnderTest.Logger, Is.EqualTo(mockLogger));
    }

    private static IAuthoringToolConfiguration CreateStandardAuthoringToolConfiguration(ILogger<AuthoringToolConfiguration>? fakeLogger = null)
    { 
        fakeLogger ??= Substitute.For<ILogger<AuthoringToolConfiguration>>();
        return new AuthoringToolConfiguration(fakeLogger);
    }
}