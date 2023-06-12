using System.IO.Abstractions;
using System.Net;
using System.Text.Json;
using System.Web;
using BackendAccess.BackendEntities;
using BusinessLogic.ErrorManagement.BackendAccess;
using Microsoft.Extensions.Logging;
using Shared.Configuration;

namespace BackendAccess.BackendServices;

public class UserWebApiServices : IUserWebApiServices
{
    private readonly HttpClient _client;
    private readonly IFileSystem _fileSystem;
    private readonly ILogger<UserWebApiServices> _logger;

    public UserWebApiServices(IAuthoringToolConfiguration configuration, HttpClient client,
        ILogger<UserWebApiServices> logger, IFileSystem fileSystem)
    {
        Configuration = configuration;
        _client = client;
        _logger = logger;
        _fileSystem = fileSystem;
    }

    public IAuthoringToolConfiguration Configuration { get; }

    /// <inheritdoc cref="IUserWebApiServices.GetUserTokenAsync"/>
    public async Task<UserTokenBE> GetUserTokenAsync(string username, string password)
    {
        var parameters = new Dictionary<string, string>
        {
            {"username", username},
            {"password", password}
        };

        try
        {
            return await SendHttpGetRequestAsync<UserTokenBE>("/Users/Login", parameters);
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine(e);
            if (e.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new BackendInvalidLoginException("Invalid Login Credentials.");
            }
            else
            {
                throw;
            }
        }
    }

    public async Task<UserInformationBE> GetUserInformationAsync(string token)
    {
        var parameters = new Dictionary<string, string>
        {
            {"WebServiceToken", token}
        };

        return await SendHttpGetRequestAsync<UserInformationBE>("/Users/UserData",
            parameters);
    }


    public async Task<bool> UploadLearningWorldAsync(string token, string backupPath, string awtPath)
    {
        // Validate that the paths are valid.
        if (!_fileSystem.File.Exists(backupPath)) throw new ArgumentException("The backup path is not valid.");
        if (!_fileSystem.File.Exists(awtPath)) throw new ArgumentException("The awt path is not valid.");

        var headers = new Dictionary<string, string>
        {
            {"token", token},
            {"Accept", "text/plain"}
        };
        var content = new MultipartFormDataContent();
        content.Add(new StreamContent(_fileSystem.File.OpenRead(backupPath)),
            "backupFile", backupPath);
        content.Add(new StreamContent(_fileSystem.File.OpenRead(awtPath)),
            "atfFile", awtPath);

        return await SendHttpPostRequestAsync<bool>("/Worlds", headers, content);
    }

    private async Task<TResponse> SendHttpPostRequestAsync<TResponse>(string url, IDictionary<string, string> headers,
        MultipartFormDataContent content)
    {
        // Set the Base URL of the API.
        // TODO: This should be set in the configuration.
        url = new Uri("https://dev.api.projekt-adler.eu/api") + url;

        var request = new HttpRequestMessage(HttpMethod.Post, url);
        foreach (var (key, value) in headers) request.Headers.Add(key, value);
        request.Content = content;

        var apiResp = await _client.SendAsync(request);
        content.Dispose();

        // This will throw if the response is not successful.
        await HandleErrorMessage(apiResp);

        return TryRead<TResponse>(await apiResp.Content.ReadAsStringAsync());
    }

    private async Task<TResponse> SendHttpGetRequestAsync<TResponse>(string url, IDictionary<string, string> parameters)
    {
        // Build the query string from url and parameters.
        var queryString = HttpUtility.ParseQueryString(string.Empty);
        foreach (var (key, value) in parameters) queryString[key] = value;

        // Set the Base URL of the API.
        // TODO: This should be set in the configuration.
        url = new Uri("https://dev.api.projekt-adler.eu/api") + url;

        url += "?" + queryString;

        var apiResp = await _client.GetAsync(url);

        // This will throw if the response is not successful.
        await HandleErrorMessage(apiResp);

        return TryRead<TResponse>(await apiResp.Content.ReadAsStringAsync());
    }

    /**
     * This method is used to handle errors that are returned by the API.
     * It will be more refined, once we have a concept for error handling.
     * 
     * @throws HttpRequestException with meaningfully message if the response is not successful.
     */
    private async Task HandleErrorMessage(HttpResponseMessage apiResp)
    {
        if (!apiResp.IsSuccessStatusCode)
        {
            var error = await apiResp.Content.ReadAsStringAsync();

            var problemDetails = TryRead<ErrorBE>(error);
            throw new HttpRequestException(problemDetails.Detail, null, apiResp.StatusCode);
        }
    }

    /**
     * This method is used to deserialize the response from the API.
     * It will deserialize the response in case-insensitive mode.
     * It will throw an exception if the response could not be deserialized.
     * 
     * @throws HttpRequestException if the response could not be deserialized.
     */
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
            throw new HttpRequestException("Das Ergebnis der Backend Api konnte nicht gelesen werden", e);
        }
    }
}