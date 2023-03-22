using ApiAccess.WebApi;
using NSubstitute;
using NUnit.Framework;
using Shared.Configuration;

namespace ApiAccessTest.WebApi;

public class UserWebApiServicesUt
{
    private IAuthoringToolConfiguration _authoringToolConfiguration;

    [Test]
    public void ApiAccess_Standard_AllPropertiesInitialized()
    {
        // Arrange
        _authoringToolConfiguration = Substitute.For<IAuthoringToolConfiguration>();

        // Act
        var userWebApiServices = new UserWebApiServices(_authoringToolConfiguration);

        // Assert
        Assert.Multiple(
            () => { Assert.That(userWebApiServices.Configuration, Is.EqualTo(_authoringToolConfiguration)); }
        );
    }

    [Test]
    public async Task GetUserTokenAsync_CorrentInput_ReturnsTokenFromAPI()
    {
        // Arrange
        _authoringToolConfiguration = Substitute.For<IAuthoringToolConfiguration>();
        var userWebApiServices = new UserWebApiServices(_authoringToolConfiguration);

        // Act
        var result = await userWebApiServices.GetUserTokenAsync("username", "password");

        // Assert
        Assert.That(result.Token, Is.EqualTo("token"));
    }
}