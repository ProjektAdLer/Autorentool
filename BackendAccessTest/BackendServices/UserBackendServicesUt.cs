using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Net;
using System.Net.Http.Handlers;
using System.Net.Sockets;
using System.Security.Authentication;
using BackendAccess.BackendServices;
using BusinessLogic.ErrorManagement.BackendAccess;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;
using RichardSzalay.MockHttp;
using Shared.Configuration;
using Shared.Networking;

namespace BackendAccessTest.BackendServices;

public class UserBackendServicesUt
{
    [Test]
    public void BackendAccess_Standard_AllPropertiesInitialized()
    {
        // Arrange
        var applicationConfiguration = Substitute.For<IApplicationConfiguration>();

        // Act
        var userWebApiServices = CreateTestableUserWebApiServices(applicationConfiguration);

        // Assert
        Assert.Multiple(
            () => { Assert.That(userWebApiServices.Configuration, Is.EqualTo(applicationConfiguration)); }
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
        mockedHttp
            .When("*")
            .Respond("application/json", responseString);
        var mockHttpClientFactory = Substitute.For<IHttpClientFactory>();
        mockHttpClientFactory
            .CreateClient(Arg.Any<ProgressMessageHandler>())
            .Returns(mockedHttp.ToHttpClient());
        
        var userWebApiServices = CreateTestableUserWebApiServices(httpClientFactory: mockHttpClientFactory);

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
            {"detail", "Error Message"}
        });
        var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
        response.StatusCode = HttpStatusCode.Unauthorized;

        response.Content = new StringContent(responseContent);
        mockedHttp
            .When("*")
            .Respond(req => response);
        var mockHttpClientFactory = Substitute.For<IHttpClientFactory>();
        mockHttpClientFactory
            .CreateClient(Arg.Any<ProgressMessageHandler>())
            .Returns(mockedHttp.ToHttpClient());

        var userWebApiServices = CreateTestableUserWebApiServices(httpClientFactory: mockHttpClientFactory);

