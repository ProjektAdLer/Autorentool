namespace Shared.Networking;

public class HttpClientFactory : IHttpClientFactory
{
    /// <summary>
    /// Creates a new HttpClient with a default handler.
    /// </summary>
    /// <returns>An instance of <see cref="HttpClient"/></returns>
    public HttpClient CreateClient()
    {
        return new HttpClient();
    }

    /// <summary>
    /// Creates a new HttpClient with a specified handler.
    /// </summary>
    /// <param name="handler">The handler to be used.</param>
    /// <returns>An instance of <see cref="HttpClient"/></returns>
    public HttpClient CreateClient(HttpMessageHandler handler)
    {
        return new HttpClient(handler);
    }
}