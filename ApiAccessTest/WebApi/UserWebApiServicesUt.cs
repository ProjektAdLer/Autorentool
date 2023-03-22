using ApiAccess.WebApi;
using NSubstitute;
using NUnit.Framework;
using RichardSzalay.MockHttp;
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
        var userWebApiServices = CreateTestableUserWebApiServices(_authoringToolConfiguration);

        // Assert
        Assert.Multiple(
            () => { Assert.That(userWebApiServices.Configuration, Is.EqualTo(_authoringToolConfiguration)); }
        );
    }

    [Test]
    public async Task GetUserTokenAsync_CorrectInput_ReturnsTokenFromAPI()
    {
        // Arrange
        var userWebApiServices = CreateTestableUserWebApiServices(_authoringToolConfiguration);

        // Act
        var result = await userWebApiServices.GetUserTokenAsync("username", "password");

        // Assert
        Assert.That(result.LmsToken, Is.EqualTo("token"));
    }

    private static UserWebApiServices CreateTestableUserWebApiServices(
        IAuthoringToolConfiguration? configuration = null,
        HttpClient? httpClient = null
    )
    {
        configuration ??= Substitute.For<IAuthoringToolConfiguration>();
        httpClient ??= new MockHttpMessageHandler().ToHttpClient();

        return new UserWebApiServices(configuration, httpClient);
    }
}