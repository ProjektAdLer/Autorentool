namespace Shared.Networking;

public interface IHttpClientFactory
{
    HttpClient CreateClient();
    HttpClient CreateClient(HttpMessageHandler handler);
}