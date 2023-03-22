using System.Net.Http.Json;
using ApiAccess.ApiResponses;
using Shared.Configuration;

namespace ApiAccess.WebApi;

public class UserWebApiServices : IUserWebApiServices
{
    private readonly HttpClient _client;

    public UserWebApiServices(IAuthoringToolConfiguration configuration, HttpClient client)
    {
        Configuration = configuration;
        _client = client;
        _client.BaseAddress = new Uri("https://api.cluuub.xyz/api");
    }

    public IAuthoringToolConfiguration Configuration { get; }

    public async Task<UserTokenWebApiResponse> GetUserTokenAsync(string username, string password)
    {
        var apiResp = await _client.GetAsync($"/api/Users/Login?username={username}&password={password}");

        // TODO: Error handling. Aber erst, wenn es ein Konzept dafür von Tobi gibt.
        if (!apiResp.IsSuccessStatusCode)
            throw new ArgumentException("Fehler beim Login (Vermutlich falscher Username oder Passwort))");

        return await apiResp.Content.ReadFromJsonAsync<UserTokenWebApiResponse>();
    }
}