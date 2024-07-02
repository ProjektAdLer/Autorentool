namespace Shared.Networking;

public interface IPreflightHttpClient
{
    public HttpClient Client { get; }
}