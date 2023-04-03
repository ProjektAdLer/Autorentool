using System.Net;
using ApiAccess.WebApi;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;
using RichardSzalay.MockHttp;
using Shared.Configuration;

namespace BackendAccessTest.WebApi;

public class UserBackendServicesUt
{
    private IAuthoringToolConfiguration _authoringToolConfiguration;

    [Test]
    public void BackendAccess_Standard_AllPropertiesInitialized()
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
        var mockedHttp = new MockHttpMessageHandler();

        var responseString = JsonConvert.SerializeObject(new Dictionary<string, string>
        {
            {"lmsToken", "expectedToken"}
        });
        mockedHttp.When("*")
            .Respond("application/json", responseString);
        var userWebApiServices = CreateTestableUserWebApiServices(null, mockedHttp.ToHttpClient());

        // Act
        var result = await userWebApiServices.GetUserTokenAsync("username", "password");

        // Assert
        Assert.That(result.LmsToken, Is.EqualTo("expectedToken"));
    }

    [Test]
    public void GetUserTokenAsync_InvalidInput_ThrowsException()
    {
        // Arrange
        var mockedHttp = new MockHttpMessageHandler();

        var responseContent = JsonConvert.SerializeObject(new Dictionary<string, string>
        {
            {"detail", "exoected Error Message"}
        });
        var response = new HttpResponseMessage(HttpStatusCode.Forbidden);

        response.Content = new StringContent(responseContent);
        mockedHttp.When("*")
            .Respond(response);

        var userWebApiServices = CreateTestableUserWebApiServices(null, mockedHttp.ToHttpClient());

        // Act
        // Assert
        Assert.ThrowsAsync<HttpRequestException>(async () =>
            await userWebApiServices.GetUserTokenAsync("username", "password"), "exoected Error Message");
    }

    [Test]
    public async Task GetUserInformationAsync_ValidInput_Returns()
    {
        var mockedHttp = new MockHttpMessageHandler();

        var responseContent = JsonConvert.SerializeObject(new Dictionary<string, object>
        {
            {"lmsUserName", "expectedUsername"},
            {"userEmail", "expectedEmail"},
            {"userId", 1},
            {"isAdmin", true}
        });

        var response = new HttpResponseMessage(HttpStatusCode.OK);
        response.Content = new StringContent(responseContent);
        mockedHttp.When("*")
            .Respond(response);

        var userWebApiServices = CreateTestableUserWebApiServices(null, mockedHttp.ToHttpClient());

        var result = await userWebApiServices.GetUserInformationAsync("token");

        Assert.Multiple(() =>
        {
            Assert.That(result.LmsUserName, Is.EqualTo("expectedUsername"));
            Assert.That(result.UserEmail, Is.EqualTo("expectedEmail"));
            Assert.That(result.UserId, Is.EqualTo(1));
            Assert.That(result.IsAdmin, Is.EqualTo(true));
        });
    }

    [Test]
    public void General_InvalidResponse_ThrowsException()
    {
        // Arrange
        var mockedHttp = new MockHttpMessageHandler();

        var response = new HttpResponseMessage(HttpStatusCode.Forbidden);

        response.Content = new StringContent("invalid response");
        mockedHttp.When("*")
            .Respond(response);

        var userWebApiServices = CreateTestableUserWebApiServices(null, mockedHttp.ToHttpClient());

        // Act
        // Assert
        Assert.ThrowsAsync<HttpRequestException>(async () =>
                await userWebApiServices.GetUserTokenAsync("username", "password"),
            "Das Ergebnis der Backend Api konnte nicht gelesen werden");
    }


    private static UserWebApiServices CreateTestableUserWebApiServices(
        IAuthoringToolConfiguration? configuration = null,
        HttpClient? httpClient = null,
        ILogger<UserWebApiServices>? logger = null
    )
    {
        configuration ??= Substitute.For<IAuthoringToolConfiguration>();
        httpClient ??= new MockHttpMessageHandler().ToHttpClient();
        logger ??= Substitute.For<ILogger<UserWebApiServices>>();

        return new UserWebApiServices(configuration, httpClient, logger);
    }
}