using System.IO.Abstractions;
using System.Net;
using System.Net.Http.Handlers;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Text.Json;
using System.Web;
using BackendAccess.BackendEntities;
using BusinessLogic.ErrorManagement.BackendAccess;
using Microsoft.Extensions.Logging;
using Shared.Configuration;
using Shared.Networking;

namespace BackendAccess.BackendServices;

public class UserWebApiServices : IUserWebApiServices
{
    private readonly HttpClient _client;
    private readonly IFileSystem _fileSystem;
    private readonly ILogger<UserWebApiServices> _logger;
    private readonly ProgressMessageHandler _progressMessageHandler;

    public UserWebApiServices(IApplicationConfiguration configuration, ProgressMessageHandler progressMessageHandler,
        IHttpClientFactory httpClientFactory, ILogger<UserWebApiServices> logger, IFileSystem fileSystem)
    {
        Configuration = configuration;
        _client = httpClientFactory.CreateClient(progressMessageHandler);
        _progressMessageHandler = progressMessageHandler;
        _logger = logger;
        _fileSystem = fileSystem;
    }

    private IProgress<int>? ProgressReporter { get; set; }

    public IApplicationConfiguration Configuration { get; }

    /// <inheritdoc cref="IUserWebApiServices.GetUserTokenAsync"/>
    public async Task<UserTokenBE> GetUserTokenAsync(string username, string password)
    {
        var parameters = new Dictionary<string, string>
        {
            { "username", username },
            { "password", password }
        };

        try
        {
            return await SendHttpGetRequestAsync<UserTokenBE>("/Users/Login", parameters);
        }
        catch (HttpRequestException e)
        {
            _logger.LogError("Failed getting user token, {ExceptionMessage}", e.Message);
            switch (e.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    throw new BackendInvalidLoginException("Invalid Login Credentials.");
                case HttpStatusCode.NotFound:
                    throw new BackendInvalidUrlException("There is no AdLer Backend at the given URL.");
                case null:
                    switch (e.InnerException)
                    {
                        case AuthenticationException:
                            throw new BackendInvalidUrlException(
                                "The SSL certificate is invalid. If the URL is correct, there is a problem with the SSL certificate of the AdLer Backend or you have to explicitly trust this certificate.");
                        case SocketException:
                            throw new BackendInvalidUrlException(
                                "The URL is not reachable. Either the URL does not exist or there is no internet connection.");
                        default:
                            throw;
                    }

                default:
                    throw;
            }
        }
        catch (UriFormatException)
        {
            throw new BackendInvalidUrlException("Invalid URL.");
        }
        catch (NotSupportedException)
        {
            throw new BackendInvalidUrlException("Invalid URL. Does the URL start with 'http://' or 'https://'?");
        }
    }

    public async Task<UserInformationBE> GetUserInformationAsync(string token)
    {
        var parameters = new Dictionary<string, string>
        {
            { "WebServiceToken", token }
        };

        return await SendHttpGetRequestAsync<UserInformationBE>("/Users/UserData",
            parameters);
    }


    public async Task<bool> UploadLearningWorldAsync(string token, string backupPath, string awtPath,
        IProgress<int>? progress = null)
    {
        // Validate that the paths are valid.
        if (!_fileSystem.File.Exists(backupPath)) throw new ArgumentException("The backup path is not valid.");
        if (!_fileSystem.File.Exists(awtPath)) throw new ArgumentException("The awt path is not valid.");

        var headers = new Dictionary<string, string>
        {
            { "token", token },
            { "Accept", "text/plain" }
        };
        var content = new MultipartFormDataContent();
        content.Add(new StreamContent(_fileSystem.File.OpenRead(backupPath)),
            "backupFile", backupPath);
        content.Add(new StreamContent(_fileSystem.File.OpenRead(awtPath)),
            "atfFile", awtPath);

        return await SendHttpPostRequestAsync<bool>("/Worlds", headers, content, progress);
    }

