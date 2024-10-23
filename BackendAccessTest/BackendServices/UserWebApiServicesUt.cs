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

public class UserWebApiServicesUt
{
    [Test]
    public void BackendAccess_Standard_AllPropertiesInitialized()
    {
        // Arrange
        var applicationConfiguration = Substitute.For<IApplicationConfiguration>();
        var httpClientFactory = Substitute.For<IHttpClientFactory>();
        httpClientFactory.CreateClient(Arg.Any<ProgressMessageHandler>())
            .Returns(new HttpClient(new MockHttpMessageHandler()));

        // Act
        var userWebApiServices =
            CreateTestableUserWebApiServices(applicationConfiguration, httpClientFactory: httpClientFactory);

        // Assert
        Assert.That(userWebApiServices.Configuration, Is.EqualTo(applicationConfiguration));
    }

    [Test]
    // ANF-ID: [AHO21]
    public async Task GetUserTokenAsync_CorrectInput_ReturnsTokenFromAPI()
    {
        // Arrange
        var mockedHttp = new MockHttpMessageHandler();

        var responseString = JsonConvert.SerializeObject(new Dictionary<string, string>
        {
            { "lmsToken", "expectedToken" }
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
    // ANF-ID: [AHO21]
    public void GetUserTokenAsync_InvalidInput_ThrowsException()
    {
        // Arrange
        var mockedHttp = new MockHttpMessageHandler();

        var responseContent = JsonConvert.SerializeObject(new Dictionary<string, string>
        {
            { "detail", "Error Message" }
        });
        var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
        response.StatusCode = HttpStatusCode.Unauthorized;

        response.Content = new StringContent(responseContent);
        mockedHttp
            .When("*")
            .Respond(_ => response);
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
    // ANF-ID: [AHO21]
    public void GetUserTokenAsync_NoAdLerBackendAtUrl_ThrowsException()
    {
        var mockedHttp = new MockHttpMessageHandler();

        var responseContent = JsonConvert.SerializeObject(new Dictionary<string, string>
        {
            { "detail", "Error Message" }
        });
        var response = new HttpResponseMessage(HttpStatusCode.NotFound);
        response.StatusCode = HttpStatusCode.NotFound;

        response.Content = new StringContent(responseContent);
        mockedHttp
            .When("*")
            .Respond(_ => response);
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
    // ANF-ID: [AHO21]
    public void GetUserTokenAsync_InvalidUrlSet_ThrowsException()
    {
        var applicationConfiguration = Substitute.For<IApplicationConfiguration>();
        applicationConfiguration[IApplicationConfiguration.BackendBaseUrl].Returns("invalidUrl");
        var httpClientFactory = Substitute.For<IHttpClientFactory>();
        var mockHttpMessageHandler = new MockHttpMessageHandler();
        httpClientFactory.CreateClient(Arg.Any<ProgressMessageHandler>())
            .Returns(new HttpClient(mockHttpMessageHandler));

        mockHttpMessageHandler.When("*").Throw(new HttpRequestException("Invalid URL", new SocketException()));

        var userWebApiServices =
            CreateTestableUserWebApiServices(applicationConfiguration, httpClientFactory: httpClientFactory);

        var ex = Assert.ThrowsAsync<BackendInvalidUrlException>(async () =>
            await userWebApiServices.GetUserTokenAsync("username", "password"));
        Assert.That(ex!.Message,
            Is.EqualTo("The URL is not reachable. Either the URL does not exist or there is no internet connection."));
    }

    [Test]
    // ANF-ID: [AHO21]
    public void GetUserTokenAsync_InvalidUriFormat_ThrowsException()
    {
        var mockedHttp = new MockHttpMessageHandler();
        var applicationConfiguration = Substitute.For<IApplicationConfiguration>();
        applicationConfiguration[IApplicationConfiguration.BackendBaseUrl].Returns("htp://invalidUrl.com");

        mockedHttp
            .When("*")
            .Throw(new UriFormatException());
        var mockHttpClientFactory = Substitute.For<IHttpClientFactory>();
        mockHttpClientFactory
            .CreateClient(Arg.Any<ProgressMessageHandler>())
            .Returns(mockedHttp.ToHttpClient());

        var userWebApiServices =
            CreateTestableUserWebApiServices(applicationConfiguration, httpClientFactory: mockHttpClientFactory);

        var ex = Assert.ThrowsAsync<BackendInvalidUrlException>(async () =>
            await userWebApiServices.GetUserTokenAsync("username", "password"));
        Assert.That(ex!.Message, Is.EqualTo("Invalid URL."));
    }

    [Test]
    // ANF-ID: [AHO21]
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
    // ANF-ID: [AHO21]
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

        var userWebApiServices =
            CreateTestableUserWebApiServices(applicationConfiguration, httpClientFactory: mockHttpClientFactory);

        var ex = Assert.ThrowsAsync<BackendInvalidUrlException>(async () =>
            await userWebApiServices.GetUserTokenAsync("username", "password"));
        Assert.That(ex!.Message,
            Is.EqualTo(
                "The SSL certificate is invalid. If the URL is correct, there is a problem with the SSL certificate of the AdLer Backend or you have to explicitly trust this certificate."));
    }

    [Test]
    // ANF-ID: [AHO21]
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

        var userWebApiServices =
            CreateTestableUserWebApiServices(applicationConfiguration, httpClientFactory: mockHttpClientFactory);

        var ex = Assert.ThrowsAsync<BackendInvalidUrlException>(async () =>
            await userWebApiServices.GetUserTokenAsync("username", "password"));
        Assert.That(ex!.Message,
            Is.EqualTo(
                "The URL is not reachable. Either the URL does not exist or there is no internet connection."));
    }

    [Test]
    // ANF-ID: [AHO21]
    public void GetUserTokenAsync_AnyOtherHttpRequestException_ThrowsException()
    {
        var mockedHttp = new MockHttpMessageHandler();

        var responseContent = JsonConvert.SerializeObject(new Dictionary<string, string>
        {
            { "detail", "Error Message" }
        });
        var response = new HttpResponseMessage(HttpStatusCode.Ambiguous);
        response.StatusCode = HttpStatusCode.Ambiguous;

        response.Content = new StringContent(responseContent);
        mockedHttp
            .When("*")
            .Respond(_ => response);
        var mockHttpClientFactory = Substitute.For<IHttpClientFactory>();
        mockHttpClientFactory
            .CreateClient(Arg.Any<ProgressMessageHandler>())
            .Returns(mockedHttp.ToHttpClient());

        var userWebApiServices = CreateTestableUserWebApiServices(httpClientFactory: mockHttpClientFactory);

        var ex = Assert.ThrowsAsync<BackendHttpRequestException>(async () =>
            await userWebApiServices.GetUserTokenAsync("username", "password"));
        Assert.That(ex!.Message, Is.EqualTo("Error Message"));
    }

    [Test]
    // ANF-ID: [AHO21]
    public async Task GetUserInformationAsync_ValidInput_Returns()
    {
        var mockedHttp = new MockHttpMessageHandler();

        var responseContent = JsonConvert.SerializeObject(new Dictionary<string, object>
        {
            { "lmsUserName", "expectedUsername" },
            { "userEmail", "expectedEmail" },
            { "userId", 1 },
            { "isAdmin", true }
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
    // ANF-ID: [AHO21]
    public void GetUserInformationAsync_InvalidToken_ThrowsException()
    {
        var mockedHttp = new MockHttpMessageHandler();

        var exception = new BackendHttpRequestException("The provided token is invalid", null, null, ErrorCodes.LmsTokenInvalid);
        mockedHttp
            .When("*")
            .Throw(exception);
        var mockHttpClientFactory = Substitute.For<IHttpClientFactory>();
        mockHttpClientFactory
            .CreateClient(Arg.Any<ProgressMessageHandler>())
            .Returns(mockedHttp.ToHttpClient());

        var userWebApiServices = CreateTestableUserWebApiServices(httpClientFactory: mockHttpClientFactory);

        var ex = Assert.ThrowsAsync<BackendInvalidTokenException>(async () =>
            await userWebApiServices.GetUserInformationAsync("token"));
        Assert.That(ex!.Message, Is.EqualTo("The provided token is invalid"));
        Assert.That(ex.InnerException, Is.EqualTo(exception));
    }
    
    [Test]
    // ANF-ID: [AHO21]
    public void GetUserInformationAsync_MoodleUnreachable_ThrowsException()
    {
        var mockedHttp = new MockHttpMessageHandler();

        var exception = new BackendHttpRequestException(
            "Das Ergebnis der Moodle Web Api konnte nicht gelesen werden. Response string is: 404 page not found\n",
            null, null, ErrorCodes.LmsError);
        mockedHttp
            .When("*")
            .Throw(exception);
        var mockHttpClientFactory = Substitute.For<IHttpClientFactory>();
        mockHttpClientFactory
            .CreateClient(Arg.Any<ProgressMessageHandler>())
            .Returns(mockedHttp.ToHttpClient());

        var userWebApiServices = CreateTestableUserWebApiServices(httpClientFactory: mockHttpClientFactory);

        var ex = Assert.ThrowsAsync<BackendMoodleApiUnreachableException>(async () =>
            await userWebApiServices.GetUserInformationAsync("token"));
        Assert.That(ex!.Message, Is.EqualTo("Das Ergebnis der Moodle Web Api konnte nicht gelesen werden. Response string is: 404 page not found\n"));
        Assert.That(ex.InnerException, Is.EqualTo(exception));
    }

    [Test]
    // ANF-ID: [AHO21]
    public void GetUserInformationAsync_OtherHttpRequestException_ThrowsException()
    {
        var mockedHttp = new MockHttpMessageHandler();

        var exception = new HttpRequestException("Error Message");
        mockedHttp
            .When("*")
            .Throw(exception);
        var mockHttpClientFactory = Substitute.For<IHttpClientFactory>();
        mockHttpClientFactory
            .CreateClient(Arg.Any<ProgressMessageHandler>())
            .Returns(mockedHttp.ToHttpClient());

        var userWebApiServices = CreateTestableUserWebApiServices(httpClientFactory: mockHttpClientFactory);

        var ex = Assert.ThrowsAsync<HttpRequestException>(async () =>
            await userWebApiServices.GetUserInformationAsync("token"));
        Assert.That(ex!.Message, Is.EqualTo("Error Message"));
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
        Assert.That(ex!.Message, Is.EqualTo("Http response could not be deserialized."));
    }

    [Test]
    // ANF-ID: [AHO22]
    public async Task UploadLearningWorldAsync_Valid_CallsHttpClient()
    {
        var mockedHttp = new MockHttpMessageHandler();

        mockedHttp
            .When("*")
            .Respond("application/json", @"{
                                               ""worldNameInLms"": ""AdLer Demo copy 5"",
                                               ""worldLmsUrl"": ""http://localhost:8085/course/view.php?id=7"",
                                               ""world3DUrl"": ""Coming soon(ish)""
                                           }");
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
        Assert.That(output.World3DUrl, Is.EqualTo("Coming soon(ish)"));
        Assert.That(output.WorldLmsUrl, Is.EqualTo("http://localhost:8085/course/view.php?id=7"));
        Assert.That(output.WorldNameInLms, Is.EqualTo("AdLer Demo copy 5"));
    }

    [Test]
    // ANF-ID: [AHO22]
    public void UploadLearningWorldAsync_OperationCanceledException_ThrowsException()
    {
        var mockedHttp = new MockHttpMessageHandler();

        var exception = new OperationCanceledException();
        mockedHttp
            .When("*")
            .Throw(exception);
        var mockHttpClientFactory = Substitute.For<IHttpClientFactory>();
        mockHttpClientFactory
            .CreateClient(Arg.Any<HttpMessageHandler>())
            .Returns(mockedHttp.ToHttpClient());

        var mockfileSystem = new MockFileSystem();
        mockfileSystem.AddFile("test.mbz", new MockFileData("testmbz"));
        mockfileSystem.AddFile("testawt.json", new MockFileData("testawt"));

        var userWebApiServices =
            CreateTestableUserWebApiServices(httpClientFactory: mockHttpClientFactory, fileSystem: mockfileSystem);

        // Act & Assert
        var ex = Assert.ThrowsAsync<TaskCanceledException>(async () =>
            await userWebApiServices.UploadLearningWorldAsync("testToken", "test.mbz", "testawt.json"));
    }

    [Test]
    // ANF-ID: [AHO22]
    public void UploadLearningWorldAsync_Exception_ThrowsException()
    {
        var mockedHttp = new MockHttpMessageHandler();

        var exception = new Exception();
        mockedHttp
            .When("*")
            .Throw(exception);
        var mockHttpClientFactory = Substitute.For<IHttpClientFactory>();
        mockHttpClientFactory
            .CreateClient(Arg.Any<HttpMessageHandler>())
            .Returns(mockedHttp.ToHttpClient());

        var mockfileSystem = new MockFileSystem();
        mockfileSystem.AddFile("test.mbz", new MockFileData("testmbz"));
        mockfileSystem.AddFile("testawt.json", new MockFileData("testawt"));

        var userWebApiServices =
            CreateTestableUserWebApiServices(httpClientFactory: mockHttpClientFactory, fileSystem: mockfileSystem);

        // Act & Assert
        var ex = Assert.ThrowsAsync<Exception>(async () =>
            await userWebApiServices.UploadLearningWorldAsync("testToken", "test.mbz", "testawt.json"));
    }

    [Test]
    // ANF-ID: [AHO22]
    public void UploadLearningWorldAsync_InvalidATFPath_ThrowsArgumentException()
    {
        var mockfileSystem = new MockFileSystem();
        mockfileSystem.AddFile("test.mbz", new MockFileData("testmbz"));
        //mockfileSystem.AddFile("testawt.json", new MockFileData("testawt"));
        var httpClientFactory = Substitute.For<IHttpClientFactory>();
        httpClientFactory.CreateClient(Arg.Any<ProgressMessageHandler>())
            .Returns(new HttpClient(new MockHttpMessageHandler()));

        var userWebApiServices =
            CreateTestableUserWebApiServices(fileSystem: mockfileSystem, httpClientFactory: httpClientFactory);

        // Act
        // Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
            await userWebApiServices.UploadLearningWorldAsync("testToken", "test.mbz", "testawt.json"));
        Assert.That(ex!.Message, Is.EqualTo("The awt path is not valid."));
    }

    [Test]
    // ANF-ID: [AHO22]
    public void UploadLearningWorldAsync_InvalidMBZPath_ThrowsArgumentException()
    {
        var mockfileSystem = new MockFileSystem();
        //mockfileSystem.AddFile("test.mbz", new MockFileData("testmbz"));
        mockfileSystem.AddFile("testawt.json", new MockFileData("testawt"));
        var httpClientFactory = Substitute.For<IHttpClientFactory>();
        httpClientFactory.CreateClient(Arg.Any<ProgressMessageHandler>())
            .Returns(new HttpClient(new MockHttpMessageHandler()));

        var userWebApiServices =
            CreateTestableUserWebApiServices(fileSystem: mockfileSystem, httpClientFactory: httpClientFactory);

        // Act
        // Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
            await userWebApiServices.UploadLearningWorldAsync("testToken", "test.mbz", "testawt.json"));
        Assert.That(ex!.Message, Is.EqualTo("The backup path is not valid."));
    }

    [Test]
    public async Task GetApiHealthcheck_Healthy_ReturnsTrue()
    {
        var mockedHttp = new MockHttpMessageHandler();
        var response = new HttpResponseMessage(HttpStatusCode.OK);
        response.Content = new StringContent("Healthy");

        mockedHttp
            .When("*")
            .Respond(_ => response);
        var mockHttpClientFactory = Substitute.For<IHttpClientFactory>();
        mockHttpClientFactory
            .CreateClient(Arg.Any<HttpMessageHandler>())
            .Returns(mockedHttp.ToHttpClient());

        var userWebApiServices =
            CreateTestableUserWebApiServices(httpClientFactory: mockHttpClientFactory);

        // Act
        var result = await userWebApiServices.GetApiHealthcheck();

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public async Task GetApiHealthcheck_NotHealthy_ReturnsFalse()
    {
        var mockedHttp = new MockHttpMessageHandler();
        var response = new HttpResponseMessage(HttpStatusCode.OK);
        response.Content = new StringContent("Not Healthy");

        mockedHttp
            .When("*")
            .Respond(_ => response);
        var mockHttpClientFactory = Substitute.For<IHttpClientFactory>();
        mockHttpClientFactory
            .CreateClient(Arg.Any<HttpMessageHandler>())
            .Returns(mockedHttp.ToHttpClient());

        var userWebApiServices =
            CreateTestableUserWebApiServices(httpClientFactory: mockHttpClientFactory);

        // Act
        var result = await userWebApiServices.GetApiHealthcheck();

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task GetApiHealthcheck_HttpRequestException_ReturnsFalse()
    {
        var mockedHttp = new MockHttpMessageHandler();
        var exception = new HttpRequestException();

        mockedHttp
            .When("*")
            .Throw(exception);
        var mockHttpClientFactory = Substitute.For<IHttpClientFactory>();
        mockHttpClientFactory
            .CreateClient(Arg.Any<HttpMessageHandler>())
            .Returns(mockedHttp.ToHttpClient());
        var logger = Substitute.For<ILogger<UserWebApiServices>>();

        var userWebApiServices =
            CreateTestableUserWebApiServices(httpClientFactory: mockHttpClientFactory, logger: logger);

        // Act
        var result = await userWebApiServices.GetApiHealthcheck();

        // Assert
        Assert.That(result, Is.False);
        logger.ReceivedWithAnyArgs().LogError(default);
    }

    [Test]
    public async Task GetApiHealthcheck_InvalidOperationException_ReturnsFalse()
    {
        var mockedHttp = new MockHttpMessageHandler();
        var exception = new InvalidOperationException();

        mockedHttp
            .When("*")
            .Throw(exception);
        var mockHttpClientFactory = Substitute.For<IHttpClientFactory>();
        mockHttpClientFactory
            .CreateClient(Arg.Any<HttpMessageHandler>())
            .Returns(mockedHttp.ToHttpClient());
        var logger = Substitute.For<ILogger<UserWebApiServices>>();

        var userWebApiServices =
            CreateTestableUserWebApiServices(httpClientFactory: mockHttpClientFactory, logger: logger);

        // Act
        var result = await userWebApiServices.GetApiHealthcheck();

        // Assert
        Assert.That(result, Is.False);
        logger.ReceivedWithAnyArgs().LogError(default);
    }

    [Test]
    public async Task GetApiHealthcheck_TaskCanceledException_ReturnsFalse()
    {
        var mockedHttp = new MockHttpMessageHandler();
        var exception = new TaskCanceledException();

        mockedHttp
            .When("*")
            .Throw(exception);
        var mockHttpClientFactory = Substitute.For<IHttpClientFactory>();
        mockHttpClientFactory
            .CreateClient(Arg.Any<HttpMessageHandler>())
            .Returns(mockedHttp.ToHttpClient());
        var logger = Substitute.For<ILogger<UserWebApiServices>>();

        var userWebApiServices =
            CreateTestableUserWebApiServices(httpClientFactory: mockHttpClientFactory, logger: logger);

        // Act
        var result = await userWebApiServices.GetApiHealthcheck();

        // Assert
        Assert.That(result, Is.False);
        logger.ReceivedWithAnyArgs().LogError(default);
    }

    [Test]
    public void GetApiHealthCheck_GetApiBaseUrl_IsNullOrWhiteSpace_ThrowsException()
    {
        var applicationConfiguration = Substitute.For<IApplicationConfiguration>();
        applicationConfiguration[IApplicationConfiguration.BackendBaseUrl].Returns("");

        var userWebApiServices = CreateTestableUserWebApiServices(applicationConfiguration);

        // Act & Assert
        var ex = Assert.ThrowsAsync<BackendInvalidUrlException>(async () =>
            await userWebApiServices.GetApiHealthcheck());
        Assert.That(ex!.Message, Is.EqualTo("No URL set in configuration yet."));
    }

    [Test]
    public void GetApiHealthCheck_GetApiBaseUrl_UriFormatException_ThrowsException()
    {
        var applicationConfiguration = Substitute.For<IApplicationConfiguration>();
        applicationConfiguration[IApplicationConfiguration.BackendBaseUrl].Returns("htp://%%invalidUrl%%.com");

        var userWebApiServices = CreateTestableUserWebApiServices(applicationConfiguration);

        // Act & Assert
        var ex = Assert.ThrowsAsync<BackendInvalidUrlException>(async () =>
            await userWebApiServices.GetApiHealthcheck());
        Assert.That(ex!.Message, Is.EqualTo("Invalid backend URL format"));
    }

    [Test]
    // ANF-ID: [AHO23]
    public async Task GetLmsWorldList_Valid_CallsHttpClient()
    {
        var mockedHttp = new MockHttpMessageHandler();

        mockedHttp
            .When("*")
            .Respond("application/json", @"{
        ""Worlds"": [
            {
                ""WorldName"": ""w1"",
                ""WorldId"": 1
            },
            {
                ""WorldName"": ""w2"",
                ""WorldId"": 2
            }
        ]
    }");


        var mockHttpClientFactory = Substitute.For<IHttpClientFactory>();
        mockHttpClientFactory
            .CreateClient(Arg.Any<HttpMessageHandler>())
            .Returns(mockedHttp.ToHttpClient());

        var userWebApiServices =
            CreateTestableUserWebApiServices(httpClientFactory: mockHttpClientFactory);

        // Act
        var output = await userWebApiServices.GetLmsWorldList("testToken", 4);

        // Assert
        Assert.That(output[0].WorldId, Is.EqualTo(1));
        Assert.That(output[0].WorldName, Is.EqualTo("w1"));
        Assert.That(output[1].WorldId, Is.EqualTo(2));
        Assert.That(output[1].WorldName, Is.EqualTo("w2"));
    }

    [Test]
    // ANF-ID: [AHO24]
    public async Task DeleteLmsWorld_ValidRequest_ReturnsTrue()
    {
        var mockedHttp = new MockHttpMessageHandler();
        var token = "testToken";
        var worldId = 123;
        var expectedUrl = $"Worlds/{worldId}";

        mockedHttp
            .When(HttpMethod.Delete, $"*{expectedUrl}")
            .WithHeaders("token", token)
            .Respond(HttpStatusCode.OK, "application/json", "true");

        var mockHttpClientFactory = Substitute.For<IHttpClientFactory>();
        mockHttpClientFactory
            .CreateClient(Arg.Any<HttpMessageHandler>())
            .Returns(mockedHttp.ToHttpClient());

        var userWebApiServices = CreateTestableUserWebApiServices(httpClientFactory: mockHttpClientFactory);

        var result = await userWebApiServices.DeleteLmsWorld(token, worldId);

        Assert.That(result, Is.True);

        mockedHttp.VerifyNoOutstandingExpectation();
    }

    private static IPreflightHttpClient CreatePreflightHttpClient()
    {
        var mockHttpHandler = new MockHttpMessageHandler();
        mockHttpHandler.When("/api/health").Respond(HttpStatusCode.OK);
        return new PreflightHttpClient(new HttpClient(mockHttpHandler));
    }


    private static UserWebApiServices CreateTestableUserWebApiServices(
        IApplicationConfiguration? configuration = null,
        ProgressMessageHandler? progressMessageHandler = null,
        IHttpClientFactory? httpClientFactory = null,
        ILogger<UserWebApiServices>? logger = null,
        IFileSystem? fileSystem = null,
        IPreflightHttpClient? preflightHttpClient = null
    )
    {
        if (configuration == null)
        {
            configuration = Substitute.For<IApplicationConfiguration>();
            configuration[IApplicationConfiguration.BackendBaseUrl].Returns("https://valid-url.org");
        }

        progressMessageHandler ??= Substitute.For<ProgressMessageHandler>();
        var mockHttpClient = new HttpClient(new MockHttpMessageHandler());
        var mockHttpClientFactory = Substitute.For<IHttpClientFactory>();
        mockHttpClientFactory.CreateClient(Arg.Any<ProgressMessageHandler>()).Returns(mockHttpClient);
        httpClientFactory ??= mockHttpClientFactory;
        logger ??= Substitute.For<ILogger<UserWebApiServices>>();
        fileSystem ??= Substitute.For<IFileSystem>();
        preflightHttpClient ??= CreatePreflightHttpClient();

        return new UserWebApiServices(configuration, progressMessageHandler, httpClientFactory, logger, fileSystem,
            preflightHttpClient);
    }
}