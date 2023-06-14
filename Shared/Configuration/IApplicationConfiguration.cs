
using Microsoft.Extensions.Logging;

namespace Shared.Configuration;

public interface IApplicationConfiguration
{
    const string BackendBaseUrl = "BackendBaseUrl";
    IObservableDictionary<string, string> Configuration { get; }
    string this[string key] { get; set; }
}