    private async Task<TResponse> SendHttpPostRequestAsync<TResponse>(string url, IDictionary<string, string> headers,
        MultipartFormDataContent content, IProgress<int>? progress = null)
    {
        // Set the Base URL of the API.
        // TODO: This should be set in the configuration.
        url = new Uri(Configuration[IApplicationConfiguration.BackendBaseUrl]) + url;

        var request = new HttpRequestMessage(HttpMethod.Post, url);
        foreach (var (key, value) in headers) request.Headers.Add(key, value);
        request.Content = content;

        ProgressReporter = progress;
        _progressMessageHandler.HttpSendProgress += OnHttpSendProgress;
        HttpResponseMessage apiResp;
        try
        {
            apiResp = await _client.SendAsync(request);
        }
        finally
        {
            ProgressReporter = null;
            _progressMessageHandler.HttpSendProgress -= OnHttpSendProgress;
            content.Dispose();
        }

        // This will throw if the response is not successful.
        await HandleErrorMessage(apiResp);

        return TryRead<TResponse>(await apiResp.Content.ReadAsStringAsync());
    }

    /// <summary>
    /// Internal helper method for making requests and parsing responses generically
    /// </summary>
    /// <exception cref="HttpRequestException">Request failed due to underlying issue such as connection issues or configuration.</exception>
    private async Task<TResponse> SendHttpGetRequestAsync<TResponse>(string url, IDictionary<string, string> parameters)
    {
        // Build the query string from url and parameters.
        var queryString = HttpUtility.ParseQueryString(string.Empty);
        foreach (var (key, value) in parameters) queryString[key] = value;

        // Set the Base URL of the API.
        // TODO: This should be set in the configuration.
        url = new Uri(Configuration[IApplicationConfiguration.BackendBaseUrl]) + url;

        url += "?" + queryString;

        var apiResp = await _client.GetAsync(url);

        // This will throw if the response is not successful.
        await HandleErrorMessage(apiResp);

        return TryRead<TResponse>(await apiResp.Content.ReadAsStringAsync());
    }

    /// <summary>
    /// This method is used to handle errors that are returned by the API.
    /// It will be more refined, once we have a concept for error handling. - philgei
    /// </summary>
    /// <param name="apiResp">The response message which should be parsed for errors.</param>
    /// <exception cref="HttpRequestException">Response was not successful, see message for details.</exception>
    private async Task HandleErrorMessage(HttpResponseMessage apiResp)
    {
        if (apiResp.IsSuccessStatusCode) return;
        if (apiResp.StatusCode == HttpStatusCode.NotFound)
        {
            throw new HttpRequestException(apiResp.ReasonPhrase, null, apiResp.StatusCode);
        }

        var error = await apiResp.Content.ReadAsStringAsync();

        var problemDetails = TryRead<ErrorBE>(error);
        throw new HttpRequestException(problemDetails.Detail, null, apiResp.StatusCode);
    }


    /// <summary>
    /// This method is used to deserialize the response from the API.
    /// It will deserialize the response in case-insensitive mode.
    /// It will throw an exception if the response could not be deserialized.
    /// </summary>
    /// <param name="responseString">The response string of the HTTP request to be deserialized.</param>
    /// <typeparam name="TResponse">The type into which the response should be deserialized.</typeparam>
    /// <returns>Returns request deserialized into a <typeparamref name="TResponse"/> object.</returns>
    /// <exception cref="HttpRequestException">The response could not be deserialized.</exception>
    private static TResponse TryRead<TResponse>(string responseString)
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<TResponse>(responseString, options)!;
        }
        catch (Exception e)
        {
            throw new HttpRequestException("Http response could not be deserialized.", e);
        }
    }

    private void OnHttpSendProgress(object? sender, HttpProgressEventArgs httpProgressEventArgs)
    {
        ProgressReporter?.Report(httpProgressEventArgs.ProgressPercentage);
    }
}