        // Act
        // Assert
        var ex = Assert.ThrowsAsync<BackendInvalidLoginException>(async () =>
            await userWebApiServices.GetUserTokenAsync("username", "password"));
        Assert.That(ex!.Message, Is.EqualTo("Invalid Login Credentials."));
    }

    [Test]
    public void GetUserTokenAsync_NoAdLerBackendAtUrl_ThrowsException()
    {
        var mockedHttp = new MockHttpMessageHandler();

        var responseContent = JsonConvert.SerializeObject(new Dictionary<string, string>
        {
            {"detail", "Error Message"}
        });
        var response = new HttpResponseMessage(HttpStatusCode.NotFound);
        response.StatusCode = HttpStatusCode.NotFound;

        response.Content = new StringContent(responseContent);
        mockedHttp
            .When("*")
            .Respond(req => response);
        var mockHttpClientFactory = Substitute.For<IHttpClientFactory>();
        mockHttpClientFactory
            .CreateClient(Arg.Any<ProgressMessageHandler>())
            .Returns(mockedHttp.ToHttpClient());

        var userWebApiServices = CreateTestableUserWebApiServices(httpClientFactory: mockHttpClientFactory);

        var ex = Assert.ThrowsAsync<BackendInvalidUrlException>(async () =>
            await userWebApiServices.GetUserTokenAsync("username", "password"));
        Assert.That(ex!.Message, Is.EqualTo("There is no AdLer Backend at the given URL."));
    }

    [Test]
    public void GetUserTokenAsync_InvalidUrlSet_ThrowsException()
    {
        var applicationConfiguration = Substitute.For<IApplicationConfiguration>();
        applicationConfiguration[IApplicationConfiguration.BackendBaseUrl].Returns("invalidUrl");

        var userWebApiServices = CreateTestableUserWebApiServices(applicationConfiguration);

        var ex = Assert.ThrowsAsync<BackendInvalidUrlException>(async () =>
            await userWebApiServices.GetUserTokenAsync("username", "password"));
        Assert.That(ex!.Message, Is.EqualTo("Invalid URL."));
    }

    [Test]
    public void GetUserTokenAsync_InvalidUrlWithUnsupportedProtocolSet_ThrowsException()
    {
        var mockedHttp = new MockHttpMessageHandler();
        var applicationConfiguration = Substitute.For<IApplicationConfiguration>();
        applicationConfiguration[IApplicationConfiguration.BackendBaseUrl].Returns("htp://invalidUrl.com");

        mockedHttp
            .When("*")
            .Throw(new NotSupportedException());
        var mockHttpClientFactory = Substitute.For<IHttpClientFactory>();
        mockHttpClientFactory
            .CreateClient(Arg.Any<ProgressMessageHandler>())
            .Returns(mockedHttp.ToHttpClient());

        var userWebApiServices =
            CreateTestableUserWebApiServices(applicationConfiguration, httpClientFactory: mockHttpClientFactory);

        var ex = Assert.ThrowsAsync<BackendInvalidUrlException>(async () =>
            await userWebApiServices.GetUserTokenAsync("username", "password"));
        Assert.That(ex!.Message, Is.EqualTo("Invalid URL. Does the URL start with 'http://' or 'https://'?"));
    }

    [Test]
    public void GetUserTokenAsync_InvalidSslCertificate_ThrowsException()
    {
        var mockedHttp = new MockHttpMessageHandler();
        var applicationConfiguration = Substitute.For<IApplicationConfiguration>();
        applicationConfiguration[IApplicationConfiguration.BackendBaseUrl].Returns("https://invalidUrl.com");

        var httpRequestException = new HttpRequestException("", new AuthenticationException());
        mockedHttp
            .When("*")
            .Throw(httpRequestException);
        
        var mockHttpClientFactory = Substitute.For<IHttpClientFactory>();
        mockHttpClientFactory
            .CreateClient(Arg.Any<ProgressMessageHandler>())
            .Returns(mockedHttp.ToHttpClient());

        var userWebApiServices = CreateTestableUserWebApiServices(applicationConfiguration, httpClientFactory: mockHttpClientFactory);

        var ex = Assert.ThrowsAsync<BackendInvalidUrlException>(async () =>
            await userWebApiServices.GetUserTokenAsync("username", "password"));
        Assert.That(ex!.Message,
            Is.EqualTo(
                "The SSL certificate is invalid. If the URL is correct, there is a problem with the SSL certificate of the AdLer Backend or you have to explicitly trust this certificate."));
    }

    [Test]
    public void GetUserTokenAsync_UrlNotReachable_ThrowsException()
    {
        var mockedHttp = new MockHttpMessageHandler();
        var applicationConfiguration = Substitute.For<IApplicationConfiguration>();
        applicationConfiguration[IApplicationConfiguration.BackendBaseUrl].Returns("https://invalidUrl.com");

        var httpRequestException = new HttpRequestException("", new SocketException());
        mockedHttp
            .When("*")
            .Throw(httpRequestException);
        
        var mockHttpClientFactory = Substitute.For<IHttpClientFactory>();
        mockHttpClientFactory
            .CreateClient(Arg.Any<ProgressMessageHandler>())
            .Returns(mockedHttp.ToHttpClient());

        var userWebApiServices = CreateTestableUserWebApiServices(applicationConfiguration, httpClientFactory: mockHttpClientFactory);

        var ex = Assert.ThrowsAsync<BackendInvalidUrlException>(async () =>
            await userWebApiServices.GetUserTokenAsync("username", "password"));
        Assert.That(ex!.Message,
            Is.EqualTo(
                "The URL is not reachable. Either the URL does not exist or there is no internet connection."));
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
        mockedHttp
            .When("*")
            .Respond(response);
        var mockHttpClientFactory = Substitute.For<IHttpClientFactory>();
        mockHttpClientFactory
            .CreateClient(Arg.Any<ProgressMessageHandler>())
            .Returns(mockedHttp.ToHttpClient());

        var userWebApiServices = CreateTestableUserWebApiServices(httpClientFactory: mockHttpClientFactory);

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

        var response = new HttpResponseMessage(HttpStatusCode.Conflict);

        response.Content = new StringContent("invalid response");
        mockedHttp
            .When("*")
            .Respond(response);
        var mockHttpClientFactory = Substitute.For<IHttpClientFactory>();
        mockHttpClientFactory
            .CreateClient(Arg.Any<HttpMessageHandler>())
            .Returns(mockedHttp.ToHttpClient());

        var userWebApiServices = CreateTestableUserWebApiServices(httpClientFactory: mockHttpClientFactory);

        // Act
        // Assert
        var ex = Assert.ThrowsAsync<HttpRequestException>(async () =>
            await userWebApiServices.GetUserTokenAsync("username", "password"));
        Assert.That(ex!.Message, Is.EqualTo("Das Ergebnis der Backend Api konnte nicht gelesen werden"));
    }

    [Test]
    public async Task UploadLearningWorldAsync_Valid_CallsHttpClient()
    {
        var mockedHttp = new MockHttpMessageHandler();

        mockedHttp
            .When("*")
            .Respond("application/json", "true");
        var mockHttpClientFactory = Substitute.For<IHttpClientFactory>();
        mockHttpClientFactory
            .CreateClient(Arg.Any<HttpMessageHandler>())
            .Returns(mockedHttp.ToHttpClient());

        var mockfileSystem = new MockFileSystem();
        mockfileSystem.AddFile("test.mbz", new MockFileData("testmbz"));
        mockfileSystem.AddFile("testawt.json", new MockFileData("testawt"));

        var userWebApiServices =
            CreateTestableUserWebApiServices(httpClientFactory: mockHttpClientFactory, fileSystem: mockfileSystem);

        // Act
        var output = await userWebApiServices.UploadLearningWorldAsync("testToken", "test.mbz", "testawt.json");

        // Assert
        Assert.That(output, Is.EqualTo(true));
    }

    [Test]
    public void UploadLearningWorldAsync_InvalidATFPath_ThrowsArgumentException()
    {
        var mockfileSystem = new MockFileSystem();
        mockfileSystem.AddFile("test.mbz", new MockFileData("testmbz"));
        //mockfileSystem.AddFile("testawt.json", new MockFileData("testawt"));

        var userWebApiServices =
            CreateTestableUserWebApiServices(fileSystem: mockfileSystem);

        // Act
        // Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
            await userWebApiServices.UploadLearningWorldAsync("testToken", "test.mbz", "testawt.json"));
        Assert.That(ex!.Message, Is.EqualTo("The awt path is not valid."));
    }

    [Test]
    public void UploadLearningWorldAsync_InvalidMBZPath_ThrowsArgumentException()
    {
        var mockfileSystem = new MockFileSystem();
        //mockfileSystem.AddFile("test.mbz", new MockFileData("testmbz"));
        mockfileSystem.AddFile("testawt.json", new MockFileData("testawt"));

        var userWebApiServices =
            CreateTestableUserWebApiServices(fileSystem: mockfileSystem);

        // Act
        // Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
            await userWebApiServices.UploadLearningWorldAsync("testToken", "test.mbz", "testawt.json"));
        Assert.That(ex!.Message, Is.EqualTo("The backup path is not valid."));
    }


    private static UserWebApiServices CreateTestableUserWebApiServices(
        IApplicationConfiguration? configuration = null,
        ProgressMessageHandler? progressMessageHandler = null,
        IHttpClientFactory? httpClientFactory = null,
        ILogger<UserWebApiServices>? logger = null,
        IFileSystem? fileSystem = null
    )
    {
        if (configuration == null)
        {
            configuration = Substitute.For<IApplicationConfiguration>();
            configuration[IApplicationConfiguration.BackendBaseUrl].Returns("https://valid-url.org");
        }

        progressMessageHandler ??= Substitute.For<ProgressMessageHandler>();
        httpClientFactory ??= Substitute.For<IHttpClientFactory>();
        logger ??= Substitute.For<ILogger<UserWebApiServices>>();
        fileSystem ??= Substitute.For<IFileSystem>();

        return new UserWebApiServices(configuration, progressMessageHandler, httpClientFactory, logger, fileSystem);
    }
}