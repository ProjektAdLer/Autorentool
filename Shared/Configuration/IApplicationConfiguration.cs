namespace Shared.Configuration;

public interface IApplicationConfiguration
{
    const string BackendBaseUrl = "BackendBaseUrl";
    const string BackendUsername = "BackendUsername";
    const string BackendToken = "BackendToken";
    IObservableDictionary<string, string> Configuration { get; }
    string this[string key] { get; set; }
}