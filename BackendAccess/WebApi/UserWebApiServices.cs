using System.Text.Json;
using ApiAccess.BackendEntities;
using Microsoft.Extensions.Logging;
using Shared.Configuration;

namespace ApiAccess.WebApi;

public class UserWebApiServices : IUserWebApiServices
{
    private readonly HttpClient _client;
    private readonly ILogger<UserWebApiServices> _logger;

    public UserWebApiServices(IAuthoringToolConfiguration configuration, HttpClient client,
        ILogger<UserWebApiServices> logger)
    {
        Configuration = configuration;
        _client = client;
        _logger = logger;
        _client.BaseAddress = new Uri("https://api.cluuub.xyz/api");
    }

    public IAuthoringToolConfiguration Configuration { get; }

    public async Task<UserTokenWebApiBE> GetUserTokenAsync(string username, string password)
    {
        var apiResp = await _client.GetAsync($"/api/Users/Login?username={username}x&password={password}");

        // This will throw if the response is not successful.
        await HandleErrorMessage(apiResp);

        return TryRead<UserTokenWebApiBE>(await apiResp.Content.ReadAsStringAsync());
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

            var problemDetails = TryRead<ErrorWebApiBE>(error);
            throw new HttpRequestException(problemDetails.Detail);
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