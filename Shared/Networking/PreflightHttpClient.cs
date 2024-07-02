namespace Shared.Networking;

public class PreflightHttpClient : IPreflightHttpClient
{
    public PreflightHttpClient()
    {
        Client = new HttpClient(new HttpClientHandler
        {
            AllowAutoRedirect = false,
        });
    }
    
    public PreflightHttpClient(HttpClient client)
    {
        Client = client;
    }
    
    public HttpClient Client { get; }
}